using System;

namespace AdventOfCode2021.Util
{
    public static class FunctionalUtils
    {

        public static T2 Pipe<T, T2>(this T val, Func<T, T2> func) => func(val);

    }
}