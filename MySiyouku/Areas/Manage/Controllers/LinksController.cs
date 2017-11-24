using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using MySiyouku.Areas.Manage.Models;
using Siyouku.Model.Database;
using Siyouku.Repositorys;
using Siyouku.Repositorys.RepositoryInterface;
using DateTime = System.DateTime;

namespace MySiyouku.Areas.Manage.Controllers
{
    public class LinksController : BaseController
    {
        private readonly ILinksRepository _linksRepository;
        private readonly IUnitOfWork _unitOfWork;
        public LinksController(ILinksRepository linksRepository, IUnitOfWork unitOfWork)
        {
            _linksRepository = linksRepository;
            _unitOfWork = unitOfWork;
        }


        // GET: Manage/Links
        public ActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public ActionResult GetLinksJson(int limit, int offset, string departmentname, string statu, string search)
        {
            var listLinks = _linksRepository.GetLinks().AsQueryable();
            if (!string.IsNullOrEmpty(search))
                listLinks = listLinks.Where(i => i.LinkName.Contains(search));
            var total = listLinks.Count();
            var rows = from a in listLinks.OrderByDescending(i => i.Id).Skip(offset).Take(limit)
                       select new
                       {
                           a.Id,
                           a.LinkName,
                           a.LinkImg,
                           a.LinkUrl,
                           a.LinkSort,
                           a.CreateTime,
                           a.CreateUser
                       };


            return MyJson(new { total, rows }, JsonRequestBehavior.AllowGet);
        }
        public ActionResult LinksAdd(LinksDetail link)
        {
            if (!ModelState.IsValid)
            {
                return View(link);
            }
            _unitOfWork.GetRepository<Links>().Add(new Links
            {
                CreateTime = new DateTime?(),
                LinkName = link.LinkName,
                LinkSort = 1,
                LinkUrl = link.LinkUrl,
                LinkImg = link.LinkImg
            });
            var r =   _unitOfWork.SaveChanges() > 0;
             
            return Json(new ManageJsonResult
            {
                Code = r ? 0 : 1,
                Msg = r ? "ok" : "SaveChanges失败！"
            });
        }

        public ActionResult LinkDelete(List<int> ids)
        {
            var links = _unitOfWork.GetRepository<Links>();
            ids.ForEach(i =>
            {
                links.Delete(links.GetEntities(x => x.Id == i));
            });
            var r = _unitOfWork.SaveChanges() > 0;

            return Json(new ManageJsonResult
            {
                Code = r ? 0 : 1,
                Msg = r ? "ok" : "SaveChanges失败！"
            });
        }

    }
}