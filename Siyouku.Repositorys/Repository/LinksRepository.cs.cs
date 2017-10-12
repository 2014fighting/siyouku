using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Siyouku.Model.Database;
using Siyouku.Repositorys.RepositoryInterface;

namespace Siyouku.Repositorys.Repository
{
    public class LinksRepository : BaseReposiory<Links>, ILinksRepository
    {
        public IQueryable<Links> GetLinks()
        {
            return GetEntities(i => true);
        }
    }
}
