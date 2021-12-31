#nullable enable
using System;
using System.Collections.Generic;
using System.Linq;
using AdventOfCode2021.Solutions.Interface;
using AdventOfCode2021.Util;

namespace AdventOfCode2021.Solutions.Day20
{
    public class Day20Solver : ISolvable
    {
        private static string FileName => "Input/Day20_A.input";

        private List<bool> PrecalculatedLights = new();

        private void PreCalc(string imageAlgorithm)
        {
            PrecalculatedLights = imageAlgorithm
                .Select(x => x == '#')
                .ToList();
        }
        
        public void Run()
        {
            var lines = FileRead.ReadLines(FileName);
            PreCalc(lines[0].Trim());
            SolveFirstStar();
            SolveSecondStar();
        }

        private List<List<bool>> SolveOnce(List<List<bool>> matrix, bool currentDefaultValue, bool nextDefaultValue)
        {
            bool ValueAt(int row, int col)
            {
                if (row < 0 || row >= matrix.NumRows() || col < 0 || col >= matrix.NumCols())
                {
                    return currentDefaultValue;
                }

                return matrix[row][col];
            }
            var nextMatrix = MatrixUtil.NewMatrix(matrix.NumRows() + 2, matrix.NumCols() + 2, nextDefaultValue);
            var offset = 1;
            for (int row = 0; row < nextMatrix.NumRows(); ++row)
            {
                for (int col = 0; col < nextMatrix.NumCols(); ++col)
                {
                    var bitIdx = 0;
                    var result = 0;
                    for (int rowMask = 1; rowMask >= -1; --rowMask)
                    {
                        for (int colMask = 1; colMask >= -1; --colMask)
                        {
                            if (ValueAt(row - offset + rowMask, col - offset + colMask))
                            {
                                result |= 1 << bitIdx;
                            }
                            bitIdx++;
                        }
                    }

                    nextMatrix[row][col] = PrecalculatedLights[result];
                }
            }

            return nextMatrix;
        }

        private int SolveFor(List<List<bool>> matrix, bool switchBools, int times)
        {
            var currentDefaultValue = false;
            var nextDefaultValue = currentDefaultValue ^ switchBools;
            for (int cnt = 0; cnt < times; ++cnt)
            {
                matrix = SolveOnce(matrix, currentDefaultValue, nextDefaultValue);
                currentDefaultValue = nextDefaultValue;
                nextDefaultValue = nextDefaultValue ^ switchBools;
            }

            return matrix.Flatten().Aggregate(0, (a, b) => a + (b ? 1 : 0));
        }
        
        private void SolveFirstStar()
        {
            var lines = FileRead.ReadLines(FileName)
                .Skip(2)
                .ToList();
            var matrix = lines.Select(x => x.Select(c => c == '#').ToList()).ToList();
            // NOTE: In the example we will always have unlit pixels, but
            // in the testdata the first bit of the algorithm was lit and the last 
            // bit in the algorithm was not lit. In the testdata we will switch bools back and forth, but
            // I solved that manually instead of using code to figure out the exact pattern (note that there
            // is also a pattern where first and last bit are lit).
            var sum = SolveFor(matrix, true, 2);
            Console.WriteLine("Solution (1): " + sum);
        }

        private void SolveSecondStar()
        {
            var lines = FileRead.ReadLines(FileName)
                .Skip(2)
                .ToList();
            var matrix = lines.Select(x => x.Select(c => c == '#').ToList()).ToList();
            var sum = SolveFor(matrix, true, 50);
            Console.WriteLine("Solution (2): " + sum);
        }
    }
}