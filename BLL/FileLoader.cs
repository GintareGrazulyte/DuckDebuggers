using System.Web;
using BLL_API;
using System.IO;

namespace BLL
{
    public class FileLoader : IFileLoader
    {
        public string Load(string folder, HttpPostedFileBase file)
        {
            if(file == null || file.ContentLength <= 0)
            {
                return null;
            }
            string fileName = Path.GetFileName(file.FileName);
            string path = Path.Combine(folder, fileName);
            file.SaveAs(path);
            return fileName;
        }
    }
}
