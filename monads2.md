In the last post, we saw that mixins are very powerful in letting our customers extend our libraries in a backwards-compatible way while still providing default implementations. Using `IEnumerable<T>`, we have this basic framework:
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

and our customer has extended this with the following code that follwows the same patterns:
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

And we noticed the repetitive code that will need to be written to implement `Shuffle` in such a way to preserve the count through `Select`s. To implement a monad, we will need two things:
1. The "source" instance that the monad is wrapping
2. A factory method that gives us an instance of the monad by wrapping a "source" instance (sometimes referred to as the "unit")
3. The functionality that the monad provides

Modeling this in C# is actually pretty straightforward:
```csharp
public delegate IEnumerableMonad<TSource> Unit<TSource>(IEnumerable<TSource> source);

public interface IEnumerableMonad<TSource> : IEnumerable<TSource>
{
  IEnumerable<TSource> Source { get; }
  Unit<TElement> Unit<TElement>();
}
```

These types meet the three requirements:
1. We have the `Source` property to expose the original instance
2. We have the `Unit` property to expose the way to wrap instances into the monad
3. The monad implements `IEnumerable<T>`, as `IEnumerable<T>` is the functionality that is being provided by the monad

Now let's update our framework code to leverage this monad:
```diff
+public static class EnumerableMonadExtensions
+{
+public static IEnumerableMonad<TUnit> Create<TElement, TUnit>(this IEnumerableMonad<TElement> monad, IEnumerable<TUnit> enumerable)
+{
+if (monad.Source is IEnumerableMonad<TElement> nestedMonad)
+{
+    enumerable = nestedMonad.Create(enumerable);
+}
+
+return monad.Unit<TUnit>()(enumerable);
+}
+}

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
+
+if (source is IEnumerableMonad<T> monad)
+{
+    return monad.Source.Count();
+}

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
-return selectMixin.Select(selector);
+var selected = selectMixin.Select(selector);
+if (source is IEnumerableMonad<TSource> mixinMonad)
+{
+return mixinMonad.Create(selected);
+}
}
+
+if (source is IEnumerableMonad<TSource> monad)
+{
+return monad.Create(monad.Source.Select(selector));
+}

return SelectDefault(source, selector);
}

private static IEnumerable<TResult> SelectDefault<TSource, TResult>(IEnumerable<TSource> source, Func<TSource, TResult> selector)
{
-if (source is ICountMixin<TSource> countMixin)
-{
-return new CountedSelectIterator<TSource, TResult>(countMixin, selector);
-}
-
-return new DefaultSelectIterator<TSource, TResult>(source, selector);
+foreach (var element in source)
+{
+yield return selector(element);
+}
}

-private sealed class CountedSelectIterator<TSource, TResult> : ICountMixin<TResult>
-{
-private readonly ICountMixin<TSource> countMixin;
-private readonly Func<TSource, TResult> selector;
-
-public CountedSelectIterator(ICountMixin<TSource> countMixin, Func<TSource, TResult> selector)
-{
-this.countMixin = countMixin;
-this.selector = selector;
-}
-
-public int Count()
-{
-return this.countMixin.Count();
-}
-
-public IEnumerator<TResult> GetEnumerator()
-{
-return DefaultSelectIterator<TSource, TResult>.Select(this.countMixin, this.selector).GetEnumerator();
-}
-
-IEnumerator IEnumerable.GetEnumerator()
-{
-return this.GetEnumerator();
-}
-}
-
-private sealed class DefaultSelectIterator<TSource, TResult> : IEnumerable<TResult>
-{
-private readonly IEnumerable<TSource> source;
-private readonly Func<TSource, TResult> selector;
-
-public DefaultSelectIterator(IEnumerable<TSource> source, Func<TSource, TResult> selector)
-{
-this.source = source;
-this.selector = selector;
-}
-
-public IEnumerator<TResult> GetEnumerator()
-{
-return Select(this.source, this.selector).GetEnumerator();
-}
-
-public static IEnumerable<TResult> Select(IEnumerable<TSource> source, Func<TSource, TResult> selector)
-{
-foreach (var element in source)
-{
-yield return selector(element);
-}
-}
-
-IEnumerator IEnumerable.GetEnumerator()
-{
-return this.GetEnumerator();
-}
-}

+public static IEnumerable<T> ApplyDefaultOptimizations<T>(this IEnumerable<T> source)
+{
+return new DefaultOpimizations<T>(source);
+}
+
+private sealed class DefaultOpimizations<T> : IEnumerableMonad<T>, ISelectMixin<T>
+{
+public DefaultOpimizations(IEnumerable<T> source)
+{
+Source = source;
+}
+
+public IEnumerable<T> Source { get; }
+
+public Unit<TElement> Unit<TElement>()
+{
+return toWrap => new DefaultOpimizations<TElement>(toWrap);
+}
+
+public IEnumerator<T> GetEnumerator()
+{
+return this.Source.GetEnumerator();
+}
+
+IEnumerator IEnumerable.GetEnumerator()
+{
+return this.GetEnumerator();
+}
+
+public IEnumerable<TResult> Select<TResult>(Func<T, TResult> selector)
+{
+return new Selected<TResult>(this.Source, selector);
+}
+
+private sealed class Selected<TResult> : IEnumerable<TResult>, ICountMixin<TResult>
+{
+private readonly IEnumerable<T> source;
+private readonly Func<T, TResult> selector;
+
+public Selected(IEnumerable<T> source, Func<T, TResult> selector)
+{
+this.source = source;
+this.selector = selector;
+}
+
+public int Count()
+{
+return this.source.Count();
+}
+
+public IEnumerator<TResult> GetEnumerator()
+{
+return this.source.Select(this.selector).GetEnumerator();
+}
+
+IEnumerator IEnumerable.GetEnumerator()
+{
+return this.GetEnumerator();
+}
+}
+}
...
}
```




TODO next time, we will see the code that our customers can now write to implement a shuffle that preserves the count































