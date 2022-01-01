#nullable enable
using System;
using System.Collections.Generic;
using System.Linq;
using AdventOfCode2021.Solutions.Interface;
using AdventOfCode2021.Util;

namespace AdventOfCode2021.Solutions.Day25
{
    public class Day25Solver : ISolvable
    {
        private static string FileName => "Input/Day25_A.input";

        public void Run()
        {
            SolveFirstStar();
            SolveSecondStar();
        }

        private bool TryMove(List<List<char>> map, out List<List<char>> nextMap)
        {
            var rows = map.NumRows();
            var cols = map.NumCols();
            nextMap = MatrixUtil.NewMatrix(rows, cols, '.');
            var moves = new List<(int, int, char)>()
            {
                (0, 1, '>'),
                (1, 0, 'v')
            };

            var moved = false;
            foreach (var (diffRow, diffCol, cucumber) in moves)
            {
                var selectedCells = map
                    .WhereCells((row, col) => map[row][col] == '>' || map[row][col] == 'v');
                nextMap = MatrixUtil.NewMatrix(rows, cols, '.');
                foreach (var (row, col) in selectedCells)
                {
                    var nextRow = (row + diffRow) % rows;
                    var nextCol = (col + diffCol) % cols;
                    if (map[row][col] == cucumber && map[nextRow][nextCol] == '.')
                    {
                        nextMap[nextRow][nextCol] = map[row][col];
                        moved = true;
                    }
                    else
                    {
                        nextMap[row][col] = map[row][col];
                    }
                }

                map = nextMap;
            }

            return moved;
        }
        
        private void SolveFirstStar()
        {
            var matrix = FileRead.ReadCharMatrix(FileName);
            int cnt;
            for (cnt = 1; TryMove(matrix, out var nextMap); ++cnt)
            {
                matrix = nextMap;
            }
            Console.WriteLine("Solution (1): " + cnt);
        }

        private void SolveSecondStar()
        {
            Console.WriteLine("Solution (2): " + 0);
        }
    }
}