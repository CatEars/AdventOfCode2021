using System;
using System.Collections.Generic;

namespace AdventOfCode2021.Util
{
    public static class PrintUtil
    {

        public static void PrintMatrix<T>(List<List<T>> matrix)
        {
            for (int row = 0; row < matrix.Count; ++row)
            {
                Console.Write(row + " | ");
                for (int col = 0; col < matrix[row].Count; ++col)
                {
                    Console.Write(matrix[row][col] + " ");
                }

                Console.WriteLine();
            }
        }

    }
}
