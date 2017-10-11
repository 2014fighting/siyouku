using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MySiyouku.Areas.Manage.Models
{
    public class ManageJsonResult
    {
        public ManageJsonResult()
        {
           
        }
        public ManageJsonResult(string msg ,int code)
        {
            this.Code = code;
            this.Msg = msg;
        }
        public string Msg { get; set; } = "";
        public int Code { get; set; } = 0;
    }
}