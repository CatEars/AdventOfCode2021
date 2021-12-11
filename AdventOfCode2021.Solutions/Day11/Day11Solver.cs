using System;
using System.Collections.Generic;
using System.Linq;
using AdventOfCode2021.Solutions.Interface;
using AdventOfCode2021.Util;

namespace AdventOfCode2021.Solutions.Day11
{
    public class Day11Solver : ISolvable
    {

        private static string FileName => "Input/Day11_A.input";

        public void Run()
        {
            SolveFirstStar();
            SolveSecondStar();
        }

        private void SolveFirstStar()
        {
            var energyMap = FileRead.ReadCharMatrix(FileName)
                .Map(x => x - '0');

            var solution = Enumerable.Range(0, 100)
                .Aggregate(0, (count, _) => count + Age(energyMap));
            Console.WriteLine("Solution (1): " + solution);
        }

        private void SolveSecondStar()
        {
            var energyMap = FileRead.ReadCharMatrix(FileName)
                .Map(x => x - '0');
            var idx = Enumerable.Range(0, 1000)
                .First(_ => Age(energyMap) == energyMap.Size());
            var solution = idx + 1;
            Console.WriteLine("Solution (2): " + solution);
        }

        private int Age(List<List<int>> energyMap)
        {
            energyMap.Apply(x => x + 1);
            return FlashTheOctopuses(energyMap);
        }

        private static IEnumerable<(int Row, int Col)> FindFlashingOctopuses(List<List<int>> energyMap)
        {
            return energyMap
                .WhereCells((row, col) => energyMap[row][col] > 9);
        }

        private int FlashTheOctopuses(List<List<int>> matrix)
        {
            var hasFlashed = MatrixUtil.NewMatrix<bool>(matrix.Count, matrix[0].Count);
            var startFlashes = FindFlashingOctopuses(matrix)
                .SideEffect(pos => hasFlashed[pos.Row][pos.Col] = true)
                .ToList();

            var flashes = new List<(int Row, int Col)>();
            flashes.AddRange(startFlashes);
            for (var idx = 0; idx < flashes.Count; ++idx)
            {
                var (row, col) = flashes[idx];
                hasFlashed[row][col] = true;
                var newFlashes = matrix
                    .EachAdjacentAndDiagonal(row, col)
                    .SideEffect(pos => matrix[pos.Row][pos.Col] += 1)
                    .Where(pos => !hasFlashed[pos.Row][pos.Col] && matrix[pos.Row][pos.Col] > 9)
                    .SideEffect(pos => hasFlashed[pos.Row][pos.Col] = true);
                flashes.AddRange(newFlashes);
            }

            foreach (var (row, col) in flashes)
            {
                matrix[row][col] = 0;
            }

            return flashes.Count;
        }
    }
}
