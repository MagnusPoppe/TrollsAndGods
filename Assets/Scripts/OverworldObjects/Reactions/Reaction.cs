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

    /// <summary>
    /// abstract method to handle what happens when you interact with object
    /// </summary>
    /// <param name="h">The hero that is interacting with the object</param>
    /// <returns>returns true if graphic change, false if not</returns>
    abstract public bool React(Hero h);
}
