using System;
using System.Collections.Generic;
using System.Linq;
using AdventOfCode2021.Solutions.Interface;
using AdventOfCode2021.Util;

namespace AdventOfCode2021.Solutions.Day1
{
    public class Day1Solver : ISolvable
    {

        private static string FileName => "Input/Day1_A.input";

        public void Run()
        {
            SolveFirstStar();
            SolveSecondStar();
        }

        private static void SolveFirstStar()
        {
            var values = FileRead.ReadLinesAsInt(FileName);
            var numIncreases = MathUtil.NumIncreasingIndexes(values);
            Console.WriteLine("Solution (1): " + numIncreases);
        }

        private static void SolveSecondStar()
        {
            var values = FileRead.ReadLinesAsInt(FileName);
            var window = values.Window(3);
            var numIncreases = MathUtil.NumIncreasingIndexes(window);
            Console.WriteLine("Solution (2): " + numIncreases);
        }

    }
}
