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
        private static string FileName => "Input/Day19_Test.input";

        private record BeaconSignal(int XDiff, int YDiff, int ZDiff)
        {
            public BeaconSignal WithOrientation((int X, int Y, int Z) orientation, int facing)
            {
                var x = XDiff * orientation.X;
                var y = YDiff * orientation.Y;
                var z = ZDiff * orientation.Z;
                var facings = new List<(int X, int Y, int Z)>()
                {
                    (x, y, z),
                    (x, z, y),
                    (y, x, z),
                    (y, z, x),
                    (z, x, y),
                    (z, y, x)
                };
                return new(facings[facing].X, facings[facing].Y, facings[facing].Z);
            }
        }

        private record Scanner(List<BeaconSignal> Signals);

        public void Run()
        {
            SolveFirstStar();
            SolveSecondStar();
        }

        private static void SolveFirstStar()
        {
            var scanners = Read(FileName);
            var scanCloud = new Scanner(new List<BeaconSignal>());
            // Anchor on the first scanner
            scanCloud.Signals.AddRange(scanners[0].Signals);
            var truePositions = scanners.Select(x => new List<BeaconSignal>()).ToList();
            truePositions[0] = scanners[0].Signals.ToList();
            var orientations = new List<(int XOrient, int YOrient, int ZOrient)>()
            {
                (1, 1, 1),
                (1, 1, -1),
                (1, -1, 1),
                (1, -1, -1),
                (-1, 1, 1),
                (-1, 1, -1),
                (-1, -1, 1),
                (-1, -1, -1),
            };

            var found = 1;
            while (found < scanners.Count)
            {
                for (var testAgainst = 1; testAgainst < scanners.Count; ++testAgainst)
                {
                    var testedScanner = scanners[testAgainst];
                    if (truePositions[testAgainst].Count > 0) continue;
                    for (var checkBeacon = 0; checkBeacon < scanners.Count; ++checkBeacon)
                    {
                        if (checkBeacon == testAgainst || truePositions[checkBeacon].Count == 0) continue;
                        var knownSignals = truePositions[checkBeacon];

                        foreach (var orientation in orientations)
                        {
                            var oriented = testedScanner
                                .Signals
                                .Select(x => x.WithOrientation(orientation, 0))
                                .ToList();
                            var originCount = new Dictionary<(int X, int Y, int Z), int>();
                            foreach (var knownSignal in knownSignals)
                            {
                                foreach (var unkownSignal in oriented)
                                {
                                    var (x, y, z) = unkownSignal;
                                    var beacon = Minus((knownSignal.XDiff, knownSignal.YDiff, knownSignal.ZDiff),
                                        (x, y, z));
                                    originCount.AddOrSet(beacon, 1);
                                }
                            }

                            if (originCount.Values.Any(x => x >= 12))
                            {
                                var beaconPosition = originCount.First(x => x.Value >= 12);
                                Console.WriteLine(
                                    $"Found scanner #{testAgainst} in positions {beaconPosition.Key}");
                                var realPositions = oriented
                                    .Select(x => Minus((x.XDiff, x.YDiff, x.ZDiff), beaconPosition.Key))
                                    .Select(x => new BeaconSignal(x.X, x.Y, x.Z))
                                    .Where(x => !scanCloud.Signals.Contains(x))
                                    .ToList();
                                scanCloud.Signals.AddRange(realPositions);
                                truePositions[testAgainst] = oriented
                                    .Select(x => Minus((x.XDiff, x.YDiff, x.ZDiff), beaconPosition.Key))
                                    .Select(x => new BeaconSignal(x.X, x.Y, x.Z))
                                    .ToList();
                                found++;
                                goto outermost;
                            }
                        }
                    }

                    outermost: ;
                }
            }

            Console.WriteLine("Solution (1): " + scanCloud.Signals.Count);
        }

        private static (int X, int Y, int Z) Minus((int X, int Y, int Z) lhs, (int X, int Y, int Z) rhs)
        {
            return (lhs.X - rhs.X, lhs.Y - rhs.Y, lhs.Z - rhs.Z);
        }

        private static void SolveSecondStar()
        {
            Console.WriteLine("Solution (2): " + 0);
        }

        private static List<Scanner> Read(string filepath)
        {
            var lines = FileRead.ReadLines(filepath);
            var scanners = new List<Scanner>();
            foreach (var line in lines)
            {
                if (line.StartsWith("--- "))
                {
                    scanners.Add(new Scanner(new List<BeaconSignal>()));
                }
                else if (line.Contains(','))
                {
                    var elements = line
                        .Trim()
                        .Split(',')
                        .Select(int.Parse)
                        .ToList();
                    var signal = new BeaconSignal(elements[0], elements[1], elements[2]);
                    scanners.End().Signals.Add(signal);
                }
            }

            return scanners;
        }
    }
}