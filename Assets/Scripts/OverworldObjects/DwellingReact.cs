using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DwellingReact : Reaction {

    Dwelling dwelling;

    public DwellingReact(Dwelling dwelling, Vector2 pos, GameObject self, Reaction[,] reactionTab)
    {
        Dwelling = dwelling;
        Pos = pos;
        Self = self;
        ReactionTab = reactionTab;
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
        throw new System.Exception();
    }
}
