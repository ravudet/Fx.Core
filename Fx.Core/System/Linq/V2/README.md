## Use cases

The contents of the `System.Linq.V2` namespace provide an external-extensibility mechanism to the `System.Linq` feature provided by .NET.
There are two main use cases for this extensibility:
1. A developer wants to provide an optimized variation of a LINQ extension method for a specific collection type (often a custom collection implementation)
2. A developer wants to provide a variation of a LINQ extension method by implementing a general-form algorithm that is different from the default implementation provided by .NET

## Mixins

### Use case 1

The existing implementation of `.Count()` comes closest to implementing use case 1.
A developer can implement the `ICollection<T>` interface in order to provide LINQ with an optimized implementation of `.Count()` for their specific collection.
However, note that:
1. `IReadOnlyCollection<T>` and `IReadOnlyList<T>` can **not** be used to provide this optimization (.NET doesn't check for these interfaces)
2. Other LINQ extension methods do not have similar public interface which a developer can implement

To overcome this, `System.Linq.V2` provides the "mixin" interfaces as well as its own extension methods for `IV2Enumerable<T>`.
For each existing LINQ extension method, there is a corresponding extension method in `System.Linq.V2`, and there is the partner mixin interface as well.
In this way, the developer can implement the mixin, and V2 will "automitically" pick up the new variation.

Back to the `.Count()` example, TODO

### Use case 2

TODO note that mixins solve this
TODO use a "range of counts" and concat as an example

## Monad

TODO