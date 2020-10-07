using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.IO;

namespace SportsBook.AzurePageBlobs.Internal
{
    internal class ConcatenatedStream : Stream
    {
        private readonly Queue<Stream> _streams;

        public ConcatenatedStream(IEnumerable<Stream> streams)
        {
            this._streams = new Queue<Stream>(streams);
        }

        public override bool CanRead
        {
            get { return true; }
        }

        public override int Read(byte[] buffer, int offset, int count)
        {
            if (this._streams.Count == 0)
                return 0;

            int bytesRead = this._streams.Peek().Read(buffer, offset, count);
            if (bytesRead == 0)
            {
                this._streams.Dequeue().Dispose();
                bytesRead += this.Read(buffer, offset + bytesRead, count - bytesRead);
            }
            return bytesRead;
        }

        public override bool CanSeek => false;

        public override bool CanWrite => false;

        public override void Flush()
        {
            throw new NotImplementedException();
        }

        public override long Length => throw new NotImplementedException();

        public override long Position
        {
            get => throw new NotImplementedException();
            set => throw new NotImplementedException();
        }

        public override long Seek(long offset, SeekOrigin origin)
        {
            throw new NotImplementedException();
        }

        public override void SetLength(long value)
        {
            throw new NotImplementedException();
        }

        public override void Write(byte[] buffer, int offset, int count)
        {
            throw new NotImplementedException();
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
                while (this._streams.Count > 0)
                    this._streams.Dequeue().Dispose();
            base.Dispose(disposing);
        }
    }
}
