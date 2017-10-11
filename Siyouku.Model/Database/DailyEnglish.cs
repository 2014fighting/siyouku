    using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Siyouku.Model.Database
{
    public class DailyEnglish: BaseField
    {
        public int Id { get; set; }

        public string ContentEg { get; set; }
        public string ContentCn { get; set; }
    }
}
