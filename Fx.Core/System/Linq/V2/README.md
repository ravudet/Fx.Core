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

Back to the `.Count()` example, the developer can implement `ICountableMixin<T>` to provide their custom logic.
Because `ICountableMixin<T>` has no other purpose, the developer doesn't need to worry about any misleading interfaces like with `ICollection<T>` (for example, a read-only collection "looks" writable if it implements `ICollection<T>`).
Once `ICountableMixin<T>` is implemented, any call to `.Count()` will use the developer's custom logic; if the mixin is not implemented, then `.Count()` will default to the standard .NET logic.

### Use case 2

We can see that use case 2 is also solved by mixins.
Take, for example, a developer who wants to provide logic for `.ToArray()` that leverages knowledge of the underlying count of the collection.
The .NET implementation already does this for cases where the count is known precisely, but it does not account for cases where the count is a range of values; in these cases, .NET will use a standard geometrically-increasing array allocation algorithm.

To make the example more concrete, say that the developer keeps implements `IV2Enumerable<T>` that has the "original" sequence, the lower limit of the count, and the upper limit of the count.
If there are two instances of this new enumerable, one that is an array of size 17 and the other that is an array of size 36 that has had `.Where` called on it, and these two instances have been concatenated, then the "range" of counts for the concatenated enumerable is a minimum of the first array size and a maximum of the the combined array sizes.
If `.ToArray()` is called on this concatenated enumerable, the developer could have logic to start with a buffer of size 17 and increase geometically from there, resulting in subsequent buffers of size 34 and 53.
The .NET implementation, which discards the context of the minimum size, would create a buffer of size 16, then 32, then 64.
Note that .NET will overallocate on the last buffer in this case.

## Monad

The desired use cases are covered by mixins alone, but there subcases of use case 2 where the general-form algorithm can be applied recursively, or where context from use case 1 is lost after an extension method is called.
Let's look at an example: the count of two concatenated enumerables is the sum of the individual enumerables' counts.

To implement this is simple enough: we will have a class (let's call it `Foo`) that implements `IConcatableMixin<T>`, and the `Concat()` implementation will return an intance of a class that implements `ICountableMixin<T>`; the `ICountableMixin<T>` class will implement `Count()` to return the sum of `first.Count() + second.Count()`.
Note how this logic will also leverage any optimizations of the `.Count()` logic provided by the specific `first` and `second` implementations.

We can notice two problems with `Foo`:
1. `new Foo(new[] { 1, 2, 3, 4 }).Count()` will actually enumerate the array because the call to `.Count()` sees **`Foo`** and not the array, and because `Foo` doesn't implement `ICountableMixin<T>`, the default logic will be used
2. `new Foo(new[] { 1, 2, 3, 4 }).Concat(new[] { 5, 6, 7, 8 }).Concat(new[] { 9, 10, 11, 12 }).Count()` will also enumerate all 3 arrays because the first `.Concat()` call returns a `ICountableMixin<T>` and not a `IConcatableMixin<T>`, so the second call to `.Concat()` uses the default logic and returns the LINQ compiler-generated iterator. This iterator obviously does not implement `ICountableMixin<T>` and so the call to `.Count()` will use the default logic as well, enumerating all of the elements.

The `IEnumerableMonad<T>` provides the infrastructure that allows the V2 extension methods to solve both of these problems.
If `Foo` implements `IEnumerableMonad<T>`, then the V2 extensions methods will behave slightly differently:
1. In this case, the `.Count()` extension method will notice that a monad is being provided and it will delegate to the underlying sequence. So, given that `Foo` does not implement `ICountableMixin<T>`, the extension method will check if the array provides optimized `Count()` logic. It doesn't so the optimization is used.
2. Here, the `.Concat()` extension method will use the monad's `Unit` to wrap the concatenated return value again in a `Foo`. Doing this means that the second call to `.Concat()` will **also** use the `Foo` logic for `Concat()`. Therefore, when `.Count()` is called, the optimized `Count()` logic will be used to add the counts of the first enumerable and the second enumerable; since the first enumerable implements `ICountableMixin<T>`, it's logic is used to recursively add the count of the first array with the count of the second array, and then popping off of the stack, this sum will be added to the count of the third array. As a result, no enumerations are required.

## Use case 3

Given the introduction of the `IEnumerableMonad<T>`, an emergent use case is also solved by V2: developers can implement their own entire LINQ implementations that compose with the existing implementation and with other developers' implementations.
If one developer has a general-form optimization for `.Where()` and implements this with a class called `Foo` that implements `IWhereableMixin<T>` and `IEnumerableMonad<T>`, and another developer has a general-form optimization for `.Select()` and implements this with a class called `Bar` that implements `ISelectableMixin<T>` and `IEnumerableMonad<T>`, then a consumer of both `Foo` and `Bar` can compose them to get the benefits of both optimizations.

## External extensibility

Note that .NET could implement any of the above optimizations.
The purpose of V2 is not to provide these optimizations directly; the purpose is to give third-party developers the ability provide these optimizations for themselves, and to do so in a way that serves their own developer customers.

For example, while the V2 extension methods are comprehensive with the `System.Linq` extension methods at this time, if .NET adds a new extension method in the future, a non-Fx.Core developer could add their own mixin interface and their own corresponding extension method to provide the same functionality that V2 is providing.
As long as the extension method follows the same conventions that Fx.Core follows in V2, then the monad and existing mixins will all compose correctly with the new, external mixin.

## Design decisions

The following design decisions were made while implementing V2:
1. LINQ extension methods that leverage non-`IEnumerable<T>` interfaces (like `ThenBy` and `ThenByDescending`) are not implemented in V2 at this time. These extension methods should be considered carefully in the future to determine if the interfaces they use represent their own monads, or if they are really just syntactic conveniences and truly are extending `IEnumerable<T>`. Regardless, in the meantime, third-party developers are able to provide their own versions of these extension methods that follow the same conventions that V2 establishes.
2. Default interface implementations are used for the mixins. Originally, there was an interface for every extension method overload. However, these interfaces didn't really have meaningful names, and it was never quite clear which interface needed to be implemented (for example, there are many overloads for `.Sum()`, and previously each overload had its own interface; the implementer would need to look through all of the "sum" interfaces and find the one that had the correct method signature in order to determine which interface their class should implement; this is tedious and error-prone). As a result, each set of overloads has a single mixin interface associated with it, and that interface has default interface implementations for all of the overloads. In this way, the developer can implement the interface, but only override the method overload that they are interested in.

The question arose why not use default interface implementations for all of the LINQ extension methods (i.e. have a single interface that has all of the LINQ extension methods and provide default interface implementations for every method). This suggestion was discarded because:
1. It doesn't establish the external-extensibility conventions that V2 currently establishes, making it more difficult for third-party developers to invent their own extension methods and mixins in the future.
2. Existing collections that are being backported to V2 may already have implementations for these methods that don't follow the V2 requirements, and so naming conflicts can result. If the existing implementations truly don't follow the V2 logic requirements, then this can result in subtle bugs that don't clearly indicate if they are the result of V2 issues, collection implementations issues, or a breach of the contract between the two.



















