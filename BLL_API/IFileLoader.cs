using System.Web;

namespace BLL_API
{
    public interface IFileLoader
    {
        string Load(string folder, HttpPostedFileBase file);
    }
}
