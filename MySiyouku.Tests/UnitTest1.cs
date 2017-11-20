using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MySiyouku.App_Start;
using Siyouku.Model;
using Siyouku.Model.Database;
using Siyouku.Repositorys;
using Siyouku.Repositorys.Repository;
using Siyouku.Repositorys.RepositoryInterface;

namespace MySiyouku.Tests
{
    [TestClass]
    public class UnitTest1
    { 
        [TestMethod]
        public void TestMethod1()
        {
            IUnitOfWork _unitOfWork=new UnitOfWork();

            _unitOfWork.GetRepository<Links>().Add(new Links
            {
                CreateTime = new DateTime?(),
                LinkName = "我的链接",
                LinkSort = 1,
                LinkUrl = "www.baidu.com",
                LinkImg = "url"
            }, false);
            _unitOfWork.SaveChanges();
        }

        [TestMethod]
        public void Test()
        {
            string tempString = "abcabsbacaaaa";
            int aCoung = tempString.Count(x => x == 'a');
        }
    }
}
