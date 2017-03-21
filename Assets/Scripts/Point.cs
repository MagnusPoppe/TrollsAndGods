using UnityEngine;

/// <summary>
/// simple class to handle x,y coordinates.
/// </summary>
public class Point
{
    public int x, y;

    public Point(int x, int y)
    {
        this.x = x;
        this.y = y;
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="Point"/> class.
    /// </summary>
    /// <param name="vector2">Vector2.</param>
    public Point(Vector2 vector)
    {
        x = (int)vector.x;
        y = (int)vector.y;
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="Point"/> class.
    /// </summary>
    /// <param name="vector3">Vector3.</param>
    public Point(Vector3 vector)
    {
        x = (int)vector.x;
        y = (int)vector.y;
    }

    public float DistanceTo(Point other)
    {
        return Vector2.Distance(new Vector2(x, y), new Vector2(other.x, other.y));
    }

    ///<returns>The vector2 of this point.</returns>
    public Vector2 ToVector2()
    {
        return new Vector2(x, y);
    }

    ///<returns>The vector3 of this point.</returns>
    public Vector3 ToVector3()
    {
        return new Vector3(x, y, 0);
    }

    public override bool Equals(object obj)
    {
        if (!obj.GetType().Equals(typeof(Point)))
        {
            return false;
        }
        Point p = (Point) obj;
        return (p.x == x && p.y == y);
    }

    public override string ToString()
    {
        return "(" + x + ", " + y + ")";
    }

    /// <summary>
    /// Checks if this pos is in bounds of a map.
    /// </summary>
    /// <param name="map">map</param>
    /// <returns>true if legal postiton.</returns>
    public bool InBounds(int[,] map)
    {
        return (0 <= x && x < map.GetLength(0) && 0 <= y && y < map.GetLength(0));
    }

    /// <summary>
    /// Checks if this position is in bounds of a map given a filter.
    /// </summary>
    /// <param name="map">map</param>
    /// <param name="filter">filter modifier</param>
    /// <returns>true if legal postiton.</returns>
    public bool InBounds(int[,] map, int[,] filter)
    {
        int range = filter.GetLength(1) / 2;
        return (0 <= x-range && x+range < map.GetLength(0) && 0 <= y-range && y+range < map.GetLength(0));
    }
}

