using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using Siyouku.Model;
using Siyouku.Repositorys.RepositoryInterface;

namespace Siyouku.Repositorys.Repository
{

    /// <summary>
    /// 抽象基本的增删改查
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public  class BaseReposiory<T>:IBaseRepository<T> where T : class
    {
        //不应该在这里放上下文
        protected readonly SiyoukuContext SiyoukuContext;

        protected BaseReposiory()
        {
            SiyoukuContext = DbFactory.GetCurrentDbContext();
        }
        public T GetByKey(object key)
        {
            return SiyoukuContext.Set<T>().Find(key);
        }
        public bool IsExists(int key)
        {
            return SiyoukuContext.Set<T>().Any();
        }
        /// <summary>
        /// 取得所要的实体
        /// </summary>
        /// <param name="whereLambda">lambda,查询全部直接给true</param>
        /// <returns>实体的Queryable对象</returns>
        public IQueryable<T> GetEntities(Expression<Func<T, bool>> whereLambda)
        {
            return SiyoukuContext.Set<T>().Where(whereLambda);
        }
        /// <summary>
        /// 执行sql 语句
        /// </summary>
        /// <param name="commonText"></param>
        /// <param name="parameters"></param>
        /// <returns></returns>
        public IQueryable<T> ExecProcGetSet(string commonText, params object[] parameters)
        {
            var tempList = SiyoukuContext.Set<T>().SqlQuery(commonText, parameters);
            return tempList.AsQueryable();
        }

        public int Add(T entity, bool isSave=true)
        {
            SiyoukuContext.Set<T>().Add(entity);
            return isSave ? SiyoukuContext.SaveChanges() : 0;
        }
        public int Update(T entity, bool isSave=true)
        {
            SiyoukuContext.Set<T>().Attach(entity);
            SiyoukuContext.Entry<T>(entity).State = EntityState.Modified;
            return isSave ? SiyoukuContext.SaveChanges() : 0;
        }

        /// <summary>
        /// 删除实体
        /// </summary>
        /// <param name="entity">实体</param>
        /// <param name="isSave">是否立即保存</param>
        /// <returns>在“isSave”为True时返回受影响的对象的数目，为False时直接返回0</returns>
        public int Delete(T entity, bool isSave=true)
        {
            SiyoukuContext.Set<T>().Remove(entity);
            return isSave ? SiyoukuContext.SaveChanges() : 0;
        }

        /// <summary>
        /// 批量删除实体
        /// </summary>
        /// <param name="entities">实体集合</param>
        /// <returns>受影响的对象的数目</returns>
        public int Delete(IEnumerable<T> entities)
        {
            SiyoukuContext.Set<T>().RemoveRange(entities);
            return SiyoukuContext.SaveChanges();
        }

        public bool Commit()
        {
             return SiyoukuContext.SaveChanges()>0;
        }

        public async Task<int> CommitSync()
        {
            return await SiyoukuContext.SaveChangesAsync();
        }
    }
}
