using System.Collections;
using System.Collections.Generic;

namespace System.Linq.V2
{
    public static partial class V2Enumerable
    {
        public static IV2Enumerable<TResult> Empty<TResult>()
        {
            return EmptyEnumerable<TResult>.Instance;
        }

        private sealed class EmptyEnumerable<T> : IV2Enumerable<T>
        {
            private EmptyEnumerable()
            {
            }

            public static EmptyEnumerable<T> Instance { get; } = new EmptyEnumerable<T>();

            public IEnumerator<T> GetEnumerator()
            {
                return Enumerator.Instance;
            }

            IEnumerator IEnumerable.GetEnumerator()
            {
                return this.GetEnumerator();
            }

            private sealed class Enumerator : IEnumerator<T>
            {
                private Enumerator()
                {
                }

                public static Enumerator Instance { get; } = new Enumerator();
                    

                public T Current
                {
                    get
                    {
                        return default;
                    }
                }

                object IEnumerator.Current
                {
                    get
                    {
                        return this.Current;
                    }
                }

                public void Dispose()
                {
                }

                public bool MoveNext()
                {
                    return false;
                }

                public void Reset()
                {
                }
            }
        }
    }
}
