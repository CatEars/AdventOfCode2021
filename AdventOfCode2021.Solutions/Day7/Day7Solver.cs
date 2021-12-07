using System;
using System.Collections.Generic;
using System.Linq;
using AdventOfCode2021.Solutions.Interface;
using AdventOfCode2021.Util;

namespace AdventOfCode2021.Solutions.Day7
{
    public class Day7Solver : ISolvable
    {

        private static string FileName => "Input/Day7_A.input";

        public void Run()
        {
            SolveFirstStar();
            SolveSecondStar();
        }

        private int CalculateBestPosition(List<int> positions, Func<int, int, int> costCalculator)
        {
            var min = MathUtil.Min(positions);
            var max = MathUtil.Max(positions);
            var testedPositions = Enumerable.Range(min, max - min);
            var costPerPosition = testedPositions
                .Select(pos => positions.Select(crab => costCalculator(pos, crab)).Sum());
            return costPerPosition.Min();
        }

        private int CostFunctionFirstStar(int expectedPos, int crabPos)
        {
            return Math.Abs(expectedPos - crabPos);
        }

        private int CostFunctionSecondStar(int expectedPos, int crabPos)
        {
            var diff = Math.Abs(expectedPos - crabPos);
            return diff * (diff + 1) / 2;
        }

        private void SolveFirstStar()
        {
            var positions = FileRead.ReadCommaList(FileName);
            var solution = CalculateBestPosition(positions, CostFunctionFirstStar);
            Console.WriteLine("Solution (1): " + solution);
        }

        private void SolveSecondStar()
        {
            var positions = FileRead.ReadCommaList(FileName);
            var solution = CalculateBestPosition(positions, CostFunctionSecondStar);
            Console.WriteLine("Solution (2): " + solution);
        }

    }
}
