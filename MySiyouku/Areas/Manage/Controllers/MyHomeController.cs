using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MySiyouku.Areas.Manage.Models;
using Siyouku.Model;
using Siyouku.Model.Database;

namespace MySiyouku.Areas.Manage.Controllers
{
    public class MyHomeController:BaseController
    {
        //readonly SiyoukuContext _siyoukuContext;
        ////构造器注入
        //public MyHomeController(SiyoukuContext siyoukuContext)
        //{
        //    _siyoukuContext = siyoukuContext;
        //}
        // GET: Manage/MyHome
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult IndexLayer()
        {

            return View();
        }

        public ActionResult HelloPage()
        {
            return View();
        }

        public ActionResult Login()
        {
            return View();
        }
        [HttpPost]
        public ActionResult ChkLogin(UserInfo model)
        {

            if (string.IsNullOrEmpty(model.UserName))
            {
                return Json(new ManageJsonResult("用户名不能为空", 1));
            }
            if (string.IsNullOrEmpty(model.PassWord))
            {
                return Json(new ManageJsonResult("登录密码不能为空", 2));
            }

            var admin = Db.UserInfos.SingleOrDefault(i => i.UserName == model.UserName);
            if (admin == null)
            {
                return Json(new ManageJsonResult("登录名不存在", 3));
            }

            if (!model.PassWord.Equals(admin.PassWord))
            {
                return Json(new ManageJsonResult("登录密码错误", 4));
            }
            Session["UserInfo"] = admin;
            Session.Timeout = 60;
            return Json(new ManageJsonResult("ok", 0));
        }
    }
}