using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;

namespace MySiyouku.Models.Common
{
    public static class StringHelper
    {
        public static string  GetStreamMd5(this Stream stream)
        {
            var oMd5Hasher = new System.Security.Cryptography.MD5CryptoServiceProvider();
            byte[] arrbytHashValue = oMd5Hasher.ComputeHash(stream);
            //由以连字符分隔的十六进制对构成的String，其中每一对表示value 中对应的元素；例如“F-2C-4A”
            string strHashData = BitConverter.ToString(arrbytHashValue);
            //替换-
            strHashData = strHashData.Replace("-", "");
            string strResult = strHashData;
            return strResult;
        }
 
        public static string GetMd5HashFromFile(this Stream stream)
        {
            try
            {
              
                System.Security.Cryptography.MD5 md5 = new System.Security.Cryptography.MD5CryptoServiceProvider();
                byte[] retVal = md5.ComputeHash(stream);
                //stream.Close();

                StringBuilder sb = new StringBuilder();
                foreach (byte t in retVal)
                {
                    sb.Append(t.ToString("x2"));
                }
                return sb.ToString();
            }
            catch (Exception ex)
            {
                throw new Exception("GetMd5HashFromFile() fail, error:" +ex.Message);
            }
        }
        /// <summary>
        /// 获取字符串中img的url集合
        /// </summary>
        /// <param name="content">字符串</param>
        /// <returns></returns>
        public static List<string> ChangeContentImg(string content)
        {
            Regex rg = new Regex("src=\"([^\"]+)\"", RegexOptions.IgnoreCase);
            var m = rg.Match(content);
            List<string> imgUrl = new List<string>();
            while (m.Success)
            {
                imgUrl.Add(m.Groups[1].Value); //这里就是图片路径                
                m = m.NextMatch();
            }
            return imgUrl;
        }

        public static object Copy(this object o)
        {
            Type t = o.GetType();
            PropertyInfo[] properties = t.GetProperties();
            Object p = t.InvokeMember("", System.Reflection.BindingFlags.CreateInstance, null, o, null);
            foreach (PropertyInfo pi in properties)
            {
                if (pi.CanWrite)
                {
                    object value = pi.GetValue(o, null);
                    pi.SetValue(p, value, null);
                }
            }
            return p;
        }

    }
}