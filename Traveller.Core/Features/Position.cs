﻿namespace Traveller.Core.Features;
public record struct Position
{
    /// <summary>Trailing / Right</summary>
    public int X { get; init; }
    /// <summary>Rimwards / Down</summary>
    public int Y { get; init; }

    public Position(int x, int y)
    {
        X = x;
        Y = y;
    }

    public Position((int, int) xy)
    {
        (X, Y) = xy;
    }

    public Position(string hex)
    {
        X = int.Parse(hex.Substring(0, 2));
        Y = int.Parse(hex.Substring(2, 2));
    }

    // Should either always convert to sector, or handle padding better.
    public override string ToString()
    {
        return X.ToString().PadLeft(2, '0') + Y.ToString().PadLeft(2, '0');
    }

    public Position ToSector() => new(
            X % Constants.Dimensions.SectorWidth,
            Y % Constants.Dimensions.SectorHeight);

    public Position ToQuadrant() => new(
            X % Constants.Dimensions.QuadrantWidth,
            Y % Constants.Dimensions.QuadrantHeight);
    
    public Position ToSubSector() => new(
            X % Constants.Dimensions.SubSectorWidth,
            Y % Constants.Dimensions.SubSectorHeight);

    // Todo: There's probably a general solution that would accept a range parameter.
    public bool IsNeighbour(Position position)
    {
        // If X is offset by >1 then it can't be a neighbour.
        if (Math.Abs(position.X - X) > 1) return false;

        /* Since this is a Hex grid one of the axes behaves slightly differently,
         * in the case of traveller then it's the Y axis.
         * 
         * This means that:
         * 0504 borders 0603 and 0604, while
         * 0604 borders 0504 and 0505
         * 
         * Thus if X is even it borders the tiles at: Y and Y-1, (Minus)
         * and  if X is odd, it borders the tiles at: Y and Y+1. (Plus)
         */
        if (position.Y == Y) return true;
        if (position.X % 2 == 0) // Is X even?
        {
            if (position.Y == (Y - 1)) return true;
        }
        else
        {
            if (position.Y == (Y + 1)) return true;
        }

        return false;
    }

    public bool IsInSector(Position sector) => 
        (Math.Abs(sector.X - X) < Constants.Dimensions.SectorWidth ) &&
        (Math.Abs(sector.Y - Y) < Constants.Dimensions.SectorHeight);

    public bool IsInQuadrant(Position quadrant) =>
        (Math.Abs(quadrant.X - X) < Constants.Dimensions.QuadrantWidth) &&
        (Math.Abs(quadrant.Y - Y) < Constants.Dimensions.QuadrantHeight);

    public bool IsInSubSector(Position subSector) =>
        (Math.Abs(subSector.X - X) < Constants.Dimensions.SubSectorWidth) &&
        (Math.Abs(subSector.Y - Y) < Constants.Dimensions.SubSectorHeight);

    public static Position operator +(Position a, Position B) => new(a.X + B.X, a.Y + B.Y);
    public bool Equals(Position other) => X == other.X &&  Y == other.Y;

    public static Position CreateSectorFromOffset(int x, int y)
    {
        return new(x * Constants.Dimensions.SectorWidth, y * Constants.Dimensions.SectorHeight);
    }
}

