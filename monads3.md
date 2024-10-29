Over the last few posts we have established a pattern for implementing mixins in C# and another pattern for implementing monads in C#. Today, we will close out the series by seeing how our customers can extend these patterns externally.

We have the following framework that we will be shipping to our customers:

```csharp
public delegate IEnumerableMonad<TSource> Unit<TSource>(IEnumerable<TSource> source);

public interface IEnumerableMonad<TSource> : IEnumerable<TSource>
{
  IEnumerable<TSource> Source { get; }
  Unit<TElement> Unit<TElement>();
}

public static class EnumerableMonadExtensions
{
  public static IEnumerableMonad<TUnit> Create<TElement, TUnit>(this IEnumerableMonad<TElement> monad, IEnumerable<TUnit> enumerable)
  {
    if (monad.Source is IEnumerableMonad<TElement> nestedMonad)
    {
      enumerable = nestedMonad.Create(enumerable);
    }

    return monad.Unit<TUnit>()(enumerable);
  }
}

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
  public static int Count<T>(this IEnumerable<T> source)
  {
    if (source is ICountMixin<T> countMixin)
    {
      return countMixin.Count();
    }

    if (source is IEnumerableMonad<T> monad)
    {
      return monad.Source.Count();
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
      var selected = selectMixin.Select(selector);
      if (source is IEnumerableMonad<TSource> mixinMonad)
      {
        return mixinMonad.Create(selected);
      }
    }

    if (source is IEnumerableMonad<TSource> monad)
    {
      return monad.Create(monad.Source.Select(selector));
    }

    return SelectDefault(source, selector);
  }

  private static IEnumerable<TResult> SelectDefault<TSource, TResult>(IEnumerable<TSource> source, Func<TSource, TResult> selector)
  {
    foreach (var element in source)
    {
      yield return selector(element);
    }
  }

  public static IEnumerable<T> ApplyDefaultOptimizations<T>(this IEnumerable<T> source)
  {
    return new DefaultOpimizations<T>(source);
  }

  private sealed class DefaultOpimizations<T> : IEnumerableMonad<T>, ISelectMixin<T>
  {
    public DefaultOpimizations(IEnumerable<T> source)
    {
      Source = source;
    }

    public IEnumerable<T> Source { get; }

    public Unit<TElement> Unit<TElement>()
    {
      return toWrap => new DefaultOpimizations<TElement>(toWrap);
    }

    public IEnumerator<T> GetEnumerator()
    {
      return this.Source.GetEnumerator();
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
      return this.GetEnumerator();
    }

    public IEnumerable<TResult> Select<TResult>(Func<T, TResult> selector)
    {
      return new Selected<TResult>(this.Source, selector);
    }

    private sealed class Selected<TResult> : IEnumerable<TResult>, ICountMixin<TResult>
    {
      private readonly IEnumerable<T> source;
      private readonly Func<T, TResult> selector;

      public Selected(IEnumerable<T> source, Func<T, TResult> selector)
      {
        this.source = source;
        this.selector = selector;
      }

      public int Count()
      {
        return this.source.Count();
      }

      public IEnumerator<TResult> GetEnumerator()
      {
        return this.source.Select(this.selector).GetEnumerator();
      }

      IEnumerator IEnumerable.GetEnumerator()
      {
        return this.GetEnumerator();
      }
    }
  }
}
```
A customer who wants to implement `Shuffle` should follow the same conventions:
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
      var shuffled = shuffleMixin.Shuffle(random);
      if (source is IEnumerableMonad<T> mixinMonad)
      {
        return mixinMonad.Create(shuffled);
      }
    }

    if (source is IEnumerableMonad<T> monad)
    {
      return monad.Create(monad.Source.Shuffle(random));
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

  public static IEnumerable<T> ApplyShuffleOptimizations<T>(this IEnumerable<T> source)
  {
    return new ShuffleOptimizations<T>(source);
  }

  private sealed class ShuffleOptimizations<T> : IEnumerableMonad<T>, IShuffleMixin<T>
  {
    public ShuffleOptimizations(IEnumerable<T> source)
    {
      Source = source;
    }

    public IEnumerable<T> Source { get; }

    public Unit<TElement> Unit<TElement>()
    {
      return toWrap => new ShuffleOptimizations<TElement>(toWrap);
    }

    public IEnumerator<T> GetEnumerator()
    {
      return this.Source.GetEnumerator();
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
      return this.GetEnumerator();
    }

    public IEnumerable<T> Shuffle(Random random)
    {
      return new Shuffled(this.Source, random);
    }

    private sealed class Shuffled : ICountMixin<T>
    {
      private readonly IEnumerable<T> source;
      private readonly Random random;

      public Shuffled(IEnumerable<T> source, Random random)
      {
        this.source = source;
        this.random = random;
      }

      public int Count()
      {
        return this.source.Count();
      }

      public IEnumerator<T> GetEnumerator()
      {
        return this.source.Shuffle(this.random).GetEnumerator();
      }

      IEnumerator IEnumerable.GetEnumerator()
      {
        return this.GetEnumerator();
      }
    }
  }
}
```
