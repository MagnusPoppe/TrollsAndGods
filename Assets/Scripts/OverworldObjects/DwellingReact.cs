using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DwellingReact : Reaction {

    Buildings.Dwelling dwelling;

    public DwellingReact(Buildings.Dwelling dwelling, Vector2 pos)
    {
        Dwelling = dwelling;
        Pos = pos;
    }

    public Buildings.Dwelling Dwelling
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
        // TODO objektreferanse
        /*
        if (dwelling.Owner != h.Player)
        {
            if (dwelling.Owner != null) dwelling.Owner.DwellingsOwned.Remove(dwelling);
            dwelling.Owner = h.Player;
            h.Player.DwellingsOwned.Add(dwelling);
        }
        */
        // todo hire units
        return true;
    }
}
