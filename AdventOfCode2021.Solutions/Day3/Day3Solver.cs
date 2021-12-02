using System;
using AdventOfCode2021.Solutions.Interface;
using AdventOfCode2021.Util;

namespace AdventOfCode2021.Solutions.Day3
{
    public class Day3Solver : ISolvable
    {
        
        private static string FileName => "Input/Day3_Test.input";
        
        public void Run()
        {
            SolveFirstStar();
        }

        public void SolveFirstStar()
        {
            var lines = FileRead.ReadTokens(FileName);
            var result = 0;
            Console.WriteLine("Solution (1): " + result);
        }
    }
}