using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SportsBook.AzurePageBlobs.Internal
{
    public class AppendStreamToBigException : Exception
    {
        public override string Message => "Stream is to large to append in single operation, please use AppendLarge operation";
    }
}
