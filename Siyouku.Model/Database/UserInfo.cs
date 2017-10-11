using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Siyouku.Model.Database
{
    [Table("tbUser")]
    [Serializable]
   public class UserInfo
    {
        /// <summary>
        /// 用户编号
        /// </summary>
        [Key]
        [MaxLength(36)]
        public string UserId { get; set; }
        /// <summary>
        /// 用户名称
        /// </summary>
        [MaxLength(18)]
        public string UserName { get; set; }
        /// <summary>
        /// 用户密码
        /// </summary>
        [MaxLength(18)]
        public string PassWord { get; set; }


        /// <summary>
        /// 手机号
        /// </summary>
        [MaxLength(18)]
        public string Mobile { get; set; }
        /// <summary>
        /// 博客主题
        /// </summary>
        [MaxLength(36)]
        public string BlogName { get; set; }
        /// <summary>
        /// 博客描述
        /// </summary>
        [MaxLength(100)]
        public string Decription { get; set; }
        /// <summary>
        /// 电子邮件
        /// </summary>
        [MaxLength(36)]
        public string Email { get; set; }
        /// <summary>
        /// 注册日期
        /// </summary>
        public DateTime RegisterTime { get; set; }

        /// <summary>
        /// 用户所有文章
        /// </summary>
        public virtual List<Article> Articles { get; set; }

    }
}
