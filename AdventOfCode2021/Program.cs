using System;
using System.Collections.Generic;
using AdventOfCode2021.Solutions.Day1;
using AdventOfCode2021.Solutions.Day10;
using AdventOfCode2021.Solutions.Day11;
using AdventOfCode2021.Solutions.Day12;
using AdventOfCode2021.Solutions.Day13;
using AdventOfCode2021.Solutions.Day14;
using AdventOfCode2021.Solutions.Day15;
using AdventOfCode2021.Solutions.Day16;
using AdventOfCode2021.Solutions.Day17;
using AdventOfCode2021.Solutions.Day18;
using AdventOfCode2021.Solutions.Day19;
using AdventOfCode2021.Solutions.Day2;
using AdventOfCode2021.Solutions.Day3;
using AdventOfCode2021.Solutions.Day4;
using AdventOfCode2021.Solutions.Day5;
using AdventOfCode2021.Solutions.Day6;
using AdventOfCode2021.Solutions.Day7;
using AdventOfCode2021.Solutions.Day8;
using AdventOfCode2021.Solutions.Day9;
using AdventOfCode2021.Solutions.Interface;

namespace AdventOfCode2021
{
    class Program
    {

        private static List<ISolvable> solutions = new()
        {
            new Day1Solver(),
            new Day2Solver(),
            new Day3Solver(),
            new Day4Solver(),
            new Day5Solver(),
            new Day6Solver(),
            new Day7Solver(),
            new Day8Solver(),
            new Day9Solver(),
            new Day10Solver(),
            new Day11Solver(),
            new Day12Solver(),
            new Day13Solver(),
            new Day14Solver(),
            new Day15Solver(),
            new Day16Solver(),
            new Day17Solver(),
            new Day18Solver(),
            new Day19Solver()
        };

        private static void TimeAll()
        {
            var start = DateTime.UtcNow;
            foreach (var sol in solutions)
            {
                sol.Run();
                Console.WriteLine(" ========= ");
            }
            var end = DateTime.UtcNow;
            var diff = end - start;
            Console.WriteLine($"Total time for {solutions.Count} solutions was {diff}");
        }

        private static void SolveFor(int day)
        {
            var solver = solutions[day - 1];
            var name = solver.GetType().FullName!.Split(".")[2];
            Console.WriteLine("Solving for " + name);
            solver.Run();
        }

        static void Main(string[] args)
        {
            var day = 19;
            SolveFor(day);
            //TimeAll();
        }
    }
}
