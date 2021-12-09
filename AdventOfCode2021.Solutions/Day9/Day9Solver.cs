using System;
using System.Collections.Generic;
using System.Linq;
using AdventOfCode2021.Solutions.Interface;
using AdventOfCode2021.Util;

namespace AdventOfCode2021.Solutions.Day9
{
    public class Day9Solver : ISolvable
    {

        private static string FileName => "Input/Day9_A.input";

        public void Run()
        {
            SolveFirstStar();
            SolveSecondStar();
        }

        private void SolveFirstStar()
        {
            var matrix = FileRead.ReadCharMatrix(FileName)
                .Map(c => c - '0');

            var lowPoints = FindLowPoints(matrix);
            var sol = lowPoints
                .Flatten()
                .Where(x => x != -1)
                .Select(x => x + 1) // Risk level is level of low points + 1
                .Sum();
            Console.WriteLine("Solution (1): " + sol);
        }

        private void SolveSecondStar()
        {
            var matrix = FileRead.ReadCharMatrix(FileName)
                .Map(c => c - '0');

            var lowPoints = FindLowPoints(matrix);
            var basins = lowPoints
                .Map((row, col, level) => level == -1 ? -1 : CalculateBasinSize(matrix, row, col))
                .Flatten()
                .Where(x => x != -1)
                .ToList();
            basins.Sort();
            var end = basins.Count - 1;
            Console.WriteLine("Solution (2): " + basins[end] * basins[end - 1] * basins[end - 2]);
        }

        private static List<List<int>> FindLowPoints(List<List<int>> matrix)
        {
            var lowPoints = matrix
                .Map((row, col, level) =>
                {
                    return matrix
                        .EachAdjacent(row, col)
                        .Any(pos => matrix[pos.Row][pos.Col] <= level)
                        ? -1
                        : level;
                });
            return lowPoints;
        }

        private int CalculateBasinSize(List<List<int>> matrix, int row, int col)
        {
            var found = MatrixUtil.NewMatrix<bool>(matrix.Count, matrix[0].Count);
            found[row][col] = true;
            var queue = new List<(int Row, int Col)>() { (row, col) };
            for (var idx = 0; idx < queue.Count; ++idx)
            {
                var (r, c) = queue[idx];
                var newPositions = matrix
                    .EachAdjacent(r, c)
                    .Where(pos => !found[pos.Row][pos.Col] && matrix[pos.Row][pos.Col] < 9)
                    .SideEffect(pos => found[pos.Row][pos.Col] = true)
                    .ToList();
                queue.AddRange(newPositions);
            }

            return queue.Count;
        }
    }
}
