using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitReaction : Reaction {

    Unit unit;
    int amount;

    public UnitReaction(Unit unit, int amount, Vector2 pos)
    {
        Unit = unit;
        Amount = amount;
        Pos = pos;
    }

    public Unit Unit
    {
        get
        {
            return unit;
        }

        set
        {
            unit = value;
        }
    }

    public int Amount
    {
        get
        {
            return amount;
        }

        set
        {
            amount = value;
        }
    }

    public override bool React(Hero h)
    {
        // Todo flee, join or fight
        return true;
    }
}
