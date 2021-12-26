using System;
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

        public static T End<T>(this List<T> collection)
        {
            if (collection.Count == 0)
            {
                throw new ArgumentException("Collection is empty");
            }

            return collection[collection.Count - 1];
        }
        
    }
}
