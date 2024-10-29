Last time, we discussed how to mixin-like functionality using interfaces and extension methods to provide external extensibility over default behavior. We did this using a `Shuffle` method extending `IEnumerable<T>`, and ended up with something that looks like:

```csharp
public interface IShuffleMixin<T> : IEnumerable<T>
{
  IEnumerable<T> Shuffle(Random random);
}

public static class EnumerableExtensions
{
  public static IEnumerable<T> Shuffle<T>(this IEnumerable<T> source, Random random)
  {
    if (source is IShuffleMixin<T> shuffleMixin)
    {
      return shuffleMixin.Shuffle(random);
    }

    return ShuffleDefault(source, random);
  }

  private static IEnumerable<T> ShuffleDefault<T>(IEnumerable<T> source, Random random)
  {
    var list = source.ToList();
    for (int i = 0; i < list.Count; ++i)
    {
      var next = random.Next(i, list.Count);
      var temp = list[i];
      list[i] = list[next];
      list[next] = temp;

      yield return list[i];
    }
  }
}
```

To really demonstrate how we can leverage monads here, we will need to assume that .NET follows this same pattern in a couple of other instances:
```csharp
public interface ICountMixin<T> : IEnumerable<T>
{
  int Count();
}

public interface ISelectMixin<TSource> : IEnumerable<TSource>
{
  IEnumerable<TResult> Select<TResult>(Func<TSource, TResult> selector);
}

public static class Enumerable
{
  ...
  public static int Count<T>(this IEnumerable<T> source)
  {
    if (source is ICountMixin<T> countMixin)
    {
      return countMixin.Count();
    }

    return CountDefault(source);
  }

  private static int CountDefault<T>(IEnumerable<T> source)
  {
    var count = 0;
    foreach (var element in source)
    {
      ++count;
    }

    return count;
  }

  public static IEnumerable<TResult> Select<TSource, TResult>(this IEnumerable<TSource> source, Func<TSource, TResult> selector)
  {
    if (source is ISelectMixin<TSource> selectMixin)
    {
      return selectMixin.Select(selector);
    }

    return SelectDefault(source, selector);
  }

  private static IEnumerable<TResult> SelectDefault<TSource, TResult>(IEnumerable<TSource> source, Func<TSource, TResult> selector)
  {
    if (source is ICountMixin<TSource> countMixin)
    {
      return new CountedSelectIterator<TSource, TResult>(countMixin, selector);
    }

    return new DefaultSelectIterator<TSource, TResult>(source, selector);
  }

  private sealed class CountedSelectIterator<TSource, TResult> : ICountMixin<TResult>
  {
    private readonly ICountMixin<TSource> countMixin;
    private readonly Func<TSource, TResult> selector;

    public CountedSelectIterator(ICountMixin<TSource> countMixin, Func<TSource, TResult> selector)
    {
      this.countMixin = countMixin;
      this.selector = selector;
    }

    public int Count()
    {
      return this.countMixin.Count();
    }

    public IEnumerator<TResult> GetEnumerator()
    {
      return DefaultSelectIterator<TSource, TResult>.Select(this.countMixin, this.selector).GetEnumerator();
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
      return this.GetEnumerator();
    }
  }

  private sealed class DefaultSelectIterator<TSource, TResult> : IEnumerable<TResult>
  {
    private readonly IEnumerable<TSource> source;
    private readonly Func<TSource, TResult> selector;

    public DefaultSelectIterator(IEnumerable<TSource> source, Func<TSource, TResult> selector)
    {
      this.source = source;
      this.selector = selector;
    }

    public IEnumerator<TResult> GetEnumerator()
    {
      return Select(this.source, this.selector).GetEnumerator();
    }

    public static IEnumerable<TResult> Select(IEnumerable<TSource> source, Func<TSource, TResult> selector)
    {
      foreach (var element in source)
      {
        yield return selector(element);
      }
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
      return this.GetEnumerator();
    }
  }
  ...
}
```


TODO select.shuffle.count































