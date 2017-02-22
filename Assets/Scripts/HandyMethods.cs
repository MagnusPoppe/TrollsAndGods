using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HandyMethods
{
    

    static public Vector2 getIsoTilePos(Vector2 pos)
    {
        float x = (int)pos.x;
        float y = (int)(pos.y * 2) / 2.0f;

        Vector2 a = new Vector2(0 + x, 0.25f + y);
        Vector2 b = new Vector2(0.5f + x, 0.5f + y);
        Vector2 c = new Vector2(1 + x, 0.25f + y);
        Vector2 d = new Vector2(0.5f + x, y);

        if (determineSideOfLine(a, b, pos) < 0) return new Vector2(x, (y + 0.5f) * 2*2);
        else if (determineSideOfLine(b, c, pos) < 0) return new Vector2(x + 1, (y + 0.5f) * 2 * 2);
        else if (determineSideOfLine(c, d, pos) < 0) return new Vector2(x + 1, y * 2 * 2);
        else if (determineSideOfLine(d, a, pos) < 0) return new Vector2(x, y * 2 * 2);
        else return new Vector2(x, (y + 0.25f) * 2 * 2);
    }

    static public Vector2 getGraphicPos(Vector2 pos)
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

    static public float determineSideOfLine(Vector2 start, Vector2 end, Vector2 point)
    {
        return (point.x - start.x) * (end.y - start.y) - (point.y - start.y) * (end.x - start.x);
    }
}
