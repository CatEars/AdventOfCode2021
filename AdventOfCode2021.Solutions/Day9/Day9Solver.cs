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
                .Select(pos => matrix[pos.Row][pos.Col] + 1) // Risk level is level of low points + 1
                .Sum();
            Console.WriteLine("Solution (1): " + sol);
        }

        private void SolveSecondStar()
        {
            var matrix = FileRead.ReadCharMatrix(FileName)
                .Map(c => c - '0');

            var lowPoints = FindLowPoints(matrix);
            var basins = lowPoints
                .Select(pos => CalculateBasinSize(matrix, pos.Row, pos.Col))
                .ToList();
            basins.SortDescending();
            Console.WriteLine("Solution (2): " + basins[0] * basins[1] * basins[2]);
        }

        private static IEnumerable<(int Row, int Col)> FindLowPoints(List<List<int>> heightMap)
        {
            return heightMap
                .WhereCells((row, col) =>
                    heightMap
                        .EachAdjacent(row, col)
                        .All(adjacentPos => heightMap[row][col] < heightMap[adjacentPos.Row][adjacentPos.Col])
                );
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
                    .SideEffect(pos => found[pos.Row][pos.Col] = true);
                queue.AddRange(newPositions);
            }

            return queue.Count;
        }
    }
}
