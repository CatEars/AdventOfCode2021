using System;
using System.Collections.Generic;
using System.Linq;
using AdventOfCode2021.Util;

namespace AdventOfCode2021.Solutions.Day1
{
    public class Solver
    {

        private static string FileName => "Input/Day1_A.input";
        
        public static void Run()
        {
            SolveFirstStar();
            SolveSecondStar();
        }

        private static void SolveFirstStar()
        {
            var values = FileRead.ReadIntList(FileName);
            var numIncreases = NumIncreasingSteps(values);
            Console.WriteLine("Solution (1): " + numIncreases);
        }
                
        private static void SolveSecondStar()
        {
            var values = FileRead.ReadIntList(FileName);
            var range = Enumerable.Range(1, values.Count - 2);
            var threeWindowValues = range
                .Select(idx => values[idx - 1] + values[idx] + values[idx + 1])
                .ToList();
            var numIncreases = NumIncreasingSteps(threeWindowValues);
            Console.WriteLine("Solution (2): " + numIncreases);
        }
        
        private static int NumIncreasingSteps(IReadOnlyList<int> values)
        {
            var range = Enumerable.Range(1, values.Count - 1);
            var numIncreases = range
                .Select(idx => values[idx - 1] < values[idx] ? 1 : 0)
                .Aggregate((a, b) => a + b);
            return numIncreases;
        }
    }
}