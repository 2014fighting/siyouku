using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Siyouku.Model.Database;

namespace Siyouku.Repositorys.RepositoryInterface
{
     public  interface IUserInfoRepository
    {
        IQueryable<UserInfo> GetUserInfos();
        UserInfo GetUserInfoById(string userId);
        void InsertUser(UserInfo userInfo);
        void DeleteUser(string userId);
        void UpdateUser(UserInfo userInfo);

    }
}
