#nullable enable
using System;
using System.Collections.Generic;
using System.Linq;
using AdventOfCode2021.Solutions.Interface;
using AdventOfCode2021.Util;

namespace AdventOfCode2021.Solutions.Day21
{
    public class Day21Solver : ISolvable
    {
        private int Player1Start = 5;
        private int Player2Start = 8;

        public void Run()
        {
            SolveFirstStar();
            SolveSecondStar();
        }

        private void SolveFirstStar()
        {
            var die = 0;
            var rolls = 0;
            var pos1 = Player1Start - 1;
            var point1 = 0;
            var pos2 = Player2Start - 1;
            var point2 = 0;

            int RollIt()
            {
                ++rolls;
                var res = ++die;
                if (die == 100) die = 0;
                return res;
            }

            var isFirst = true;
            while (point1 < 1000 && point2 < 1000)
            {
                var add = RollIt() + RollIt() + RollIt();
                if (isFirst)
                {
                    pos1 = (pos1 + add) % 10;
                    point1 += pos1 + 1;
                }
                else
                {
                    pos2 = (pos2 + add) % 10;
                    point2 += pos2 + 1;
                }

                isFirst = !isFirst;
            }

            var score = point1 >= 1000 ? point2 : point1;
            Console.WriteLine("Solution (1): " + score * rolls);
        }

        private (long, long) AddWins((long, long) lhs, (long, long) rhs)
        {
            return (lhs.Item1 + rhs.Item1, lhs.Item2 + rhs.Item2);
        }
        
        private void SolveSecondStar()
        {
            var cache =
                new Dictionary<(bool IsFirstPlayer, int APos, int BPos, int APoints, int BPoints), (long AWins, long
                    BWins)>();

            (long, long) Wins(bool isFirst, int apos, int bpos, int apoints, int bpoints, int indent = 0)
            {
                
                if (apoints >= 21)
                {
                    return (1, 0);
                }
                else if (bpoints >= 21)
                {
                    return (0, 1);
                }
                
                var key = (isFirst, apos, bpos, apoints, bpoints);
                if (cache.ContainsKey(key))
                {
                    return cache[key];
                }

                var wins = (0L, 0L);
                foreach (var roll1 in Enumerable.Range(1, 3))
                foreach (var roll2 in Enumerable.Range(1, 3))
                foreach (var roll3 in Enumerable.Range(1, 3))
                {
                    var sum = roll1 + roll2 + roll3;
                    var nextApos = (apos + (isFirst ? sum : 0)) % 10;
                    var nextBpos = (bpos + (isFirst ? 0 : sum)) % 10;
                    var nextApoints = (apoints + (isFirst ? nextApos + 1 : 0));
                    var nextBpoints = (bpoints + (isFirst ? 0 : nextBpos + 1));
                    var winsForThisRoll = Wins(!isFirst, nextApos, nextBpos, nextApoints, nextBpoints, indent + 1);
                    wins = AddWins(wins, winsForThisRoll);
                }

                return cache[key] = wins;
            }

            var totalWins = Wins(true, Player1Start - 1, Player2Start - 1, 0, 0);

            Console.WriteLine("Solution (2): " + totalWins);
        }
    }
}