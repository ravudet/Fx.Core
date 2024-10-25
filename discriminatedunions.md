Modeling disciminated unions in C# generally causes problems. This is often because adding new members to the union is a breaking change, but in C# looks like an additive change (and therefore not a break). It most often is manifested as the result of previously unreachable code branches. Take the following example:

```
public enum ChessPiece
{
  King = 0,
  Queen = 1,
  Knight = 2,
  Rook = 3,
  Bishop = 4,
  Pawn = 5,
}

public static char PieceToChar(ChessPiece piece)
{
  switch (piece)
  {
    case ChessPiece.King:
      return 'K';
    case ChessPiece.Queen:
      return 'Q';
    case ChessPiece.Knight:
      return 'N';
    case ChessPiece.Rook:
      return 'R';
    case ChessPiece.Bishop:
      return 'B';
    case ChessPiece.Pawn:
      return 'P';
    default:
      throw new Exception("UNREACHABLE CODE");
  }
}

public static string PieceToImageFile(ChessPiece piece)
{
  switch (piece)
  {
    case ChessPiece.King:
      return @"c:\king.png";
    case ChessPiece.Queen:      
      return @"c:\queen.png";
    case ChessPiece.Knight:
      return @"c:\knight.png";
    case ChessPiece.Rook:
      return @"c:\rook.png";
    case ChessPiece.Bishop:
      return @"c:\bishop.png";
    case ChessPiece.Pawn:
      return @"c:\pawn.png";
    default:
      throw new Exception("UNREACHABLE CODE");
  }
}
```

In both of these methods, there is an unreachable line of code. This "isn't a problem" since we control `ChessPiece` and both methods. However, it becomes a maintenance concern when we ship this type to customers and if we ever add new chess pieces. Let's add a new chess piece call `Foo`:

```diff
public enum ChessPiece
{
  King = 0,
  Queen = 1,
  Knight = 2,
  Rook = 3,
  Bishop = 4,
  Pawn = 5,
+ Foo = 6,
}
```

Now, we have to go find all of the places `ChessPiece` is referenced and add a branch for `Foo` where applicable. We can solve this problem by using inheritance:

```
public abstract class ChessPiece
{
  public abstract char CharacterRepresentation();

  public abstract string ImageFile();
}

public sealed class King : ChessPiece
{
  public sealed override char CharacterRepresentation()
  {
    return 'K';
  }

  public sealed override string ImageFile()
  {
    return @"c:\king.png";
  }
}

public sealed class Queen : ChessPiece
{
  public sealed override char CharacterRepresentation()
  {
    return 'Q';
  }

  public sealed override string ImageFile()
  {
    return @"c:\queen.png";
  }
}

public sealed class Knight : ChessPiece
{
  public sealed override char CharacterRepresentation()
  {
    return 'N';
  }

  public sealed override string ImageFile()
  {
    return @"c:\knight.png";
  }
}

public sealed class Rook : ChessPiece
{
  public sealed override char CharacterRepresentation()
  {
    return 'R';
  }

  public sealed override string ImageFile()
  {
    return "c:\rook.png";
  }
}

public sealed class Bishop : ChessPiece
{
  public sealed override char CharacterRepresentation()
  {
    return 'B';
  }

  public sealed override string ImageFile()
  {
    return @"c:\bishop.png";
  }
}

public sealed class Pawn : ChessPiece
{
  public sealed override char CharacterRepresentation()
  {
    return 'P';
  }

  public sealed override string ImageFile()
  {
    return @"c:\pawn.png";
  }
}
```

Now, to add `Foo`, we just add a new class:

```diff
+public sealed class Foo : ChessPiece
+{
+ public sealed override char CharacterRepresentation()
+ {
+   return 'F';
+ }
+
+ public sealed override string ImageFile()
+ {
+   return @"c:\foo.png";
+ }
+}
```

Any caller that was previously invoking `PieceToChar(piece)` is now invoking `piece.CharacterRepresentation()` and does not need to be updated with the addition of `Foo`. However, this only works because we have complete control of the functionality that chess pieces are providing on a per-piece basis. Let's say that we ship this to a customer and they decide that they would like to compute the score of a game of chess. To follow the existing pattern, they need to make a feature request and we can add a new abstract method:

```diff
public abstract class ChessPiece
{
  ...
+ public abstract int Score();
}

public sealed class King : ChessPiece
{
  ...
+ public sealed override int Score()
+ {
+   return int.MaxValue;
+ }
}

public sealed class Queen : ChessPiece
{
  ...
+ public sealed override int Score()
+ {
+   return 9;
+ }
}

public sealed class Knight : ChessPiece
{
  ...
+ public sealed override int Score()
+ {
+   return 3;
+ }
}

public sealed class Rook : ChessPiece
{
  ...
+ public sealed override int Score()
+ {
+   return 5;
+ }
}

public sealed class Bishop : ChessPiece
{
  ...
+ public sealed override int Score()
+ {
+   return 3;
+ }
}

public sealed class Pawn : ChessPiece
{
  ...
+ public sealed override int Score()
+ {
+   return 1;
+ }
}

public sealed class Foo : ChessPiece
{
  ...
+ public sealed override int Score()
+ {
+   return 10;
+ }
}
```

If the customer does not want to wait, or if another customer wants to use a different scoring mechanism, they end up needing to write their own `switch` statement just like the ones we started with:

```
public static int SpecialScore(ChessPiece piece)
{
  switch (piece)
  {
    case King:
      return 0;
    case Queen:
      return 1;
    case Knight:
      return 4;
    case Rook:
      return 5;
    case Bishop:
      return 8;
    case Pawn:
      return 9;
    case Foo:
      return 15;
    default:
      throw new Exception("UNREACHABLE CODE");
  }
}
```

If we add yet another type of piece, say `Bar`, this **looks** like an additive change and therefore not a breaking change, but in actuality, `ChessPiece` **semantically** represents a discriminated union, and new members of the union should be treated like a breaking change. This is illustrated in the above case: if we add `Bar`, then we have a customer who now is throwing an exception which declares that the line of code that throws the exception as unreachable. 

### Solution

We can actually model the union and solve this problem through the use of nested types and the visitor pattern. Let's take our first bit of code and design it a little differently:
```
public abstract class ChessPiece
{
  private ChessPiece()
  {
  }

  protected abstract TResult Accept<TResult, TContext>(Visitor<TResult, TContext> visitor, TContext context);

  public abstract class Visitor<TResult, TContext>
  {
    public TResult Dispatch(ChessPiece node, TContext context)
    {
      return node.Accept(this, context);
    }

    public abstract TResult Visit(King node, TContext context);

    public abstract TResult Visit(Queen node, TContext context);

    public abstract TResult Visit(Knight node, TContext context);

    public abstract TResult Visit(Rook node, TContext context);

    public abstract TResult Visit(Bishop node, TContext context);

    public abstract TResult Visit(Pawn node, TContext context);
  }

  public sealed class King : ChessPiece
  {
    protected sealed override TResult Accept<TResult, TContext>(Visitor<TResult, TContext> visitor, TContext context)
    {
      return visitor.Visit(this, context);
    }
  }

  public sealed class Queen : ChessPiece
  {
    protected sealed override TResult Accept<TResult, TContext>(Visitor<TResult, TContext> visitor, TContext context)
    {
      return visitor.Visit(this, context);
    }
  }

  public sealed class Knight : ChessPiece
  {
    protected sealed override TResult Accept<TResult, TContext>(Visitor<TResult, TContext> visitor, TContext context)
    {
      return visitor.Visit(this, context);
    }
  }

  public sealed class Rook : ChessPiece
  {
    protected sealed override TResult Accept<TResult, TContext>(Visitor<TResult, TContext> visitor, TContext context)
    {
      return visitor.Visit(this, context);
    }
  }

  public sealed class Bishop : ChessPiece
  {
    protected sealed override TResult Accept<TResult, TContext>(Visitor<TResult, TContext> visitor, TContext context)
    {
      return visitor.Visit(this, context);
    }
  }

  public sealed class Pawn : ChessPiece
  {
    protected sealed override TResult Accept<TResult, TContext>(Visitor<TResult, TContext> visitor, TContext context)
    {
      return visitor.Visit(this, context);
    }
  }
}
```

Let's note a few things about this code. First is the `private` constructor in `ChessPiece`. This ensures that only *nested* types are able to derive from `ChessPiece`, and since nested types cannot be externally added, we have guaranteed the exact set of types that derive from `ChessPiece`. Also observe that the `abstract` method `Accept` in `ChessPiece` combined with the `Visitor` class is able to accomplish what we were doing before with inheritance, but in an externally-extensible way. We can now implement `ToChar` and `ToImageFile` methods without needing to add explicit methods to the `ChessPiece` base type:

```
public struct Void
{
}

public sealed class ToCharVisitor : ChessPiece.Visitor<char, Void>
{
  public sealed override char Visit(ChessPiece.King node, Void context)
  {
    return 'K';
  }

  public sealed override char Visit(ChessPiece.Queen node, Void context)
  {
    return 'Q';
  }

  public sealed override char Visit(ChessPiece.Knight node, Void context)
  {
    return 'N';
  }

  public sealed override char Visit(ChessPiece.Rook node, Void context)
  {
    return 'R';
  }

  public sealed override char Visit(ChessPiece.Bishop node, Void context)
  {
    return 'B';
  }

  public sealed override char Visit(ChessPiece.Pawn node, Void context)
  {
    return 'P';
  }
}

public sealed class ToImageFileVisitor : ChessPiece.Visitor<string, Void>
{
  public sealed override string Visit(ChessPiece.King node, Void context)
  {
    return @"c:\king.png";
  }

  public sealed override string Visit(ChessPiece.Queen node, Void context)
  {
    return @"c:\queen.png";
  }

  public sealed override string Visit(ChessPiece.Knight node, Void context)
  {
    return @"c:\knight.png";
  }

  public sealed override string Visit(ChessPiece.Rook node, Void context)
  {
    return @"c:\rook.png";
  }

  public sealed override string Visit(ChessPiece.Bishop node, Void context)
  {
    return @"c:\bishop.png";
  }

  public sealed override string Visit(ChessPiece.Pawn node, Void context)
  {
    return @"c:\pawn.png";
  }
}
```

Our customer can also implement their special score using a visitor as well:

```
public sealed class ToSpecialScoreVisitor : ChessPiece.Visitor<int, Void>
{
  public sealed override int Visit(ChessPiece.King node, Void context)
  {
    return 0;
  }

  public sealed override int Visit(ChessPiece.Queen node, Void context)
  {
    return 1;
  }

  public sealed override int Visit(ChessPiece.Knight node, Void context)
  {
    return 4;
  }

  public sealed override int Visit(ChessPiece.Rook node, Void context)
  {
    return 5;
  }

  public sealed override int Visit(ChessPiece.Bishop node, Void context)
  {
    return 8;
  }

  public sealed override int Visit(ChessPiece.Pawn node, Void context)
  {
    return 9;
  }
}
```

Now, very importantly, let's try to add `Foo` to the set of chess pieces:

```diff
public abstract class ChessPiece
{
  ...
+ public sealed class Foo : ChessPiece
+ {
+ }
}
```

This doesn't compile because we haven't implemented the `abstract` method `Accept` yet. The implemenation for that method is always the same, so let's copy it from one of the other pieces:

```diff
public abstract class ChessPiece
{
  ...
  public sealed class Foo : ChessPiece
  {
+   protected sealed override TResult Accept<TResult, TContext>(Visitor<TResult, TContext> visitor, TContext context)
+   {
+     return visitor.Visit(this, context);
+   }
  }
}
```

Now this doesn't compile because there's no corresponding `Visit` method defined on `Visitor` for the new `Foo` type, so let's add that method:

```diff
public abstract class ChessPiece
{
  ...
  public abstract class Visitor<TResult, TContext>
  {
    ...
+   public abstract TResult Visit(Foo node, TContext context);
  }
  ...
  public sealed class Foo : ChessPiece
  {
    protected sealed override TResult Accept<TResult, TContext>(Visitor<TResult, TContext> visitor, TContext context)
    {
      return visitor.Visit(this, context);
    }
  }
}
```

Now none of our visitor implementations compile. This is **by design** because it now surfaces the fact that we have introduced a breaking change. Adding a member to a discriminated union, as previously discussed, is a breaking change, and this model actually surfaces that by requiring a new `abtract` method on our visitor, and adding a new `abstract` method is itself a breaking change. 

By modeling our discriminated union with an `abstract` class that has a `private` constructor and by modeling our union members as nested derived types of that `abstract` class and by exposing external extensibility of this type hierarchy through the use of the visitor pattern (instead of inheritance), we are able to ship code that makes clear through compiler errors whenever breaking changes are introduced without reducing the flexibility of the union nor its members. 























