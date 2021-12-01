using System.Collections.Generic;
using AdventOfCode2021.Solutions.Day1;
using AdventOfCode2021.Solutions.Day2;
using AdventOfCode2021.Solutions.Day3;
using AdventOfCode2021.Solutions.Interface;

namespace AdventOfCode2021
{
    class Program
    {

        static void Main(string[] args)
        {
            var solutions = new List<ISolvable>()
            {
                new Day1Solver(),
                new Day2Solver(),
                new Day3Solver()
            };

            var day = 1;
            var solver = solutions[day - 1];
            solver.Run();
        }
    }
}