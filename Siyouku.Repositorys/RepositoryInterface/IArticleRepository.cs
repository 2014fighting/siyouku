using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Siyouku.Model.Database;

namespace Siyouku.Repositorys.RepositoryInterface
{
    public interface IArticleRepository
    {
        IQueryable<Article> GetArticles();
        bool InsertArt(Article artInfo);
        bool DeleteArt(int artId);
        bool Commit();
        Task<int> CommitSync();
    }
}
