using System;
using System.Collections.Generic;
using System.Linq;

namespace AdventOfCode2021.Util
{
    public static class LinqExtensions
    {

        public static bool Empty<T>(this IEnumerable<T> collection)
        {
            return !collection.Any();
        }

        public static int Sum(this IEnumerable<int> collection)
        {
            return collection.Aggregate(0, (lhs, rhs) => lhs + rhs);
        }

        /// <summary>
        /// Calculates a new collection from the first one. A window is used to sum up
        /// every passable index that fully fits into that window.
        /// </summary>
        /// <param name="collection"></param>
        /// <param name="windowSize"></param>
        /// <returns></returns>
        public static List<int> Window(this IReadOnlyList<int> collection, int windowSize)
        {
            if (windowSize > collection.Count)
            {
                throw new ArgumentException("Window size greater than size of collection");
            }
            var result = new List<int>();
            var sum = 0;
            var idx = 0;
            while (idx < windowSize)
            {
                sum += collection[idx];
                ++idx;
            }
            result.Add(sum);

            for (; idx < collection.Count; ++idx)
            {
                sum -= collection[idx - windowSize];
                sum += collection[idx];
                result.Add(sum);
            }

            return result;
        }
        
    }
}