#nullable enable
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using AdventOfCode2021.Solutions.Interface;
using AdventOfCode2021.Util;

namespace AdventOfCode2021.Solutions.Day24
{
    public class Day24Solver : ISolvable
    {
        private static string FileName => "Input/Day24_A.input";

        private static List<List<(int Command, string Variable, string Value)>> ProgramBlocks = new();

        private static void StoreProgram(string filepath)
        {
            var lines = FileRead.ReadLines(filepath);
            var functionPointer = new List<string>()
            {
                "add",
                "mul",
                "div",
                "mod",
                "eql"
            };
            foreach (var line in lines)
            {
                if (line.StartsWith("inp"))
                {
                    ProgramBlocks.Add(new List<(int, string, string)>());
                }
                else
                {
                    var statement = line.Trim().Split(" ");
                    ProgramBlocks.Last().Add((functionPointer.IndexOf(statement[0]), statement[1], statement[2]));
                }
            }
        }

        private static List<Func<long, long, long>> Operations = new()
        {
            (a, b) => a + b,
            (a, b) => a * b,
            (a, b) => a / b,
            (a, b) => a % b,
            (a, b) => a == b ? 1 : 0
        };

        private static List<long> TenMul = new()
        {
            0L,
            1L,
            10L,
            100L,
            1000L,
            10000L,
            100000L,
            1000000L,
            10000000L,
            100000000L,
            1000000000L,
            10000000000L,
            100000000000L,
            1000000000000L,
            10000000000000L,
            100000000000000L
        };

        public void Run()
        {
            StoreProgram(FileName);
            SolveFirstStar();
            SolveSecondStar();
        }
        
        private static long NewZ(long zcarry, long testedNumber, int blockIdx)
        {
            var storage = new Dictionary<string, long>()
            {
                {"z", zcarry},
                {"w", testedNumber},
                {"x", 0},
                {"y", 0},
            };

            long ResolveValue(string x)
            {
                return long.TryParse(x, out var res) ? res : storage[x];
            }

            foreach (var (command, variable, val) in ProgramBlocks[blockIdx])
            {
                var value = ResolveValue(val);
                storage[variable] = Operations[command](storage[variable], value);
            }

            return storage["z"];
        }

        private static long Solve(List<int> range)
        {
            var cache = new Dictionary<(long, int), long>();

            long RecursiveSolve(long zCarry, int blockIdx)
            {
                if (blockIdx >= 14)
                {
                    return zCarry == 0 ? 0 : -1;
                } 
                else if (zCarry > 26L * 26 * 26 * 26 * 26)
                {
                    return -1;
                }

                var key = (zCarry, blockIdx);
                if (cache.ContainsKey(key))
                {
                    return cache[key];
                }

                foreach (var testedNum in range)
                {
                    var nextZ = NewZ(zCarry, testedNum, blockIdx);
                    var lowerValue = RecursiveSolve(nextZ, blockIdx + 1);
                    if (lowerValue != -1)
                    {
                        var tenMult = TenMul[14 - blockIdx];
                        var finalValue = tenMult * testedNum + lowerValue;
                        return cache[key] = finalValue;
                    }
                }

                return cache[key] = -1;
            }

            return RecursiveSolve(0, 0);
        }


        private static void SolveFirstStar()
        {
            var order = Enumerable.Range(1, 9).Reverse().ToList();
            Console.WriteLine("Solution (1): " + Solve(order));
        }

        private static void SolveSecondStar()
        {
            var order = Enumerable.Range(1, 9).ToList();
            Console.WriteLine("Solution (2): " + Solve(order));
        }
    }
}