using System;
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

        private record Point(int Row, int Col);


        private Point ConvertToPoint(string arg)
        {
            var split = arg.Split(",");
            return new Point(int.Parse(split[1]), int.Parse(split[0]));
        }

        private void SolveFirstStar()
        {
            var lines = FileRead.ReadLines(FileName);
            var (folds, pointsList) = lines.Partition(line => line.StartsWith("fold along"));
            var points = pointsList
                .Where(x => x.Trim().Any())
                .Select(ConvertToPoint)
                .ToList();
            var maxRows = MathUtil.Max(points.Select(p => p.Row).ToArray()) + 1;
            var maxCols = MathUtil.Max(points.Select(p => p.Col).ToArray()) + 1;
            var matrix = MatrixUtil.NewMatrix(maxRows, maxCols, false);
            foreach (var point in points)
            {
                matrix[point.Row][point.Col] = true;
            }

            var currentMaxRow = maxRows;
            var currentMaxCol = maxCols;
            foreach (var fold in folds)
            {
                if (fold.Contains("y="))
                {
                    // fold up
                    var nextMaxRow = int.Parse(fold.Split("y=")[1]);
                    for (var row = nextMaxRow; row < currentMaxRow; ++row)
                    {
                        for (var col = 0; col < currentMaxCol; ++col)
                        {
                            if (matrix[row][col])
                            {
                                var distanceFromMiddle = row - nextMaxRow;
                                var endingRow = nextMaxRow - distanceFromMiddle;
                                matrix[endingRow][col] = true;
                            }
                        }
                    }

                    currentMaxRow = nextMaxRow;
                }
                else
                {
                    // fold left
                    var nextMaxCol = int.Parse(fold.Split("x=")[1]);
                    for (var row = 0; row < currentMaxRow; ++row)
                    {
                        for (var col = nextMaxCol; col < currentMaxCol; ++col)
                        {
                            if (matrix[row][col])
                            {
                                var distanceFromMiddle = col - nextMaxCol;
                                var endingCol = nextMaxCol - distanceFromMiddle;
                                matrix[row][endingCol] = true;
                            }
                        }
                    }

                    currentMaxCol = nextMaxCol;
                }

                break;
            }

            var sol = matrix
                .WhereCells((row, col) => row < currentMaxRow && col < currentMaxCol && matrix[row][col])
                .Count();

            Console.WriteLine("Solution (1): " + sol);
        }

        private void SolveSecondStar()
        {
            var lines = FileRead.ReadLines(FileName);
            var (folds, pointsList) = lines.Partition(line => line.StartsWith("fold along"));
            var points = pointsList
                .Where(x => x.Trim().Any())
                .Select(ConvertToPoint)
                .ToList();
            var maxRows = MathUtil.Max(points.Select(p => p.Row).ToArray()) + 1;
            var maxCols = MathUtil.Max(points.Select(p => p.Col).ToArray()) + 1;
            var matrix = MatrixUtil.NewMatrix(maxRows, maxCols, false);
            foreach (var point in points)
            {
                matrix[point.Row][point.Col] = true;
            }

            var currentMaxRow = maxRows;
            var currentMaxCol = maxCols;
            foreach (var fold in folds)
            {
                if (fold.Contains("y="))
                {
                    // fold up
                    var nextMaxRow = int.Parse(fold.Split("y=")[1]);
                    for (var row = nextMaxRow; row < currentMaxRow; ++row)
                    {
                        for (var col = 0; col < currentMaxCol; ++col)
                        {
                            if (matrix[row][col])
                            {
                                var distanceFromMiddle = row - nextMaxRow;
                                var endingRow = nextMaxRow - distanceFromMiddle;
                                matrix[endingRow][col] = true;
                            }
                        }
                    }

                    currentMaxRow = nextMaxRow;
                }
                else
                {
                    // fold left
                    var nextMaxCol = int.Parse(fold.Split("x=")[1]);
                    for (var row = 0; row < currentMaxRow; ++row)
                    {
                        for (var col = nextMaxCol; col < currentMaxCol; ++col)
                        {
                            if (matrix[row][col])
                            {
                                var distanceFromMiddle = col - nextMaxCol;
                                var endingCol = nextMaxCol - distanceFromMiddle;
                                matrix[row][endingCol] = true;
                            }
                        }
                    }

                    currentMaxCol = nextMaxCol;
                }
            }

            Console.WriteLine("Solution (2): ");
            for (int row = 0; row < currentMaxRow; ++row)
            {
                for (int col = 0; col < currentMaxCol; ++col)
                {
                    if (matrix[row][col])
                    {
                        Console.Write('#');
                    }
                    else
                    {
                        Console.Write('.');
                    }
                }

                Console.WriteLine();
            }

        }

    }
}
