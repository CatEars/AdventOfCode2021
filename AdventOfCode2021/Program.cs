﻿using System;
using System.Collections.Generic;
using AdventOfCode2021.Solutions.Day1;
using AdventOfCode2021.Solutions.Day10;
using AdventOfCode2021.Solutions.Day11;
using AdventOfCode2021.Solutions.Day12;
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
        };

        private static void SolveFor(int day)
        {
            var solver = solutions[day - 1];
            var name = solver.GetType().FullName!.Split(".")[2];
            Console.WriteLine("Solving for " + name);
            solver.Run();
        }

        static void Main(string[] args)
        {
            var day = 10;
            SolveFor(day);
        }
    }
}
