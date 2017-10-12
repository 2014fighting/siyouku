using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data.Entity;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Mvc;
using HtmlAgilityPack;
using MySiyouku.Models;
using MySiyouku.Models.Common;
using MySiyouku.Models.NetModel;
using Siyouku.Model.Database;
using Webdiyer.WebControls.Mvc;
using System.Diagnostics;
using AutoMapper.QueryableExtensions;
using MySiyouku.Models.ViewModel;
using Siyouku.Model;
using Siyouku.Repositorys.Repository;
using Siyouku.Repositorys.RepositoryInterface;

namespace MySiyouku.Controllers
{
    public class HomeController : BaseController
    {
        readonly IUserInfoRepository _userInfoRepository;
        readonly ITagsRepository _tagsRepository;
        readonly IArticleRepository _articleRepository;
        private readonly ILinksRepository _linksRepository;
        

        //构造器注入
        public HomeController(IUserInfoRepository userInfoRepository,ITagsRepository tagsRepository, IArticleRepository articleRepository, ILinksRepository linksRepository)
        {
            _userInfoRepository = userInfoRepository;
            _tagsRepository = tagsRepository;
            _articleRepository = articleRepository;  
                _linksRepository =linksRepository;

        }

        // GET: Home
        public ActionResult Index(ArticleSearch search)
        {
            var x= _userInfoRepository.GetUserInfos();
    

            ViewBag.users = x.First().UserName;
            var result = _articleRepository.GetArticles();
            if (!string.IsNullOrWhiteSpace(search.KeyWord))
            {
                result = result.Where(i => i.Title.Contains(search.KeyWord));
            }

            if (!string.IsNullOrWhiteSpace(search.TagName))
            {
                result = result.Where(i => i.Tags.Any(a=>a.CatName==search.TagName));
            }
            var pageData = result.OrderByDescending(a => a.Id).Select(i => new ArticleListDto()
            {
                Id = i.Id,
                Img = i.Img,
                CollectTime = i.CollectTime,
                CollectUser = i.CollectUser,
                CategoryId = i.CategoryId,
                Pviews = i.Pviews,
                Title = i.Title,
                Summary = i.Summary,
                Tags=i.Tags,
            }).ToPagedList(search.Page, search.Rows);
            if (Request.IsAjaxRequest())
                return PartialView("_ArticleList", pageData);
            return View(pageData);
        }

        public ActionResult Detail(int id)
        {
            var result = _articleRepository.GetArticles().FirstOrDefault(i => i.Id == id);
            if(result==null) throw new Exception("not find！！");
            if (string.IsNullOrEmpty(result.Pviews))
            {
                result.Pviews = "0";
            }
            result.Pviews = (Convert.ToInt32(result.Pviews.Replace("万","0000"))+1).ToString();
            string  kw = string.Empty;
            result.Tags.ForEach(i =>
            {
                kw = kw+ ","+i.CatName;
            });
            ViewBag.Keywords = kw.Trim(',');
            _articleRepository.Commit();
            return View(result);
        }
        public ActionResult MyDemoAction()
        {
            var list = new List<ArticleListDto>();
            HtmlDocument hd = new HtmlDocument();
            //http://36kr.com/p/5056891.html
            HtmlWeb htmlWeb = new HtmlWeb();
            HtmlDocument htmlDoc = htmlWeb.Load(@"http://www.cnblogs.com/");

            //选择博客园首页文章列表
            htmlDoc.DocumentNode.SelectNodes("//div[@id='post_list']/div[@class='post_item']").
                AsParallel().ToList().ForEach(ac =>
                {
                    //抓取图片，因为有空的，所以拿变量存起来
                    HtmlNode node = ac.SelectSingleNode(".//p[@class='post_item_summary']/a/img");

                    list.Add(new ArticleListDto
                    {
                        Url = ac.SelectSingleNode(".//a[@class='titlelnk']").Attributes["href"].Value,
                        Title = ac.SelectSingleNode(".//a[@class='titlelnk']").InnerText,
                        //图片如果为空，显示默认图片
                        Img =
                            node == null
                                ? VirtualPathUtility.ToAbsolute("~/Content/img/IMG_1039.PNG")
                                : node.Attributes["src"].Value,
                        Content = ac.SelectSingleNode(".//p[@class='post_item_summary']").InnerText
                    });
                });
            return View();
        }

        public ActionResult TagList()
        {
                return View(_tagsRepository.GetTags().OrderBy(i => i.Id).ProjectTo<TagsVm>().ToList());
        }

        public ActionResult About()
        {
            return View();
        }



        public ActionResult Messages()
        {
            return View();
        }
        public ActionResult GetToP(int count)
        {
            var x = _articleRepository.GetArticles().OrderByDescending(i => i.Pviews).Take(count).ToList();
            return View(x);
        }
        public string DownLoadImg(string url)
        {
            if (string.IsNullOrEmpty(url) || url == "undefined")
                return "";
            Bitmap img = null;
            HttpWebRequest req;
            HttpWebResponse res = null;
            var resultUrl = string.Empty;
            var saveUrl = string.Empty;
            try
            {
                System.Uri httpUrl = new System.Uri(url);
                req = (HttpWebRequest)(WebRequest.Create(httpUrl));
                req.Timeout = 180000; //设置超时值10秒
                req.UserAgent = "XXXXX";
                req.Accept = "XXXXXX";
                req.Method = "GET";
                res = (HttpWebResponse)(req.GetResponse());
                var imgstream = res.GetResponseStream();

                img = new Bitmap(imgstream); //获取图片流    
                var tempsp = url.Split('/');
                resultUrl = "~/Content/img/" + tempsp.LastOrDefault();
                saveUrl = Server.MapPath(resultUrl);

                img.Save(saveUrl); //随机名
            }

            catch (Exception ex)
            {
                string aa = ex.Message;
            }
            finally
            {
                res.Close();
            }
            return VirtualPathUtility.ToAbsolute(resultUrl);
        }

        public ActionResult CrawlerToDb()
        {
            Stopwatch watch = new Stopwatch();
            watch.Start();

            var taglist = Db.Tag.ToList();
            var result = new BaijiaArticel();
            var icount = 0;
            for (int j = 0; j <20; j++)
            {
                string url =
                    $"http://baijia.baidu.com/ajax/labellatestarticle?page={j}&pagesize=20&prevarticalid=765376&flagtogether=1&labelid=3";

                var str = url.GetPostPage(null);
                if (str != null)
                {
                    result = str.JsonConvert<BaijiaArticel>();
                }
                if (result.errno.Equals(0))
                {
                    result.data.list.ForEach(i =>
                    {
                        if (!Db.Articles.Any(x => x.CollectId == i.ID))
                        {
                            var listTag = new List<Tag>();
                            i.m_label_names.ForEach(w => {
                                var tag = taglist.FirstOrDefault<Tag>(t => t.CatName == w.m_name);
                                if (tag == null)
                                    listTag.Add(new Tag() { CatName = w.m_name, CreateTime = DateTime.Now, UpdateTime = DateTime.Now });
                                else
                                    listTag.Add(tag);
                            });
                            var x = new Article()
                            {
                                Img = DownLoadImg(i.m_image_url),
                                Title = i.m_title,
                                CategoryId = "百度百家",
                                Summary = i.m_summary,
                                Content = GetContent(i.m_display_url),
                                CollectTime = i.m_create_time,
                                CollectUser = i.m_writer_name,
                                Pviews = i.hotcount,
                                CollectId = i.ID,
                                UserId = "1",
                                PublishTime = DateTime.Now,
                                Tags = listTag

                            };
                            if (!string.IsNullOrEmpty(x.Content) && !string.IsNullOrEmpty(x.Img))
                            {
                                Db.Articles.Add(x);
                                icount++;
                            }

                        }

                    });
                    Db.SaveChanges();
                }

            }

            watch.Stop();

            var lts = watch.ElapsedMilliseconds;
            return Json(new { result = "ok", TimeSpan = lts, count = icount});
        }

        public string GetContent(string url)
        {
            try
            {
                var list = new List<ArticleListDto>();
                HtmlDocument hd = new HtmlDocument();
                //http://36kr.com/p/5056891.html
                HtmlWeb htmlWeb = new HtmlWeb();
                HtmlDocument htmlDoc = htmlWeb.Load(url);
                HtmlNode htmlNode = htmlDoc.DocumentNode.SelectSingleNode("//div[@id='page']");
                //选择博客园首页文章列表

                var x = htmlNode.SelectSingleNode(".//div[@class='article-detail']").InnerHtml;
                var sss = ChangeContentImg(x);
                return sss;
            }
            catch (Exception)
            {

                return "";
            }

        }
        
        /// <summary>
        /// 获取字符串中img的url集合
        /// </summary>
        /// <param name="content">字符串</param>
        /// <returns></returns>
        public string ChangeContentImg(string content)
        {
            var result = content;
            Regex rg = new Regex("src=\"([^\"]+)\"", RegexOptions.IgnoreCase);
            var m = rg.Match(content);

            while (m.Success)
            {
                var url = DownLoadImg(m.Groups[1].Value);
                result = result.Replace(m.Groups[1].Value, url);
                m = m.NextMatch();
            }
            return result;
        }


        public ActionResult Chat()
        {
            return View();
        }


        public ActionResult GetLink(int count)
        {
            var x = _linksRepository.GetLinks().OrderBy(i => i.LinkSort).Take(count).ToList();
            return View(x);
        }

    }
}