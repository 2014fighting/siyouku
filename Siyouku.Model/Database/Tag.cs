using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Siyouku.Model.Database
{
    public class Tag: BaseField
    {
        public int Id { get; set; }

        public string CatName { get; set; }
         
        public virtual List<Article> ListArticle { get; set; }
    }
}
