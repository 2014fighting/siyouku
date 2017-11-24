using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Siyouku.Model;
using Siyouku.Model.Database;
using Siyouku.Repositorys.RepositoryInterface;

namespace Siyouku.Repositorys.Repository
{
    public class ArticleRepository : BaseReposiory<Article>, IArticleRepository
    {
      
        public IQueryable<Article> GetArticles()
        {
            return GetEntities(i => true);
        }
 
        bool IArticleRepository.DeleteArt(int artId)
        {
          return  Delete(GetEntities(i => i.Id == artId).FirstOrDefault())>0;
        }

       
        bool IArticleRepository.InsertArt(Article artInfo)
        {
           return Add(artInfo)>0;
        }
    }
}
