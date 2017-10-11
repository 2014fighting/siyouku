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
    public class TagsController : BaseController
    {
        // GET: Manage/Tags
        public ActionResult Index()
        {
            return View();
        }
        [HttpGet]

        public ActionResult GetTagsJson(int limit, int offset, string departmentname, string statu, string search)
        {
            var listTags = Db.Tag.AsQueryable();
            if (!string.IsNullOrEmpty(search))
                listTags = listTags.Where(i => i.CatName.Contains(search));
            var total = listTags.Count();
            var rows = from a in listTags.OrderByDescending(i => i.Id).Skip(offset).Take(limit)
                       select new
                       {
                           a.Id,
                           a.CatName,
                           a.ListArticle.Count,
                           a.CreateTime,
                           a.CreateUser
                       };


            return Json(new {total, rows }, JsonRequestBehavior.AllowGet);
        }

        public async Task<ActionResult> TagsAdd()
        {
            ViewBag.tags = await Db.Tag.Take(20).ToListAsync();
            return View();
        }
        [HttpPost]
        [ValidateInput(false)]
        public async Task<ActionResult> TagsAdd(Tag tags)
        {
            if (!ModelState.IsValid)
            {
                return View(tags);
            }

          
            Db.Tag.Add(new Tag()
            {
                CatName = tags.CatName,
                CreateTime =DateTime.Now,
                CreateUser = CurUserInfo.UserName

            });
            var r = await Db.SaveChangesAsync() > 0;
            return Json(new ManageJsonResult()
            {
                Code = r ? 0 : 1,
                Msg = r ? "ok" : "SaveChanges失败！"
            });
        }
  
        public ActionResult TagsDelete(List<int> ids)
        {
            ids.ForEach(i =>
            {
                var tempx = Db.Tag.SingleOrDefault(x => x.Id == i);
                if(tempx!=null)
                Db.Tag.Remove(tempx);
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