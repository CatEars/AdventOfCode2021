using System;

namespace AdventOfCode2021.Util
{
    public static class BinaryUtil
    {

        public static int CountBits(int value, int numToCount = 32)
        {
            var bits = 0;
            for (var idx = 0; idx < numToCount; ++idx)
            {
                bits += (value & (1 << idx)) > 0 ? 1 : 0;
            }

            return bits;
        }

        public static long BinaryToDecimal(string binaryString)
        {
            return Convert.ToInt64(binaryString, 2);
        }

        public static int BinaryInvert(int x, int bits)
        {
            var cnt = 0;
            for (var bit = 0; bit < bits; ++bit)
            {
                cnt += (1 << bit) ^ ((1 << bit) & x);
            }

            return cnt;
        }

    }
}
