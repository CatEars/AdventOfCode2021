using System;
using System.Collections.Generic;
using System.Linq;
using AdventOfCode2021.Solutions.Interface;
using AdventOfCode2021.Util;

namespace AdventOfCode2021.Solutions.Day12
{
    public class Day12Solver : ISolvable
    {

        private static string FileName => "Input/Day12_A.input";

        public void Run()
        {
            SolveFirstStar();
            SolveSecondStar();
        }

        private Dictionary<string, List<string>> GetGraph(string filepath)
        {
            var lines = FileRead.ReadLines(filepath);
            var result = new Dictionary<string, List<string>>();
            foreach (var line in lines)
            {
                var stations = line.Trim().Split("-");
                result.Append(stations[0], stations[1]);
                result.Append(stations[1], stations[0]);
                result[stations[0]].Sort();
                result[stations[1]].Sort();
            }

            // No outgoing paths from "end"
            result.Remove("end");
            return result;
        }

        private record State(Dictionary<string, int> Visited, Dictionary<string, long> Roads);

        private bool AllLowerCase(string val)
        {
            return val.ToLower() == val;
        }

        private void Dfs(Dictionary<string, List<string>> graph, State state, string now, int maxVisits)
        {
            state.Visited.AddOrSet(now, 1);
            state.Roads.AddOrSet(now, 1);
            if (graph.TryGetValue(now, out var conn))
            {
                foreach (var neigh in conn)
                {
                    var visitations = state.Visited.TryGetValue(neigh, out var val) ? val : 0;
                    if (AllLowerCase(neigh) && visitations >= maxVisits)
                    {
                        continue;
                    }

                    var nextMaxVisits = maxVisits;
                    if (nextMaxVisits == 2 && AllLowerCase(neigh) && visitations == 1)
                    {
                        nextMaxVisits = 1;
                    }
                    Dfs(graph, state, neigh, nextMaxVisits);
                }
            }

            state.Visited[now] -= 1;
        }

        private void SolveFirstStar()
        {
            var graph = GetGraph(FileName);
            var state = new State(new Dictionary<string, int>(), new Dictionary<string, long>());
            Dfs(graph, state, "start", 1);
            var sol = state.Roads["end"];
            Console.WriteLine("Solution (1): " + sol);
        }

        private void SolveSecondStar()
        {
            var graph = GetGraph(FileName);
            var state = new State(new Dictionary<string, int>(), new Dictionary<string, long>());
            state.Visited.AddOrSet("start", 1);
            Dfs(graph, state, "start", 2);
            var sol = state.Roads["end"];
            Console.WriteLine("Solution (2): " + sol);
        }
    }
}
