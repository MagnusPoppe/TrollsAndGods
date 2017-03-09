using System.Collections;
using System.Collections.Generic;
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
        this.x = (int)vector.x;
        this.y = (int)vector.y;
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="Point"/> class.
    /// </summary>
    /// <param name="vector3">Vector3.</param>
    public Point(Vector3 vector)
    {
        this.x = (int)vector.x;
        this.y = (int)vector.y;
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
}

