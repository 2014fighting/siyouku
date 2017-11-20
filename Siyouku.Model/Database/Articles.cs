using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Siyouku.Model.Database
{
    [Table("tbArticle")]
    [Serializable]
    public class Article
    {
        /// <summary>
        /// 文章编号
        /// </summary>
        [Key]
        public int Id { get; set; }
        /// <summary>
        /// 采集编号
        /// </summary>
        public string CollectId { get; set; }
        /// <summary>
        /// 采集时间
        /// </summary>
        public string CollectTime { get; set; }
        /// <summary>
        /// 采集作者
        /// </summary>
        public string CollectUser { get; set; }
        /// <summary>
        /// 文章标题
        /// </summary>
        [MaxLength(360)]
        public string Title { get; set; }
        /// <summary>
        /// 文章内容
        /// </summary>
        public string Content { get; set; }
        /// <summary>
        /// 文章简介
        /// </summary>
        public string Summary { get; set; }
        /// <summary>
        /// 浏览量
        /// </summary>
        public int? Pviews { get; set; }

        /// <summary>
        /// 文章类别
        /// </summary>
        [MaxLength(36)]
        public string CategoryId { get; set; } 
        
        /// <summary>
        /// 图片
        /// </summary>
        [MaxLength(300)]
        public string Img { get; set; }

        /// <summary>
        /// 发布时间
        /// </summary>
        public DateTime? PublishTime { get; set; }
        /// <summary>
        /// 最后修改时间
        /// </summary>
        public DateTime? LastMdifyTime { get; set; }
        /// <summary>
        /// 用户编号
        /// </summary>
        [MaxLength(36)]
        public string UserId { get; set; }
        /// <summary>
        /// 文章的所属者
        /// </summary>
        public virtual UserInfo User { get; set; }

        /// <summary>
        /// 标签
        /// </summary>
        public virtual List<Tag> Tags { get; set; }
    }
}
