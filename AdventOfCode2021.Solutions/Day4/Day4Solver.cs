using System;
using System.Collections.Generic;
using System.Linq;
using AdventOfCode2021.Solutions.Interface;
using AdventOfCode2021.Util;

namespace AdventOfCode2021.Solutions.Day4
{
    public class Day4Solver : ISolvable
    {

        private static string FileName => "Input/Day4_A.input";

        public void Run()
        {
            SolveFirstStar();
            SolveSecondStar();
        }

        private record BingoNumbers(List<int> Numbers);

        private record BingoBoard(List<List<int>> Board);

        private (BingoNumbers, List<BingoBoard>) ReadInput(string filepath)
        {
            var lines = FileRead.ReadLines(filepath);
            var numbers = lines[0];
            var nums = numbers
                .Split(",")
                .Select(x => x.Trim())
                .Where(x => !string.IsNullOrWhiteSpace(x))
                .Select(int.Parse)
                .ToList()
                .Pipe(x => new BingoNumbers(x));

            var boards = new List<BingoBoard>();
            var idx = 1;
            while (idx + 5 < lines.Count)
            {
                idx++; // skip empty line
                var indexes = Enumerable.Range(idx, 5);
                var board = indexes
                    .Select(index => lines[index])
                    .Select(line => StringUtil.IntoValidInts(line))
                    .ToList()
                    .Pipe(x => new BingoBoard(x));

                boards.Add(board);
                idx += 5;
            }

            return (nums, boards);
        }

        private List<List<bool>> NewMarkingPlate()
        {
            var marks = new List<List<bool>>();
            for (var row = 0; row < 5; ++row)
            {
                marks.Add(new List<bool>());
                for (var col = 0; col < 5; ++col)
                {
                    marks[row].Add(false);
                }
            }

            return marks;
        }

        private bool HasWon(List<List<bool>> marks)
        {
            var cols = Enumerable.Range(0, 5);
            var rows = Enumerable.Range(0, 5);

            return cols.Any(col => Enumerable.Range(0, 5).All(row => marks[row][col])) ||
                   rows.Any(row => Enumerable.Range(0, 5).All(col => marks[row][col]));
        }

        private int SumUnmarked(BingoBoard board, List<List<bool>> marks)
        {
            var rows = Enumerable.Range(0, 5);
            return rows
                .Select(row =>
                    Enumerable.Range(0, 5)
                        .Sum(col => marks[row][col] ? 0 : board.Board[row][col]))
                .Sum();
        }

        private void Mark(BingoBoard board, List<List<bool>> marks, int calledNumber)
        {
            for (var row = 0; row < 5; ++row)
            for (var col = 0; col < 5; ++col)
            {
                if (board.Board[row][col] == calledNumber)
                {
                    marks[row][col] = true;
                    return;
                }
            }
        }

        private int SolveFirst()
        {
            var (nums, boards) = ReadInput(FileName);
            var marks = boards.Select(_ => NewMarkingPlate()).ToList();
            for (int idx = 0; idx < nums.Numbers.Count; idx++)
            {
                var number = nums.Numbers[idx];
                for (var boardIdx = 0; boardIdx < boards.Count; ++boardIdx)
                {
                    var board = boards[boardIdx];
                    Mark(board, marks[boardIdx], number);
                    if (HasWon(marks[boardIdx]))
                    {
                        return number * SumUnmarked(board, marks[boardIdx]);
                    }
                }
            }

            throw new Exception("No winner??");
        }

        private int SolveSecond()
        {
            var (nums, boards) = ReadInput(FileName);
            var marks = boards.Select(_ => NewMarkingPlate()).ToList();
            var hasWon = boards.Select(_ => false).ToList();
            var lastScore = 0;

            for (int idx = 0; idx < nums.Numbers.Count; idx++)
            {
                var number = nums.Numbers[idx];
                for (var boardIdx = 0; boardIdx < boards.Count; ++boardIdx)
                {
                    if (hasWon[boardIdx]) continue;
                    var board = boards[boardIdx];
                    Mark(board, marks[boardIdx], number);
                    if (HasWon(marks[boardIdx]))
                    {
                        hasWon[boardIdx] = true;
                        lastScore = number * SumUnmarked(board, marks[boardIdx]);
                    }
                }
            }

            return lastScore;
        }

        private void SolveFirstStar()
        {
            var sol = SolveFirst();
            Console.WriteLine("Solution (1): " + sol);
        }

        private void SolveSecondStar()
        {
            var sol = SolveSecond();
            Console.WriteLine("Solution (2): " + sol);
        }

    }
}
