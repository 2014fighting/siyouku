using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Siyouku.Model;
using Siyouku.Model.Database;

namespace MySiyouku.Areas.Manage.Controllers
{
    public class BaseController : Controller
    {


        public UserInfo CurUserInfo => (UserInfo)Session["UserInfo"];
        // GET: Manage/Base
        protected SiyoukuContext Db { get; } //初始化一个数据库访问对象
        public BaseController()
        {
            if (Db == null)
            {//如果不存在一个数据访问对象,则实例一个出来
                Db = DbFactory.GetCurrentDbContext();
            }
        }

        /// <summary>
        /// 返回处理过的时间的Json字符串
        /// </summary>
        /// <param name="date"></param>
        /// <returns></returns>
        public ContentResult JsonDate(object date)
        {
            var timeConverter = new IsoDateTimeConverter { DateTimeFormat = "yyyy-MM-dd HH:mm:ss" };
            return Content(JsonConvert.SerializeObject(date, Formatting.Indented, timeConverter));

        }

        protected override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            base.OnActionExecuting(filterContext);

            //访问的Action
            string action = filterContext.RouteData.Values["Action"].ToString();
            //访问的Controller
            string controller = filterContext.RouteData.Values["Controller"].ToString();
            if (action == "ChkLogin" || action == "Login")
                return;
            //校验用户是否已经登录
            if (CurUserInfo == null)
            {
                //用户没有登录
                filterContext.HttpContext.Response.Redirect(Url.Action("Login","MyHome"));
                //                Response.Write("<script>window.parent.location='/Admin/Home/Login';</script>");
                //                Response.End();
                //LogHelper.Log("用户未登录跳转");
                
            }
        }
    }
}