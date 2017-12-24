using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using AutoMapper;
using MySiyouku.Areas.Manage.Models;
using MySiyouku.Models;
using MySiyouku.Models.AutoMapper.Profiles;
using MySiyouku.Models.ViewModel;
using Siyouku.Model.Database;

namespace MySiyouku.App_Start
{
    public class AutoMapperConfig
    {
        public static void InstallAutoMapper()
        {

            Mapper.Initialize(cfg => {
                cfg.AddProfile<ArticlePf>();
                cfg.CreateMap<Article, ArticleDetail>().ForMember(d=>d.Tags,otp=>otp.Ignore());
                cfg.CreateMap<Applet, AppletVm>();
                cfg.CreateMap<Picture, PictureVm>();
                cfg.CreateMap<Links, LinksDetail>();
                cfg.CreateMap<LinksDetail, Links>()
                    .ForMember(d => d.LinkSort, otp => otp.Ignore())
                    .ForMember(d => d.CreateTime, otp => otp.Ignore())
                    .ForMember(d => d.CreateUser, otp => otp.Ignore())
                    .ForMember(d => d.UpdateTime, otp => otp.Ignore())
                    .ForMember(d => d.UpdateUser, otp => otp.Ignore());
                cfg.CreateMap<Tag, TagsVm>().
                ForMember(d=>d.ArticleCount,otp=>otp.MapFrom(i=>i.ListArticle.Count));
                cfg.CreateMap<Tag, SelectsModel>().
                ForMember(d=>d.name,otp=>otp.MapFrom(i=>i.CatName));
            });

          

            Mapper.AssertConfigurationIsValid();//验证所有映射
        }
    }
}