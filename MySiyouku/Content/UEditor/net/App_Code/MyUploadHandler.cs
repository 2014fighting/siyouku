using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;
using Qiniu.IO;
using Qiniu.IO.Model;
using Qiniu.Util;
using Siyouku.Model.Database;
using Siyouku.Repositorys.RepositoryInterface;

namespace MySiyouku.Content.UEditor.net.App_Code
{
    /// <summary>
    /// MyUploadHandler 的摘要说明
    /// </summary>
    public class MyUploadHandler : Handler
    {

        public UploadConfig UploadConfig { get; private set; }
        public UploadResult Result { get; private set; }
        public IPictureRepository _pictureRepository;
        public MyUploadHandler(HttpContext context, UploadConfig config, IPictureRepository pictureRepository)
            : base(context)
        {
            _pictureRepository = pictureRepository;
            this.UploadConfig = config;
            this.Result = new UploadResult() { State = UploadState.Unknown };
        }

        public override void Process()
        {

            byte[] uploadFileBytes = null;
            string uploadFileName = null;
            var accessKey = "HNXvIcHqurHJAEDnVI4v2_qDGDvVlAXhipa6uVX-";
            var secretKey = "pQOAZutqyXr679mnDclCnIeaHgF0w3U-eWDFZJsp";
            HttpPostedFile file = Request.Files[UploadConfig.UploadFieldName];
            Stream myStream = file.InputStream;

            uploadFileName = file.FileName;

             var fileLen = file.ContentLength;
            //// 读取文件的 byte[]   
            byte[] bytes = new byte[fileLen];
           
            //  var temps= myStream.Read(bytes, 0, fileLen);

            if (!CheckFileType(uploadFileName))
            {
                Result.State = UploadState.TypeNotAllow;
                WriteResult();
                return;
            }
            if (!CheckFileSize(file.ContentLength))
            {
                Result.State = UploadState.SizeLimitExceed;
                WriteResult();
                return;
            }

            var md5value = Md5With32(bytes);
            if (_pictureRepository.Exist(md5value))
            {
                Result.Url = _pictureRepository.GetBymd5(md5value).ImgName;
                Result.State = UploadState.Success;
                WriteResult();
                return;
            }
            var maxid = _pictureRepository.GetMaxId();
            var imgname = "img" + (maxid + 1)+".jpg";
            _pictureRepository.AddImg(new Picture()
            {
                ImgName = imgname,
                CreateUser ="system",
                CreateTime = DateTime.Now,
                Md5Value = md5value
            });

           
            // 生成(上传)凭证时需要使用此Mac
            // 这个示例单独使用了一个Settings类，其中包含AccessKey和SecretKey
            // 实际应用中，请自行设置您的AccessKey和SecretKey
            Mac mac = new Mac(accessKey, secretKey);
            string bucket = "siyouku";
            //string saveKey = uploadFileName;
            string saveKey = imgname;

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
            // putPolicy.DeleteAfterDays = 1;

            // 生成上传凭证，参见
            // https://developer.qiniu.com/kodo/manual/upload-token            
            string jstr = putPolicy.ToJsonString();
            string token = Auth.CreateUploadToken(mac, jstr);
            try
            {
                // 请不要使用UploadManager的UploadStream方法，因为此流不支持查找(无法获取Stream.Length)
                // 请使用FormUploader或者ResumableUploader的UploadStream方法
                FormUploader fu = new FormUploader();
                var result = fu.UploadStream(myStream, saveKey, token);
                var x = Newtonsoft.Json.JsonConvert.DeserializeObject<QiniuResult>(result.Text);
                Result.Url = x.key;
                Result.State = UploadState.Success;
            }
            catch (Exception e)
            {
                Result.State = UploadState.FileAccessError;
                Result.ErrorMessage = e.Message;
            }
            finally
            {
                WriteResult();
            }

        }

        private void WriteResult()
        {
            this.WriteJson(new
            {
                state = GetStateMessage(Result.State),
                url = Result.Url,
                title = Result.OriginFileName,
                original = Result.OriginFileName,
                error = Result.ErrorMessage
            });
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
        private string GetStateMessage(UploadState state)
        {
            switch (state)
            {
                case UploadState.Success:
                    return "SUCCESS";
                case UploadState.FileAccessError:
                    return "文件访问出错，请检查写入权限";
                case UploadState.SizeLimitExceed:
                    return "文件大小超出服务器限制";
                case UploadState.TypeNotAllow:
                    return "不允许的文件格式";
                case UploadState.NetworkError:
                    return "网络错误";
            }
            return "未知错误";
        }

        private bool CheckFileType(string filename)
        {
            var fileExtension = Path.GetExtension(filename).ToLower();
            return UploadConfig.AllowExtensions.Select(x => x.ToLower()).Contains(fileExtension);
        }

        private bool CheckFileSize(int size)
        {
            return size < UploadConfig.SizeLimit;
        }
    }

    public class UploadConfig
    {
        /// <summary>
        /// 文件命名规则
        /// </summary>
        public string PathFormat { get; set; }

        /// <summary>
        /// 上传表单域名称
        /// </summary>
        public string UploadFieldName { get; set; }

        /// <summary>
        /// 上传大小限制
        /// </summary>
        public int SizeLimit { get; set; }

        /// <summary>
        /// 上传允许的文件格式
        /// </summary>
        public string[] AllowExtensions { get; set; }

        /// <summary>
        /// 文件是否以 Base64 的形式上传
        /// </summary>
        public bool Base64 { get; set; }

        /// <summary>
        /// Base64 字符串所表示的文件名
        /// </summary>
        public string Base64Filename { get; set; }
    }

    public class UploadResult
    {
        public UploadState State { get; set; }
        public string Url { get; set; }
        public string OriginFileName { get; set; }

        public string ErrorMessage { get; set; }
    }

    public enum UploadState
    {
        Success = 0,
        SizeLimitExceed = -1,
        TypeNotAllow = -2,
        FileAccessError = -3,
        NetworkError = -4,
        Unknown = 1,
    }

    public class QiniuResult
    {
        public string hash { get; set; }
        public string key { get; set; }
    }
}