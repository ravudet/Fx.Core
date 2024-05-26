namespace System.Linq.Expressions
{
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Diagnostics;

    public interface ITree<out TValue>
    {
        TValue Value { get; }

        IEnumerable<ITree<TValue>> Subtrees { get; }
    }

    public sealed class Node<T> : ITree<T>
    {
        public Node(T value, IEnumerable<ITree<T>> subtrees)
        {
            this.Value = value;
            this.Subtrees = subtrees;
        }

        public T Value { get; }

        public IEnumerable<ITree<T>> Subtrees { get; }
    }

    public static class Node
    {
        public static Node<T> Create<T>(T value)
        {
            return new Node<T>(value, Enumerable.Empty<ITree<T>>());
        }

        public static Node<T> Create<T>(T value, IEnumerable<ITree<T>> subtrees)
        {
            return new Node<T>(value, subtrees);
        }
    }

    public abstract class PotentialMatch<T>
    {
        private PotentialMatch()
        {
        }

        public sealed class NoMatch : PotentialMatch<T>
        {
            public T Value { get; }
        }

        public sealed class Match : PotentialMatch<T>
        {
            public ITree<T> Original { get; }
        }
    }

    public static class ExpressionStuff
    {
        public static ITree<T> Replace<T>(this ITree<T> tree, ITree<T> template, ITree<T> replacement, IEqualityComparer<T> comparer)
        {
            return tree.MatchTemplate(template, comparer).ReplaceMatches(replacement);
        }

        public static ITree<T> ReplaceMatches<T>(this ITree<PotentialMatch<T>> matches, ITree<T> replacement)
        {
            if (matches.Value is PotentialMatch<T>.Match match)
            {
                return replacement;
            }
            else if (matches.Value is PotentialMatch<T>.NoMatch noMatch)
            {
                return Node.Create(noMatch.Value, matches.Subtrees.Select(subtree => subtree.ReplaceMatches(replacement)));
            }
            else
            {
                throw new Exception("TODO");
            }
        }

        public static ITree<T> ReconstructOriginal<T>(this ITree<PotentialMatch<T>> matches)
        {
            if (matches.Value is PotentialMatch<T>.Match match)
            {
                return match.Original;
            }
            else if (matches.Value is PotentialMatch<T>.NoMatch noMatch)
            {
                return Node.Create(noMatch.Value, matches.Subtrees.Select(subtree => subtree.ReconstructOriginal()));
            }
            else
            {
                throw new Exception("TODO");
            }
        }

        public static ITree<PotentialMatch<T>> MatchTemplate<T>(this ITree<T> tree, ITree<T> template, IEqualityComparer<T> comparer)
        {
            if (comparer.Equals(tree.Value, template.Value))
            {
            }

            return null;
        }
    }
}
