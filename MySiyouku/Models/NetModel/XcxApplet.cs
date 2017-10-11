using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MySiyouku.Models.NetModel
{
    public class XcxApplet
    {
        public int id { get; set; }

        public string categoryName { get; set; }

        public string name { get; set; }

        public string saomaUrl { get; set; }

        public string sum { get; set; }

        public string logoUrl { get; set; }
    }

    public class Result
    {
        public List<XcxApplet> dataList { get; set; }
        public string category { get; set; }
        public int  status { get; set; }
        public int pageNum { get; set; }
    }
    
}