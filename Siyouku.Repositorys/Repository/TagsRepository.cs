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
    public class TagsRepository : BaseReposiory<Tag>, ITagsRepository
    {
        public bool DeleteTag(int id)
        {
           return Delete(GetEntities(i => i.Id == id).FirstOrDefault(), false) > 0;
        }

        public IQueryable<Tag> GetTags()
        {
            return GetEntities(i => true);
        }
    }
}
