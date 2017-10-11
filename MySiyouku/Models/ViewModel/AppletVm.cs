using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MySiyouku.Models.ViewModel
{
    public class AppletVm
    {
        public int Id { get; set; }

        public string CategoryName { get; set; }

        public string Name { get; set; }

        public string SaomiaoUrl { get; set; }

        public string Summary { get; set; }

        public string LogoUrl { get; set; }
    }
}