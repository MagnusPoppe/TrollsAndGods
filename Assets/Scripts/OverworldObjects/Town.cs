using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Town class for the players towns. Creates the tree of builings, and holds 
/// the purchase variable that is flipped every time you buy something, and for every new day.
/// </summary>
public class Town
{
    protected BuildingTree buildingTree;
    protected UnitTree unitTree;
    protected bool canPurchase;

    /// <summary>
    /// Constructor that builds the buildingtree with corresponding town according to townId variable
    /// </summary>
    /// <param name="townId">which town shall be built</param>
    public Town(int townId)
    {
        buildingTree = new BuildingTree(townId);
        unitTree = new UnitTree();
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
