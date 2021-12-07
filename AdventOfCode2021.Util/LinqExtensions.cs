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

        public static (List<T> Passing, List<T> Failing) Partition<T>(this IEnumerable<T> collection, Predicate<T> pred)
        {
            var passing = new List<T>();
            var failing = new List<T>();
            foreach (var item in collection)
            {
                if (pred(item))
                {
                    passing.Add(item);
                }
                else
                {
                    failing.Add(item);
                }
            }

            return (passing, failing);
        }

        public static string StringConcat<T>(this IEnumerable<T> collection)
        {
            return string.Join("", collection);
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
