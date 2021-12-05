using System;
using System.Collections.Generic;
using System.Linq;
using AdventOfCode2021.Solutions.Interface;
using AdventOfCode2021.Util;

namespace AdventOfCode2021.Solutions.Day5
{
    public class Day5Solver : ISolvable
    {

        private static string FileName => "Input/Day5_A.input";

        public void Run()
        {
            SolveFirstStar();
            SolveSecondStar();
        }

        private record Point(int X, int Y);

        private Point Parse(string[] point)
        {
            return new Point(int.Parse(point[0]), int.Parse(point[1]));
        }

        private bool HorizontalOrVertical(Point a, Point b)
        {
            return a.X == b.X || a.Y == b.Y;
        }

        private int SolveFor(List<(Point, Point)> segments)
        {
            var matrix = MatrixUtil.NewMatrix(1000, 1000, 0);
            foreach (var (a, b) in segments)
            {
                Mark(matrix, a, b);
            }

            return matrix.SelectMany(x => x.Select(y => y > 1 ? 1 : 0)).Sum();
        }

        private void SolveFirstStar()
        {
            var segments = FileRead.ReadLines(FileName)
                .Select(line => line.Split(" -> "))
                .Select(seg => (Parse(seg[0].Split(",")), Parse(seg[1].Split(","))))
                .Where(points => HorizontalOrVertical(points.Item1, points.Item2))
                .ToList();

            Console.WriteLine("Solution (1): " + SolveFor(segments));
        }

        private void SolveSecondStar()
        {
            var segments = FileRead.ReadLines(FileName)
                .Select(line => line.Split(" -> "))
                .Select(seg => (Parse(seg[0].Split(",")), Parse(seg[1].Split(","))))
                .ToList();

            Console.WriteLine("Solution (2): " + SolveFor(segments));
        }

        private void Mark(List<List<int>> matrix, Point a, Point b)
        {
            var yAdd = a.Y < b.Y ? 1 : a.Y == b.Y ? 0 : -1;
            var xAdd = a.X < b.X ? 1 : a.X == b.X ? 0 : -1;
            for (int row = a.Y, col = a.X; (row, col) != (b.Y, b.X); )
            {
                matrix[row][col] += 1;
                col += xAdd;
                row += yAdd;
            }

            matrix[b.Y][b.X] += 1;
        }



    }
}
