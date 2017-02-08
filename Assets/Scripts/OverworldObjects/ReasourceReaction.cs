using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResourceReaction : Reaction
{

    int resID;
    int amount;

    public int ResID
    {
        get
        {
            return resID;
        }

        set
        {
            resID = value;
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

    public ResourceReaction(int resID, int amount, Vector2 pos, GameObject self,Reaction[,] reactionTab)
    {
        ResID = resID;
        Amount = amount;
        Pos = pos;
        Self = self;
        ReactionTab = reactionTab;
    }

    public override bool React(Hero h)
    {
        h.Player.
    }
}
