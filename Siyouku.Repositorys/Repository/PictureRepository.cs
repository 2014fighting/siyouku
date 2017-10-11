using System;
using System.Collections.Generic;
using System.Linq;
using Siyouku.Model.Database;
using Siyouku.Repositorys.RepositoryInterface;

namespace Siyouku.Repositorys.Repository
{
    public class PictureRepository : BaseReposiory<Picture>, IPictureRepository
    {
        public int AddImg(Picture pictrue)
        {
           return Add(pictrue, true);
        }

        public bool Exist(string md5Value)
        {
            return GetEntities(i => i.Md5Value == md5Value).Any();
        }

        public Picture GetBymd5(string md5Value)
        {
            return GetEntities(i=>i.Md5Value==md5Value).FirstOrDefault();
        }

        public int  GetMaxId()
        {
           return GetEntities(x=>true).Max(i => i.Id);
        }

        public IQueryable<Picture> GetList()
        {
            return GetEntities(i => true);
        }
    }
}