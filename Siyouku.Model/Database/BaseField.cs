using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Siyouku.Model.Database
{
    public class BaseField
    {
        public  DateTime? CreateTime { get; set; }
        public  DateTime? UpdateTime { get; set; }

        public string  CreateUser { get; set; }
        public string  UpdateUser { get; set; }
    }
}
