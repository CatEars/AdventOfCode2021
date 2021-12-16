using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using AdventOfCode2021.Solutions.Interface;
using AdventOfCode2021.Util;

namespace AdventOfCode2021.Solutions.Day16
{
    public class Day16Solver : ISolvable
    {

        private static string FileName => "Input/Day16_A.input";

        public void Run()
        {
            SolveFirstStar();
            SolveSecondStar();
        }

        private class BitStream
        {
            public List<char> Characters { get; set; }

            public int ProgramCounter { get; set; }

            public IEnumerable<char> Take(int count)
            {
                var chars = Characters
                    .Skip(ProgramCounter)
                    .Take(count);

                ProgramCounter += count;
                return chars;
            }

            public long TakeInt(int count)
            {
                return Take(count).Pipe(BitsToNum);
            }

            public int VersionCounter { get; set; } = 0;

            public int Indent { get; set; } = 0;

            public Stack<long> Stack { get; set; } = new();
        }

        public static long BitsToNum(IEnumerable<char> chars)
        {
            return chars
                .Pipe(IntoString)
                .Pipe(BinaryUtil.BinaryToDecimal);
        }

        private void SolveFirstStar()
        {
            var fullPacket = File.ReadAllText(FileName).Trim();
            var stream = IntoBitStream(fullPacket).ToList();
            var bitStream = new BitStream()
            {
                Characters = stream,
                ProgramCounter = 0
            };
            Parse(bitStream);
            var sol = bitStream.VersionCounter;
            Console.WriteLine("Solution (1): " + sol);
        }

        private void SolveSecondStar()
        {
            var fullPacket = File.ReadAllText(FileName).Trim();
            var stream = IntoBitStream(fullPacket).ToList();
            var bitStream = new BitStream()
            {
                Characters = stream,
                ProgramCounter = 0
            };
            Parse(bitStream);
            var sol = bitStream.Stack.Peek();
            Console.WriteLine("Solution (2): " + sol);
        }

        private void Parse(BitStream bitStream)
        {
            var version = bitStream.TakeInt(3);
            bitStream.VersionCounter += (int) version;
            var typeId = bitStream.TakeInt(3);
            bitStream.Indent += 1;
            if (typeId == 4)
            {
                ParseLiteral(bitStream);
            }
            else
            {
                var operatorFuncs = new Dictionary<int, Func<long, long, long>>()
                {
                    { 0, (a, b) => a + b },
                    { 1, (a, b) => a * b },
                    { 2, Math.Min },
                    { 3, Math.Max },
                    { 5, (a, b) => a > b ? 1 : 0 },
                    { 6, (a, b) => a < b ? 1 : 0 },
                    { 7, (a, b) => a == b ? 1 : 0},
                };

                ParseOperator(bitStream, operatorFuncs[(int)typeId]);
            }
            bitStream.Indent -= 1;
        }

        private void ParseOperator(BitStream bitStream, Func<long, long, long> operatorFunc)
        {
            var lengthTypeId = bitStream.TakeInt(1);

            if (lengthTypeId == 0)
            {
                var numBitsForSubPackages = bitStream.TakeInt(15);
                var expectedEnd = bitStream.ProgramCounter + numBitsForSubPackages;
                bitStream.Indent += 1;
                Parse(bitStream);
                var first = bitStream.Stack.Pop();
                while (bitStream.ProgramCounter < expectedEnd)
                {
                    Parse(bitStream);
                    var second = bitStream.Stack.Pop();
                    first = operatorFunc(first, second);
                }
                bitStream.Stack.Push(first);
                bitStream.Indent -= 1;
            }
            else
            {
                var numSubPackages = bitStream.TakeInt(11);
                bitStream.Indent += 1;
                Parse(bitStream);
                var first = bitStream.Stack.Pop();
                for (var idx = 1; idx < numSubPackages; ++idx)
                {
                    Parse(bitStream);
                    var second = bitStream.Stack.Pop();
                    first = operatorFunc(first, second);
                }
                bitStream.Stack.Push(first);
                bitStream.Indent -= 1;
            }
        }

        private void ParseLiteral(BitStream bitStream)
        {
            char continueBit;
            var number = "";
            do
            {
                continueBit = bitStream.Take(1).First();

                number += bitStream
                    .Take(4)
                    .Pipe(IntoString);
            } while (continueBit == '1');

            bitStream.Stack.Push(BinaryUtil.BinaryToDecimal(number));
        }

        private static string IntoString(IEnumerable<char> s)
        {
            return string.Join("", s);
        }

        private IEnumerable<char> IntoBitStream(string fullPacket)
        {
            var convert = new Dictionary<char, List<char>>()
            {
                { '0', new List<char>() { '0', '0', '0', '0' } },
                { '1', new List<char>() { '0', '0', '0', '1' } },
                { '2', new List<char>() { '0', '0', '1', '0' } },
                { '3', new List<char>() { '0', '0', '1', '1' } },
                { '4', new List<char>() { '0', '1', '0', '0' } },
                { '5', new List<char>() { '0', '1', '0', '1' } },
                { '6', new List<char>() { '0', '1', '1', '0' } },
                { '7', new List<char>() { '0', '1', '1', '1' } },
                { '8', new List<char>() { '1', '0', '0', '0' } },
                { '9', new List<char>() { '1', '0', '0', '1' } },
                { 'A', new List<char>() { '1', '0', '1', '0' } },
                { 'B', new List<char>() { '1', '0', '1', '1' } },
                { 'C', new List<char>() { '1', '1', '0', '0' } },
                { 'D', new List<char>() { '1', '1', '0', '1' } },
                { 'E', new List<char>() { '1', '1', '1', '0' } },
                { 'F', new List<char>() { '1', '1', '1', '1' } }
            };
            var result = new List<char>();
            foreach (var c in fullPacket)
            {
                result.AddRange(convert[c]);
            }

            return result;
        }

    }
}
