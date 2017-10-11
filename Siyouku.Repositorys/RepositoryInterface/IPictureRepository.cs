using System.Linq;
using System.Security.Cryptography.X509Certificates;
using Siyouku.Model.Database;

namespace Siyouku.Repositorys.RepositoryInterface
{
    public interface IPictureRepository
    {
        int GetMaxId();
        int AddImg(Picture pictrue);

        bool Exist(string md5Value);

        Picture GetBymd5(string md5Value);

        IQueryable<Picture> GetList();
    }
}