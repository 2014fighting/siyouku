using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Siyouku.Model;
using Siyouku.Repositorys.Repository;
using Siyouku.Repositorys.RepositoryInterface;

namespace Siyouku.Repositorys
{
    public class UnitOfWork : IUnitOfWork
    {
        //不应该在这里放上下文
        protected readonly SiyoukuContext _context;
        private bool disposed = false;
        private Dictionary<Type, object> repositories;
        public UnitOfWork()
        {
            _context = DbFactory.GetCurrentDbContext();
        }
        

        public int ExecuteSqlCommand(string sql, params object[] parameters) => _context.Database.ExecuteSqlCommand(sql, parameters);

        public IQueryable<TEntity> FromSql<TEntity>(string sql, params object[] parameters) where TEntity : class
        {
           return _context.Set<TEntity>().SqlQuery(sql, parameters).AsQueryable();
        }


        public IBaseRepository<T> GetRepository<T>() where T : class
        {
            if (repositories == null)
            {
                repositories = new Dictionary<Type, object>();
            }

            var type = typeof(T);
            if (!repositories.ContainsKey(type))
            {
                repositories[type] = new BaseReposiory<T>(_context);
            }

            return (IBaseRepository<T>)repositories[type];
        }

        public int SaveChanges()
        {
            return _context.SaveChanges();
        }

        public async Task<int> SaveChangesAsync()
        {
            return await _context.SaveChangesAsync();
        }

        public void Dispose()
        {
            Dispose(true);

            GC.SuppressFinalize(this);
        }
        protected virtual void Dispose(bool disposing)
        {
            if (!disposed)
            {
                if (disposing)
                {
                    // clear repositories
                    repositories?.Clear();

                    // dispose the db context.
                    _context.Dispose();
                }
            }

            disposed = true;
        }
    }
}
