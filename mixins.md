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
