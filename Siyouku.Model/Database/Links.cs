using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Siyouku.Model.Database
{
     public class Links:BaseField
    {
        public int Id { get; set; }

        [MaxLength(200)]
        public string LinkName { get; set; }

        [MaxLength(500)]
        public string LinkUrl { get; set; }

        [MaxLength(500)]
        public string LinkImg { get; set; }

        public int LinkSort { get; set; }
    }
}
