using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using MySiyouku.Content.UEditor.net.App_Code;
using MySiyouku.Models.Common;
using MySiyouku.Models.NetModel;
using Qiniu.IO;
using Qiniu.IO.Model;
using Qiniu.Util;
using Siyouku.Model;
using Siyouku.Model.Database;

namespace MySiyouku.Controllers
{
    public class WenqingController : BaseController
    {
        readonly SiyoukuContext _siyoukuContext;
        //构造器注入
        public WenqingController(SiyoukuContext siyoukuContext)
        {
            _siyoukuContext = siyoukuContext;
        }
        // GET: Wenqing
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Daziji()
        {
            return View();
        }
 
        public ActionResult WeTalk()
        {    
            return View();
        }

        public ActionResult Zhaopianqiang()
        {
            return View();
        }

        public ActionResult EnglishStudy()
        {
            return View(_siyoukuContext.DailyEnglish.OrderByDescending(i=>i.Id).Take(3).ToList());
        }
        public ActionResult GetDailyEnglishJson(int pagesize, int pagenumber)
        {
            var list= Db.DailyEnglish.AsQueryable();
             
            var total = list.Count();
            var rows = from a in list.OrderByDescending(i => i.Id)
                       .Skip(pagesize*pagenumber-pagesize).Take(pagesize)
                       select new
                       {
                           a.Id,
                           a.ContentCn,
                           a.ContentEg,
                           a.CreateTime,
                           a.CreateUser
                       };


            return JsonDate(new { total, rows });
        }
 
        [HttpGet]
        public string  QiniuUplodtest()
        {


            var accessKey = "HNXvIcHqurHJAEDnVI4v2_qDGDvVlAXhipa6uVX-";
            var secretKey = "pQOAZutqyXr679mnDclCnIeaHgF0w3U-eWDFZJsp";

            // 生成(上传)凭证时需要使用此Mac
            // 这个示例单独使用了一个Settings类，其中包含AccessKey和SecretKey
            // 实际应用中，请自行设置您的AccessKey和SecretKey
            Mac mac = new Mac(accessKey, secretKey);
            string bucket = "siyouku";
            string saveKey = "$(etag)";

            // 使用前请确保AK和BUCKET正确，否则此函数会抛出异常(比如code612/631等错误)
            Qiniu.Common.Config.AutoZone(accessKey, bucket, false);


            // 上传策略，参见 
            // https://developer.qiniu.com/kodo/manual/put-policy
            PutPolicy putPolicy = new PutPolicy();
            // 如果需要设置为"覆盖"上传(如果云端已有同名文件则覆盖)，请使用 SCOPE = "BUCKET:KEY"
            // putPolicy.Scope = bucket + ":" + saveKey;
            putPolicy.Scope = bucket;
            putPolicy.SaveKey = saveKey;
            // 上传策略有效期(对应于生成的凭证的有效期)          
            putPolicy.SetExpires(3600);
            // 上传到云端多少天后自动删除该文件，如果不设置（即保持默认默认）则不删除
            putPolicy.DeleteAfterDays = 1;

            // 生成上传凭证，参见
            // https://developer.qiniu.com/kodo/manual/upload-token            
            string jstr = putPolicy.ToJsonString();
            string token = Auth.CreateUploadToken(mac, jstr);
            try
            {
                string url = "http://images2015.cnblogs.com/blog/208266/201509/208266-20150913215146559-472190696.jpg";
                var wReq = System.Net.WebRequest.Create(url) as System.Net.HttpWebRequest;
                var resp = wReq.GetResponse() as System.Net.HttpWebResponse;
 
                using (var stream =resp.GetResponseStream())
                {
                    //MemoryStream destination = new MemoryStream();
                    //stream.CopyTo(destination);
                    //var md5Val = destination.GetStreamMd5();

                    // 请不要使用UploadManager的UploadStream方法，因为此流不支持查找(无法获取Stream.Length)
                    // 请使用FormUploader或者ResumableUploader的UploadStream方法
                    FormUploader fu = new FormUploader();
                    var result = fu.UploadStream(stream, null, token);
                   // Console.WriteLine(result);
                }
            }
            catch (Exception ex)
            {
               // Console.WriteLine(ex);
            }


            return "ok";
        }


        public ActionResult GetUploadToken()
        {
 
            var accessKey = "HNXvIcHqurHJAEDnVI4v2_qDGDvVlAXhipa6uVX-";
            var secretKey = "pQOAZutqyXr679mnDclCnIeaHgF0w3U-eWDFZJsp";

            // 生成(上传)凭证时需要使用此Mac
            // 这个示例单独使用了一个Settings类，其中包含AccessKey和SecretKey
            // 实际应用中，请自行设置您的AccessKey和SecretKey
            Mac mac = new Mac(accessKey, secretKey);
            string bucket = "siyouku";
            string saveKey = "1.jpg";


            // 使用前请确保AK和BUCKET正确，否则此函数会抛出异常(比如code612/631等错误)
            Qiniu.Common.Config.AutoZone(accessKey, bucket, false);


            // 上传策略，参见 
            // https://developer.qiniu.com/kodo/manual/put-policy
            PutPolicy putPolicy = new PutPolicy();
            // 如果需要设置为"覆盖"上传(如果云端已有同名文件则覆盖)，请使用 SCOPE = "BUCKET:KEY"
            // putPolicy.Scope = bucket + ":" + saveKey;
            putPolicy.Scope = bucket;
            // 上传策略有效期(对应于生成的凭证的有效期)          
            putPolicy.SetExpires(3600);
            // 上传到云端多少天后自动删除该文件，如果不设置（即保持默认默认）则不删除
            putPolicy.DeleteAfterDays = 1;

            // 生成上传凭证，参见
            // https://developer.qiniu.com/kodo/manual/upload-token            
            string jstr = putPolicy.ToJsonString();
            string token = Auth.CreateUploadToken(mac, jstr);


            return Json(new { token }, JsonRequestBehavior.AllowGet);
        }


        public ActionResult Aini()
        {
            return View();
        }
      
        public string  QiniuUplod(string imgurl)
        {
              
            var accessKey = "HNXvIcHqurHJAEDnVI4v2_qDGDvVlAXhipa6uVX-";
            var secretKey = "pQOAZutqyXr679mnDclCnIeaHgF0w3U-eWDFZJsp";

            // 生成(上传)凭证时需要使用此Mac
            // 这个示例单独使用了一个Settings类，其中包含AccessKey和SecretKey
            // 实际应用中，请自行设置您的AccessKey和SecretKey
            Mac mac = new Mac(accessKey, secretKey);
            string bucket = "siyouku";
            string saveKey = imgurl.Substring(imgurl.LastIndexOf('/')+1,imgurl.Length- imgurl.LastIndexOf('/')-1);


            // 使用前请确保AK和BUCKET正确，否则此函数会抛出异常(比如code612/631等错误)
            Qiniu.Common.Config.AutoZone(accessKey, bucket, false);


            // 上传策略，参见 
            // https://developer.qiniu.com/kodo/manual/put-policy
            PutPolicy putPolicy = new PutPolicy();
            // 如果需要设置为"覆盖"上传(如果云端已有同名文件则覆盖)，请使用 SCOPE = "BUCKET:KEY"
             putPolicy.Scope = bucket + ":" + saveKey;
            putPolicy.Scope = bucket;
            // 上传策略有效期(对应于生成的凭证的有效期)          
            putPolicy.SetExpires(3600);
            // 上传到云端多少天后自动删除该文件，如果不设置（即保持默认默认）则不删除
            //putPolicy.DeleteAfterDays = 1;

            // 生成上传凭证，参见
            // https://developer.qiniu.com/kodo/manual/upload-token            
            string jstr = putPolicy.ToJsonString();
            string token = Auth.CreateUploadToken(mac, jstr);
            try
            {
                
                var wReq = System.Net.WebRequest.Create(imgurl) as System.Net.HttpWebRequest;
                var resp = wReq.GetResponse() as System.Net.HttpWebResponse;
                using (var stream = resp.GetResponseStream())
                {
                    // 请不要使用UploadManager的UploadStream方法，因为此流不支持查找(无法获取Stream.Length)
                    // 请使用FormUploader或者ResumableUploader的UploadStream方法
                    FormUploader fu = new FormUploader();
                    var result = fu.UploadStream(stream, saveKey, token);
                    var x = Newtonsoft.Json.JsonConvert.DeserializeObject<QiniuResult>(result.Text);
                    return $"http://img.siyouku.cn/{x.key}";
                }
            }
            catch (Exception ex)
            {
                return "";
            }

 
        }
        public ActionResult GetxcxList()
        {
            Stopwatch watch = new Stopwatch();//监控抓取耗时
            watch.Start();
            //https://www.xcxdh666.com/pageList.htm?pageNum=0  dataList
            var result = new Result();

            for (int j = 0; j <54; j++)
            {
                string url =
                    $"https://www.xcxdh666.com/pageList.htm?pageNum={j}";

                var str = url.GetPostPage(null);//HttpWebRequest 请求页面
                if (str != null)
                {
                    result = str.JsonConvert<Result>();  //string   的序列化扩展方法
                }

                result.dataList.ForEach(i =>
                {
                    if (!Db.Applet.Any(x => x.Name == i.name))//判断重复插入
                    {
                        var x = new Applet()
                        {
                            CategoryName = string.IsNullOrEmpty(i.categoryName) ? "其它" : i.categoryName,
                            Name = i.name,
                            SaomiaoUrl = QiniuUplod($"http://img.xcxdh666.com/wxappnav/{i.saomaUrl}"),
                            Summary = i.sum,
                            LogoUrl = QiniuUplod($"http://img.xcxdh666.com/wxappnav/{i.logoUrl}"),
                            SortNum = j,
                            CreateUser = "wenqing",
                            CreateTime = DateTime.Now

                        };
                        Db.Applet.Add(x);
                    }

                });

                Db.SaveChanges();


            }
            watch.Stop();
            return Content("爬取完成！本次请求总共耗时："+ watch.ElapsedMilliseconds);
        }


        public ActionResult Kydemo()
        {
            return View();
        }

        public ActionResult MyInfo()
        {
            return View();
        }

        public ActionResult Missyou()
        {
            return View();
        }

        public ActionResult Animation()
        {
            return View();
        }

        public ActionResult Daojishi()
        {
            return View();
        }

        public ActionResult Nav()
        {
            return View();
        }
    }
}