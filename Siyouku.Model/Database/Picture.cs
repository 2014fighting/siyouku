using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Siyouku.Model.Database
{
    [Table("tbPicture")]
    [Serializable]
   public  class Picture: BaseField
    {
        [Key]
        public int Id { get; set; }

        public string ImgName { get; set; }

        public string Md5Value { get; set; }
    }
}
