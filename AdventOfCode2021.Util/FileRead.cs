using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace AdventOfCode2021.Util
{
    public static class FileRead
    {

        public static List<int> ReadLinesAsInt(string filepath)
        {
            return File.ReadLines(filepath)
                .Select(int.Parse)
                .ToList();
        }

        public static List<string> ReadTokens(string filepath, string separator=" ")
        {
            return File
                .ReadLines(filepath)
                .Select(x => x.Trim())
                .SelectMany(x => x.Split(separator))
                .ToList();
        }

        public static List<List<string>> ReadMatrix(string filepath)
        {
            return File
                .ReadLines(filepath)
                .Select(x => x.Trim().Split().ToList())
                .ToList();
        }

        public static List<List<char>> ReadCharMatrix(string filepath)
        {
            return File
                .ReadLines(filepath)
                .Select(x => x.Trim().ToCharArray().ToList())
                .ToList();
        }

        /// <summary>
        /// Reads all lines and then converts those lines with a custom function.
        /// </summary>
        /// <param name="fpath"></param>
        /// <param name="convert"></param>
        /// <returns></returns>
        public static List<T> ReadLinesAndConvert<T>(string fpath, Func<List<string>, T> convert)
        {
            var matrix = ReadMatrix(fpath);
            return matrix
                .Select(convert)
                .ToList();
        }

        public static List<string> ReadLines(string fileName)
        {
            return File.ReadLines(fileName)
                .Select(x => x.Trim())
                .ToList();
        }
    }
}
