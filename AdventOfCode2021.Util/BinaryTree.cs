using System;
using System.Collections.Generic;

namespace AdventOfCode2021.Util
{

    public record TreeSearchContext(int Depth);

    public enum TreeSearchOrder
    {
        LeftToRight = 1
    }

    internal interface ITreeSearchOrderer<TState> where TState : new()
    {
        IEnumerable<BinaryTree<TState>> Order(BinaryTree<TState> tree);
    }

    public class BinaryTree<TState> where TState : new()
    {
        
        public BinaryTree<TState>? Parent { get; set; }
        public BinaryTree<TState>? Left { get; set; }
        public BinaryTree<TState>? Right { get; set; }

        public TState State { get; set; } = new();
        
        public bool IsLeftChild => Parent != null && Parent.Left == this;
        public bool IsRightChild => Parent != null && Parent.Right == this;
        public bool IsLeaf => Left == null && Right == null;
        
        public BinaryTree<TState> Leftmost() => Left == null ? this : Left.Leftmost();
        public BinaryTree<TState> Rightmost() => Right == null ? this : Right.Rightmost();
        
        public static BinaryTree<TState> Root()
        {
            return new();
        }

        public static BinaryTree<TState> Child(BinaryTree<TState> parent)
        {
            var child = Root();
            parent.AddChild(child);
            return child;
        }
        
        public BinaryTree<TState> AddChild(BinaryTree<TState> child)
        {
            child.Parent = this;
            if (Left == null)
            {
                Left = child;
            }
            else
            {
                Right = child;
            }

            return child;
        }


        private BinaryTree<TState>? FindLeftToRight(Func<BinaryTree<TState>, TreeSearchContext, bool> predicate,
            TreeSearchContext searchContext)
        {
            if (Left != null)
            {
                var left = Left.FindLeftToRight(predicate, searchContext with {Depth = searchContext.Depth + 1});
                if (left != null) return left;
            }
            var self = predicate(this, searchContext) ? this : null;
            if (self != null) return self;

            if (Right != null)
            {
                var right = Right.FindLeftToRight(predicate, searchContext with {Depth = searchContext.Depth + 1});
                if (right != null) return right;
            }

            return null;
        }

        public BinaryTree<TState>? Find(Func<BinaryTree<TState>, TreeSearchContext, bool> predicate, TreeSearchOrder order)
        {
            if (order == TreeSearchOrder.LeftToRight)
            {
                return FindLeftToRight(predicate, new TreeSearchContext(0));
            }
            else
            {
                throw new ArgumentException($"{order} is not a valid ordering when searching the tree");
            }
        }
        
    }
}