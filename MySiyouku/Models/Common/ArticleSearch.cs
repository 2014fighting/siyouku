using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MySiyouku.Models.Common
{
    public class ArticleSearch:BaseQuery
    {
        public string KeyWord { get; set; }

        public string TagName { get; set; }
    }
}