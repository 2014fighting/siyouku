using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Siyouku.Model;
using Siyouku.Model.Database;
using Siyouku.Repositorys.RepositoryInterface;

namespace Siyouku.Repositorys.Repository
{
    public class UserInfoRepository :BaseReposiory<UserInfo> ,IUserInfoRepository
    {
   

        public void DeleteUser(string userId)
        {
            Delete(SiyoukuContext.UserInfos.Find(userId));
        }

     
        public UserInfo GetUserInfoById(string userId)
        {
            return GetByKey(userId);
        }

        public IQueryable<UserInfo> GetUserInfos()
        {
            return GetEntities(i=>true);
        }

        public void InsertUser(UserInfo userInfo)
        {
            Add(userInfo);
        }

 
        public void UpdateUser(UserInfo userInfo)
        {
            Update(userInfo);
        }
    }
}
