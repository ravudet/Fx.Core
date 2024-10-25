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
      return "c:\king.png";
    case ChessPiece.Queen:      
      return "c:\queen.png";
    case ChessPiece.Knight:
      return "c:\knight.png";
    case ChessPiece.Rook:
      return "c:\rook.png";
    case ChessPiece.Bishop:
      return "c:\bishop.png";
    case ChessPiece.Pawn:
      return "c:\pawn.png";
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
    return "c:\king.png";
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
    return "c:\queen.png";
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
    return "c:\knight.png";
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
    return "c:\bishop.png";
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
    return "c:\pawn.png";
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
+   return "c:\foo.png";
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
