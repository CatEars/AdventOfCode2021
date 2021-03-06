using System;
using System.Collections.Generic;
using System.Linq;
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
using AdventOfCode2021.Solutions.Day20;
using AdventOfCode2021.Solutions.Day21;
using AdventOfCode2021.Solutions.Day22;
using AdventOfCode2021.Solutions.Day23;
using AdventOfCode2021.Solutions.Day24;
using AdventOfCode2021.Solutions.Day25;
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
            new Day19Solver(),
            new Day20Solver(),
            new Day21Solver(),
            new Day22Solver(),
            new Day23Solver(),
            new Day24Solver(),
            new Day25Solver()
        };

        private static void TimeAll()
        {
            var timePerProblem = new List<(DateTime, DateTime)>();
            var start = DateTime.UtcNow;
            foreach (var sol in solutions)
            {
                var a = DateTime.Now;
                sol.Run();
                var b = DateTime.Now;
                Console.WriteLine(" ========= ");
                timePerProblem.Add((a, b));
            }
            var end = DateTime.UtcNow;
            var diff = end - start;
            Console.WriteLine($"Total time for {solutions.Count} solutions was {diff}");
            foreach (var idx in Enumerable.Range(0, timePerProblem.Count))
            {
                var x = timePerProblem[idx].Item2 - timePerProblem[idx].Item1;
                Console.WriteLine($"{idx + 1} - Took {x} to solve");
            }
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
            //var day = 25;
            //SolveFor(day);
            TimeAll();
        }
    }
}
