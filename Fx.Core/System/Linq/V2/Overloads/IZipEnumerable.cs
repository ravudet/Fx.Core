namespace System.Linq.V2
{
    public interface IZipEnumerable<TFirst> : IV2Enumerable<TFirst>
    {
        public IV2Enumerable<(TFirst First, TSecond Second, TThird Third)> Zip<TSecond, TThird>(
            IV2Enumerable<TSecond> second,
            IV2Enumerable<TThird> third)
        {
            return this.ZipDefault(second, third);
        }

        public IV2Enumerable<TResult> Zip<TSecond, TResult>(
            IV2Enumerable<TSecond> second,
            Func<TFirst, TSecond, TResult> resultSelector)
        {
            return this.ZipDefault(second, resultSelector);
        }

        public IV2Enumerable<(TFirst First, TSecond Second)> Zip<TSecond>(IV2Enumerable<TSecond> second)
        {
            return this.ZipDefault(second);
        }
    }
}