using System.Collections.Generic;

namespace AdventOfCode2021.Util
{
    public static class ListExtensions
    {

        public static void SortDescending<T>(this List<T> collection)
        {
            collection.Sort();
            collection.Reverse();
        }

    }
}
