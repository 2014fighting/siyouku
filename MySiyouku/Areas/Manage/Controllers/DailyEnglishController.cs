using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using MySiyouku.Areas.Manage.Models;
using Siyouku.Model.Database;

namespace MySiyouku.Areas.Manage.Controllers
{
    public class DailyEnglishController : BaseController
    {
        // GET: Manage/DailyEnglish
        public ActionResult Index()
        {
            return View();
        }
        [HttpGet]

        public ActionResult GetDailyEnglishJson(int limit, int offset,   string statu, string search)
        {
            var listTags = Db.DailyEnglish.AsQueryable();
            if (!string.IsNullOrEmpty(search))
                listTags = listTags.Where(i => i.ContentCn.Contains(search));
            var total = listTags.Count();
            var rows = from a in listTags.OrderByDescending(i => i.Id).Skip(offset).Take(limit)
                       select new
                       {
                           a.Id,
                           a.ContentCn,
                           a.ContentEg,
                           a.CreateTime,
                           a.CreateUser
                       };


            return Json(new { total, rows }, JsonRequestBehavior.AllowGet);
        }

        public async Task<ActionResult> DailyEnglishAdd()
        {
            ViewBag.dailyEnglish = await Db.DailyEnglish.Take(20).ToListAsync();
            return View();
        }
        [HttpPost]
        [ValidateInput(false)]
        public async Task<ActionResult> DailyEnglishAdd(DailyEnglish dailyEnglish)
        {
            if (!ModelState.IsValid)
            {
                return View(dailyEnglish);
            }


            Db.DailyEnglish.Add(new DailyEnglish()
            {
                ContentCn = dailyEnglish.ContentCn,
                ContentEg = dailyEnglish.ContentEg,
                CreateTime = DateTime.Now,
                CreateUser = CurUserInfo.UserName

            });
            var r = await Db.SaveChangesAsync() > 0;
            return Json(new ManageJsonResult()
            {
                Code = r ? 0 : 1,
                Msg = r ? "ok" : "SaveChanges失败！"
            });
        }

        public ActionResult DailyEnglishDelete(List<int> ids)
        {
            ids.ForEach(i =>
            {
                var tempx = Db.DailyEnglish.SingleOrDefault(x => x.Id == i);
                if (tempx != null)
                    Db.DailyEnglish.Remove(tempx);
            });
            var r = Db.SaveChanges();
            return Json(new ManageJsonResult()
            {
                Code = r > 0 ? 0 : 1,
                Msg = r > 0 ? "ok" : "SaveChanges失败！"
            });
        }
    }
}