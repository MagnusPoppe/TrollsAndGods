using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// parent class for Reactions, wich governs what happens when objects are triggered
/// </summary>
abstract public class Reaction {

    Vector2 pos;

    public Vector2 Pos
    {
        get
        {
            return pos;
        }

        set
        {
            pos = value;
        }
    }

    abstract public bool React(Hero h);
}
