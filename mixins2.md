To continue the previous post about the "mixin pattern", let's see what it would look like to allow our `Shuffle` method to preserve its count.

.NET has internally introduced something like the following interface:
```
private interface IIterator<T> : IEnumerable<T>
{
  bool TryGetCount(out int count);
}
```

And our current `Shuffle` implementation looks like:

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

If .NET would make the `IIterator` interface public, then we could implement that interface like so:

```
public static class EnumerableExtensions
{
  public static IEnumerable<T> Shuffle<T>(this IEnumerable<T> source, Random random)
  {
-   var list = source.ToList();
-   for (int i = 0; i < list.Count; ++i)
-   {
-     var next = random.Next(i, list.Count);
-     var temp = list[i];
-     list[i] = list[next];
-     list[next] = temp;
-
-     yield return list[i];
-   }
+   return new ShuffleIterator<T>(source, random);
  }

+ private sealed class ShuffleIterator<T> : IIterator<T>
+ {
+   private readonly IEnumerable<T> source;
+   private readonly Random random;
+
+   public ShuffleIterator(IEnumerable<T> source, Random random)
+   {
+     this.source = source;
+     this.random = random;
+   }
+
+   public IEnumerator<TResult> GetEnumerator()
+   {
+     var list = source.ToList();
+     for (int i = 0; i < list.Count; ++i)
+     {
+       var next = random.Next(i, list.Count);
+       var temp = list[i];
+       list[i] = list[next];
+       list[next] = temp;
+
+        yield return list[i];
+     }
+   }
+
+   public bool TryGetCount(out int count)
+   {
+     if (this.source is ICollection<TSource> collectionOfT)
+     {
+         count = collectionOfT.Count;
+         return true;
+     }
+
+     if (source is ICollection collection)
+     {
+         count = collection.Count;
+         return true;
+     }
+
+     if (source is IIterator<T> iterator)
+     {
+       return iterator.TryGetCount(out count);
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
