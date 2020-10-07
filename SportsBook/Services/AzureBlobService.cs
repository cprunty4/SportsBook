using System.IO;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Microsoft.WindowsAzure.Storage;
using SportsBook.Interfaces;

namespace SportsBook.Services
{
    public class AzureBlobService : IAzureBlobService
    {
        private readonly IConfiguration _configuration;
        private string _connectionString;

        private string _blobContainerName;

        public AzureBlobService(IConfiguration configuration)
        {
            _configuration = configuration;
            _connectionString = configuration.GetSection("AzureStorage").GetValue<string>("ConnectionString");
            _blobContainerName = configuration.GetSection("AzureStorage").GetValue<string>("BlobContainerName");                         
        }

        public Task<Stream> GetTeamLogoImageBlobStreamAsync(string LogoImageFileName)
        {
            bool created = CloudStorageAccount.TryParse(_connectionString, out CloudStorageAccount storageAccount);


            throw new System.NotImplementedException();
        }
    }
}