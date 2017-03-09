using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Governs what happens when interacting with a unit on the map
/// </summary>
public class UnitReaction : Reaction {

    UnitTree units;

    public UnitReaction(UnitTree units, Point pos)
    {
        Units = units;
        Pos = pos;
    }

    public UnitTree Units
    {
        get
        {
            return units;
        }

        set
        {
            units = value;
        }
    }



    /// <summary>
    /// unit either flees, joins hero or fights.
    /// </summary>
    /// <param name="h">Hero interacting with unit</param>
    /// <returns>Returns True</returns>
    public override bool React(Hero h)
    {
        // Todo flee, join or fight
        // Returns true if hero remains. False if unit fleed, joined, or was defeated
        return true;
    }
}
