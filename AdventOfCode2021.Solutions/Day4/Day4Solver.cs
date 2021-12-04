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

        private record BingoBoard(List<List<int>> Board, List<List<bool>> Marks);

        private (BingoNumbers, List<BingoBoard>) ReadInput(string filepath)
        {
            var lines = FileRead.ReadLines(filepath);
            var numbers = StringUtil.IntoValidInts(lines[0], ",");
            var bingoNumbers = new BingoNumbers(numbers);

            var boards = new List<BingoBoard>();
            // Each board is 5 rows long and 5 columns wide. The text file separates them with an empty line.
            var idx = 1;
            while (idx + 5 < lines.Count)
            {
                idx++;
                var marks = MatrixUtil.NewMatrix(5, 5, false);
                var indexes = Enumerable.Range(idx, 5);
                var board = indexes
                    .Select(index => StringUtil.IntoValidInts(lines[index]))
                    .Pipe(board => new BingoBoard(board.ToList(), marks));
                boards.Add(board);
                idx += 5;
            }

            return (bingoNumbers, boards);
        }

        private bool HasWon(BingoBoard board)
        {
            var marks = board.Marks;
            var cols = Enumerable.Range(0, 5);
            var rows = Enumerable.Range(0, 5);

            return cols.Any(col => marks.SelectCol(col).All(x => x)) ||
                   rows.Any(row => marks.SelectRow(row).All(x => x));
        }

        private int SumUnmarked(BingoBoard board)
        {
            return board.Board
                .Map((row, col) => board.Marks[row][col] ? 0 : board.Board[row][col])
                .SelectMany(row => row)
                .Sum();
        }

        private void Mark(BingoBoard board, int calledNumber)
        {
            board.Marks.Apply((row, col) => board.Board[row][col] == calledNumber || board.Marks[row][col]);
        }

        private (int Round, BingoBoard Board) Solve(BingoBoard board, BingoNumbers nums)
        {
            for (var round = 0; round < nums.Numbers.Count; ++round)
            {
                Mark(board, nums.Numbers[round]);
                if (HasWon(board))
                {
                    return (round, board);
                }
            }

            throw new Exception("All boards will eventually win");
        }

        private List<(BingoBoard Board, int FinalNumber)> SolveBoards(List<BingoBoard> boards, BingoNumbers nums)
        {
            var solutions = boards
                .Select(board => Solve(board, nums))
                .ToList();
            solutions.Sort((x, y) => x.Round.CompareTo(y.Round));
            return solutions
                .Select(x => (x.Board, nums.Numbers[x.Round]))
                .ToList();
        }

        private void SolveFirstStar()
        {
            var (nums, boards) = ReadInput(FileName);
            var solutions = SolveBoards(boards, nums);
            var sol = SumUnmarked(solutions[0].Board) * solutions[0].FinalNumber;
            Console.WriteLine("Solution (1): " + sol);
        }

        private void SolveSecondStar()
        {
            var (nums, boards) = ReadInput(FileName);
            var solutions = SolveBoards(boards, nums);
            var last = solutions.Count - 1;
            var sol = SumUnmarked(solutions[last].Board) * solutions[last].FinalNumber;
            Console.WriteLine("Solution (2): " + sol);
        }

    }
}
