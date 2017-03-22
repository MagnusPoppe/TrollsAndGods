using System;
using UnityEngine;

public class HandyMethods
{
    
    /// <summary>
    /// Gets the logical position of a isometric tile
    /// </summary>
    /// <param name="pos">Graphical position</param>
    /// <returns>Logical position</returns>
    static public Point getIsoTilePos(Vector2 pos)
    {
        float x = (int)pos.x;
        float y = (int)(pos.y * 2) / 2.0f;

        Vector2 a = new Vector2(0 + x, 0.25f + y);
        Vector2 b = new Vector2(0.5f + x, 0.5f + y);
        Vector2 c = new Vector2(1 + x, 0.25f + y);
        Vector2 d = new Vector2(0.5f + x, y);

        Vector2 tempValue;

        if (determineSideOfLine(a, b, pos) < 0) tempValue = new Vector2(x, (y + 0.5f) * 2*2);
        else if (determineSideOfLine(b, c, pos) < 0) tempValue = new Vector2(x + 1, (y + 0.5f) * 2 * 2);
        else if (determineSideOfLine(c, d, pos) < 0) tempValue = new Vector2(x + 1, y * 2 * 2);
        else if (determineSideOfLine(d, a, pos) < 0) tempValue = new Vector2(x, y * 2 * 2);
        else tempValue = new Vector2(x, (y + 0.25f) * 2 * 2);

        return new Point(tempValue);
    }

    /// <summary>
    /// Gets the graphical position of a isometric tile
    /// </summary>
    /// <param name="pos">Logical position</param>
    /// <returns>Graphical position</returns>
    static public Vector2 getGraphicPosForIso(Vector2 pos)
    {
        Vector2 modified;
        if (pos.y % 2 == 0)
        {
            modified = new Vector2(pos.x, pos.y / 2 / 2);
        }
        else
        {
            modified = new Vector2(pos.x + 0.5f, pos.y / 2 / 2);
        }
        return modified;
    }

    static public Vector2 getGraphicPosForIso(Point pos)
    {
        return getGraphicPosForIso(pos.ToVector2());
    }

    

    /// <summary>
    /// Checks if point is on one or the other side of a line.
    /// </summary>
    /// <param name="start">Start of line</param>
    /// <param name="end">End of line</param>
    /// <param name="point">Point</param>
    /// <returns>Negative if on one side, positive if on other side, 0 if on line</returns>
    static public float determineSideOfLine(Vector2 start, Vector2 end, Vector2 point)
    {
        return (point.x - start.x) * (end.y - start.y) - (point.y - start.y) * (end.x - start.x);
    }

    static public int[,] Copy2DArray(int[,] array)
    {
        int[,] output = new int[array.GetLength(0), array.GetLength(1)];
        for (int y = 0; y < array.GetLength(0); y++) {
            for (int x = 0; x < array.GetLength(1); x++) {
                output[x, y] = array[x, y];
            }
        }
        return output;
    }

    /// <summary>
    /// Debug method for 2D arrays.
    /// </summary>
    /// <param name="map"></param>
    static public void print2DArray(int[,] map)
    {
        string msg = "";
        for (int y = 0; y < map.GetLength(1); y++)
        {
            for (int x = 0; x < map.GetLength(0); x++)
            {
                msg += map[x, y] +" ";
            }
            msg += "\n";
        }
        Debug.Log(msg);
    }


    /// <summary>
    /// Returns distance to target in a offset grid, ignoring obstacles
    /// </summary>
    /// <param name="a">Start pos</param>
    /// <param name="b">End pos</param>
    /// <returns>Distance</returns>
    static public int DistanceHex(Point a, Point b)
    {
        Vector3 s = oddROffsetToCube(a);
        Vector3 g = oddROffsetToCube(b);
        return cubeDistance(s, g);
    }

    /// <summary>
    /// Transelates offset cordinates to cube cordinates
    /// </summary>
    /// <param name="pos">Offset cordinates to be transelated</param>
    /// <returns>Cube cordinates</returns>
    static public Vector3 oddROffsetToCube(Point pos)
    {
        int x = (pos.x - ((pos.y - 1 * (pos.y & 1)) / 2));
        int z = pos.y;
        int y = -x - z;
        return new Vector3(x, y, z);
    }

    /// <summary>
    /// Returns distance to target in a cube grid, ignoring obstacles
    /// </summary>
    /// <param name="a">Start pos</param>
    /// <param name="b">End pos</param>
    /// <returns>Distance</returns>
    static public int cubeDistance(Vector3 a, Vector3 b)
    {
        return (int)Math.Max(Math.Abs(a.x - b.x), Math.Max(Math.Abs(a.y - b.y), Math.Abs(a.z - b.z)));
    }
}
