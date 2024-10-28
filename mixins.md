Today I would like to talk about something I'm referring to as the "mixins pattern". This pattern is trying to establish some conventions a library can follow to simulate the "mixins" feature that is availabe in other languages.

The C# feature that currently does the best impression of mixins is the extension method feature. Extension methods allow for the external extensibility of a type without deriving or changing the type. `IEnumerable<T>` is probably the most prolific example in .NET. To illustrate, I am able to add a `Shuffle` method to `IEnumerable<T>` without making any changes to the .NET framework itself:

```
public static class EnumerableExtensions
{
  public static IEnumerable<T> Shuffle<T>(this IEnumerable<T> source, Random random)
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

This is wonderful! Now, every implementation of `IEnumerable<T>` can also be shuffled to produce a random sequence of its elements! However, you may notice that this implementation is not ideal because of its requirement to get the count of elements, requiring a `ToList()` to be called as the first step. There are some solutions to that which I hope to discuss in the future. Instead, let's use another example from .NET itself; in LINQ's earliest versions, it had code similar to the following:

```
public static class EnumerableExtensions
{
  public static int Count<T>(this IEnumerable<T> source)
  {
    if (source is ICollection<T> collectionOfT)
    {
      return collectionOfT.Count;
    }

    if (source is ICollection collection)
    {
      return collection.Count;
    }

    var count = 0;
    foreach (var element in source)
    {
      ++count;
    }

    return count;
  }

  public static IEnumerable<TResult> Select<TSource, TResult>(this IEnumerable<TSource> source, Func<TSource, TResult> selector)
  {
    foreach (var element in source)
    {
      yield return selector(element);
    }
  }
}
```

This implementation of `Count` is actually pretty forward-looking. Someone realized that most of the existing implementations of `IEnumerable<T>` already knew their own count, so it would be very slow to individually count the elements as an `O(n)` operation instead of just returning the count as an `O(1)` operation. However, look what happens when we call `Select`:

```
var sequence = new[] { 1, 2, 3, 4 };
var count = sequence
  .Select(element => element * 2)
  .Count();
```

Because `Select` is called, the return type of `sequence.Select(...)` is no longer `ICollection<T>`, so the `Count` method has to individually enumerate all of the elements in order to get the count. But we know **semantically** that `Select` won't change the number of elements in the sequence. Somewhere along the way, .NET updated their implementations to look more like this (NOTE: I'm taking a number of liberties to reduce the complexity of this code; the .NET team has a **very** involved design for LINQ that is entirely out of scope):

```diff
public static class EnumerableExtensions
{
+ private interface IIterator<T> : IEnumerable<T>
+ {
+   bool TryGetCount(out int count);
+ }

  public static int Count<T>(this IEnumerable<T> source)
  {
    if (source is IColleciton<T> collectionOfT)
    {
      return collectionOfT.Count;
    }

    if (source is ICollection collection)
    {
      return collection.Count;
    }

+   if (source is IIterator<T> iterator && iterator.TryGetCount(out var count))
+   {
+     return count;
+   }

    var count = 0;
    foreach (var element in source)
    {
      ++count;
    }

    return count;
  }

  public static IEnumerable<TResult> Select<TSource, TResult>(this IEnumerable<TSource> source, Func<TSource, TResult> selector)
  {
-   foreach (var element in source)
-   {
-     yield return selector(element);
-   }
+   return new SelectIterator<TSource, TResult>(source, selector);
  }

+ private sealed class SelectIterator<TSource, TResult> : IIterator<TResult>
+ {
+   private readonly IEnumerable<TSource> source;
+   private readonly Func<TSource, TResult> selector;
+
+   public SelectIterator(IEnumerable<TSource> source, Func<TSource, TResult> selector)
+   {
+     this.source = source;
+     this.selector = selector;
+   }
+
+   public IEnumerator<TResult> GetEnumerator()
+   {
+     foreach (var element in this.source)
+     {
+       yield return this.selector(element);
+     }
+   }
+
+   public bool TryGetCount(out int count)
+   {
+     if (this.source is ICollection<TSource> collectionOfT)
+     {
+       count = collectionOfT.Count;
+       return true;
+     }
+
+     if (source is ICollection collection)
+     {
+       count = collection.Count;
+       return true;
+     }
+
+     if (source is IIterator<T> iterator && iterator.TryGetCount(out var count))
+     {
+       return count;
+     }
+
+     count = -1;
+     return false;
+   }
+
+   IEnumerator IEnumerable.GetEnumerator()
+   {
+     return this.GetEnumerator();
+   }
+ }
}
```











































