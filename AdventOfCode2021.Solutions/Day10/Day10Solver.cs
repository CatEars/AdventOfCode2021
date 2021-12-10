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

        public void Run()
        {
            SolveFirstStar();
            SolveSecondStar();
        }

        private string FindFirstCorruptOrNull(string line)
        {
            var stack = new Stack<char>();
            var ender = new HashSet<char>() { ')', ']', '}', '>' };
            var match = new Dictionary<char, char>()
            {
                {')', '('},
                {']', '['},
                {'}', '{'},
                {'>', '<'},
            };
            foreach (var character in line)
            {
                if (!ender.Contains(character))
                {
                    stack.Push(character);
                }
                else if (stack.Empty() && ender.Contains(character))
                {
                    return character.ToString();
                }
                else if (!stack.Empty() && match.ContainsKey(character) && stack.Peek() == match[character])
                {
                    stack.Pop();
                }
                else if (!stack.Empty() && match.ContainsKey(character) && stack.Peek() != match[character])
                {
                    return character.ToString();
                }
            }

            return null;
        }

        private string FindCompletion(string line)
        {
            var stack = new Stack<char>();
            var ender = new HashSet<char>() { ')', ']', '}', '>' };
            var match = new Dictionary<char, char>()
            {
                {')', '('},
                {']', '['},
                {'}', '{'},
                {'>', '<'},
            };
            foreach (var character in line)
            {
                if (!ender.Contains(character))
                {
                    stack.Push(character);
                }
                else if (!stack.Empty() && match.ContainsKey(character) && stack.Peek() == match[character])
                {
                    stack.Pop();
                }
                else
                {
                    throw new Exception("Wrong assumptions for " + line);
                }
            }

            var oppositeMatch = new Dictionary<char, char>()
            {
                { '(', ')' },
                { '[', ']' },
                { '{', '}' },
                { '<', '>' },
            };
            var builder = new StringBuilder();
            while (stack.Any())
            {
                var curr = stack.Pop();
                builder.Append(oppositeMatch[curr]);
            }

            return builder.ToString();
        }

        private int ToHighScore(string arg)
        {
            if (arg == ")")
            {
                return 3;
            }
            else if (arg == "]")
            {
                return 57;
            }
            else if (arg == "}")
            {
                return 1197;
            }
            else if (arg == ">")
            {
                return 25137;
            }

            throw new Exception($"bad character: {arg}");
        }

        private void SolveFirstStar()
        {
            var lines = FileRead.ReadLines(FileName);
            var solution = lines
                .Select(FindFirstCorruptOrNull)
                .Where(character => character != null)
                .Select(ToHighScore)
                .Sum();
            Console.WriteLine("Solution (1): " + solution);
        }

        private long ScoreLine(string line)
        {
            var points = new Dictionary<char, long>()
            {
                { ')', 1 },
                { ']', 2 },
                { '}', 3 },
                { '>', 4 }
            };
            var sum = 0L;
            foreach (var c in line)
            {
                sum *= 5L;
                sum += points[c];
            }

            return sum;
        }

        private void SolveSecondStar()
        {
            var lines = FileRead.ReadLines(FileName);
            var scores = lines
                .Where(line => FindFirstCorruptOrNull(line) == null)
                .Select(FindCompletion)
                .Select(ScoreLine)
                .ToList();
            scores.Sort();
            var solution = scores[scores.Count / 2];
            Console.WriteLine("Solution (2): " + solution);
        }
    }
}
