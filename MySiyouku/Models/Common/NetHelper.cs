using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Web;
using System.Web.Script.Serialization;

namespace MySiyouku.Models.Common
{
    public static class NetHelper
    {
        
            public static string GetPage(this string posturl)
            {
                Encoding encoding = Encoding.UTF8;
                try
                {
                    // 设置参数
                    var request = WebRequest.Create(posturl) as HttpWebRequest;
                    if (request == null) return string.Empty;
                    var cookieContainer = new CookieContainer();
                    request.CookieContainer = cookieContainer;
                    request.AllowAutoRedirect = true;
                    request.Method = "GET";
                    request.ContentType = "application/x-www-form-urlencoded";
                    //发送请求并获取相应回应数据
                    var response = request.GetResponse() as HttpWebResponse;

                    if (response == null) return string.Empty;

                    //直到request.GetResponse()程序才开始向目标网页发送Post请求
                    Stream instream = response.GetResponseStream();
                    if (instream == null) return string.Empty;
                    var sr = new StreamReader(instream, encoding);
                    string content = sr.ReadToEnd();
                    string err = string.Empty;
                    return content;
                }
                catch (Exception ex)
                {
                    string err = ex.Message;
                    return string.Empty;
                }
            }

            public static string GetPostPage(this string posturl, string postData)
            {
                Encoding encoding = Encoding.UTF8;
                byte[] data = null;
                if (!string.IsNullOrEmpty(postData)) data = encoding.GetBytes(postData);
                try
                {
                    // 设置参数
                    var request = WebRequest.Create(posturl) as HttpWebRequest;
                    if (request == null) return string.Empty;
                    var cookieContainer = new CookieContainer();
                    request.CookieContainer = cookieContainer;
                    request.AllowAutoRedirect = true;
                    request.Method = "POST";
                    request.ContentType = "application/x-www-form-urlencoded";
                    if (data != null)
                    {
                        request.ContentLength = data.Length;
                        Stream outstream = request.GetRequestStream();
                        outstream.Write(data, 0, data.Length);
                        outstream.Close();
                    }
                    //发送请求并获取相应回应数据
                    var response = request.GetResponse() as HttpWebResponse;
                    if (response == null) return string.Empty;

                    //直到request.GetResponse()程序才开始向目标网页发送Post请求
                    Stream instream = response.GetResponseStream();
                    if (instream == null) return string.Empty;
                    var sr = new StreamReader(instream, encoding);
                    //返回结果网页（html）代码
                    string content = sr.ReadToEnd();
                    string err = string.Empty;
                    //Response.Write(content);
                    return content;
                }
                catch (Exception ex)
                {
                    string err = ex.Message;
                    return string.Empty;
                }
            }

            public static T JsonConvert<T>(this string data)
            {
                var json = new JavaScriptSerializer();
                return json.Deserialize<T>(data);
            }

            //public static T PostFile(string url, string file, NameValueCollection nvc = null)
            //{
            //    HttpHelper helper = new HttpHelper();
            //    string content = helper.PostStream(url, new string[] { file }, nvc);
            //    VerifyErrorCode(content);

            //    T result = JsonConvert.DeserializeObject<T>(content);
            //    return result;
            //}

            //public static String GetStreamMd5(this Stream stream)
            //{
            //    var oMd5Hasher = new System.Security.Cryptography.MD5CryptoServiceProvider();
            //    byte[] arrbytHashValue = oMd5Hasher.ComputeHash(stream);
            //    //由以连字符分隔的十六进制对构成的String，其中每一对表示value 中对应的元素；例如“F-2C-4A”
            //    string strHashData = BitConverter.ToString(arrbytHashValue);
            //    //替换-
            //    strHashData = strHashData.Replace("-", "");
            //    string strResult = strHashData;
            //    return strResult;
            //}

            public static string HandleAuthUrl(this string url)
            {
                //
                string appid = ConfigurationManager.AppSettings["appId"];
                return string.Format(
                    "https://open.weixin.qq.com/connect/oauth2/authorize?appid={0}&redirect_uri={1}&response_type=code&scope=snsapi_base&state=1&connect_redirect=1#wechat_redirect", appid, url);

            }

            /// <summary>
            /// 获取所有的快递公司
            /// </summary>
            /// <returns></returns>
            public static string GetKuaidi()
            {
                var getUrl =
                    string.Format("https://route.showapi.com/64-20?showapi_appid=10432&showapi_timestamp={0}&showapi_sign=0c05274d3ca7468e8e019466a8491cf9", DateTime.Now.ToString("yyyyMMddHHmmss"));
                return GetPage(getUrl);
            }

            public static string GetKuaidiNo(this string comp, string expCode)
            {
                var url = string.Format(
                    "https://route.showapi.com/64-19?com={1}&nu={0}&showapi_appid=10432&showapi_timestamp={2}&showapi_sign=0c05274d3ca7468e8e019466a8491cf9",
                    expCode, comp, DateTime.Now.ToString("yyyyMMddHHmmss"));
                return url.GetPage();
            }

            public static bool IsImages(this string ext)
            {
                if (ext.Equals(".jpg", StringComparison.OrdinalIgnoreCase)) return true;
                if (ext.Equals(".png", StringComparison.OrdinalIgnoreCase)) return true;
                if (ext.Equals(".bmp", StringComparison.OrdinalIgnoreCase)) return true;
                if (ext.Equals(".jpeg", StringComparison.OrdinalIgnoreCase)) return true;
                return false;
            }

            public static string GetKuaidiInfo100(this string comp, string expCode)
            {
                string str = "http://wap.kuaidi100.com/wap_result.jsp?rand=20120517&id={0}&fromWeb=null&&postid={1}";

                return string.Format(str, comp, expCode);
            }
 
    }
}