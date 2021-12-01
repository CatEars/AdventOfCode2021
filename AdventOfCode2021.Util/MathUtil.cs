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
        
    }
    
}