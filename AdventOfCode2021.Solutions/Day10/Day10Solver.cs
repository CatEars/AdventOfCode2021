using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AdventOfCode2021.Solutions.Interface;
using AdventOfCode2021.Util;

namespace AdventOfCode2021.Solutions.Day10
{
    public class Day10Solver : ISolvable
    {

        private static string FileName => "Input/Day10_A.input";

        private static Dictionary<char, char> EndToStart { get; } = new()
        {
            {')', '('},
            {']', '['},
            {'}', '{'},
            {'>', '<'},
        };

        private static Dictionary<char, char> StartToEnd { get; } = EndToStart.Reverse();

        private static Dictionary<string, int> SyntaxScoring { get; } = new()
        {
            { ")", 3 },
            { "]", 57 },
            { "}", 1197 },
            { ">", 25137 }
        };

        public void Run()
        {
            SolveFirstStar();
            SolveSecondStar();
        }

        private void SolveFirstStar()
        {
            var lines = FileRead.ReadLines(FileName);
            var solution = lines
                .Select(FindFirstCorruptOrNull)
                .Where(character => character != null)
                .Select(character => SyntaxScoring[character])
                .Sum();
            Console.WriteLine("Solution (1): " + solution);
        }

        private void SolveSecondStar()
        {
            var lines = FileRead.ReadLines(FileName);
            var scores = lines
                .Where(line => FindFirstCorruptOrNull(line) == null)
                .Select(FindLineCompletion)
                .Select(ScoreLineCompletion)
                .ToList();
            scores.Sort();
            var solution = scores[scores.Count / 2];
            Console.WriteLine("Solution (2): " + solution);
        }


        private string FindFirstCorruptOrNull(string line)
        {
            var stack = new Stack<char>();
            foreach (var character in line)
            {
                if (StartToEnd.ContainsKey(character))
                {
                    stack.Push(character);
                    continue;
                }
                // assume character is ending character

                if (stack.Empty())
                {
                    return character.ToString();
                }

                if (stack.Peek() == EndToStart[character])
                {
                    stack.Pop();
                }
                else if (stack.Peek() != EndToStart[character])
                {
                    return character.ToString();
                }
            }

            return null;
        }

        private string FindLineCompletion(string line)
        {
            var stack = new Stack<char>();
            foreach (var character in line)
            {
                if (StartToEnd.ContainsKey(character))
                {
                    stack.Push(character);
                }
                else
                {
                    // assume character is ending key
                    stack.Pop();
                }
            }

            var builder = new StringBuilder();
            while (stack.Any())
            {
                var curr = stack.Pop();
                builder.Append(StartToEnd[curr]);
            }

            return builder.ToString();
        }

        private long ScoreLineCompletion(string line)
        {
            var points = new List<char>() { ')', ']', '}', '>' };
            var sum = 0L;
            foreach (var c in line)
            {
                sum *= 5L;
                sum += points.IndexOf(c) + 1;
            }

            return sum;
        }

    }
}
