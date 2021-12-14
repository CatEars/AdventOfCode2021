using System.Collections.Generic;
using System.Linq;

namespace AdventOfCode2021.Util
{
    public static class StringUtil
    {

        public static List<int> IntoValidInts(string values, string separator = " ")
        {
            return values.Split(separator)
                .Where(x => int.TryParse(x, out var _))
                .Select(int.Parse)
                .ToList();
        }

        public static Dictionary<char, int> CharCount(string s)
        {
            var result = new Dictionary<char, int>();
            foreach (var c in s)
            {
                result.AddOrSet(c, 1);
            }

            return result;
        }

    }
}
