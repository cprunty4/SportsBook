using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Text;

namespace SportsBook.AzurePageBlobs.Internal
{
    public static class UTF8ByteArrayExtensions
    {
        // Adapted from: https://stackoverflow.com/a/283648

        // According to: https://stackoverflow.com/a/3999301
        // One of the nice features about UTF-8 is that if a sequence of bytes represents a character and that
        // sequence of bytes appears anywhere in valid UTF-8 encoded data then it always represents that character.

        private static readonly byte[][] UTF8WhitespaceByteArrays =
            new string[] { " ", "\t", "\n", "\r" }.Select(x => Encoding.UTF8.GetBytes(x)).ToArray();

        public static int FindFirstNonWhitespaceChar(this byte[] searchArray)
        {
            int idx = 0;
            while (idx < searchArray.Length)
            {
                bool foundWhitespaceMatch = false;
                foreach (byte[] whitespaceBytes in UTF8WhitespaceByteArrays)
                {
                    if (IsMatch(searchArray, idx, whitespaceBytes))
                    {
                        foundWhitespaceMatch = true;
                        idx += whitespaceBytes.Length;
                        break;
                    }
                }

                if (!foundWhitespaceMatch)
                    break;
            }

            return idx;
        }
        public static int FindFirstWhitespaceCharInTrailingWhitespace(this byte[] searchArray)
        {
            int idx = searchArray.Length;
            while (idx > 0)
            {
                bool foundWhitespaceMatch = false;
                foreach (byte[] whitespaceBytes in UTF8WhitespaceByteArrays)
                {
                    if (IsMatch(searchArray, idx - whitespaceBytes.Length, whitespaceBytes))
                    {
                        foundWhitespaceMatch = true;
                        idx -= whitespaceBytes.Length;
                        break;
                    }
                }

                if (!foundWhitespaceMatch)
                    break;
            }

            return idx;
        }

        public static bool IsMatch(this byte[] array, int position, byte[] candidate)
        {
            if (position < 0 || candidate.Length < 1 || candidate.Length > (array.Length - position))
                return false;

            for (int i = 0; i < candidate.Length; i++)
                if (array[position + i] != candidate[i])
                    return false;

            return true;
        }
    }
}
