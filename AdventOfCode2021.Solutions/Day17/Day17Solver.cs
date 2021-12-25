#nullable enable
using System;
using System.Collections.Generic;
using System.Linq;
using AdventOfCode2021.Solutions.Interface;
using AdventOfCode2021.Util;

namespace AdventOfCode2021.Solutions.Day17
{
    public class Day17Solver : ISolvable
    {

        private record TargetArea(int MinX, int MaxX, int MinY, int MaxY);

        private static TargetArea TestArea = new(20, 30, -10, -5);
        
        private static TargetArea RealArea = new(169, 206, -108, -68);

        private static TargetArea Area = RealArea;

        public void Run()
        {
            SolveFirstStar();
            SolveSecondStar();
        }

        private static Dictionary<int, List<int>> FindStepMapForY()
        {
            var result = new Dictionary<int, List<int>>();
            for (var startY = Area.MinY - 1; startY <= Math.Abs(Area.MinY) + 1; ++startY)
            {
                var step = 2*startY + 1;
                var speed = -(startY + 1);

                if (startY < 0)
                {
                    speed = startY;
                    step = 0;
                }
                var sink = 0;
                while (sink >= Area.MinY)
                {
                    step++;
                    sink += speed;
                    speed -= 1;
                    if (Area.MinY <= sink && sink <= Area.MaxY)
                    {
                        result.Append(step, startY);
                    }
                }
            }

            return result;
        }

        private static Dictionary<int, List<int>> FindStepMapForX()
        {
            var result = new Dictionary<int, List<int>>();
            for (var startX = 1; startX <= Area.MaxX; ++startX)
            {
                var step = 0;
                var length = 0;
                var speed = startX;
                while (step <= 250 && length <= Area.MaxX)
                {
                    length += speed;
                    speed -= 1;
                    speed = Math.Max(0, speed);
                    step += 1;
                    if (Area.MinX <= length && length <= Area.MaxX)
                    {
                        result.Append(step, startX);
                    }
                } 
            }
            return result;
        }
        
        private static void SolveFirstStar()
        {
            // X does not actually matter since X and Y are independent
            // and X drags to 0, we just have to find an X that will stop within the target region
            // and prove that there for all steps before there is some solution.
            // An example for the test area is startX = 6 which will end on X=21, there are solutions
            // for step=5,4,3,2,1
            
            // Maximum Y then....
            // The maximum point of the Y position will be
            // v_y + (v_y - 1) + .... + 0
            // We can also generate all of those solutions as 1, 1 + 2, 1 + 2 + 3, ...
            // The falling positions are the same. First we fall with 1, then 2
            // So falling follows the same pattern, t=1 => 1, t=2 => 1 + 2, t=3 => 1 + 2 + 3, ...
            // Insight: Whatever velocity we shoot with, we will pass by Y=0 on the way down (unless we shoot downwards)
            // Insight #2: The velocity on the downward slope will be v_y + 1, v_y + 2, ...
            // We will only need to search until v_y + 1 > Area.MinY
            var yStep = FindStepMapForY();
            var maxY = yStep.Values
                .SelectMany(x => x)
                .Select(x => x * (x + 1) / 2)
                .Aggregate(Math.Max);
            Console.WriteLine("Solution (1): " + maxY);
        }

        private static void SolveSecondStar()
        {
            var xStep = FindStepMapForX();
            var yStep = FindStepMapForY();
            var velos = new HashSet<(int, int)>();
            for (var step = 0; step < 250; ++step)
            {
                if (xStep.ContainsKey(step) && yStep.ContainsKey(step))
                {
                    foreach (var x in xStep[step])
                    {
                        foreach (var y in yStep[step])
                        {
                            velos.Add((x, y));
                        }
                    }
                }
            }

            Console.WriteLine("Solution (2): " + velos.Count);
        }
    }
}