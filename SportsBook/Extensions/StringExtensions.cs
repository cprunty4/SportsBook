using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SportsBook.Extensions
{
    public static class StringExtensions
    {
        public static int ToIntegerValue(this string stringValue)
        {
            int result = 0;

            int.TryParse(stringValue, out result);

            return result;
        }

        public static long ToLongValue(this string stringValue)
        {
            long result = 0;

            long.TryParse(stringValue, out result);

            return result;
        }

        public static DateTime ToDateTimeValue(this string stringValue)
        {
            DateTime result = DateTime.MinValue;

            DateTime.TryParse(stringValue, out result);

            return result;
        }

        public static Guid? ToGuidValue(this string stringValue)
        {
            Guid result = Guid.Empty;

            if (string.IsNullOrEmpty(stringValue))
                return null;

            Guid.TryParse(stringValue, out result);

            return (Guid?)result;
        }


    }
}
