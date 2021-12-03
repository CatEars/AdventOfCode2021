using System;
using System.Collections.Generic;
using System.Linq;
using AdventOfCode2021.Solutions.Interface;
using AdventOfCode2021.Util;

namespace AdventOfCode2021.Solutions.Day4
{
    public class Day4Solver : ISolvable
    {
        
        private static string FileName => "Input/Day4_Test.input";
        
        public void Run()
        {
            SolveFirstStar();
            //SolveSecondStar();
        }
        
        private void SolveFirstStar()
        {
            var matrix = FileRead.ReadLines(FileName);
            Console.WriteLine("Solution (1): " + 0);
        }
        
        private void SolveSecondStar()
        {
            var matrix = FileRead.ReadLines(FileName);
            Console.WriteLine("Solution (2): " + 0);
        }

    }
}