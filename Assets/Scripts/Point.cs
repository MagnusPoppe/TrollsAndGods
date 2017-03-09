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

    public Point(Vector2 v)
    {
        x = (int)v.x;
        y = (int)v.y;
    }
}

