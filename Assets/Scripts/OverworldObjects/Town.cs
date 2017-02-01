using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Town
{
    protected BuildingTree buildingTree;
    protected bool canPurchase;

    public Town(int townId)
    {
        buildingTree = new BuildingTree(townId);
        CanPurchase = true;
    }

    public bool CanPurchase
    {
        get
        {
            return canPurchase;
        }

        set
        {
            canPurchase = value;
        }
    }
}
