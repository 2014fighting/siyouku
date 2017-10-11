using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Siyouku.Model.Database;

namespace Siyouku.Model
{
    public class SiyoukuContext : DbContext
    {
        public IDbSet<Article> Articles { get; set; }
        public IDbSet<UserInfo> UserInfos { get; set; }
        public IDbSet<Tag> Tag { get; set; }
        public IDbSet<DailyEnglish> DailyEnglish { get; set; }
        public IDbSet<Applet> Applet { get; set; }

        public IDbSet<Picture> Picture { get; set; }

        public SiyoukuContext()
            : base("SiyoukuSql")
        {
        }
 
    }
 
}
