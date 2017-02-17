using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResourceReaction : Reaction
{

    Resources.type resourceID;
    int amount;

    public Resources.type ResourceID
    {
        get
        {
            return resourceID;
        }

        set
        {
            resourceID = value;
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

    public ResourceReaction(Resources.type resID, int amount, Vector2 pos)
    {
        ResourceID = resID;
        Amount = amount;
        Pos = pos;
    }

    public override bool React(Hero h)
    {
        h.Player.Resources.adjustResource(resourceID, amount);

        return true;
    }
}
