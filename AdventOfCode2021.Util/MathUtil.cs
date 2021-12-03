using System;
using System.Collections.Generic;
using System.Linq;

namespace AdventOfCode2021.Util
{

    public static class MathUtil
    {

        public static int NumIncreasingIndexes(IReadOnlyList<int> intList)
        {
            if (intList.Count < 2)
            {
                return 0;
            }
            else
            {
                var indices = Enumerable.Range(1, intList.Count - 1);
                return indices.Count(idx => intList[idx - 1] < intList[idx]);
            }
        }

        public static int BinaryToDecimal(string binaryString)
        {
            return Convert.ToInt32(binaryString, 2);
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
