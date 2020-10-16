using System.IO;
using System.Threading.Tasks;

namespace SportsBook.Interfaces
{
    public interface IAzureBlobService
    {
        string GetImageUri(string LogoImageFileName);

    }
}