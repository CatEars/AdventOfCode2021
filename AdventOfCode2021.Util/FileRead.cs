using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace AdventOfCode2021.Util
{
    public static class FileRead
    {

        public static List<int> ReadIntList(string filepath)
        {
            return File.ReadLines(filepath)
                .Select(x => int.Parse(x))
                .ToList();
        }
        
    }
}