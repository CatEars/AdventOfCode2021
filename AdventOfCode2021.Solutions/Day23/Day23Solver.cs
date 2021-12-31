#nullable enable
using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using AdventOfCode2021.Solutions.Interface;
using Priority_Queue;

namespace AdventOfCode2021.Solutions.Day23
{
    public class Day23Solver : ISolvable
    {
        public static string FileName => "Input/Day23_Test.input";

        
        private static Dictionary<int, (int X, int Y)> Position = new();

        private static Dictionary<char, (int Low, int High)> HomeTurf = new();

        private static Dictionary<char, int> ShellWeight = new()
        {
            {'A', 1},
            {'B', 10},
            {'C', 100},
            {'D', 1000},
        };
        
        public static List<(int X, int Y, int Id)> UpperPoints = new()
        {
            (0, 0, 0),
            (1, 0, 1),

            (3, 0, 3),

            (5, 0, 5),

            (7, 0, 7),

            (9, 0, 9),
            (10, 0, 10),
        };
        
        private void Calculate1()
        {
            var pos = new Dictionary<int, (int X, int Y)>();
            var upperPoints = UpperPoints;
            var innerPoints = new List<(int X, int Y, int Id)>()
            {
                (2, 1, 11), (2, 2, 12),
                (4, 1, 21), (4, 2, 22),
                (6, 1, 31), (6, 2, 32),
                (8, 1, 41), (8, 2, 42),
            };
            foreach (var p1 in innerPoints)
            {
                foreach (var p2 in upperPoints)
                {
                    if (!pos.ContainsKey(p2.Id))
                    {
                        pos.Add(p2.Id, (p2.X, p2.Y));
                    }
                }
                pos.Add(p1.Id, (p1.X, p1.Y));
            }

            Position = pos;
            HomeTurf = new()
            {
                {'A', (11, 12)},
                {'B', (21, 22)},
                {'C', (31, 32)},
                {'D', (41, 42)},
            };
        }

        private void Calculate2()
        {
            var pos = new Dictionary<int, (int X, int Y)>();
            var upperPoints = UpperPoints;
            var innerPoints = new List<(int X, int Y, int Id)>()
            {
                (2, 1, 11), (2, 2, 12), (2, 3, 13), (2, 4, 14),
                (4, 1, 21), (4, 2, 22), (4, 3, 23), (4, 4, 24),
                (6, 1, 31), (6, 2, 32), (6, 3, 33), (6, 4, 34),
                (8, 1, 41), (8, 2, 42), (8, 3, 43), (8, 4, 44),
            };
            foreach (var p1 in innerPoints)
            {
                foreach (var p2 in upperPoints)
                {
                    if (!pos.ContainsKey(p2.Id))
                    {
                        pos.Add(p2.Id, (p2.X, p2.Y));
                    }
                }
                pos.Add(p1.Id, (p1.X, p1.Y));
            }

            Position = pos;
            HomeTurf = new()
            {
                {'A', (11, 14)},
                {'B', (21, 24)},
                {'C', (31, 34)},
                {'D', (41, 44)},
            };
        }

        private record State(int DistanceHere, ImmutableDictionary<int, char> Map)
        {

            private int PriorDistanceHere = -1;
            
            public int LowerBoundDistanceLeft()
            {
                if (PriorDistanceHere != -1) return PriorDistanceHere;
                var sum = 0;
                foreach (var entry in Map)
                {
                    var color = entry.Value;
                    var pos = Position[entry.Key];
                    var home = HomeTurf[color];
                    var dist = ShellWeight[color] * Enumerable.Range(home.Low, home.High - home.Low + 1)
                        .Select(x => Position[x])
                        .Select(x => Math.Abs(x.X - pos.X) + Math.Abs(x.Y - pos.Y))
                        .Min();
                    if (home.Low <= entry.Key && entry.Key <= home.High)
                    {
                        for (var idx = entry.Key + 1; idx <= home.High; ++idx)
                        {
                            if (Map.ContainsKey(idx) && Map[idx] != color)
                            {
                                sum += ((home.Low - entry.Key) + 4) * ShellWeight[color];
                                break;
                            }
                        }
                    }
                    sum += dist;
                }
                return PriorDistanceHere = sum;
            }

            public bool IsFinalState()
            {
                return LowerBoundDistanceLeft() == 0;
            }

            public string Encode()
            {
                var elements = Map.Select(x => $"_{x.Key}-{x.Value}_");
                var encoded = string.Join("", elements);
                return $"{DistanceHere}-{encoded}";
            }
        }

        public void Run()
        {
            SolveFirstStar();
            SolveSecondStar();
        }

        private void SolveFirstStar()
        {
            Calculate1();
            var initialMap = ImmutableDictionary<int, char>.Empty
                .Add(11, 'C')
                .Add(12, 'B')
                .Add(21, 'A')
                .Add(22, 'A')
                .Add(31, 'D')
                .Add(32, 'B')
                .Add(41, 'D')
                .Add(42, 'C');
            var initialState = new State(0, initialMap);
            var result = SolveFor(initialState);
            Console.WriteLine("Solution (1): " + result);
        }

        private int SolveFor(State initialState)
        {
            var queue = new SimplePriorityQueue<State>();
            queue.Enqueue(initialState, initialState.DistanceHere + initialState.LowerBoundDistanceLeft());
            var visited = new HashSet<string>();
            var count = 0;
            while (queue.Any())
            {
                var current = queue.Dequeue();
                var here = current.Encode();

                if (visited.Contains(here)) continue;
                visited.Add(here);
                if (current.IsFinalState())
                {
                    return current.DistanceHere;
                }

                if ((++count) % 1000 == 0)
                {
                    Console.WriteLine("Count: " + count);
                    Console.WriteLine("Distance here: " + current.DistanceHere);
                    Console.WriteLine("Expected Left: " + current.LowerBoundDistanceLeft());
                    
                }
                
                var occupiedTopSpaces = current.Map.Keys
                    .Where(x => x <= 10)
                    .ToList();
                
                foreach (var character in current.Map)
                {
                    var posId = character.Key;
                    var pos = Position[posId]; 
                    var color = character.Value;
                    var home = HomeTurf[color];
                    var onHomeTurf = home.Low <= posId && posId <= home.High;
                    if (onHomeTurf)
                    {
                        // if there is someone behind us on home turf that needs to get out
                        // we need to get out, even if we are in the correct spot.
                        var stay = true;
                        for (int checkId = posId + 1; checkId <= home.High; ++checkId)
                        {
                            if (current.Map.ContainsKey(checkId) && current.Map[checkId] != color)
                            {
                                stay = false;
                                break;
                            }
                        }

                        if (stay) continue;
                    }

                    var inAHut = posId >= 11;
                    if (inAHut)
                    {
                        // I am in a hut and want to move out
                        var anyoneInFront = false;
                        for (int idx = posId - 1; (idx % 10) != 0; --idx)
                        {
                            if (current.Map.ContainsKey(idx))
                            {
                                anyoneInFront = true;
                                break;
                            };
                        }
                        if (anyoneInFront) continue;

                        foreach (var testedPos in UpperPoints)
                        {
                            var lowX = Math.Min(testedPos.X, pos.X);
                            var highX = Math.Max(testedPos.X, pos.X);
                            if (occupiedTopSpaces.Any(x => lowX <= x && x <= highX))
                                continue; // someone in the way
                            
                            var nextMap = current.Map
                                .Remove(posId)
                                .Add(testedPos.Id, color);
                            var nextDist = current.DistanceHere + ShellWeight[color] * Dist(pos, testedPos);
                            var state = new State(nextDist, nextMap);
                            var priority = state.DistanceHere + state.LowerBoundDistanceLeft();
                            queue.Enqueue(state, priority);
                        }
                    }
                    else
                    {
                        // I am outside, only 1 place I might choose to go to.
                        // highest number of my color. Anyone in the way?
                        var resultX = Position[HomeTurf[color].Low].X;
                        var myX = pos.X;
                        var lowX = Math.Min(myX, resultX);
                        var highX = Math.Max(myX, resultX);
                        
                        if (occupiedTopSpaces.Any(x => x != myX && lowX <= x && x <= highX)) continue;
                        // No one is in the way for me to reach X = resultX!
                        
                        // Is there anyone already in my hut? :(
                        var anyoneElseHome = Enumerable.Range(home.Low, home.High - home.Low + 1)
                            .Where(homeIdx => current.Map.ContainsKey(homeIdx))
                            .Select(homeIdx => current.Map[homeIdx])
                            .Any(otherColor => otherColor != color);
                        if (anyoneElseHome) continue;
                        
                        var target = HomeTurf[color].High;
                        while (current.Map.ContainsKey(target)) --target;
                        var targetPos = Position[target];
                        var dist = Math.Abs(targetPos.X - pos.X) + Math.Abs(targetPos.Y - pos.Y);
                        var nextMap = current.Map
                            .Remove(posId)
                            .Add(target, color);
                        var nextDist = current.DistanceHere + ShellWeight[color] * dist;
                        var state = new State(nextDist, nextMap);
                        var priority = state.DistanceHere + state.LowerBoundDistanceLeft();
                        queue.Enqueue(state, priority);
                    }
                }
            }
            return -1;
        }

        private int Dist((int X, int Y) pos, (int X, int Y, int Id) testedPos)
        {
            return Math.Abs(pos.X - testedPos.X) + Math.Abs(pos.Y - testedPos.Y);
        }

        private void SolveSecondStar()
        {
            Calculate2();
            var initialMap = ImmutableDictionary<int, char>.Empty
                .Add(11, 'C')
                .Add(12, 'D')
                .Add(13, 'D')
                .Add(14, 'B')
                .Add(21, 'A')
                .Add(22, 'C')
                .Add(23, 'B')
                .Add(24, 'A')
                .Add(31, 'D')
                .Add(32, 'B')
                .Add(33, 'A')
                .Add(34, 'B')
                .Add(41, 'D')
                .Add(42, 'A')
                .Add(43, 'C')
                .Add(44, 'C');
            var initialState = new State(0, initialMap);
            var result = SolveFor(initialState);
            Console.WriteLine("Solution (2): " + result);
        }
    }
}