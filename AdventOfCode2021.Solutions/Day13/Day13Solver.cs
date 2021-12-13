using System;
using System.Collections.Generic;
using System.Linq;
using AdventOfCode2021.Solutions.Interface;
using AdventOfCode2021.Util;

namespace AdventOfCode2021.Solutions.Day13
{
    public class Day13Solver : ISolvable
    {

        private static string FileName => "Input/Day13_A.input";

        public void Run()
        {
            SolveFirstStar();
            SolveSecondStar();
        }

        private void SolveFirstStar()
        {
            var (folds, points) = ReadInput(FileName);
            var paper = GeneratePaperFromPoints(points);
            paper = Fold(paper, folds.First());

            var sol = paper
                .Flatten()
                .Count(x => x);

            Console.WriteLine("Solution (1): " + sol);
        }

        private void SolveSecondStar()
        {
            var (folds, points) = ReadInput(FileName);
            var paper = GeneratePaperFromPoints(points);
            foreach (var fold in folds)
            {
                paper = Fold(paper, fold);
            }

            var solution = paper;
            Console.WriteLine("Solution (2): ");
            foreach (var (row, col) in solution.Enumerate())
            {
                var printChar = paper[row][col] ? "■" : " ";
                Console.Write(printChar);
                if (col == solution.NumCols() - 1)
                {
                    Console.WriteLine();
                }
            }
        }

        private List<List<bool>> Fold(List<List<bool>> paper, string fold)
        {
            if (fold.Contains("y="))
            {
                // fold up
                var nextMaxRow = int.Parse(fold.Split("y=")[1]);
                var pointsAffectedByFold = paper
                    .WhereCells((row, col) => row >= nextMaxRow && paper[row][col]);
                foreach (var (row, col) in pointsAffectedByFold)
                {
                    var distanceFromMiddle = row - nextMaxRow;
                    var endingRow = nextMaxRow - distanceFromMiddle;
                    paper[endingRow][col] = true;
                }
                return paper.Portion(nextMaxRow, paper.NumCols());
            }
            else
            {
                // fold left
                var nextMaxCol = int.Parse(fold.Split("x=")[1]);
                var pointsAffectedByFold = paper
                    .WhereCells((row, col) => col >= nextMaxCol && paper[row][col]);
                foreach (var (row, col) in pointsAffectedByFold)
                {
                    var distanceFromMiddle = col - nextMaxCol;
                    var endingCol = nextMaxCol - distanceFromMiddle;
                    paper[row][endingCol] = true;
                }

                return paper.Portion(paper.NumRows(), nextMaxCol);
            }
        }

        private record Point(int Row, int Col);

        private Point ConvertToPoint(string arg)
        {
            var split = arg.Split(",");
            return new Point(int.Parse(split[1]), int.Parse(split[0]));
        }

        private (List<string> Folds, List<Point> Points) ReadInput(string filepath)
        {
            var lines = FileRead.ReadLines(filepath);
            var (folds, pointsList) = lines.Partition(line => line.StartsWith("fold along"));
            var points = pointsList
                .Where(x => x.Trim().Any())
                .Select(ConvertToPoint)
                .ToList();
            return (folds, points);
        }

        private static List<List<bool>> GeneratePaperFromPoints(List<Point> points)
        {
            var maxRows = MathUtil.Max(points.Select(p => p.Row).ToArray()) + 1;
            var maxCols = MathUtil.Max(points.Select(p => p.Col).ToArray()) + 1;
            var matrix = MatrixUtil.NewMatrix(maxRows, maxCols, false);
            foreach (var (row, col) in points)
            {
                matrix[row][col] = true;
            }

            return matrix;
        }
    }
}
