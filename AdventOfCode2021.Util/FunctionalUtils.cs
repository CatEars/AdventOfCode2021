using System;
using System.Linq;

namespace AdventOfCode2021.Util
{
    public static class FunctionalUtils
    {

        public static T2 Pipe<T, T2>(this T val, Func<T, T2> func) => func(val);

        public static Func<T, bool> OneOf<T>(params Func<T, bool>[] funcs)
        {
            return val =>
            {
                foreach (var func in funcs)
                {
                    if (func(val))
                    {
                        return true;
                    }
                }

                return false;
            };
        }

        public static Func<T, T> MultiApply<T>(Func<T, T> func, int n) =>
            obj => Enumerable.Range(0, n).Aggregate(obj, (o, _) => func(o));
    }
}
