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

public static string PieceToString()
{
}

public static string PieceToImageFile()
{
}

public static int PieceToScore()
{
}
```
