#nullable enable
using System;
using System.Collections.Generic;
using System.Linq;
using AdventOfCode2021.Solutions.Interface;
using AdventOfCode2021.Util;

namespace AdventOfCode2021.Solutions.Day18
{
    public class Day18Solver : ISolvable
    {

        private static string FileName => "Input/Day18_A.input";

        private class Tree
        {

            public Tree? Lhs;
            public Tree? Rhs;
            public Tree? Parent;
            public int LhsValue = -1;
            public int RhsValue = -1;
            public bool IsLeftChild = false;
            public bool IsRightChild = false;

            public bool HasLeftChild => Lhs != null || LhsValue != -1;

            public void AddNum(int symbol)
            {
                if (HasLeftChild)
                {
                    RhsValue = symbol;
                }
                else
                {
                    LhsValue = symbol;
                }
            }

            public static Tree New(Tree? lhs, Tree? rhs, Tree? parent, int lhsValue, int rhsValue)
            {
                return new Tree
                {
                    Lhs = lhs,
                    Rhs = rhs,
                    Parent = parent,
                    LhsValue = lhsValue,
                    RhsValue = rhsValue
                };
            }

            public void Print()
            {
                Console.Write("[");
                if (Lhs == null)
                {
                    Console.Write(LhsValue);
                }
                else
                {
                    Lhs.Print();
                }
                Console.Write(",");
                if (Rhs == null)
                {
                    Console.Write(RhsValue);
                }
                else
                {
                    Rhs.Print();
                }
                Console.Write("]");
            }
        }

        private static Tree Root()
        {
            return Tree.New(null, null, null, -1, -1);
        }

        private static Tree Child(Tree parent)
        {
            return Tree.New(null, null, parent, -1, -1);
        }
        
        private static Tree ParseSnailNumber(string line)
        {
            var stack = new Stack<Tree>();
            var root = Root();
            stack.Push(root);
            var idx = 1;
            while (idx < line.Length)
            {
                var symbol = line[idx];
                if ('0' <= symbol && symbol <= '9')
                {
                    stack.Peek().AddNum(symbol - '0');
                }
                else if (symbol == '[')
                {
                    var parent = stack.Peek();
                    var child = Child(parent);
                    if (parent.HasLeftChild)
                    {
                        parent.Rhs = child;
                        child.IsRightChild = true;
                    }
                    else
                    {
                        parent.Lhs = child;
                        child.IsLeftChild = true;
                    }
                    stack.Push(child);
                }
                else if (symbol == ']')
                {
                    stack.Pop();
                }

                idx += 1;
            }

            return root;
        }
        
        public void Run()
        {
            SolveFirstStar();
            SolveSecondStar();
        }

        private static Tree AddSnailNumber(Tree lhs, Tree rhs)
        {
            var newParent = Root();
            newParent.Lhs = lhs;
            lhs.IsLeftChild = true;
            lhs.Parent = newParent;
            newParent.Rhs = rhs;
            rhs.IsRightChild = true;
            rhs.Parent = newParent;
            return newParent;
        }

        private static Tree ReduceSnailNumber(Tree root)
        {
            var explodable = FindExplodable(root);
            var splittable = FindSplittable(root);
            while (explodable != null || splittable != null)
            {
                if (explodable != null)
                {
                    ExplodeSnailNumber(explodable);
                }
                else if (splittable != null)
                {
                    SplitSnailNumber(splittable);
                }

                explodable = FindExplodable(root);
                splittable = FindSplittable(root);
            }

            return root;
        }

        private static void SplitSnailNumber(Tree splittable)
        {
            if (splittable.LhsValue >= 10)
            {
                var split = Child(splittable);
                split.LhsValue = splittable.LhsValue >> 1;
                split.RhsValue = (splittable.LhsValue >> 1) + (splittable.LhsValue & 1);
                split.IsLeftChild = true;
                splittable.Lhs = split;
                splittable.LhsValue = -1;
            }
            else
            {
                var split = Child(splittable);
                split.LhsValue = splittable.RhsValue >> 1;
                split.RhsValue = (splittable.RhsValue >> 1) + (splittable.RhsValue & 1);
                split.IsRightChild = true;
                splittable.Rhs = split;
                splittable.RhsValue = -1;
            }
        }

        private static void ExplodeSnailNumber(Tree explodable)
        {
            var lhsBase = explodable.LhsValue;
            var rhsBase = explodable.RhsValue;
            
            if (explodable.IsRightChild)
            {
                if (explodable.Parent.LhsValue != -1)
                {
                    explodable.Parent.LhsValue += lhsBase;
                }
                else
                {
                    var rightMost = FindRightmost(explodable.Parent.Lhs);
                    rightMost.RhsValue += lhsBase;
                }

                var firstAvailable = explodable.Parent;
                while (firstAvailable.IsRightChild) firstAvailable = firstAvailable.Parent;
                firstAvailable = firstAvailable.Parent;
                if (firstAvailable == null)
                {
                    // ignore
                } 
                else if (firstAvailable.RhsValue != -1)
                {
                    firstAvailable.RhsValue += rhsBase;
                }
                else
                {
                    var rightMost = FindLeftmost(firstAvailable.Rhs);
                    rightMost.LhsValue += rhsBase;
                }
                explodable.Parent.RhsValue = 0;
                explodable.Parent.Rhs = null;
            }
            else if (explodable.IsLeftChild)
            {
                // Take rightmost and add to leftmost in parent chain
                if (explodable.Parent.RhsValue != -1)
                {
                    explodable.Parent.RhsValue += rhsBase;
                }
                else
                {
                    var rightMost = FindLeftmost(explodable.Parent.Rhs);
                    rightMost.LhsValue += rhsBase;
                }

                // Take leftmost and add to rightmost in parent chain
                var firstAvailable = explodable.Parent;
                while (firstAvailable.IsLeftChild) firstAvailable = firstAvailable.Parent;
                firstAvailable = firstAvailable.Parent;
                if (firstAvailable == null)
                {
                    // ignore, at root, no lefty
                }
                else if (firstAvailable.LhsValue != -1)
                {
                    firstAvailable.LhsValue += lhsBase;
                }
                else
                {
                    var rightMost = FindRightmost(firstAvailable.Lhs);
                    rightMost.RhsValue += lhsBase;
                }
                explodable.Parent.LhsValue = 0;
                explodable.Parent.Lhs = null;
            }
        }

        private static Tree FindLeftmost(Tree root)
        {
            if (root.Lhs != null) return FindLeftmost(root.Lhs);
            return root;
        }

        private static Tree FindRightmost(Tree root)
        {
            if (root.Rhs != null) return FindRightmost(root.Rhs);
            return root;
        }

        private static Tree? FindSplittable(Tree root)
        {
            if (root.LhsValue >= 10)
            {
                return root;
            }
            if (root.Lhs != null)
            {
                var lhsSplit = FindSplittable(root.Lhs);
                if (lhsSplit != null) return lhsSplit;
            }

            if (root.RhsValue >= 10)
            {
                return root;
            }

            if (root.Rhs != null)
            {
                var rhsSplit = FindSplittable(root.Rhs);
                if (rhsSplit != null) return rhsSplit;
            }

            return null;
        }

        private static Tree? FindExplodable(Tree root, int depth = 0)
        {
            if (depth >= 4 && root.LhsValue >= 0)
            {
                if (root.RhsValue == -1)
                {
                    throw new ArgumentException("Tried to explode a non-balanced pair");
                }
                return root;
            }
            if (root.Lhs != null)
            {
                var leftExplode = FindExplodable(root.Lhs, depth + 1);
                if (leftExplode != null) return leftExplode;
            }

            if (root.Rhs != null)
            {
                return FindExplodable(root.Rhs, depth + 1);
            }

            return null;
        }

        private static long Magnitude(Tree root)
        {
            long lhs = 0;
            if (root.LhsValue >= 0)
            {
                lhs += root.LhsValue;
            }
            else
            {
                lhs += Magnitude(root.Lhs);
            }

            long rhs = 0;
            if (root.RhsValue >= 0)
            {
                rhs += root.RhsValue;
            }
            else
            {
                rhs += Magnitude(root.Rhs);
            }

            return 3 * lhs + 2 * rhs;
        }
        
        private static void SolveFirstStar()
        {
            var lines = FileRead.ReadLines(FileName);
            var snailNumbers = lines.Select(ParseSnailNumber).ToList();
            var result = snailNumbers.Aggregate((aggregator, current) =>
                AddSnailNumber(aggregator, current).Pipe(ReduceSnailNumber));
            result.Print();
            Console.WriteLine();
            Console.WriteLine("Solution (1): " + Magnitude(result));
        }

        private static void SolveSecondStar()
        {
            var lines = FileRead.ReadLines(FileName);
            var maxMagn = 0L;
            for (var x = 0; x < lines.Count; x++)
            {
                for (var y = 0; y < lines.Count; ++y)
                {
                    if (y == x) continue;
                    var a = ParseSnailNumber(lines[x]);
                    var b = ParseSnailNumber(lines[y]);
                    var mag = AddSnailNumber(a, b).Pipe(ReduceSnailNumber).Pipe(Magnitude);
                    maxMagn = Math.Max(maxMagn, mag);
                }
            }

            Console.WriteLine("Solution (2): " + maxMagn);
        }

    }
}
