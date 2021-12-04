using System;
using System.Collections.Generic;

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

        public static List<List<T>> Map<T>(this List<List<T>> matrix, Func<int, int, T, T> func)
        {
            var copy = Copy(matrix);
            for (var row = 0; row < matrix.Count; ++row)
            {
                for (var col = 0; col < matrix[row].Count; ++col)
                {
                    copy[row][col] = func(row, col, matrix[row][col]);
                }
            }

            return copy;
        }

        public static List<List<T>> Map<T>(this List<List<T>> matrix, Func<T, T> func)
        {
            return matrix.Map((_, _, x) => func(x));
        }

        public static List<List<T>> Map<T>(this List<List<T>> matrix, Func<int, int, T> func)
        {
            return matrix.Map((row, col, _) => func(row, col));
        }

        public static List<List<T>> Copy<T>(this List<List<T>> matrix)
        {
            var copy = NewMatrix<T>(matrix.Count, matrix[0].Count);
            copy.Apply((row, col) => matrix[row][col]);
            return copy;
        }
    }
}
