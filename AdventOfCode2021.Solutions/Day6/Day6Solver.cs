using System;
using System.Collections.Generic;
using System.Linq;
using AdventOfCode2021.Solutions.Interface;
using AdventOfCode2021.Util;

namespace AdventOfCode2021.Solutions.Day6
{
    public class Day6Solver : ISolvable
    {

        private static string FileName => "Input/Day6_A.input";

        public void Run()
        {
            SolveFirstStar();
            SolveSecondStar();
        }

        private record Counter(Dictionary<int, long> Fish);

        private Counter Age(Counter current)
        {
            var next = new Counter(new Dictionary<int, long>());
            var births = current.Fish.GetOrZero(0);
            next.Fish.AddOrSet(6, births);
            next.Fish.AddOrSet(8, births);

            for (var age = 1; age <= 8; ++age)
            {
                var nextDay = current.Fish.GetOrZero(age);
                next.Fish.AddOrSet(age - 1, nextDay);
            }

            return next;
        }

        private long SolveForDays(List<int> days, int numDays)
        {
            var start = new Counter(new Dictionary<int, long>());
            foreach (var day in days)
            {
                start.Fish.AddOrSet(day, 1);
            }

            var solution = Enumerable.Range(0, numDays)
                .Aggregate(start, (counter, _) => Age(counter));

            return solution.Fish.Values.Sum();
        }

        private void SolveFirstStar()
        {
            var fishies = FileRead.ReadCommaList(FileName);
            var sol = SolveForDays(fishies, 80);
            Console.WriteLine("Solution (1): " + sol);
        }

        private void SolveSecondStar()
        {
            var fishies = FileRead.ReadCommaList(FileName);
            var sol = SolveForDays(fishies, 256);
            Console.WriteLine("Solution (2): " + sol);
        }

    }
}
