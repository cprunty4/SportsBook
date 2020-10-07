using System.IO;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.WindowsAzure.Storage.Blob;

namespace SportsBook.AzurePageBlobs.Interfaces
{
    public interface IAzurePageBlob
    {
        bool IsInitialized { get; }
        void Initialize();
        Task<BlobProperties> GetPropertiesAsync(CancellationToken? cancellationToken = null);
        Task CreateLargePageBlobAsync(Stream sourceStreamToWrite, CancellationToken? cancellationToken = null);
        Task<BlobSegmentInfo> AppendAsync(string stringToWrite, CancellationToken? cancellationToken = null);
        Task<BlobSegmentInfo> AppendAsync(Stream sourceStreamToWrite, CancellationToken? cancellationToken = null);
        Task<string> DownloadToStringAsJsonArrayAsync(CancellationToken? cancellationToken = null);
        Task<string> DownloadToStringAsync(CancellationToken? cancellationToken = null);
        Task<Stream> DownloadToStreamAsJsonArrayAsync(CancellationToken? cancellationToken = null);
        Task<Stream> DownloadToStreamAsync(CancellationToken? cancellationToken = null);

        Task<string> DownloadToStringAsync(int byteOffset, int bytesToRead,
            CancellationToken? cancellationToken = null);

        Task<bool> ExistsAsync(CancellationToken? cancellationToken = null);
        Task<bool> DeleteIfExistsAsync(CancellationToken? cancellationToken = null);
    }
}
