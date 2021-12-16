using System;
using System.Collections.Generic;
using System.Linq;
using AdventOfCode2021.Solutions.Interface;
using AdventOfCode2021.Util;
using Priority_Queue;

namespace AdventOfCode2021.Solutions.Day15
{
    public class Day15Solver : ISolvable
    {

        private static string FileName => "Input/Day15_A.input";

        public void Run()
        {
            SolveFirstStar();
            SolveSecondStar();
        }

        private void SolveFirstStar()
        {
            var matrix = FileRead
                .ReadCharMatrix(FileName)
                .Map(x => int.Parse(x.ToString()));
            matrix[0][0] = 0;
            var dist = CalculateDistanceWithDijkstra(matrix);
            var sol = dist[0][0];
            Console.WriteLine("Solution (1): " + sol);
        }

        private static List<List<int>> CalculateDistanceWithDijkstra(List<List<int>> matrix)
        {
            var dist = matrix.Map(x => 100000000);
            var visited = matrix.Map(x => false);
            var startRow = matrix.NumRows() - 1;
            var startCol = matrix.NumCols() - 1;
            dist[startRow][startCol] = matrix[startRow][startCol];
            var queue = new SimplePriorityQueue<(int Row, int Col)>();
            queue.Enqueue((startRow, startCol), 0);
            while (queue.Any())
            {
                var element = queue.Dequeue();
                var (row, col) = element;
                if (visited[row][col]) continue;
                var now = dist[row][col];
                foreach (var (nextRow, nextCol) in matrix.EachAdjacent(row, col))
                {
                    if (!visited[nextRow][nextCol])
                    {
                        var nextDist = now + matrix[nextRow][nextCol];

                        if (dist[nextRow][nextCol] > nextDist)
                        {
                            dist[nextRow][nextCol] = nextDist;
                            queue.Enqueue((nextRow, nextCol), nextDist);
                        }
                    }
                }
            }

            return dist;
        }

        private void SolveSecondStar()
        {
            var matrix = FileRead
                .ReadCharMatrix(FileName)
                .Map(x => int.Parse(x.ToString()));
            var realMatrix = MatrixUtil.NewMatrix<int>(matrix.NumRows() * 5, matrix.NumCols() * 5);
            for (var row = 0; row < 5; ++row)
            {
                for (var col = 0; col < 5; ++col)
                {
                    var added = row + col;
                    var extraRow = row * matrix.NumRows();
                    var extraCol = col * matrix.NumCols();
                    var updatedProjection = matrix.Map(element => ((element - 1 + added) % 9) + 1);
                    updatedProjection.ProjectOnto(realMatrix, extraRow, extraCol);
                }
            }
            realMatrix[0][0] = 0;
            var dist = CalculateDistanceWithDijkstra(realMatrix);
            var sol = dist[0][0];
            Console.WriteLine("Solution (2): " + sol);
        }
    }
}
