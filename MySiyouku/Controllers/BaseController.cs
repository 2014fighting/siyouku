using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MySiyouku.Models.Common;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Siyouku.Model;

namespace MySiyouku.Controllers
{
    public class BaseController: Controller
    {
        protected SiyoukuContext Db { get; } //初始化一个数据库访问对象
        public BaseController()
        {
            if (Db == null)
            {//如果不存在一个数据访问对象,则实例一个出来
                Db = DbFactory.GetCurrentDbContext();
            }
        }
        /// <summary>
        /// 在发生异常时调用
        /// </summary>
        protected override void OnException(ExceptionContext filterContext)
        {
            LogHelper.Error(filterContext.Exception);
            ////if (!filterContext.ExceptionHandled && filterContext.Exception is NullReferenceException)
            //if (!filterContext.ExceptionHandled)
            //{
            //    //获取出现异常的controller名和action名，用于记录
            //    string controllerName = (string)filterContext.RouteData.Values["controller"];
            //    string actionName = (string)filterContext.RouteData.Values["action"];
            //    //定义一个HandErrorInfo，用于Error视图展示异常信息
            //    HandleErrorInfo model = new HandleErrorInfo(filterContext.Exception, controllerName, actionName);

            //    //filterContext.Result = new ViewResult
            //    //{
            //    //    ViewName = this.View().MasterName,
            //    //    ViewData = new ViewDataDictionary<HandleErrorInfo>(model)  //定义ViewData，泛型

            //    //};
            //    filterContext.ExceptionHandled = true;
            //    filterContext.Result = new RedirectResult(Url.Action("ErrorPage", new ViewResult
            //    {
            //        ViewName = this.View().MasterName,
            //        ViewData = new ViewDataDictionary<HandleErrorInfo>(model)  //定义ViewData，泛型

            //    }));


            //}
            //base.OnException(filterContext);
        }

        /// <summary>
        /// 返回处理过时间的json
        /// </summary>
 
        /// <param name="date"></param>
        /// <returns></returns>
        protected ContentResult JsonDate(object date)
        {
            var timeConverter = new IsoDateTimeConverter { DateTimeFormat = "yyyy-MM-dd HH:mm:ss" };

            return Content(JsonConvert.SerializeObject(date, Formatting.Indented, timeConverter));
        }

        public ActionResult ErrorPage()
        {
            return View();
        }
    }
}