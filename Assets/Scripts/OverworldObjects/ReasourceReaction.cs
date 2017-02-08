using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResourceReaction : Reaction
{

    int resID;
    int amount;

    public ResourceReaction(int resID, int amount, Vector2 pos, GameObject self,Reaction[,] reactionTab)
    {

    }

    public override bool React(Hero h)
    {
        throw new NotImplementedException();
    }
}
