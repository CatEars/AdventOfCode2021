using System;
using System.Collections.Generic;
using System.Linq;
using AdventOfCode2021.Solutions.Interface;
using AdventOfCode2021.Util;

namespace AdventOfCode2021.Solutions.Day8
{
    public class Day8Solver : ISolvable
    {

        private static string FileName => "Input/Day8_A.input";

        public void Run()
        {
            SolveFirstStar();
            SolveSecondStar();
        }

        private List<(List<int> Patterns, List<int> Output)> Read(string filename)
        {
            var lines = FileRead.ReadLines(filename);
            return lines
                .Select(line =>
                {
                    var split = line.Split(" | ");
                    var patterns = split[0].Trim().Split(" ").Select(ToBits).ToList();
                    var output = split[1].Trim().Split(" ").Select(ToBits).ToList();
                    return (patterns, output);
                })
                .ToList();
        }

        private int ToBits(string value)
        {
            var count = 0;
            foreach (var c in value)
            {
                int bit = c - 'a';
                count |= 1 << bit;
            }

            return count;
        }

        private static Func<int, bool> HasLengthF(int length) => x => BinaryUtil.CountBits(x) == length;

        private static readonly Func<int, bool> IsOne = HasLengthF(2);
        private static readonly Func<int, bool> IsFour = HasLengthF(4);
        private static readonly Func<int, bool> IsSeven = HasLengthF(3);
        private static readonly Func<int, bool> IsEight = HasLengthF(7);

        private record BasicValues(int One, int Four, int Seven, int Eight);

        private static bool IsThree(int bits, BasicValues basics) =>
            (BinaryUtil.CountBits(bits) == 5) && (bits & basics.One) == basics.One;


        private static bool IsSix(int bits, BasicValues basics) =>
            BinaryUtil.CountBits(bits) == 6 && ((bits | basics.One) == basics.Eight);


        private static bool IsNine(int bits, BasicValues basics) =>
            BinaryUtil.CountBits(bits) == 6 && ((bits | basics.Four) == bits);

        private static bool IsZero(int bits, BasicValues basics) =>
            BinaryUtil.CountBits(bits) == 6 && !IsSix(bits, basics) && !IsNine(bits, basics);

        private record AdvancedValues(int Three, int Six, int Nine, int Zero);

        private static bool IsFive(int bits, AdvancedValues advanced) =>
             BinaryUtil.CountBits(bits) == 5 && bits != advanced.Three &&
                   (bits | advanced.Nine) == advanced.Nine;

        private static bool IsTwo(int bits, BasicValues basics, AdvancedValues advanced) =>
            BinaryUtil.CountBits(bits) == 5 && bits != advanced.Three &&
                  (bits | advanced.Nine) == basics.Eight;

        private void SolveFirstStar()
        {
            var input = Read(FileName);
            var selector = FunctionalUtils.OneOf(IsOne, IsFour, IsSeven, IsEight);
            var sol = input
                .SelectMany(x => x.Output)
                .Where(selector)
                .Count();
            Console.WriteLine("Solution (1): " + sol);
        }

        private void SolveSecondStar()
        {
            var input = Read(FileName);
            var sum = 0;
            foreach (var (patterns, output) in input)
            {
                var one = patterns.Find(x => IsOne(x));
                var four = patterns.Find(x => IsFour(x));
                var seven = patterns.Find(x => IsSeven(x));
                var eight = patterns.Find(x => IsEight(x));
                var basics = new BasicValues(one, four, seven, eight);
                var three = patterns.Find(x => IsThree(x, basics));
                var six = patterns.Find(x => IsSix(x, basics));
                var nine = patterns.Find(x => IsNine(x, basics));
                var zero = patterns.Find(x => IsZero(x, basics));
                var advanced = new AdvancedValues(three, six, nine, zero);
                var five = patterns.Find(x => IsFive(x, advanced));
                var two = patterns.Find(x => IsTwo(x, basics, advanced));
                var res = new List<int>()
                {
                    zero, one, two, three, four, five, six, seven, eight, nine
                };
                var value = 1000 * res.IndexOf(output[0]) +
                               100 * res.IndexOf(output[1]) +
                               10 * res.IndexOf(output[2]) +
                               1 * res.IndexOf(output[3]);
                sum += value;
            }

            Console.WriteLine("Solution (2): " + sum);
        }
    }
}
