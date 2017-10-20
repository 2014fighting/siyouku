using System;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MySiyouku.App_Start;
using Siyouku.Model;
using Siyouku.Model.Database;
using Siyouku.Repositorys.Repository;
using Siyouku.Repositorys.RepositoryInterface;

namespace MySiyouku.Tests
{
    [TestClass]
    public class UnitTest1
    {
        //readonly IUserInfoRepository _userInfoRepository;
        //readonly IArticleRepository _articleRepository; 

        //readonly ITagsRepository _tagsRepository;
        ////构造器注入
        //public UnitTest1(IUserInfoRepository userInfoRepository,
        //    ITagsRepository tagsRepository, IArticleRepository articleRepository)
        //{
        //    IocConfig.InstallIoc();
        //    _userInfoRepository = userInfoRepository;
        //    _articleRepository = articleRepository;
        //    _tagsRepository = tagsRepository;

        //}
        [TestMethod]
        public void TestMethod1()
        {
            ITagsRepository _tagsRepository=new TagsRepository();
            IArticleRepository _articleRepository=new ArticleRepository();
            _tagsRepository.DeleteTag(3016);
            _articleRepository.DeleteArt(6857);
            //_articleRepository.Commit();
            var SiyoukuContext = DbFactory.GetCurrentDbContext();
            SiyoukuContext.SaveChanges();

        }
    }
}
