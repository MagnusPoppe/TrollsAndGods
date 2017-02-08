using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HandyMethods {

    readonly static float HEIGHTOFISOMETRICTILE = 1;

	static public Vector2 getIsoTilePos(Vector2 pos)
    {
        float x = (int)pos.x;
        float y = (int)(pos.y/HEIGHTOFISOMETRICTILE)*HEIGHTOFISOMETRICTILE;

        Vector2 a = new Vector2(0+x, HEIGHTOFISOMETRICTILE*0.5f+y);
        Vector2 b = new Vector2(0.5f+x, HEIGHTOFISOMETRICTILE +y);
        Vector2 c = new Vector2(1+x, HEIGHTOFISOMETRICTILE * 0.5f +y);
        Vector2 d = new Vector2(0.5f+x, 0+y);

        if (determineSideOfLine(a, b, pos) < 0) return new Vector2(x, (y+ HEIGHTOFISOMETRICTILE)*2);
        else if (determineSideOfLine(b, c, pos) < 0) return new Vector2(x+1, (y+ HEIGHTOFISOMETRICTILE)*2);
        else if (determineSideOfLine(c, d, pos) < 0) return new Vector2(x+1, y*2);
        else if (determineSideOfLine(d, a, pos) < 0) return new Vector2(x, y*2);
        else return new Vector2(x,(y+ HEIGHTOFISOMETRICTILE * 0.5f)*2);
    }

    static public float determineSideOfLine(Vector2 start, Vector2 end, Vector2 point)
    {
        return (point.x-start.x)*(end.y-start.y)-(point.y-start.y)*(end.x-start.x);
    }
}
