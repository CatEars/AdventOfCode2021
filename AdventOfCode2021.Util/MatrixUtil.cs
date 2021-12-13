using System;
using System.Collections.Generic;
using System.Linq;

namespace AdventOfCode2021.Util
{
    public static class MatrixUtil
    {

        public static List<List<T>> NewMatrix<T>(int rows, int cols, T defaultValue = default)
        {
            var matrix = new List<List<T>>();
            for (var row = 0; row < rows; ++row)
            {
                matrix.Add(new List<T>());
                for (var col = 0; col < cols; ++col)
                {
                    matrix[row].Add(defaultValue);
                }
            }

            return matrix;
        }

        public static void Apply<T>(this List<List<T>> matrix, Func<int, int, T, T> func)
        {
            for (var row = 0; row < matrix.Count; ++row)
            {
                for (var col = 0; col < matrix[row].Count; ++col)
                {
                    matrix[row][col] = func(row, col, matrix[row][col]);
                }
            }
        }

        public static void Apply<T>(this List<List<T>> matrix, Func<T, T> func)
        {
            matrix.Apply((_, _, x) => func(x));
        }

        public static void Apply<T>(this List<List<T>> matrix, Func<int, int, T> func)
        {
            matrix.Apply((row, col, _) => func(row, col));
        }

        public static List<List<T2>> Map<T, T2>(this List<List<T>> matrix, Func<int, int, T, T2> func)
        {
            var copy = NewMatrix<T2>(matrix.Count, matrix[0].Count);
            for (var row = 0; row < matrix.Count; ++row)
            {
                for (var col = 0; col < matrix[row].Count; ++col)
                {
                    copy[row][col] = func(row, col, matrix[row][col]);
                }
            }

            return copy;
        }

        public static List<List<T2>> Map<T, T2>(this List<List<T>> matrix, Func<T, T2> func)
        {
            return matrix.Map((_, _, x) => func(x));
        }

        public static List<List<T2>> Map<T, T2>(this List<List<T>> matrix, Func<int, int, T2> func)
        {
            return matrix.Map((row, col, _) => func(row, col));
        }

        public static List<List<T>> Copy<T>(this List<List<T>> matrix)
        {
            var copy = NewMatrix<T>(matrix.Count, matrix[0].Count);
            copy.Apply((row, col) => matrix[row][col]);
            return copy;
        }

        public static IEnumerable<T> SelectCol<T>(this List<List<T>> matrix, int col)
        {
            return Enumerable.Range(0, matrix.Count).Select(row => matrix[row][col]);
        }

        public static IEnumerable<T> SelectRow<T>(this List<List<T>> matrix, int row)
        {
            return matrix[row];
        }

        public static IEnumerable<(int Row, int Col)> EachAdjacent<T>(this List<List<T>> matrix, int row, int col)
        {
            var diffRow = new List<int>() { -1, 0, 1, 0 };
            var diffCol = new List<int>() { 0, -1, 0, 1 };
            var adjacents = new List<(int, int)>();
            for (var diffIdx = 0; diffIdx < diffRow.Count; ++diffIdx)
            {
                var nr = row + diffRow[diffIdx];
                var nc = col + diffCol[diffIdx];
                if (0 <= nr && nr < matrix.Count &&
                    0 <= nc && nc < matrix[nr].Count)
                {
                    adjacents.Add((nr, nc));
                }
            }

            return adjacents;
        }

        public static IEnumerable<(int Row, int Col)> EachAdjacentAndDiagonal<T>(this List<List<T>> matrix, int row,
            int col)
        {
            var diffRow = new List<int>() { -1, 0, 1, 0, 1, 1, -1, -1 };
            var diffCol = new List<int>() { 0, -1, 0, 1, 1, -1, 1, -1 };
            var adjacents = new List<(int, int)>();
            for (var diffIdx = 0; diffIdx < diffRow.Count; ++diffIdx)
            {
                var nr = row + diffRow[diffIdx];
                var nc = col + diffCol[diffIdx];
                if (0 <= nr && nr < matrix.Count &&
                    0 <= nc && nc < matrix[nr].Count)
                {
                    adjacents.Add((nr, nc));
                }
            }

            return adjacents;
        }


        public static IEnumerable<T> Flatten<T>(this List<List<T>> matrix)
        {
            return matrix.SelectMany(x => x);
        }

        public static IEnumerable<(int Row, int Col)> WhereCells<T>(this List<List<T>> matrix, Func<int, int, bool> predicate)
        {
            return matrix
                .Map((row, col) => predicate(row, col) ? (row, col) : (-1, -1))
                .Flatten()
                .Where(pos => pos.Item1 != -1);
        }

        public static int Size<T>(this List<List<T>> matrix)
        {
            return matrix.Count * matrix[0].Count;
        }

        public static List<List<T>> Portion<T>(this List<List<T>> matrix, int numRows, int numCols)
        {
            return NewMatrix<T>(numRows, numCols)
                .Map((row, col) => matrix[row][col]);
        }

        public static IEnumerable<(int Row, int Col)> Enumerate<T>(this List<List<T>> matrix)
        {
            List<(int Row, int Col)> list = new();
            for (var row = 0; row < matrix.Count; ++row)
            {
                for (var col = 0; col < matrix[0].Count; ++col)
                {
                    list.Add((row, col));
                }
            }

            return list;
        }

        public static int NumRows<T>(this List<List<T>> matrix)
        {
            return matrix.Count;
        }

        public static int NumCols<T>(this List<List<T>> matrix)
        {
            return matrix[0].Count;
        }
    }
}
