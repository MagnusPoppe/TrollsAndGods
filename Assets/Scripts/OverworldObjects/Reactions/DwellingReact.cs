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
        
        if (!dwelling.Owner.Equals(h.Player))
        {
            if (dwelling.Owner != null) dwelling.Owner.DwellingsOwned.Remove(dwelling);
            dwelling.Owner = h.Player;
            h.Player.DwellingsOwned.Add(dwelling);
            //todo inital hiring of units
            return true;
        }
        
        // todo hire units
        return false;
    }
}
