using System;
using System.IO;
using System.Text;
using SportsBook.AzurePageBlobs.Internal;

namespace SportsBook.AzurePageBlobs
{
    public class FilterJsonOuterArrayBracketsStream : Stream
    {
        private readonly Stream _stream;
        private long _length;
        private int _startOffSet;

        public override bool CanRead => this._stream.CanRead;
        public override bool CanSeek => this._stream.CanSeek;
        public override bool CanWrite => false;
        public override long Length => this._length;

        public FilterJsonOuterArrayBracketsStream(Stream stream)
        {
            if (!stream.CanSeek)
                throw new ArgumentException("Filtering JSON Outer Array Brackets requires a " +
                                            $"{nameof(stream)} parameter derived from {nameof(Stream)} " +
                                            $"with the {nameof(stream.CanSeek)} property set to true.");

            this._stream = stream;
            this._length = this._stream.Length;
            this._startOffSet = 0;

            this.InitStartPosAndLength();
        }

        public override int Read(byte[] buffer, int offset, int count)
        {
            long bytesRemainingInStream = this.Length - this.Position;
            int maxBytesToRead = bytesRemainingInStream < count
                ? (int)bytesRemainingInStream
                : count;

            int bytesRead = this._stream.Read(buffer, offset, maxBytesToRead);

            return bytesRead;
        }

        public override long Position
        {
            get => this._stream.Position - this._startOffSet;
            set => this._stream.Position = value + this._startOffSet;
        }

        public override void Write(byte[] buffer, int offset, int count)
        {
            throw new NotSupportedException("Stream does not support write operations.");
        }

        public override long Seek(long offset, SeekOrigin origin)
        {
            long adjustedOffsetToSeekTo = origin == SeekOrigin.Begin
                ? this._startOffSet + offset
                : (origin == SeekOrigin.End
                    ? offset - (this._stream.Length - this._length - this._startOffSet)
                    : offset);

            this.ValidateSeekParameters(offset, origin);

            return this._stream.Seek(adjustedOffsetToSeekTo, origin) - this._startOffSet;
        }

        private void ValidateSeekParameters(long offset, SeekOrigin origin)
        {
            long actualResultingOffset;
            if (origin == SeekOrigin.Current)
                actualResultingOffset = this._stream.Position - this._startOffSet + offset;
            else if (origin == SeekOrigin.End)
                actualResultingOffset = this._length + offset;
            else
                actualResultingOffset = offset;

            if (actualResultingOffset < 0 || actualResultingOffset > this._length)
                throw new ArgumentOutOfRangeException(
                    $"Attempting to seek outside the range of the stream! {nameof(SeekOrigin)}: {origin}, " +
                    $"{nameof(offset)}: {offset}, Adjusted Position: {actualResultingOffset}");
        }

        public override void SetLength(long value)
        {
            this._stream.SetLength(value - this._startOffSet);
            this._length = value - this._startOffSet;
        }

        public override void Flush()
        {
            this._stream.Flush();
        }

        private void InitStartPosAndLength()
        {
            if (this.SetStartPos())
                this.SetLength();
            this.Seek(0, SeekOrigin.Begin);
        }

        // According to: https://stackoverflow.com/a/3999301
        // One of the nice features about UTF-8 is that if a sequence of bytes represents a character and that
        // sequence of bytes appears anywhere in valid UTF-8 encoded data then it always represents that character.
        private bool SetStartPos()
        {
            byte[] startBracketByteArray = Encoding.UTF8.GetBytes("[");

            int bufferLength = (int)(this._stream.Length < 1024 ? this._stream.Length : 1024);
            byte[] readBuffer = new byte[bufferLength];

            this._stream.Seek(0, SeekOrigin.Begin);
            int bytesRead = this._stream.Read(readBuffer, 0, readBuffer.Length);

            if (bytesRead != readBuffer.Length)
                throw new InvalidOperationException("Stream read operation read fewer bytes than available and expected!");

            int nonWhitespaceCharIdx = readBuffer.FindFirstNonWhitespaceChar();
            if (readBuffer.IsMatch(nonWhitespaceCharIdx, startBracketByteArray))
            {
                this._startOffSet = nonWhitespaceCharIdx + startBracketByteArray.Length;
                return true;
            }

            return false;
        }

        private void SetLength()
        {
            byte[] endBracketByteArray = Encoding.UTF8.GetBytes("]");

            int bufferLength = (int)(this._stream.Length < 1024 ? this._stream.Length : 1024);
            byte[] readBuffer = new byte[bufferLength];

            this._stream.Seek(-bufferLength, SeekOrigin.End);
            int bytesRead = this._stream.Read(readBuffer, 0, readBuffer.Length);

            if (bytesRead != readBuffer.Length)
                throw new InvalidOperationException("Stream read operation read fewer bytes than available and expected!");

            int whitespaceCharBufferIdx = readBuffer.FindFirstWhitespaceCharInTrailingWhitespace();
            long whitespaceCharStreamIdx = this._stream.Length - bufferLength + whitespaceCharBufferIdx;

            if (readBuffer.IsMatch(whitespaceCharBufferIdx - endBracketByteArray.Length, endBracketByteArray))
                this._length = whitespaceCharStreamIdx - endBracketByteArray.Length - this._startOffSet;
            else
                this._length = whitespaceCharStreamIdx - this._startOffSet;
        }
    }
}
