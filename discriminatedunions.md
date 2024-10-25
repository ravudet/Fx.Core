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

public static string PieceToImageFile()
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
}
```
