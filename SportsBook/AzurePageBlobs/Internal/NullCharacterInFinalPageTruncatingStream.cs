using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.IO;

namespace SportsBook.AzurePageBlobs.Internal
{
    internal class NullCharacterInFinalPageTruncatingStream : Stream
    {
        private readonly Stream _stream;
        private long? _lengthTruncatedAtNullCharacters;
        private const int ByteCountInPage = 512;

        public NullCharacterInFinalPageTruncatingStream(Stream stream)
        {
            if (!stream.CanSeek)
                throw new ArgumentException($"The {nameof(stream)} parameter has a {nameof(stream.CanSeek)} " +
                                            $"property value of {stream.CanSeek} which is not supported.");
            if (!stream.CanRead)
                throw new ArgumentException($"The {nameof(stream)} parameter has a {nameof(stream.CanRead)} " +
                                            $"property value of {stream.CanRead} which is not supported.");
            this._stream = stream;
        }

        public override void Flush()
        {
            this._stream.Flush();
        }

        public override int Read(byte[] buffer, int offset, int count)
        {
            if (this._lengthTruncatedAtNullCharacters == null)
                this.InitializeLength();

            long blobLength = this._stream.Length;
            long positionAfterRead = this._stream.Position + count;
            if (positionAfterRead > blobLength)
                positionAfterRead = blobLength;

            if (positionAfterRead >= blobLength)
            {
                long expectedUsedBytesRead = this.UsedLength - this._stream.Position;
                if (expectedUsedBytesRead <= 0)
                    return 0;

                int bytesRead = this._stream.Read(buffer, offset, count);
                if (bytesRead < expectedUsedBytesRead)
                    return bytesRead;
                return (int)expectedUsedBytesRead;
            }

            return this._stream.Read(buffer, offset, count);
        }

        public override long Seek(long offset, SeekOrigin origin)
        {
            return this._stream.Seek(offset, origin);
        }

        public override void SetLength(long value)
        {
            this._lengthTruncatedAtNullCharacters = null;
            this._stream.SetLength(value);
        }

        public override void Write(byte[] buffer, int offset, int count)
        {
            this._lengthTruncatedAtNullCharacters = null;
            this._stream.Write(buffer, offset, count);
        }

        public override bool CanRead => this._stream.CanRead;
        public override bool CanSeek => this._stream.CanSeek;
        public override bool CanWrite => this._stream.CanWrite;
        public override long Length => this.UsedLength;

        public virtual long UsedLength
        {
            get
            {
                if (this._lengthTruncatedAtNullCharacters == null)
                    this.InitializeLength();

                return this._lengthTruncatedAtNullCharacters.Value;
            }
        }

        public override long Position
        {
            get => this._stream.Position;
            set => this._stream.Position = value;
        }

        protected virtual void InitializeLength()
        {
            long savedPosition = this._stream.Position;
            try
            {
                long blobLength = this._stream.Length;
                long positionOfLastPage = blobLength - ByteCountInPage;
                byte[] buffer = new byte[ByteCountInPage];

                this._stream.Seek(positionOfLastPage, SeekOrigin.Begin);

                int bytesRead = this._stream.Read(buffer, 0, ByteCountInPage);
                //if (bytesRead != ByteCountInPage)
                //    throw new ApplicationException("Unable to read the last page of the page blob as a full page " +
                //                                   $"({ByteCountInPage} bytes) from the child stream!");

                long bytesUsed = positionOfLastPage;
                for (int i = buffer.Length - 1; i >= 0; i--)
                {
                    if (buffer[i] != 0)
                    {
                        bytesUsed = positionOfLastPage + i + 1;
                        break;
                    }
                }

                this._lengthTruncatedAtNullCharacters = bytesUsed;
            }
            finally
            {
                this._stream.Seek(savedPosition, SeekOrigin.Begin);
            }
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
                this._stream.Dispose();
            base.Dispose(disposing);
        }
    }
}
