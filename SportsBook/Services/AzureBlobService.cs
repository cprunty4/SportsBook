using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Blob;
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

        public string GetTeamLogoImageUri(string LogoImageFileName)
        {
            string absoluteUri = string.Empty;
            // Call GetBlobFromContainer and return it's absolute uri

            CloudBlockBlob cloudBlockBlob = this.GetBlobInContainer(_blobContainerName, LogoImageFileName);

            return cloudBlockBlob.Uri.AbsoluteUri;
        }

        private CloudBlockBlob GetBlobInContainer(string blobContainerName, string logoImageFileName)
        {
            // TODO See stackoverflow https://stackoverflow.com/questions/38398520/displaying-images-from-azure-blob-container
            // throw new NotImplementedException();
            bool created = CloudStorageAccount.TryParse(_connectionString, out CloudStorageAccount storageAccount);

            CloudBlobClient cloudBlobClient = storageAccount.CreateCloudBlobClient();

            CloudBlobContainer cloudBlobContainer = cloudBlobClient.GetContainerReference(_blobContainerName);

            CloudBlockBlob cloudBlockBlob = cloudBlobContainer.GetBlockBlobReference($"images/{logoImageFileName}");

            return cloudBlockBlob;

        }
    }
}