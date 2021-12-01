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
        
    }
}