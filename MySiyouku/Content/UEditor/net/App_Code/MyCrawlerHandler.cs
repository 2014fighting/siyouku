using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Security.Cryptography;
using System.Text;
using System.Web;
using System.Web.Mvc;
using MySiyouku.Models.Common;
using Qiniu.IO;
using Qiniu.IO.Model;
using Qiniu.Util;
using Siyouku.Model.Database;
using Siyouku.Repositorys.Repository;
using Siyouku.Repositorys.RepositoryInterface;

namespace MySiyouku.Content.UEditor.net.App_Code
{
    public class MyCrawlerHandler : Handler
    {
        private string[] Sources;
        private Crawler[] Crawlers;
       

        public MyCrawlerHandler(HttpContext context) : base(context)
        {

        }

        public override void Process()
        {
            Sources = Request.Form.GetValues("source[]");
            if (Sources == null || Sources.Length == 0)
            {
                WriteJson(new
                {
                    state = "参数错误：没有指定抓取源"
                });
                return;
            }
            Crawlers = Sources.Select(x => new Crawler(x, Server,new PictureRepository()).Fetch()).ToArray();
            WriteJson(new
            {
                state = "SUCCESS",
                list = Crawlers.Select(x => new
                {
                    state = x.State,
                    source = x.SourceUrl,
                    url = x.ServerUrl
                })
            });
        }
    }

    public class Crawler
    {
        public IPictureRepository _pictureRepository;
        public string SourceUrl { get; set; }
        public string ServerUrl { get; set; }
        public string State { get; set; }

        private HttpServerUtility Server { get; set; }


        public Crawler(string sourceUrl, HttpServerUtility server, IPictureRepository pictureRepository)
        {
            _pictureRepository = pictureRepository;
            this.SourceUrl = sourceUrl;
            this.Server = server;
        }

        public Crawler Fetch()
        {
            if (!IsExternalIPAddress(this.SourceUrl))
            {
                State = "INVALID_URL";
                return this;
            }
            var request = HttpWebRequest.Create(this.SourceUrl) as HttpWebRequest;
            using (var response = request.GetResponse() as HttpWebResponse)
            {
                if (response.StatusCode != HttpStatusCode.OK)
                {
                    State = "Url returns " + response.StatusCode + ", " + response.StatusDescription;
                    return this;
                }
                if (response.ContentType.IndexOf("image") == -1)
                {
                    State = "Url is not an image";
                    return this;
                }
                try
                {
      
                    using (var stream = response.GetResponseStream())
                    {
                        //var md5value = stream.GetStreamMd5();
                 
                            var maxid = _pictureRepository.GetMaxId();
                            var imgname = "img" + (maxid + 1) + ".jpg";
                            _pictureRepository.AddImg(new Picture()
                            {
                                ImgName = imgname,
                                CreateUser = "system",
                                CreateTime = DateTime.Now,
                                Md5Value = imgname
                            });
                            // 请不要使用UploadManager的UploadStream方法，因为此流不支持查找(无法获取Stream.Length)
                            // 请使用FormUploader或者ResumableUploader的UploadStream方法
                            FormUploader fu = new FormUploader();
                            var result = fu.UploadStream(stream, imgname, GetUploadToken(imgname));
                            var x = Newtonsoft.Json.JsonConvert.DeserializeObject<QiniuResult>(result.Text);
                            if (x.key == null)
                                throw new Exception(result.Text);
                            this.ServerUrl = x.key;
                        }

                  
                   
                    State = "SUCCESS";
                }
                catch (Exception e)
                {
                    State = "抓取错误：" + e.Message;
                }
                return this;
            }
        }
        private string Md5With32(byte[] query)
        {
            MD5 md5 = MD5.Create();
            byte[] bytes = md5.ComputeHash(query);
            // 第四步：把二进制转化为大写的十六进制
            StringBuilder result = new StringBuilder();
            for (int i = 0; i < bytes.Length; i++)
            {
                string hex = bytes[i].ToString("X");
                if (hex.Length == 1)
                {
                    result.Append("0");
                }
                result.Append(hex);
            }
            return result.ToString();
        }
        private bool IsExternalIPAddress(string url)
        {
            var uri = new Uri(url);
            switch (uri.HostNameType)
            {
                case UriHostNameType.Dns:
                    var ipHostEntry = Dns.GetHostEntry(uri.DnsSafeHost);
                    foreach (IPAddress ipAddress in ipHostEntry.AddressList)
                    {
                        byte[] ipBytes = ipAddress.GetAddressBytes();
                        if (ipAddress.AddressFamily == System.Net.Sockets.AddressFamily.InterNetwork)
                        {
                            if (!IsPrivateIP(ipAddress))
                            {
                                return true;
                            }
                        }
                    }
                    break;

                case UriHostNameType.IPv4:
                    return !IsPrivateIP(IPAddress.Parse(uri.DnsSafeHost));
            }
            return false;
        }

        private bool IsPrivateIP(IPAddress myIPAddress)
        {
            if (IPAddress.IsLoopback(myIPAddress)) return true;
            if (myIPAddress.AddressFamily == System.Net.Sockets.AddressFamily.InterNetwork)
            {
                byte[] ipBytes = myIPAddress.GetAddressBytes();
                // 10.0.0.0/24 
                if (ipBytes[0] == 10)
                {
                    return true;
                }
                // 172.16.0.0/16
                else if (ipBytes[0] == 172 && ipBytes[1] == 16)
                {
                    return true;
                }
                // 192.168.0.0/16
                else if (ipBytes[0] == 192 && ipBytes[1] == 168)
                {
                    return true;
                }
                // 169.254.0.0/16
                else if (ipBytes[0] == 169 && ipBytes[1] == 254)
                {
                    return true;
                }
            }
            return false;
        }

        public string  GetUploadToken(string sk)
        {

            var accessKey = "HNXvIcHqurHJAEDnVI4v2_qDGDvVlAXhipa6uVX-";
            var secretKey = "pQOAZutqyXr679mnDclCnIeaHgF0w3U-eWDFZJsp";

            // 生成(上传)凭证时需要使用此Mac
            // 这个示例单独使用了一个Settings类，其中包含AccessKey和SecretKey
            // 实际应用中，请自行设置您的AccessKey和SecretKey
            Mac mac = new Mac(accessKey, secretKey);
            string bucket = "siyouku";
            //string saveKey = Path.GetFileName(this.SourceUrl);
            string saveKey = sk;


            // 使用前请确保AK和BUCKET正确，否则此函数会抛出异常(比如code612/631等错误)
            Qiniu.Common.Config.AutoZone(accessKey, bucket, false);


            // 上传策略，参见 
            // https://developer.qiniu.com/kodo/manual/put-policy
            PutPolicy putPolicy = new PutPolicy();
            // 如果需要设置为"覆盖"上传(如果云端已有同名文件则覆盖)，请使用 SCOPE = "BUCKET:KEY"
            putPolicy.Scope = bucket + ":" + saveKey;
            //putPolicy.Scope = bucket;
            // 上传策略有效期(对应于生成的凭证的有效期)          
            putPolicy.SetExpires(3600);
            // 上传到云端多少天后自动删除该文件，如果不设置（即保持默认默认）则不删除
            //putPolicy.DeleteAfterDays = 1;

            // 生成上传凭证，参见
            // https://developer.qiniu.com/kodo/manual/upload-token            
            string jstr = putPolicy.ToJsonString();
            string token = Auth.CreateUploadToken(mac, jstr);


            return token;
        }
    }
}