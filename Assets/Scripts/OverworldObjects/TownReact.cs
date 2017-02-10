using System.Collections;
using System.Collections.Generic;
using Town;
using UnityEngine;

public class TownReact : Reaction {

    Town.Town town;

    public TownReact(Town.Town town, Vector2 pos, GameObject self, Reaction[,] reactionTab)
    {
        Town = town;
        Pos = pos;
        Self = self;
        ReactionTab = reactionTab;
    }

    public Town.Town Town
    {
        get
        {
            return town;
        }

        set
        {
            town = value;
        }
    }

    public override bool React(Hero h)
    {
        if (town.Player.Equals(h.Player))
        {
            //Todo visit town
        }
        else
        {
            //Todo battle
        }
        return true;
    }
}
