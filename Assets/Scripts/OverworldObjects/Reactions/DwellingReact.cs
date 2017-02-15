using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using OverworldObjects;

public class DwellingReact : Reaction {

    Dwelling dwelling;

    public DwellingReact(Dwelling dwelling, Vector2 pos)
    {
        Dwelling = dwelling;
        Pos = pos;
    }

    public Dwelling Dwelling
    {
        get
        {
            return dwelling;
        }

        set
        {
            dwelling = value;
        }
    }

    public override bool React(Hero h)
    {
        /*
        if (dwelling.Owner != h.PlayerID)
        {
            if (dwelling.Owner != null) GameManager.get.DwellingsOwned.Remove(dwelling);
            dwelling.Owner = h.PlayerID;
            h.Player.DwellingsOwned.Add(dwelling);
        }
        */
        // todo hire units
        return true;
    }
}
