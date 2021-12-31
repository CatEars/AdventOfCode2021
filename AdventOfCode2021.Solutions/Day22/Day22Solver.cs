#nullable enable
using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using AdventOfCode2021.Solutions.Interface;
using AdventOfCode2021.Util;

namespace AdventOfCode2021.Solutions.Day22
{
    public class Day22Solver : ISolvable
    {
        public static string FileName => "Input/Day22_A.input";

        private record Region(
            bool TurnOn,
            int MinX,
            int MaxX,
            int MinY,
            int MaxY,
            int MinZ,
            int MaxZ
        )
        {
            public long Volume()
            {
                return ((long) (MaxX - MinX + 1)) * ((long) (MaxY - MinY + 1)) * ((long) (MaxZ - MinZ + 1));
            }
        }

        private List<Region> ReadInstructions(string filepath)
        {
            var lines = FileRead.ReadLines(filepath);
            var instructions = new List<Region>();
            foreach (var line in lines)
            {
                bool turnOn = line.StartsWith("on");
                var elements = line.Split(",")
                    .Select(x => x.Split("=")[1])
                    .Select(x => x.Split("..").Select(int.Parse).ToList())
                    .ToList();
                instructions.Add(new Region(
                    turnOn,
                    elements[0][0],
                    elements[0][1],
                    elements[1][0],
                    elements[1][1],
                    elements[2][0],
                    elements[2][1]));
            }

            return instructions;
        }

        private bool FindOverlap(Region a, Region b, bool turnOn, out Region? overlap)
        {
            if (a.MaxX < b.MinX || a.MinX > b.MaxX || 
                a.MaxY < b.MinY || a.MinY > b.MaxY ||
                a.MaxZ < b.MinZ || a.MinZ > b.MaxZ)
            {
                overlap = new Region(false, 0, 0, 0, 0, 0, 0);
                return false;
            }

            var minX = Math.Max(a.MinX, b.MinX);
            var maxX = Math.Min(a.MaxX, b.MaxX);
            var minY = Math.Max(a.MinY, b.MinY);
            var maxY = Math.Min(a.MaxY, b.MaxY);
            var minZ = Math.Max(a.MinZ, b.MinZ);
            var maxZ = Math.Min(a.MaxZ, b.MaxZ);
            overlap = new Region(turnOn, minX, maxX, minY, maxY, minZ, maxZ);
            return true;
        }

        private List<Region> DissectRegion(Region region, Region toRemove)
        {
            // Dissect a region into 1-7 new regions by splitting in all possible directions
            var turnedOn = region.TurnOn;
            var A = toRemove;
            var finalRegions = new List<Region>()
            {
                // split block in X, these "extend" the removed region so that we cover that block in a long line
                new(turnedOn, region.MinX, A.MinX - 1, A.MinY, A.MaxY, A.MinZ, A.MaxZ),
                new(turnedOn, A.MaxX + 1, region.MaxX, A.MinY, A.MaxY, A.MinZ, A.MaxZ),

                // blocks above and below
                new(turnedOn, region.MinX, region.MaxX, A.MinY, A.MaxY, region.MinZ, A.MinZ - 1),
                new(turnedOn, region.MinX, region.MaxX, A.MinY, A.MaxY, A.MaxZ + 1, region.MaxZ),
                
                // Blocks on the sides
                new (turnedOn, region.MinX, region.MaxX, region.MinY, A.MinY - 1, region.MinZ, region.MaxZ),
                new (turnedOn, region.MinX, region.MaxX, A.MaxY + 1, region.MaxY, region.MinZ, region.MaxZ),
            };

            return finalRegions.Where(x => x.Volume() > 0).ToList();
        }
        
        public void Run()
        {
            SolveFirstStar();
            SolveSecondStar();
        }

        private void SolveFirstStar()
        {
            var instructions = ReadInstructions(FileName);
            var sparseMatrix = new HashSet<(int, int, int)>();
            foreach (var inst in instructions)
            {
                for (var x = Math.Max(inst.MinX, -50); x <= Math.Min(inst.MaxX, 50); ++x)
                for (var y = Math.Max(inst.MinY, -50); y <= Math.Min(inst.MaxY, 50); ++y)
                for (var z = Math.Max(inst.MinZ, -50); z <= Math.Min(inst.MaxZ, 50); ++z)
                {
                    if (inst.TurnOn)
                    {
                        sparseMatrix.Add((x, y, z));
                    }
                    else
                    {
                        sparseMatrix.Remove((x, y, z));
                    }
                }
            }
            Console.WriteLine("Solution (1): " + sparseMatrix.Count);
        }

        private ImmutableList<Region> AddRegion(ImmutableList<Region> litRegions, Region toPlace)
        {
            var relevantRegions = litRegions.Where(x => FindOverlap(toPlace, x, true, out _)).ToImmutableList();
            var exceptRelevants = litRegions.Except(relevantRegions).ToImmutableList();
            var splitRegions = relevantRegions.SelectMany(x =>
            {
                FindOverlap(x, toPlace, toPlace.TurnOn, out var overlap);
                return DissectRegion(x, overlap!);
            }).ToImmutableList();
            return exceptRelevants
                .AddRange(splitRegions)
                .Add(toPlace)
                .Where(x => x.TurnOn)
                .ToImmutableList();
        }
        
        private void SolveSecondStar()
        {
            var litRegions = ImmutableList<Region>.Empty;
            var instructions = ReadInstructions(FileName);
            litRegions = instructions.Aggregate(litRegions, AddRegion);
            var lit = litRegions.Select(x => x.Volume()).Sum();
            Console.WriteLine("Solution (2): " + lit);
        }
    }
}