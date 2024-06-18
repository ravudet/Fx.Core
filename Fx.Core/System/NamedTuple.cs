using System.Runtime.CompilerServices;

namespace System
{
    public abstract class NamedTuple<TName, TValue>
    {
        protected NamedTuple(TValue value) //// TODO better if this is private
        {
            this.Value = value;
        }

        public TValue Value { get; }
    }

    public sealed class Single<TSingleName, TSingleValue> : NamedTuple<TSingleName, TSingleValue>
    {
        public Single(TSingleValue value)
            : base(value)
        {
        }
    }

    public sealed class RavudetTuple<TTupleName, TTupleValue, TTheRest, TTheRestName, TTheRestValue> : NamedTuple<TTupleName, TTupleValue> where TTheRest : NamedTuple<TTheRestName, TTheRestValue>
    {
        public RavudetTuple(TTupleValue value, TTheRest theRest)
            : base(value)
        {
            this.TheRest = theRest;
        }

        public TTheRest TheRest { get; }
    }

    public static class NamedTuple<TName>
    {
        public static Single<TName, TValue> Create<TValue>(TValue value)
        {
            return new Single<TName, TValue>(value);
        }

        public static RavudetTuple<TName, TValue, NamedTuple<TTheRestName, TTheRestValue>, TTheRestName, TTheRestValue> Create<TValue, TTheRestName, TTheRestValue>(TValue value, NamedTuple<TTheRestName, TTheRestValue> theRest) 
        {
            return new RavudetTuple<TName, TValue, NamedTuple<TTheRestName, TTheRestValue>, TTheRestName, TTheRestValue>(value, theRest);
        }
    }

    public static class NamedTuple2<TName>
    {
        public static Single<TName, TValue> Create<TValue>(TValue value)
        {
            return new Single<TName, TValue>(value);
        }

        public static RavudetTuple<TName, TValue, TTheRest, TTheRestName, TTheRestValue> Create<TValue, TTheRest, TTheRestName, TTheRestValue>(TValue value, TTheRest theRest) where TTheRest : NamedTuple<TTheRestName, TTheRestValue>
        {
            return new RavudetTuple<TName, TValue, TTheRest, TTheRestName, TTheRestValue>(value, theRest);
        }
    }

    public static class NamedTuple3<TName>
    {
        public static Fluent<TName> Fluent { get; } = new Fluent<TName>();
    }

    public sealed class Fluent<TName>
    {
        public Single<TName, TValue> Create<TValue>(TValue value)
        {
            return new Single<TName, TValue>(value);
        }

        public RavudetTuple<TName, TValue, TTheRest, TTheRestName, TTheRestValue> Create<TValue, TTheRest, TTheRestName, TTheRestValue>(TValue value, TTheRest theRest) where TTheRest : NamedTuple<TTheRestName, TTheRestValue>
        {
            return new RavudetTuple<TName, TValue, TTheRest, TTheRestName, TTheRestValue>(value, theRest);
        }
    }

    public static class NamedTuple4
    {
        public static Fluent4<TTheRest, TTheRestName, TTheRestValue> Create4<TTheRest, TTheRestName, TTheRestValue>(this TTheRest theRest) where TTheRest : NamedTuple<TTheRestName, TTheRestValue>
        {
            return new Fluent4<TTheRest, TTheRestName, TTheRestValue>();
        }
    }

    public sealed class Fluent4<TTheRest, TTheRestName, TTheRestValue> where TTheRest : NamedTuple<TTheRestName, TTheRestValue>
    {
        public static NestedFluent4<TName, TTheRest, TTheRestName, TTheRestValue> Nested<TName>()
        {
            return new NestedFluent4<TName, TTheRest, TTheRestName, TTheRestValue>();
        }
    }

    public sealed class NestedFluent4<TName, TTheRest, TTheRestName, TTheRestValue> where TTheRest : NamedTuple<TTheRestName, TTheRestValue>
    {
        public RavudetTuple<TName, TValue, TTheRest, TTheRestName, TTheRestValue> Create<TValue>(TValue value)
        {
            return new RavudetTuple<TName, TValue, TTheRest, TTheRestName, TTheRestValue>(value, null);
        }
    }

    public static class Driver
    {
        public static void DoWork()
        {
            var result = NamedTuple<int>.Create(
                "qwer", 
                NamedTuple<string>.Create(
                    "Asdf",
                    NamedTuple<double>.Create("zxcvz")));

            var result2 = NamedTuple2<int>.Create<string, RavudetTuple<string, string, Single<double, string>, double, string>, string, string>(
                "qwer",
                NamedTuple2<string>.Create<string, Single<double, string>, double, string>(
                    "Asdf",
                    NamedTuple2<double>.Create("zxcvz")));

            /*var result3 = NamedTuple3<double>.Fluent.Create(
                "asdf",
                NamedTuple3<double>.Fluent.Create("zxcv"));*/

            var result4 = new Single<double, string>("zxcv").Create4()
        }
    }
}
