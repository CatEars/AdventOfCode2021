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

        private class TreeNode
        {
            public int Value { get; set; } = -1;
        }

        public void Run()
        {
            SolveFirstStar();
            SolveSecondStar();
        }

        private static void SolveFirstStar()
        {
            var lines = FileRead.ReadLines(FileName);
            var snailNumbers = lines.Select(ParseSnailNumber).ToList();
            var result = snailNumbers.Aggregate((aggregator, current) =>
                AddSnailNumber(aggregator, current).Pipe(ReduceSnailNumber));
            Console.WriteLine("Solution (1): " + Magnitude(result));
        }

        private static void SolveSecondStar()
        {
            var lines = FileRead.ReadLines(FileName);
            var maxMagnitude = 0L;
            for (var x = 0; x < lines.Count; x++)
            {
                for (var y = 0; y < lines.Count; ++y)
                {
                    if (y == x) continue;
                    var lhs = ParseSnailNumber(lines[x]);
                    var rhs = ParseSnailNumber(lines[y]);
                    var magnitude = AddSnailNumber(lhs, rhs).Pipe(ReduceSnailNumber).Pipe(Magnitude);
                    maxMagnitude = Math.Max(maxMagnitude, magnitude);
                }
            }

            Console.WriteLine("Solution (2): " + maxMagnitude);
        }

        private static BinaryTree<TreeNode> ParseSnailNumber(string line)
        {
            var stack = new Stack<BinaryTree<TreeNode>>();
            var root = BinaryTree<TreeNode>.Root();
            stack.Push(root);
            var idx = 1;
            while (idx < line.Length)
            {
                var symbol = line[idx];
                if ('0' <= symbol && symbol <= '9')
                {
                    var value = symbol - '0';
                    var leaf = BinaryTree<TreeNode>.Child(stack.Peek());
                    leaf.State.Value = value;
                }
                else if (symbol == '[')
                {
                    var parent = stack.Peek();
                    var child = BinaryTree<TreeNode>.Child(parent);
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

        private static BinaryTree<TreeNode> AddSnailNumber(BinaryTree<TreeNode> lhs, BinaryTree<TreeNode> rhs)
        {
            var parent = BinaryTree<TreeNode>.Root();
            parent.AddChild(lhs);
            parent.AddChild(rhs);
            return parent;
        }

        private static BinaryTree<TreeNode> ReduceSnailNumber(BinaryTree<TreeNode> root)
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

        private static void SplitSnailNumber(BinaryTree<TreeNode> splittable)
        {
            var split = BinaryTree<TreeNode>.Root();
            var left = BinaryTree<TreeNode>.Child(split);
            var right = BinaryTree<TreeNode>.Child(split);
            left.State.Value = splittable.State.Value >> 1;
            right.State.Value = (splittable.State.Value >> 1) + (splittable.State.Value & 1);
            split.Parent = splittable.Parent;

            if (splittable.IsLeftChild)
            {
                splittable.Parent!.Left = split;
            }
            else
            {
                splittable.Parent!.Right = split;
            }
        }

        private static void ExplodeSnailNumber(BinaryTree<TreeNode> explodable)
        {
            var lhsBase = explodable.Left.State.Value;
            var rhsBase = explodable.Right.State.Value;

            // Go back up until we are no longer the "left child"
            // Back up once, and select the left child
            // In that branch, find the rightmost child
            // Add our leftmost value to that node
            var lefty = explodable;
            while (lefty.IsLeftChild) lefty = lefty.Parent;
            if (lefty.Parent != null)
            {
                lefty = lefty.Parent.Left;
                lefty = lefty.Rightmost();
                lefty.State.Value += lhsBase;
            }

            var righty = explodable;
            while (righty.IsRightChild) righty = righty.Parent;
            if (righty.Parent != null)
            {
                righty = righty.Parent.Right;
                righty = righty.Leftmost();
                righty.State.Value += rhsBase;
            }

            // EXPLODE!
            explodable.Left = null;
            explodable.Right = null;
            explodable.State.Value = 0;
        }

        private static BinaryTree<TreeNode>? FindSplittable(BinaryTree<TreeNode> root)
        {
            return root.Find((node, _) => node.IsLeaf && node.State.Value >= 10, TreeSearchOrder.LeftToRight);
        }

        private static BinaryTree<TreeNode>? FindExplodable(BinaryTree<TreeNode> root)
        {
            return root.Find((node, context) => !node.IsLeaf && context.Depth >= 4, TreeSearchOrder.LeftToRight);
        }

        private static int Magnitude(BinaryTree<TreeNode> root)
        {
            if (root.IsLeaf)
            {
                return root.State.Value;
            }
            else
            {
                return 3 * Magnitude(root.Left) + 2 * Magnitude(root.Right);
            }
        }
    }
}