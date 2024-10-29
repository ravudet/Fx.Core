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

Notice that we are back to our problem from last time: `Shuffle` semantically doesn't affect the count, but if we call `sequence.Shuffle(new Random()).Count()`, the original count of `sequence` is lost by the time the `Count` method is called. Luckily because of the `IShuffleMixin`, we can address this in an externally extensible way:

```csharp
public sealed class ShuffleIterator<T> : IShuffleMixin<T>
{
  private readonly IEnumerable<T> source;

  public ShuffleIterator(IEnumerable<T> source)
  {
    this.source = source;
  }

  public IEnumerable<T> Shuffle(Random random)
  {
    if (this.source is ICountMixin<T> countMixin)
    {
      return new CountedShuffled(countMixin, random);
    }

    return this.source.Shuffle(random);
  }

  private sealed class CountedShuffled : ICountMixin<T>
  {
    private readonly ICountMixin<T> counted;
    private readonly Random random;

    public CountedShuffled(ICountMixin<T> counted, Random random)
    {
      this.counted = counted;
      this.random = random;
    }

    public int Count()
    {
      return this.counted.Count();
    }

    public IEnumerator<T> GetEnumerator()
    {
      return this.counted.Shuffle(this.random).GetEnumerator();
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
      return this.GetEnumerator();
    }
  }

  public IEnumerator<T> GetEnumerator()
  {
    return this.source.GetEnumerator();
  }

  IEnumerator IEnumerable.GetEnumerator()
  {
    return this.GetEnumerator();
  }
}
```

Wonderful! We've allowed the count of the original sequence to be preserved even after a `Shuffle` has been performed. In fact, `new ShuffleIterator<T>(sequence).Shuffle(new Random()).Count()` doesn't even do **any** of the shuffling; this is quite a performance benefit. However, notice that if `new ShuffleIterator<T>(sequence).Select(_ => _).Shuffle(new Random()).Count()` is called, the call to `Select` causes us to lose track of the `ShuffleIterator` instance, which means that the count of `sequence` will not be preserved by the `Select` call and therefore not preserved by the `Shuffle` call. Now, we *could* achieve this by writing quite a lot of in `ShuffleIterator`, but we would need to write the same code in practically *every* mixin implementation that gets written. Let's see how monads can be used to achieve this instead. We will explore this in the next post.
