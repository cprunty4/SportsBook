using System.IO;
using System.Threading.Tasks;

namespace SportsBook.Interfaces
{
    public interface IAzureBlobService
    {
        Task<Stream> GetTeamLogoImageBlobStreamAsync(string LogoImageFileName);
    }
}