using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using AutoMapper.QueryableExtensions;
using MySiyouku.Models.ViewModel;
using Siyouku.Model;
using Siyouku.Repositorys.RepositoryInterface;
using Webdiyer.WebControls.Mvc;

namespace MySiyouku.Controllers
{
    public class ImgServiceController : BaseController
    {
        private readonly IPictureRepository _pictureRepository;
        private readonly SiyoukuContext _siyoukuContext;
        public ImgServiceController(SiyoukuContext siyoukuContext, IPictureRepository pictureRepository)
        {
            _siyoukuContext = siyoukuContext;
            _pictureRepository = pictureRepository;
        }
        // GET: ImgService
        public ActionResult Index()
        {
            return View();
        }
        [HttpGet]
        public ActionResult GetAllPic(int page=1,int size=3)
        {
           var result= _pictureRepository.GetList().OrderBy(i => i.Id).Skip(size * (page - 1)).Take(size).ProjectTo<PictureVm>();
            var rows = result.ToList();
            var total = result.Count();

            return JsonDate(new {total, rows});
        }
    }
}