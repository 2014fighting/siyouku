using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using Siyouku.Repositorys.RepositoryInterface;

namespace MySiyouku.Areas.Manage.Controllers
{
    public class LinksController : BaseController
    {
        private readonly ILinksRepository _linksRepository;
        public LinksController(ILinksRepository linksRepository)
        {
            _linksRepository = linksRepository;
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


            return MyJson(new { total, rows },JsonRequestBehavior.AllowGet);
        }
        public ActionResult  LinksAdd()
        {
            return View();
        }

    }
}