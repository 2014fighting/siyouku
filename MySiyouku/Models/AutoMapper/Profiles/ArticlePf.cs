using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using AutoMapper;
using MySiyouku.Areas.Manage.Models;
using Siyouku.Model.Database;

namespace MySiyouku.Models.AutoMapper.Profiles
{
    public class ArticlePf: Profile
    {
        //5.0以后的版本采用构造函数的方式
        public ArticlePf()
        {
            CreateMap<ArticleDetail, Article>()
                .ForMember(i=>i.CollectId, otp=>otp.Ignore())
                .ForMember(i=>i.CollectTime, otp=>otp.Ignore())
                .ForMember(i=>i.CollectUser, otp=>otp.Ignore())
                .ForMember(i=>i.User, otp=>otp.Ignore())
                .ForMember(i=>i.Tags, otp=>otp.Ignore())
                .ForMember(i=>i.Pviews, otp=>otp.Ignore())
                .ForMember(i=>i.PublishTime, otp=>otp.Ignore())
                .ForMember(i=>i.UserId, otp=>otp.Ignore())
                ;
            // Use CreateMap... Etc.. here (Profile methods are the same as configuration methods)
        }
    }
}