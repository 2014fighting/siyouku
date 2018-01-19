using MySiyouku.Areas.Manage.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.Owin.Security.Provider;
using MySiyouku.Models;
using Siyouku.Model.Database;
using Siyouku.Repositorys;
using Siyouku.Repositorys.RepositoryInterface;

namespace MySiyouku.Areas.Manage.Controllers
{
    public class ArticleController : BaseController
    {
        private readonly IUnitOfWork _unitOfWork;
        readonly IUserInfoRepository _userInfoRepository;
        readonly ITagsRepository _tagsRepository;
        readonly IArticleRepository _articleRepository;

        //构造器注入
        public ArticleController(IUserInfoRepository userInfoRepository,
            ITagsRepository tagsRepository,
            IArticleRepository articleRepository, IUnitOfWork unitOfWork)
        {
            _userInfoRepository = userInfoRepository;
            _tagsRepository = tagsRepository;
            _articleRepository = articleRepository;
            _unitOfWork = unitOfWork;
        }
        // GET: Manage/Article
        public ActionResult Index()
        {
            return View();
        }
        [HttpGet]

        public ActionResult GetArticleJson(int limit, int offset, string departmentname, string statu,string search)
        {
            var listRes = _unitOfWork.GetRepository<Article>().GetEntities();
            if (!string.IsNullOrEmpty(search))
                listRes = listRes.Where(i => i.Title.Contains(search));
            var total = listRes.Count();
            var rows = from a in listRes.OrderByDescending(i=>i.Id).Skip(offset).Take(limit)
                       select new
                       {
                           a.Id,
                           a.Title,
                           a.Content,
                           a.CategoryId,
                           a.CollectId,
                           a.Img,
                           a.Summary
                       };


            return MyJson(new { total,  rows }, JsonRequestBehavior.AllowGet);
        }

        public  ActionResult ArticleAdd()
        {
            return View();
        }
        [HttpPost]
        [ValidateInput(false)]
        public async Task<ActionResult> ArticleAdd(ArticleDetail article)
        {
            #region 验证
            if (!ModelState.IsValid)
            {
                return View(article);
            }

            var lisTags = new List<Tag>();
            foreach (var s in article.Tags)
            {
                lisTags.Add(Db.Tag.FirstOrDefault(i => i.Id == s));
            }
            if (lisTags.Count == 0)
            {

                return Json(new ManageJsonResult()
                {
                    Code = 2,
                    Msg = "请选择标签！"
                });
            }


            if (string.IsNullOrEmpty(article.Img))
                return Json(new ManageJsonResult()
                {
                    Code = 3,
                    Msg = "请上传缩略图！！"
                }); 
            #endregion
            _articleRepository.InsertArt(new Article
            {
                IsShow =true,
                Title = article.Title,
                Img = article.Img,
                Content = article.Content,
                PublishTime = DateTime.Now,
                Pviews =0,
                CategoryId = article.CategoryId,
                Summary = article.Summary,
                UserId = CurUserInfo.UserId,
                Tags = lisTags,
                CollectTime = DateTime.Now.ToString("G"),
                CollectUser = CurUserInfo.UserName

            });
            
            var r =await _unitOfWork.SaveChangesAsync()>0;
           
           
            return Json(new ManageJsonResult()
            {
                Code = r ? 0 : 1,
                Msg = r? "ok" : "SaveChanges失败！"
            });
        }

        public async Task<ActionResult> ArticleEdit(int? id)
        {
 
            if (!id.HasValue)
                return View();
            var result = await _articleRepository.GetArticles().FirstOrDefaultAsync(i => i.Id == id);
            var article = Mapper.Map<ArticleDetail>(result);
            var temptags = string.Empty;
            result.Tags.ForEach(i =>
            {
                temptags += i.Id + ",";
            });
            ViewBag.tags = temptags.TrimEnd(',');
            return View(article);
        }
        [HttpPost]
        [ValidateInput(false)]
        public async Task<ActionResult> ArticleEdit(ArticleDetail article)
        {
            if (!ModelState.IsValid)
            {
                return View(article);
            }
          
            if (article.Tags.Count == 0)
            {
                return Json(new ManageJsonResult()
                {
                    Code = 2,
                    Msg = "请勾选标签！"
                });
            }

            if (string.IsNullOrEmpty(article.Img))
                return Json(new ManageJsonResult()
                {
                    Code = 3,
                    Msg = "请上传缩略图！！"
                });
 
            var tempart = _articleRepository.GetArticles().FirstOrDefault(i => i.Id == article.Id);
            if(tempart==null)
                return Json(new ManageJsonResult()
                {
                    Code = 404,
                    Msg = "没有找到数据！"
                });
            tempart.Tags.Clear();
            Mapper.Map(article, tempart);


            var lisTags = new List<Tag>();
            foreach (var s in article.Tags)
            {
                lisTags.Add(_tagsRepository.GetTags().FirstOrDefault(i=>i.Id==s));
            }
            tempart.Tags = lisTags;
            tempart.LastMdifyTime=DateTime.Now;

             var r = await _unitOfWork.SaveChangesAsync()> 0;

            return Json(new ManageJsonResult()
            {
                Code = r ? 0 : 1,
                Msg = r ? "ok" : "SaveChanges失败！"
            });
        }
        public ActionResult ArticleDelete(List<int>ids)
        {
            ids.ForEach( i =>
            {
                _articleRepository.DeleteArt(i);
            });
            var r= _unitOfWork.SaveChanges()>0;
 
            return Json(new ManageJsonResult()
            {
                Code = r ? 0 : 1,
                Msg = r? "ok" : "SaveChanges失败！"
            });
        }
        public ActionResult ShowOrHide(List<int> ids,bool isShow)
        {
            var article = _unitOfWork.GetRepository<Article>();

            ids.ForEach(i =>
            {
                var firstOrDefault = article.GetEntities().FirstOrDefault(x => x.Id == i);
                if(firstOrDefault!=null)
                firstOrDefault.IsShow = isShow;
                _unitOfWork.SaveChanges();
            });
            
            return Json(new ManageJsonResult()
            {
                Code =0,
                Msg =  "ok"
            });
        }

        public ActionResult GetListjson()
        {
            var lstRes = _tagsRepository.GetTags().AsQueryable();
            return Json(new { items = lstRes.ProjectTo<SelectsModel>().ToList(), total_count = lstRes.Count() }, JsonRequestBehavior.AllowGet);
        }
    }
}