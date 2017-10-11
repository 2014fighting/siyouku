using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Siyouku.Model;
using Siyouku.Repositorys.RepositoryInterface;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using MySiyouku.Models.ViewModel;

namespace MySiyouku.Controllers
{
    public class AppletController :BaseController
    {
        readonly SiyoukuContext _siyoukuContext;
        readonly IUserInfoRepository _userInfoRepository;
        //构造器注入
        public AppletController(SiyoukuContext siyoukuContext, IUserInfoRepository userInfoRepository)
        {
            _siyoukuContext = siyoukuContext;
            _userInfoRepository = userInfoRepository;
        }

        // GET: APPlet
        public ActionResult Index(string KeyWord,string category)
        {
            var applet = _siyoukuContext.Applet.AsQueryable();
            if (!string.IsNullOrEmpty(KeyWord))
            {
                applet = applet.Where(i => i.Name.Contains(KeyWord));
            }
            if (!string.IsNullOrEmpty(category))
            {
                applet = applet.Where(i => i.CategoryName.Contains(category));
            }
            ViewBag.category = string.IsNullOrEmpty(category)?"全部":category;
            return View(applet.OrderBy(i=>i.Id).Take(20).ProjectTo<AppletVm>().ToList());
        }

        public ActionResult GetAppletJson(int pagesize, int pagenumber,string category)
        {
            var list = Db.Applet.AsQueryable();
            if(!string.IsNullOrEmpty(category)&& category != "全部")
            {

                list = list.Where(i => i.CategoryName.Contains(category));
            }
            var total = list.Count();
            var rows =  list.OrderBy(i => i.Id)
                    .Skip(pagesize * pagenumber - pagesize).Take(pagesize).ProjectTo<AppletVm>();
            return JsonDate(new { total, rows });
        }
    }
}