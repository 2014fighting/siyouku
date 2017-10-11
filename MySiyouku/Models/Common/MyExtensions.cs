using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MySiyouku.Models.Common
{
    public static class MyExtensions
    {
        public static List<int> StrToInts(this string s)
        { 
            var res=new List<int>();
            foreach (var s1 in s.Split(','))
            {
                res.Add(Convert.ToInt32(s1));
            }
            return res;
        }
    }
}