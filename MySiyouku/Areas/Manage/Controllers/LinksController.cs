using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using AutoMapper;
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
        public ActionResult GetLinksJson(int limit, int offset,
            string departmentname, string statu, string search)
        {
            var listLinks = _linksRepository.GetLinks().AsQueryable();
            if (!string.IsNullOrEmpty(search))
                listLinks = listLinks.Where(i => i.LinkName.Contains(search));
            var total = listLinks.Count();
            var rows = from a in listLinks.OrderByDescending(i => i.LinkSort).Skip(offset).Take(limit)
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
            var maxsort = _unitOfWork.GetRepository<Links>().GetEntities().OrderByDescending(i => i.LinkSort)
                .FirstOrDefault();
            if (!ModelState.IsValid)
            {
                return View(link);
            }
            _unitOfWork.GetRepository<Links>().Add(new Links
            {
                CreateTime = DateTime.Now,
                CreateUser = CurUserInfo.UserName,
                LinkName = link.LinkName,
                LinkSort = maxsort?.LinkSort + 1 ?? 1,
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


        public  ActionResult LinkEdit(int? id)
        {

            if (!id.HasValue)
                return View();
            var result =   _unitOfWork.GetRepository<Links>().GetByKey(id);
  
            return View(Mapper.Map<LinksDetail>(result));
        }
        [HttpPost]
        public async Task<ActionResult> LinkEdit(LinksDetail links)
        {
            if (!ModelState.IsValid)
            {
                return View(links);
            }
            var result = _unitOfWork.GetRepository<Links>().GetByKey(links.Id);
            Mapper.Map(links, result);
            var r = await _unitOfWork.SaveChangesAsync() > 0;

            return Json(new ManageJsonResult
            {
                Code = r ? 0 : 1,
                Msg = r ? "ok" : "SaveChanges失败！"
            });
        }

        public ActionResult GoTop(int? id)
        {
            var link = _unitOfWork.GetRepository<Links>().GetEntities().OrderByDescending(x => x.LinkSort).FirstOrDefault();


            if (!id.HasValue|| link == null)
                   return Json(new ManageJsonResult
            {
                Code =   1,
                Msg = "置顶失败!"
            });

            var result = _unitOfWork.GetRepository<Links>().GetByKey(id);
            result.LinkSort = link.LinkSort + 1;
            var r =   _unitOfWork.SaveChanges() > 0;

            return Json(new ManageJsonResult
            {
                Code = r ? 0 : 1,
                Msg = r ? "ok" : "SaveChanges失败！"
            });
        }

    }
}