using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MySiyouku.Areas.Manage.Models.Common;
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
        /// <inheritdoc />
        /// <summary>
        /// 返回JsonResult
        /// </summary>
        /// <param name="data">数据</param>
        /// <param name="contentType">内容类型</param>
        /// <param name="contentEncoding">内容编码</param>
        /// <param name="behavior">行为</param>
        /// <returns>JsonReuslt</returns>
        protected override JsonResult Json(object data, string contentType, System.Text.Encoding contentEncoding, JsonRequestBehavior behavior)
        {
            return new CustomJsonResult
            {
                Data = data,
                ContentType = contentType,
                ContentEncoding = contentEncoding,
                JsonRequestBehavior = behavior,
                FormateStr = "yyyy-MM-dd HH:mm:ss"
            };
        }

        /// <summary>
        /// 返回JsonResult.24         /// </summary>
        /// <param name="data">数据</param>
        /// <param name="behavior">行为</param>
        /// <param name="format">json中dateTime类型的格式</param>
        /// <returns>Json</returns>
        protected JsonResult MyJson(object data, JsonRequestBehavior behavior, string format= "yyyy-MM-dd HH:mm:ss")
        {
            return new CustomJsonResult
            {
                Data = data,
                JsonRequestBehavior = behavior,
                FormateStr = format
            };
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