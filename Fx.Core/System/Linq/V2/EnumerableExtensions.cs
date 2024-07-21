using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Xml.Schema;

namespace System.Linq.V2
{
    public static class EnumerableExtensions
    {
        public static IV2Enumerable<T> Shuffle<T>(this IV2Enumerable<T> self, Random random)
        {
            //// TODO do caller expect the result to always be the same without a tolist?

            if (self is IShuffleableMixin<T> shuffle)
            {
                return shuffle.Shuffle(random);
            }

            return self.ShuffleDefault(random);
        }

        internal static IV2Enumerable<T> ShuffleDefault<T>(this IV2Enumerable<T> self, Random random)
        {
            if (self is IEnumerableMonad<T> monad)
            {
                return monad.Source.Shuffle(random);
            }

            return new ShuffleEnumerable<T>(self, random);
        }

        private sealed class ShuffleEnumerable<T> : IV2Enumerable<T>, IFirstableMixin<T> //// TODO should this be a monad?
        {
            private readonly IV2Enumerable<T> self;

            private readonly Random random;

            public ShuffleEnumerable(IV2Enumerable<T> self, Random random)
            {
                this.self = self;
                this.random = random;
            }

            public IEnumerator<T> GetEnumerator()
            {
                if (this.self is ICountableMixin<T> countable && this.self is IElementAtableMixin<T> elementAtable)
                {
                    var enumerable = new CountPlusElementAt(countable, elementAtable);
                    return ShuffleImplementation(enumerable);
                }
                else
                {
                    return ShuffleImplementation(new Listable(this.self.ToList()));
                }
            }

            private sealed class Listable : ICountableMixin<T>, IElementAtableMixin<T>
            {
                private readonly IReadOnlyList<T> list;

                public Listable(IReadOnlyList<T> list)
                {
                    this.list = list;
                }

                public int Count()
                {
                    return this.list.Count;
                }

                public T ElementAt(int index)
                {
                    return this.list[index];
                }

                public IEnumerator<T> GetEnumerator()
                {
                    throw new NotImplementedException();
                }

                IEnumerator IEnumerable.GetEnumerator()
                {
                    throw new NotImplementedException();
                }
            }

            /// <summary>
            /// TODO can you make this a struct?
            /// </summary>
            private sealed class CountPlusElementAt : ICountableMixin<T>, IElementAtableMixin<T>
            {
                private readonly ICountableMixin<T> countable;

                private readonly IElementAtableMixin<T> elementAtable;

                public CountPlusElementAt(ICountableMixin<T> countable, IElementAtableMixin<T> elementAtable)
                {
                    this.countable = countable;
                    this.elementAtable = elementAtable;
                }

                public int Count()
                {
                    return this.countable.Count();
                }

                public T ElementAt(int index)
                {
                    return this.elementAtable.ElementAt(index);
                }

                public IEnumerator<T> GetEnumerator()
                {
                    throw new NotImplementedException();
                }

                IEnumerator IEnumerable.GetEnumerator()
                {
                    throw new NotImplementedException();
                }
            }

            private IEnumerator<T> ShuffleImplementation<TEnumerable>(TEnumerable enumerable) where TEnumerable : ICountableMixin<T>, IElementAtableMixin<T>
            {
                var array = new T?[enumerable.Count()];
                for (int i = 0; i < array.Length; ++i)
                {
                    var next = this.random.Next();
                    if (array[next] == null)
                    {
                        array[next] = enumerable.ElementAt(next);
                    }

                    var temp = array[i];
                    array[i] = array[next];
                    array[next] = temp;

                    yield return array[i];
                }
            }

            public T First()
            {
                var next = this.random.Next(this.self.Count());
                return this.self.ElementAt(next);
            }

            IEnumerator IEnumerable.GetEnumerator()
            {
                return this.GetEnumerator();
            }
        }
    }

    public interface IShuffleableMixin<T> : IV2Enumerable<T>
    {
        public IV2Enumerable<T> Shuffle(Random random)
        {
            return this.ShuffleDefault(random);
        }
    }
}
