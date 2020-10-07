using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SportsBook.AzurePageBlobs
{
    public class BlobSegmentInfo
    {
        public BlobSegmentInfo(int offset, int length)
        {
            this.Offset = offset;
            this.Length = length;
        }

        public int Offset { get; }
        public int Length { get; }
    }
}
