using System;
using System.Collections.Generic;
using System.Linq;
using AdventOfCode2021.Solutions.Interface;
using AdventOfCode2021.Util;

namespace AdventOfCode2021.Solutions.Day3
{
    public class Day3Solver : ISolvable
    {

        private static string FileName => "Input/Day3_A.input";

        public void Run()
        {
            SolveFirstStar();
            SolveSecondStar();
        }

        private void SolveFirstStar()
        {
            var matrix = FileRead.ReadLines(FileName);
            var half = (matrix.Count >> 1) + (matrix.Count & 1);

            var numOnesPerColumn = Enumerable
                .Range(0, matrix[0].Length)
                .Select(col => matrix.Count(line => line[col] == '1'));

            var gamma = numOnesPerColumn
                .Select(numOnes => numOnes >= half ? 1 : 0)
                .StringConcat()
                .Pipe(BinaryUtil.BinaryToDecimal);

            var epsilon = BinaryUtil.BinaryInvert(gamma, matrix[0].Length);
            Console.WriteLine("Solution (1): " + gamma * epsilon);
        }

        private void SolveSecondStar()
        {
            var matrix = FileRead.ReadLines(FileName);
            var oxygen = FindLevel(matrix, (zeroes, ones) => zeroes >  ones ? '0' : '1');
            var carbon = FindLevel(matrix, (zeroes, ones) => zeroes <= ones ? '0' : '1');
            Console.WriteLine("Solution (2): " + oxygen * carbon);
        }

        private int FindLevel(List<string> matrix, Func<int, int, char> bitCriteria)
        {
            var keptRows = matrix;
            var col = 0;
            while (keptRows.Count > 1)
            {
                var ones = keptRows.Count(line => line[col] == '1');
                var zeros = keptRows.Count - ones;
                var charToKeep = bitCriteria(zeros, ones);
                keptRows = keptRows
                    .Where(line => line[col] == charToKeep)
                    .ToList();
                col++;
            }

            return BinaryUtil.BinaryToDecimal(keptRows[0].StringConcat());
        }
    }
}
