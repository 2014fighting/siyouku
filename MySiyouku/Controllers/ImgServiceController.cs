using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using AutoMapper.QueryableExtensions;
using MySiyouku.Models.ViewModel;
using Siyouku.Model;
using Siyouku.Model.Database;
using Siyouku.Repositorys;
using Siyouku.Repositorys.RepositoryInterface;
using Webdiyer.WebControls.Mvc;

namespace MySiyouku.Controllers
{
    public class ImgServiceController : BaseController
    {
        private readonly IUnitOfWork _unitOfWork;
 
        public ImgServiceController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        // GET: ImgService
        public ActionResult Index()
        {
            return View();
        }
        [HttpGet]
        public ActionResult GetAllPic(int page=1,int size=3)
        {
           var result= _unitOfWork.GetRepository<Picture>().GetEntities().OrderBy(i => i.Id).Skip(size * (page - 1)).Take(size).ProjectTo<PictureVm>();
            var rows = result.ToList();
            var total = result.Count();
            return JsonDate(new {total, rows});
        }
    }
}