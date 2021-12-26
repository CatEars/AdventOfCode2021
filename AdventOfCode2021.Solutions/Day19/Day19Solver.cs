#nullable enable
using System;
using System.Collections.Generic;
using System.Linq;
using AdventOfCode2021.Solutions.Interface;
using AdventOfCode2021.Util;

namespace AdventOfCode2021.Solutions.Day19
{
    public class Day19Solver : ISolvable
    {
        private static string FileName => "Input/Day19_A.input";

        private record Scanner(List<Vector3> Signals);

        public void Run()
        {
            SolveFirstStar();
            SolveSecondStar();
        }

        private static List<Func<Vector3, Vector3>> FacingMapping = new()
        {
            v => new Vector3(v.X, v.Y, v.Z),
            v => new Vector3(v.X, v.Z, v.Y),
            v => new Vector3(v.Y, v.X, v.Z),
            v => new Vector3(v.Y, v.Z, v.X),
            v => new Vector3(v.Z, v.X, v.Y),
            v => new Vector3(v.Z, v.Y, v.X)
        };
        
        private static void SolveFirstStar()
        {
            var scanners = Read(FileName);
            var (scanCloud, _) = FindScanCloud(scanners);

            Console.WriteLine("Solution (1): " + scanCloud.Signals.Count);
        }

        private static void SolveSecondStar()
        {
            var scanners = Read(FileName);
            var (_, beacons) = FindScanCloud(scanners);
            var dist = 0;
            for (var a = 0; a < beacons.Count; ++a)
            {
                for (var b = a + 1; b < beacons.Count; ++b)
                {
                    var man = beacons[a].Sub(beacons[b]).Manhattan();
                    dist = Math.Max(dist, man);
                }
            }
            Console.WriteLine("Solution (2): " + dist);
        }

        private static (Scanner Scanner, List<Vector3> Beacons) _cache = (null, null);
        private static (Scanner Scanner, List<Vector3> Beacons) FindScanCloud(List<Scanner> scanners)
        {
            if (_cache != (null, null)) return _cache;
            var scanCloud = new Scanner(new List<Vector3>());
            var beacons = new List<Vector3>();
            // Anchor on the first scanner
            scanCloud.Signals.AddRange(scanners[0].Signals);
            var truePositions = scanners.Select(x => new List<Vector3>()).ToList();
            truePositions[0] = scanners[0].Signals.ToList();
            var orientations = new List<Vector3>()
            {
                new(1, 1, 1),
                new(1, 1, -1),
                new(1, -1, 1),
                new(1, -1, -1),
                new(-1, 1, 1),
                new(-1, 1, -1),
                new(-1, -1, 1),
                new(-1, -1, -1),
            };

            var found = 1;
            while (found < scanners.Count)
            {
                for (var testAgainst = 1; testAgainst < scanners.Count; ++testAgainst)
                {
                    var testedScanner = scanners[testAgainst];
                    if (truePositions[testAgainst].Count > 0) continue;

                    foreach (var facing in FacingMapping)
                    {
                        foreach (var orientation in orientations)
                        {
                            var oriented = testedScanner
                                .Signals
                                .Select(x => x.Mult(orientation))
                                .Select(x => facing(x))
                                .ToList();

                            var unkownVectorCloud = CreateVectorCloud(oriented);
                            for (var checkBeacon = 0; checkBeacon < scanners.Count; ++checkBeacon)
                            {
                                if (checkBeacon == testAgainst || truePositions[checkBeacon].Count == 0) continue;
                                var knownSignals = truePositions[checkBeacon];
                                var knownVectorCloud = CreateVectorCloud(knownSignals);
                                var intersection = knownVectorCloud
                                    .Intersect(unkownVectorCloud)
                                    .ToList();
                                // Intersection.count is all the common vectors in the vector cloud
                                // Note that we count both a->b and b->a (we don't know which is a and which is b, so take both)
                                // if we expect 12 beacons to be common then we expect to have 11+10+9+...+1 matching unique vectors
                                // This is the same as 11 * 12 / 2 = 11 * 6 = 66
                                // If we find at least 66 matching vectors we have 12 beacons that line up.
                                var numMatchingVectors = intersection.Count / 2;
                                var expectedFrom12Beacons = 11 * 12 / 2;
                                if (numMatchingVectors >= expectedFrom12Beacons)
                                {
                                    var position = FindBeaconPosition(intersection, knownSignals, oriented);
                                    if (position == null)
                                        continue;
                                    beacons.Add(position);
                                    var truePos = oriented
                                        .Select(x => x.Add(position))
                                        .ToList();
                                    truePositions[testAgainst] = truePos;
                                    var notAlreadyKnown = truePos
                                        .Where(x => !scanCloud.Signals.Contains(x));
                                    scanCloud.Signals.AddRange(notAlreadyKnown);
                                    found++;
                                    goto outermost;
                                }
                            }
                        }
                    }

                    outermost: ;
                }
            }

            return _cache = (scanCloud, beacons);
        }

        private static Vector3? FindBeaconPosition(List<Vector3> intersection, List<Vector3> knownSignals, List<Vector3> oriented)
        {
            IEnumerable<Vector3> NeighborVectors(Vector3 origin, List<Vector3> comparators)
            {
                return comparators
                    .Select(x => x.Sub(origin))
                    .Where(x => x != Vector3.Origin);
            }
            
            bool OneOfTheBeacons(Vector3 signal, List<Vector3> comparators)
            {
                return intersection
                    .Intersect(NeighborVectors(signal, comparators))
                    .Count() >= 11;
            }

            var knownCommonBeacons = knownSignals
                .Where(x => OneOfTheBeacons(x, knownSignals))
                .ToList();
            var unkownCommonBeacons = oriented
                .Where(x => OneOfTheBeacons(x, oriented))
                .ToList();
            if (knownCommonBeacons.Count != 12)
            {
                return null;
            }

            if (unkownCommonBeacons.Count != 12)
            {
                return null;
            }

            bool SimilarBeacons(Vector3 known, Vector3 unkown)
            {
                var vectorCloud1 = NeighborVectors(known, knownCommonBeacons);
                var vectorCloud2 = NeighborVectors(unkown, unkownCommonBeacons);
                var intersection = vectorCloud1.Intersect(vectorCloud2);
                var count = intersection.Count();
                return count == 11;
            }

            List<int> pairing = knownCommonBeacons
                .Select(x => -1)
                .ToList();
            List<bool> used = knownCommonBeacons
                .Select(x => false)
                .ToList();
            
            bool Dfs(int knownIdx)
            {
                if (knownIdx >= 12)
                {
                    return true;
                }

                for (var other = 0; other < 12; ++other)
                {
                    if (!used[other] && SimilarBeacons(knownCommonBeacons[knownIdx], unkownCommonBeacons[other]))
                    {
                        used[other] = true;
                        pairing[knownIdx] = other;
                        return true;
                        if (Dfs(knownIdx + 1))
                        {
                            return true;
                        }
                        pairing[knownIdx] = -1;
                        used[other] = false;
                    }
                }

                return false;
            }

            if (Dfs(0))
            {
                var a = knownCommonBeacons[0];
                var b = unkownCommonBeacons[pairing[0]];
                return a.Sub(b);
            }

            return null;
        }

        private static List<Vector3> CreateVectorCloud(List<Vector3> oriented)
        {
            List<Vector3> result = new ();
            foreach (var vector in oriented)
            {
                foreach (var other in oriented)
                {
                    // Do not include self in vector cloud
                    if (vector == other) continue;
                    result.Add(other.Sub(vector));
                }
            }

            return result;
        }


        private static List<Scanner> Read(string filepath)
        {
            var lines = FileRead.ReadLines(filepath);
            var scanners = new List<Scanner>();
            foreach (var line in lines)
            {
                if (line.StartsWith("--- "))
                {
                    scanners.Add(new Scanner(new List<Vector3>()));
                }
                else if (line.Contains(','))
                {
                    var elements = line
                        .Trim()
                        .Split(',')
                        .Select(int.Parse)
                        .ToList();
                    var signal = new Vector3(elements[0], elements[1], elements[2]);
                    scanners.End().Signals.Add(signal);
                }
            }

            return scanners;
        }
    }
}