using System.IO;
using System.Threading.Tasks;

namespace SportsBook.Interfaces
{
    public interface IAzureBlobService
    {
        string GetTeamLogoImageUri(string LogoImageFileName);
    }
}