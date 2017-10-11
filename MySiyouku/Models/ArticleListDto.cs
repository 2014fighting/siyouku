using Siyouku.Model.Database;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MySiyouku.Models
{
    public class ArticleListDto
    {
        public int Id { get; set; }

        /// <summary>
        /// 链接
        /// </summary>
        public string Url { get; set; }
        /// <summary>
        /// 标题
        /// </summary>
        public string Title { get; set; }
        /// <summary>
        /// 头像地址
        /// </summary>
        public string Img { get; set; }
        /// <summary>
        /// 正文内容
        /// </summary>
        public string Content { get; set; }

        public string Summary { get; set; }
        public string  Pviews { get; set; }
        public string CollectTime { get; set; }
        public string CollectUser { get; set; }
        public string CategoryId { get; set; }

        public List<Tag> Tags { get; set; }

    }
}