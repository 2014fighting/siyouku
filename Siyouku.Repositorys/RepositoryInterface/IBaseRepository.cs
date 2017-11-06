using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Siyouku.Repositorys.RepositoryInterface
{
    public interface IBaseRepository<T> where T : class
    {
        T GetByKey(object key);

        bool IsExists(int key);

        IQueryable<T> GetEntities(Expression<Func<T, bool>> whereLambda=null);

        IQueryable<T> ExecProcGetSet(string commonText, params object[] parameters);

        int Add(T entity, bool isSave);
        int Update(T entity, bool isSave);

        int Delete(T entity, bool isSave);
        int Delete(IEnumerable<T> entities);

        bool Commit();
        Task<int> CommitSync();
    }
}
