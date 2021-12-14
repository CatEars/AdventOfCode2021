using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AdventOfCode2021.Solutions.Interface;
using AdventOfCode2021.Util;

namespace AdventOfCode2021.Solutions.Day14
{
    public class Day14Solver : ISolvable
    {

        private static string FileName => "Input/Day14_A.input";

        public void Run()
        {
            SolveFirstStar();
            SolveSecondStar();
        }

        private record Transform(char First, char Second, char Insert);

        private string Age(string polymer, List<Transform> transforms)
        {
            var dict = new Dictionary<string, char>();
            foreach (var tran in transforms)
            {
                dict[tran.First.ToString() + tran.Second.ToString()] = tran.Insert;
            }
            var builder = new StringBuilder();
            for (var idx = 0; idx < polymer.Length; ++idx)
            {
                builder.Append(polymer[idx]);
                if (idx < polymer.Length - 1)
                {
                    var key = polymer[idx].ToString() + polymer[idx + 1];
                    if (dict.ContainsKey(key))
                    {
                        builder.Append(dict[key]);
                    }
                }
            }

            return builder.ToString();
        }

        private void SolveFirstStar()
        {
            var lines = FileRead.ReadLines(FileName);
            var polymer = lines[0].Trim();
            var transforms = lines
                .Skip(2)
                .Select(x => x.Trim().Split(" -> "))
                .Select(x => new Transform(x[0][0], x[0][1], x[1][0]))
                .ToList();
            var resultingPolymer = Enumerable
                .Range(0, 10)
                .Aggregate(polymer, (p, _) => Age(p, transforms));
            var count = StringUtil.CharCount(resultingPolymer)
                .Values
                .ToArray();

            var maximum = MathUtil.Max(count);
            var minimum = MathUtil.Min(count);
            var sol = maximum - minimum;
            Console.WriteLine(maximum + ", " + minimum);
            Console.WriteLine("Solution (1): " + sol);
        }

        private Dictionary<string, long> Age(Dictionary<string, long> current, Dictionary<string, string> transforms)
        {
            var next = new Dictionary<string, long>();
            foreach (var key in current.Keys)
            {
                var num = current[key];
                if (transforms.ContainsKey(key))
                {
                    var insert = transforms[key];
                    var a = key[0] + insert;
                    var b = insert + key[1];
                    next.AddOrSet(a, num);
                    next.AddOrSet(b, num);
                }
            }

            return next;
        }

        private void SolveSecondStar()
        {
            var lines = FileRead.ReadLines(FileName);
            var polymer = lines[0].Trim();
            var transforms = lines
                .Skip(2)
                .Select(x => x.Trim().Split(" -> "))
                .ToDictionary(key => key[0], val => val[1]);
            var start = new Dictionary<string, long>();
            for (int idx = 0; idx < polymer.Length - 1; ++idx)
            {
                var s = polymer[idx].ToString() + polymer[idx + 1];
                start.AddOrSet(s, 1);
            }

            var result = Enumerable
                .Range(0, 40)
                .Aggregate(start, (s, _) => Age(s, transforms))
                .ToList();
            var charCount = new Dictionary<char, long>();
            foreach (var pair in result)
            {
                charCount.AddOrSet(pair.Key[0], pair.Value);
                charCount.AddOrSet(pair.Key[1], pair.Value);
            }

            foreach (var key in charCount.Keys)
            {
                var pre = (charCount[key] & 1) > 0;
                charCount[key] /= 2;
                if (pre && key == polymer.First())
                {
                    charCount[key] += 1;
                }
                else if (pre && key == polymer.Last())
                {
                    charCount[key] += 1;
                }
            }
            var count = charCount.Values.ToList();
            count.Sort();
            var minimum = count.First();
            var maximum = count.Last();
            var sol = maximum - minimum;
            Console.WriteLine(minimum + ", " + maximum);
            Console.WriteLine("Solution (2): " + sol);
        }
    }
}
