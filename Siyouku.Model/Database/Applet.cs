using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Siyouku.Model.Database
{
    [Table("tbApplet")]
    [Serializable]
    public class Applet: BaseField
    {
        [Key]
        public int Id { get; set; }

        public string CategoryName { get; set; }

        public string Name { get; set; }

        public string SaomiaoUrl { get; set; }

        public string Summary { get; set; }

        public string LogoUrl { get; set; }

        public double SortNum { get; set; } = 0;

        public string Content { get; set; }
    }
}
