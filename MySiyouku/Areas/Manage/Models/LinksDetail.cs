using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace MySiyouku.Areas.Manage.Models
{
    public class LinksDetail
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "链接名称必填")]
        [MaxLength(200)]
        public string LinkName { get; set; }
        [Required(ErrorMessage = "链接地址必填")]
        [MaxLength(500)]
        public string LinkUrl { get; set; }
        [Required(ErrorMessage = "链接图片必填")]
        [MaxLength(500)]
        public string LinkImg { get; set; }

        public int LinkSort { get; set; }
    }
}