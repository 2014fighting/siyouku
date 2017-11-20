using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Siyouku.Repositorys.RepositoryInterface;

namespace Siyouku.Repositorys
{
    public interface IUnitOfWork: IDisposable
    {
        IBaseRepository<T> GetRepository<T>() where T : class;

        int SaveChanges();
        Task<int> SaveChangesAsync();

        int ExecuteSqlCommand(string sql, params object[] parameters);

        IQueryable<TEntity> FromSql<TEntity>(string sql, params object[] parameters) where TEntity : class;
    }
}
