using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Blob;
using System.IO;
using System.Text;
using System.Threading;
using SportsBook.AzurePageBlobs.Internal;
using Microsoft.WindowsAzure.Storage.RetryPolicies;


namespace SportsBook.AzurePageBlobs
{
    public class AzurePageBlob
    {
        public const int ByteCountInPage = 512;
        public const int DefaultBufferSize = ByteCountInPage * 2 * 1024;


        protected static readonly byte[] FullPageOfNullChars = new byte[ByteCountInPage];

        protected readonly CloudStorageAccount CloudStorageAccount;
        protected readonly string ContainerName;
        protected readonly string BlobPath;
        protected readonly Encoding Encoding;

        protected CloudBlobClient CloudBlobClient;
        protected CloudBlobContainer BlobContainer;
        protected BlobRequestOptions RequestOptions;
        protected OperationContext OperationContext;

        protected bool VerifiedThatContainerExists;
        protected bool VerifiedThatBlobExists;

        public bool IsInitialized { get; protected set; }


        /// <summary>
        /// Azure Page blobs only support 4MB limit
        /// https://docs.microsoft.com/en-us/azure/storage/common/storage-scalability-targets#azure-blob-storage-scale-targets
        /// </summary>
        private const int AzurePageBlobUploadBlockLimit = 4194304;

        public AzurePageBlob(CloudStorageAccount cloudStorageAccount, string containerName, string blobPath,
            Encoding encoding = null)
        {
            this.CloudStorageAccount = cloudStorageAccount;
            this.ContainerName = containerName;
            this.BlobPath = blobPath;
            this.Encoding = encoding ?? Encoding.UTF8;
            this.IsInitialized = false;
        }

        public virtual void Initialize()
        {
            if (this.IsInitialized)
                return;

            this.RequestOptions = CreateBlobRequestOptions();
            this.OperationContext = CreateOperationContext();

            this.CloudBlobClient = this.CloudStorageAccount.CreateCloudBlobClient();
            this.BlobContainer = this.CloudBlobClient.GetContainerReference(this.ContainerName);

            this.IsInitialized = true;
        }

        /// <summary>
        /// Returns a <see cref="BlobProperties"/> instance with data about the page blob including length.
        /// </summary>
        /// <param name="cancellationToken">Optional cancellation token</param>
        /// <returns>A <see cref="BlobProperties"/> instance with data about the page blob including length.</returns>
        public virtual async Task<BlobProperties> GetPropertiesAsync(CancellationToken? cancellationToken = null)
        {
            if (cancellationToken == null)
                cancellationToken = CancellationToken.None;

            bool blobExists = await this.ExistsAsync(cancellationToken.Value);
            if (!blobExists)
                return null;

            CloudPageBlob pageBlob = this.BlobContainer.GetPageBlobReference(this.BlobPath);
            return await this.GetPropertiesAsync(pageBlob, cancellationToken.Value);
        }

        /// <summary>
        /// Creates a Page Blob for large files (>4MB) and for newly created Page blobs.  If an error occurs during upload, an exception is thrown and the file is deleted
        /// </summary>
        /// <param name="sourceStreamToWrite"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public virtual async Task CreateLargePageBlobAsync(Stream sourceStreamToWrite, CancellationToken? cancellationToken = null)
        {
            try
            {
                if (cancellationToken == null)
                    cancellationToken = CancellationToken.None;

                EnsureInitialized();

                var pageBlob = this.BlobContainer.GetPageBlobReference(this.BlobPath);

                var pageBlobSizeInfo = await this.PreparePageBlobAsync(pageBlob, (int)sourceStreamToWrite.Length, cancellationToken.Value);

                if (pageBlobSizeInfo.UsedBytesFromLastPage.Length != 0)
                {
                    throw new Exception("Page Blob is already in use and cannot be appended to or file corruption might be caused");
                }

                // ensure the last page of the blob is filled to 512 page size withh null characters
                int updatedUsedToFullPageByteCount = CalculateBlobLengthThroughEndOfPage((int)sourceStreamToWrite.Length);
                var streamToFillOutLastPage = new MemoryStream(FullPageOfNullChars, 0, (int)(updatedUsedToFullPageByteCount - sourceStreamToWrite.Length));

                var mergedStream = new MemoryStream(updatedUsedToFullPageByteCount);
                // never cross the streams ;)
                sourceStreamToWrite.CopyTo(mergedStream);
                streamToFillOutLastPage.CopyTo(mergedStream);

                // reset position for upload
                mergedStream.Position = 0;

                await pageBlob.UploadFromStreamAsync(mergedStream, CreateAccessCondition(pageBlob), RequestOptions, OperationContext, cancellationToken.Value);
            }
            catch (Exception e)
            {
                // delete blob since we don't want to append data and corrupt it
                await DeleteIfExistsAsync();

                throw;
            }
        }

        /// <summary>
        /// Appends a <see cref="String"/> to the end of the page blob and creates it if it doesn't already exist.
        /// </summary>
        /// <param name="stringToWrite"><see cref="String"/> contents to be appended to the page blob.</param>
        /// <param name="cancellationToken">Optional cancellation token</param>
        /// <returns><see cref="BlobSegmentInfo"/> which includes an offset and length of the <see cref="String"/> in bytes where it was appended to the page blob.</returns>
        public virtual async Task<BlobSegmentInfo> AppendAsync(string stringToWrite, CancellationToken? cancellationToken = null)
        {
            byte[] bytesToWrite = this.Encoding.GetBytes(stringToWrite);

            if (bytesToWrite.Length >= AzurePageBlobUploadBlockLimit)
                throw new AppendStreamToBigException();

            return await this.AppendAsync(new MemoryStream(bytesToWrite), cancellationToken);
        }

        /// <summary>
        /// Appends the contents of a <see cref="Stream"/> to the end of the page blob and creates it if it doesn't already exist.
        /// </summary>
        /// <param name="sourceStreamToWrite"><see cref="Stream"/> contents to be appended to the page blob.</param>
        /// <param name="cancellationToken">Optional cancellation token</param>
        /// <returns><see cref="BlobSegmentInfo"/> which includes an offset and length of the <see cref="Stream"/> in bytes where it was appended to the page blob.</returns>
        public virtual async Task<BlobSegmentInfo> AppendAsync(Stream sourceStreamToWrite, CancellationToken? cancellationToken = null)
        {
            if (sourceStreamToWrite.CanSeek && sourceStreamToWrite.Length >= AzurePageBlobUploadBlockLimit)
                throw new AppendStreamToBigException();

            return await this.AppendImplAsync(sourceStreamToWrite, cancellationToken);
        }

        internal async Task<BlobSegmentInfo> AppendImplAsync(Stream sourceStreamToWrite, CancellationToken? cancellationToken = null, Func<Task> concurrencyTestFunc = null)
        {
            if (cancellationToken == null)
                cancellationToken = CancellationToken.None;

            this.EnsureInitialized();

            CloudPageBlob pageBlob = this.BlobContainer.GetPageBlobReference(this.BlobPath);

            // Note: We only force the use of a buffer here if the stream that is passed cannot seek. We need the
            //       length to fill out the page with null characters, so it must support seeking unless we find a
            //       different algorithm.
            if (!sourceStreamToWrite.CanSeek)
                sourceStreamToWrite =
                    await BuildMemoryStreamThatCanSeekAsync(sourceStreamToWrite, cancellationToken.Value);

            InternalPageBlobSizeInfo pageBlobSizeInfo = await this.PreparePageBlobAsync(pageBlob,
                (int)sourceStreamToWrite.Length, cancellationToken.Value);
            Stream streamToWrite = BuildStreamOfPagesToWrite(sourceStreamToWrite, pageBlobSizeInfo);

            int byteOffset = pageBlobSizeInfo.OffsetOfLastPage + pageBlobSizeInfo.UsedBytesFromLastPage.Length;
            int streamLength = (int)sourceStreamToWrite.Length;

            if (concurrencyTestFunc != null)
                await concurrencyTestFunc();

            await pageBlob.WritePagesAsync(streamToWrite, pageBlobSizeInfo.OffsetOfLastPage, null,
                CreateAccessCondition(pageBlob), this.RequestOptions, this.OperationContext, cancellationToken.Value);

            return new BlobSegmentInfo(byteOffset, streamLength);
        }

        /// <summary>
        /// Returns the full contents of the page blob as a <see cref="String"/>, surrounded by square brackets. e.g. "[--Page Blob Contents--]"
        /// </summary>
        /// <param name="cancellationToken">Optional cancellation token</param>
        /// <returns>The full contents of the page blob as a <see cref="String"/>, surrounded by square brackets. e.g. "[--Page Blob Contents--]"</returns>
        public virtual async Task<string> DownloadToStringAsJsonArrayAsync(CancellationToken? cancellationToken = null)
        {
            Stream stream = await this.DownloadToStreamAsJsonArrayAsync(cancellationToken);
            using (StreamReader sr = new StreamReader(stream, this.Encoding))
            {
                return sr.ReadToEnd();
            }
        }

        /// <summary>
        /// Returns the full contents of the page blob as a <see cref="String"/>.
        /// </summary>
        /// <param name="cancellationToken">Optional cancellation token</param>
        /// <returns>The full contents of the page blob as a <see cref="String"/>.</returns>
        public virtual async Task<string> DownloadToStringAsync(CancellationToken? cancellationToken = null)
        {
            Stream stream = await this.DownloadToStreamAsync(cancellationToken);
            using (StreamReader sr = new StreamReader(stream, this.Encoding))
            {
                return sr.ReadToEnd();
            }
        }

        /// <summary>
        /// Returns the full contents of the page blob as a <see cref="Stream"/>, surrounded by square brackets. e.g. "[--Page Blob Contents--]"
        /// </summary>
        /// <param name="cancellationToken">Optional cancellation token</param>
        /// <returns>The full contents of the page blob as a <see cref="Stream"/>, surrounded by square brackets. e.g. "[--Page Blob Contents--]"
        /// Important: This <see cref="Stream"/> is read-only and does NOT support Seek, Position, or Length operations.</returns>
        public virtual async Task<Stream> DownloadToStreamAsJsonArrayAsync(CancellationToken? cancellationToken = null)
        {
            Stream blobStream = await this.DownloadToStreamAsync(cancellationToken);
            return new ConcatenatedStream(
                new[]
                {
                    new MemoryStream(this.Encoding.GetBytes("[")),
                    blobStream,
                    new MemoryStream(this.Encoding.GetBytes("]"))
                });
        }

        /// <summary>
        /// Returns the full contents of the page blob as a <see cref="String"/>.
        /// </summary>
        /// <param name="cancellationToken">Optional cancellation token</param>
        /// <returns>The full contents of the page blob as a <see cref="String"/>.
        /// Important: This <see cref="Stream"/> is intended as Read-Only and DOES support Seek, Position, and Length operations.</returns>
        public virtual async Task<Stream> DownloadToStreamAsync(CancellationToken? cancellationToken = null)
        {
            if (cancellationToken == null)
                cancellationToken = CancellationToken.None;

            this.EnsureInitialized();

            CloudPageBlob pageBlob = this.BlobContainer.GetPageBlobReference(this.BlobPath);

            bool blobExists =
                await pageBlob.ExistsAsync(this.RequestOptions, this.OperationContext, cancellationToken.Value);
            if (blobExists)
            {
                Stream downloadPageBlobStream = await pageBlob.OpenReadAsync(CreateAccessCondition(pageBlob),
                    this.RequestOptions, this.OperationContext, cancellationToken.Value);
                return new NullCharacterInFinalPageTruncatingStream(downloadPageBlobStream);
            }

            return new MemoryStream(new byte[0], false);
        }

        /// <summary>
        /// Returns the requested contents of the page blob as a <see cref="String"/>.
        /// </summary>
        /// <param name="byteOffset">Byte offset to start reading from.</param>
        /// <param name="bytesToRead">Byte count to read.</param>
        /// <param name="cancellationToken">Optional cancellation token</param>
        /// <returns>The requested contents of the page blob as a <see cref="String"/>.</returns>
        public virtual async Task<string> DownloadToStringAsync(int byteOffset, int bytesToRead,
            CancellationToken? cancellationToken = null)
        {
            InternalDownloadToBufferResult downloadResults =
                await this.DownloadToBufferAsync(byteOffset, bytesToRead, cancellationToken);
            int bytesToReturn = bytesToRead < downloadResults.TotalBytesRead
                ? bytesToRead
                : downloadResults.TotalBytesRead;
            return this.Encoding.GetString(downloadResults.Buffer, downloadResults.ByteOffset, bytesToReturn);
        }

        protected virtual async Task<InternalDownloadToBufferResult> DownloadToBufferAsync(int byteOffset, int bytesToRead, CancellationToken? cancellationToken)
        {
            if (cancellationToken == null)
                cancellationToken = CancellationToken.None;

            this.EnsureInitialized();

            CloudPageBlob pageBlob = this.BlobContainer.GetPageBlobReference(this.BlobPath);

            int pageOffset = CalculatePageOffset(byteOffset);
            int byteOffsetWithinPage = byteOffset % ByteCountInPage;
            int pageBytesToRead = CalculatePageBytesToRead(byteOffsetWithinPage, bytesToRead);
            int totalBytesRead = 0;

            var readBuffer = new byte[pageBytesToRead];

            bool blobExists =
                await pageBlob.ExistsAsync(this.RequestOptions, this.OperationContext, cancellationToken.Value);
            if (blobExists)
            {
                using (Stream stream = await pageBlob.OpenReadAsync(CreateAccessCondition(pageBlob),
                    this.RequestOptions, this.OperationContext, cancellationToken.Value))
                {
                    stream.Seek(pageOffset, SeekOrigin.Begin);

                    while (totalBytesRead < pageBytesToRead)
                    {
                        int bytesRead = await stream.ReadAsync(readBuffer, 0, pageBytesToRead,
                            cancellationToken.Value);

                        if (bytesRead == 0)
                            break;

                        totalBytesRead += bytesRead;
                    }
                }
            }

            int requestedBytesRead = bytesToRead < totalBytesRead ? bytesToRead : totalBytesRead;

            return new InternalDownloadToBufferResult(readBuffer, byteOffsetWithinPage, requestedBytesRead);
        }

        /// <summary>
        /// Checks if the page blob exists.
        /// </summary>
        /// <param name="cancellationToken">Optional cancellation token</param>
        /// <returns>true if the page blob exists, false if it does not exist.</returns>
        public virtual async Task<bool> ExistsAsync(CancellationToken? cancellationToken = null)
        {
            if (cancellationToken == null)
                cancellationToken = CancellationToken.None;

            this.EnsureInitialized();
            bool containerExists = await this.BlobContainer.ExistsAsync(this.RequestOptions, this.OperationContext,
                cancellationToken.Value);
            if (!containerExists)
                return false;

            CloudPageBlob pageBlob = this.BlobContainer.GetPageBlobReference(this.BlobPath);
            return await pageBlob.ExistsAsync(this.RequestOptions, this.OperationContext, cancellationToken.Value);
        }

        /// <summary>
        /// Deletes the page blob if it exists.
        /// </summary>
        /// <param name="cancellationToken">Optional cancellation token</param>
        /// <returns>true if it was deleted, false if it did not exist.</returns>
        public virtual async Task<bool> DeleteIfExistsAsync(CancellationToken? cancellationToken = null)
        {
            if (cancellationToken == null)
                cancellationToken = CancellationToken.None;

            this.EnsureInitialized();

            CloudPageBlob pageBlob = this.BlobContainer.GetPageBlobReference(this.BlobPath);

            return await pageBlob.DeleteIfExistsAsync(DeleteSnapshotsOption.IncludeSnapshots, CreateAccessCondition(),
                this.RequestOptions, this.OperationContext, cancellationToken.Value);
        }

        /// <summary>
        /// Deletes the page blob container if it exists.
        /// </summary>
        /// <param name="cancellationToken">Optional cancellation token</param>
        /// <returns>true if it was deleted, false if it did not exist.</returns>
        internal virtual async Task<bool> DeleteContainerIfExistsAsync(CancellationToken? cancellationToken = null)
        {
            if (cancellationToken == null)
                cancellationToken = CancellationToken.None;

            this.EnsureInitialized();
            return await this.BlobContainer.DeleteIfExistsAsync(CreateAccessCondition(), this.RequestOptions,
                this.OperationContext, cancellationToken.Value);
        }

        protected void EnsureInitialized()
        {
            this.Initialize();
        }

        protected async Task<BlobProperties> GetPropertiesAsync(CloudPageBlob pageBlob,
            CancellationToken cancellationToken)
        {
            if (pageBlob.Properties == null || pageBlob.Properties.Length < 0)
                await pageBlob.FetchAttributesAsync(CreateAccessCondition(pageBlob), this.RequestOptions, this.OperationContext,
                    cancellationToken);

            return pageBlob.Properties;
        }

        protected static async Task<Stream> BuildMemoryStreamThatCanSeekAsync(Stream sourceStreamToWrite,
            CancellationToken cancellationToken)
        {
            var memStream = new MemoryStream();
            await sourceStreamToWrite.CopyToAsync(memStream, DefaultBufferSize, cancellationToken);
            memStream.Position = 0;
            return memStream;
        }

        protected async Task<InternalPageBlobSizeInfo> PreparePageBlobAsync(CloudPageBlob pageBlob,
            int sourceBufferByteCountToWrite, CancellationToken cancellationToken)
        {
            EnsureBlobExistsResult result = await this.EnsureBlobExistsAsync(pageBlob, byteCountToWrite: sourceBufferByteCountToWrite);

            InternalPageBlobSizeInfo sizeInfo;
            switch (result)
            {
                case EnsureBlobExistsResult.Created:
                    sizeInfo = new InternalPageBlobSizeInfo();
                    break;
                case EnsureBlobExistsResult.AlreadyExisted:
                    sizeInfo = await this.ResizePageBlobIfNeededAsync(pageBlob, sourceBufferByteCountToWrite,
                        cancellationToken);
                    break;
                default:
                    throw new NotSupportedException($"Unrecognized enum value ({result}) returned from the method {nameof(this.EnsureBlobExistsAsync)}.");
            }

            return sizeInfo;
        }

        protected async Task<InternalPageBlobSizeInfo> ResizePageBlobIfNeededAsync(CloudPageBlob pageBlob,
            int sourceBufferByteCountToWrite, CancellationToken cancellationToken)
        {
            BlobProperties blobProperties = await this.GetPropertiesAsync(pageBlob, cancellationToken);

            int offsetOfLastPage = CalculateOffsetOfLastPage((int)blobProperties.Length);
            byte[] usedBytesFromLastPage =
                await this.GetUsedBytesWrittenToLastPageAsync(pageBlob, offsetOfLastPage, cancellationToken);
            var sizeInfo = new InternalPageBlobSizeInfo(offsetOfLastPage, usedBytesFromLastPage);

            int currentTotalBytesUsed =
                CalculateTotalBytesUsed((int)blobProperties.Length, usedBytesFromLastPage.Length);
            await this.ResizePageBlobIfNeededAsync(pageBlob, (int)blobProperties.Length, currentTotalBytesUsed,
                sourceBufferByteCountToWrite, cancellationToken);
            return sizeInfo;
        }

        protected async Task ResizePageBlobIfNeededAsync(CloudPageBlob pageBlob, int currentTotalBytesThroughFullPage,
            int currentTotalUsedBytes, int byteCountToWrite, CancellationToken cancellationToken)
        {
            long updatedUsedLength = currentTotalUsedBytes + byteCountToWrite;
            long updatedBlobLength = CalculateBlobLengthThroughEndOfPage((int)updatedUsedLength);

            if (updatedBlobLength != currentTotalBytesThroughFullPage)
                await pageBlob.ResizeAsync(updatedBlobLength, CreateAccessCondition(pageBlob), this.RequestOptions,
                    this.OperationContext, cancellationToken);
        }

        protected async Task<byte[]> GetUsedBytesWrittenToLastPageAsync(CloudPageBlob pageBlob, long offsetOfLastPage,
            CancellationToken cancellationToken)
        {
            byte[] pageBlobReadBuffer = new byte[ByteCountInPage];
            int bytesRead = await pageBlob.DownloadRangeToByteArrayAsync(pageBlobReadBuffer, 0, offsetOfLastPage,
                ByteCountInPage, CreateAccessCondition(pageBlob), this.RequestOptions, this.OperationContext,
                cancellationToken);
            //if (bytesRead != ByteCountInPage)
            //    throw new ApplicationException("Failure to download the requested byte count from the page blob! " +
            //                                   $"(bytes expected: {ByteCountInPage}, bytes downloaded: {bytesRead})");

            string value = this.Encoding.GetString(pageBlobReadBuffer).TrimEnd('\x00');
            return this.Encoding.GetBytes(value);
        }

        protected static Stream BuildStreamOfPagesToWrite(Stream sourceStreamToWrite, InternalPageBlobSizeInfo sizeInfo)
        {
            int updatedUsedByteCount = sizeInfo.UsedBytesFromLastPage.Length + (int)sourceStreamToWrite.Length;
            int updatedUsedToFullPageByteCount = CalculateBlobLengthThroughEndOfPage(updatedUsedByteCount);

            var streamForPreviouslyUsedBytes = new MemoryStream(sizeInfo.UsedBytesFromLastPage, false);

            if (updatedUsedByteCount == updatedUsedToFullPageByteCount)
                return new ConcatenatedStream(new[] { streamForPreviouslyUsedBytes, sourceStreamToWrite });

            var streamToFillOutLastPage = new MemoryStream(FullPageOfNullChars, 0,
                (int)(updatedUsedToFullPageByteCount - updatedUsedByteCount));
            return new ConcatenatedStream(new[] { streamForPreviouslyUsedBytes, sourceStreamToWrite, streamToFillOutLastPage });
        }

        protected async Task EnsureContainerExistsAsync()
        {
            if (this.VerifiedThatContainerExists)
                return;

            await this.BlobContainer.CreateIfNotExistsAsync(this.RequestOptions, this.OperationContext);
            this.VerifiedThatContainerExists = true;
        }

        protected async Task<EnsureBlobExistsResult> EnsureBlobExistsAsync(CloudPageBlob pageBlob, int byteCountToWrite, CancellationToken? cancellationToken = null)
        {
            if (this.VerifiedThatBlobExists)
                return EnsureBlobExistsResult.AlreadyExisted;

            if (cancellationToken == null)
                cancellationToken = CancellationToken.None;

            await this.EnsureContainerExistsAsync();

            bool blobExists = await pageBlob.ExistsAsync(this.RequestOptions, this.OperationContext, cancellationToken.Value);
            EnsureBlobExistsResult result = blobExists
                ? EnsureBlobExistsResult.AlreadyExisted
                : EnsureBlobExistsResult.Created;

            if (!blobExists)
            {
                long initialByteLength = CalculateBlobLengthThroughEndOfPage(byteCountToWrite);
                await pageBlob.CreateAsync(initialByteLength, CreateAccessCondition(pageBlob), this.RequestOptions, this.OperationContext);
            }

            this.VerifiedThatBlobExists = true;
            return result;
        }

        protected static int CalculateBlobLengthThroughEndOfPage(int usedByteCount)
        {
            return (usedByteCount / ByteCountInPage * ByteCountInPage)
                   + (usedByteCount % ByteCountInPage > 0 ? ByteCountInPage : 0);
        }

        protected static int CalculatePageOffset(int byteOffset)
        {
            return (byteOffset / ByteCountInPage) * ByteCountInPage;
        }

        private static int CalculatePageBytesToRead(int offsetWithinFirstPageToRead, int bytesToRead)
        {
            return CalculateBlobLengthThroughEndOfPage(offsetWithinFirstPageToRead + bytesToRead);
        }

        protected static int CalculateOffsetOfLastPage(int currentTotalBytesToNextFullPage)
        {
            return (currentTotalBytesToNextFullPage / ByteCountInPage - 1) * ByteCountInPage;
        }

        protected static int CalculateTotalBytesUsed(int currentTotalBytesToNextFullPage, int usedByteCountFromLastPage)
        {
            return CalculateOffsetOfLastPage(currentTotalBytesToNextFullPage) + usedByteCountFromLastPage;
        }

        private static AccessCondition CreateAccessCondition(CloudPageBlob pageBlob = null)
        {
            bool pageBlobPropertiesIsInitialized = pageBlob?.Properties != null && pageBlob.Properties.Length >= 0;

            return new AccessCondition()
            {
                IfMatchETag = pageBlobPropertiesIsInitialized && !string.IsNullOrEmpty(pageBlob.Properties.ETag) ? pageBlob.Properties.ETag : null,
                //IfNoneMatchETag = ,
                //IfMaxSizeLessThanOrEqual = ,
                //IfAppendPositionEqual = ,
                //IfModifiedSinceTime = ,
                //IfNotModifiedSinceTime = ,
                //IfSequenceNumberEqual = ,
                //IfSequenceNumberLessThan = ,
                //IfSequenceNumberLessThanOrEqual = ,
                //LeaseId = ,
            };
        }

        // Tuning Recommendations:
        // https://azure.microsoft.com/en-us/blog/azure-storage-client-library-retry-policy-recommendations/

        private static BlobRequestOptions CreateBlobRequestOptions()
        {
            return new BlobRequestOptions()
            {
                //UseTransactionalMD5 = true,
                RetryPolicy = new ExponentialRetry(TimeSpan.FromMilliseconds(100), 10),
                ParallelOperationThreadCount = 1,
                //SingleBlobUploadThresholdInBytes = ,
                ServerTimeout = TimeSpan.FromSeconds(5),
                MaximumExecutionTime = TimeSpan.FromSeconds(30),
                //AbsorbConditionalErrorsOnRetry = ,
                //StoreBlobContentMD5 = ,
                //LocationMode = ,
                //DisableContentMD5Validation = ,
            };
        }

        private static OperationContext CreateOperationContext()
        {
            return new OperationContext()
            {
                //ClientRequestID = ,
                //CustomUserAgent = ,
                //StartTime = ,
                //EndTime = ,
                //LastResult = { },
                //UserHeaders = ,
                //LogLevel = ,
                //RequestResults = { },
            };
        }

        protected class InternalDownloadToBufferResult
        {
            public InternalDownloadToBufferResult(byte[] buffer, int byteOffset, int totalBytesRead)
            {
                this.Buffer = buffer;
                this.ByteOffset = byteOffset;
                this.TotalBytesRead = totalBytesRead;
            }

            public byte[] Buffer { get; }
            public int ByteOffset { get; }
            public int TotalBytesRead { get; }
        }

        protected class InternalPageBlobSizeInfo
        {
            public InternalPageBlobSizeInfo()
            {
                this.OffsetOfLastPage = 0;
                this.UsedBytesFromLastPage = new byte[0];
            }

            public InternalPageBlobSizeInfo(int offsetOfLastPage, byte[] usedBytesFromLastPage)
            {
                this.OffsetOfLastPage = offsetOfLastPage;
                this.UsedBytesFromLastPage = usedBytesFromLastPage;
            }

            public int OffsetOfLastPage { get; }
            public byte[] UsedBytesFromLastPage { get; }
        }

        protected enum EnsureBlobExistsResult
        {
            Created,
            AlreadyExisted,
        }
    }
}
