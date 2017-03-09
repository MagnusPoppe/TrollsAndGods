using System.Collections;
using System.Collections.Generic;
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
    /// Gets the logical position of a hex tile
    /// </summary>
    /// <param name="pos">Graphical position</param>
    /// <returns>Logical position</returns>
    static public Vector2 getHexTilePos(Vector2 pos)
    {
        float x = (int)pos.x;
        float y = (int)pos.y;

        Vector2 a = new Vector2(0 + x, 0.66f + y);
        Vector2 b = new Vector2(0.5f + x, 1 + y);
        Vector2 c = new Vector2(1 + x, 0.66f + y);
        Vector2 d = new Vector2(1 + x, 0.33f + y);
        Vector2 e = new Vector2(0.5f + x, y);
        Vector2 f = new Vector2(0 + x, 0.33f + y);

        if (determineSideOfLine(a, b, pos) < 0) return new Vector2(x, (y + 1));
        else if (determineSideOfLine(b, c, pos) < 0) return new Vector2(x + 1, (y + 1));
        else if (determineSideOfLine(d, e, pos) < 0) return new Vector2(x + 1, (y - 1));
        else if (determineSideOfLine(e, f, pos) < 0) return new Vector2(x, (y - 1));
        else return new Vector2(x, y);

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

    /// <summary>
    /// Gets the graphical position of a hex tile
    /// </summary>
    /// <param name="pos">Graphical position</param>
    /// <returns>Logical position</returns>
    static public Vector2 getGraphicPosForHex(Vector2 pos)
    {
        Vector2 modified;
        if (pos.y % 2 == 0)
        {
            modified = new Vector2(pos.x, pos.y);
        }
        else
        {
            modified = new Vector2(pos.x + 0.5f, pos.y);
        }
        return modified;
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
}
