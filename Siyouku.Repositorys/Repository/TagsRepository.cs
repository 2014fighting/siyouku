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
       
        public IQueryable<Tag> GetTags()
        {
            return GetEntities(i => true);
        }
    }
}
