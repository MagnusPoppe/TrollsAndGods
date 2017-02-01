using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Town class for the players towns. Creates the tree of builings, and holds 
/// the purchase variable that is flipped every time you buy something, and for every new day.
/// </summary>
public class Town
{
    protected const int TOWNSIZE = 15;
    protected bool hasBuiltThisRound;
    protected BuildingTree buildingTree;
    protected UnitTree unitTree;

    /// <summary>
    /// Constructor that builds the buildingtree with corresponding town according to townId variable
    /// </summary>
    /// <param name="townId">which town shall be built</param>
    public Town(int townId)
    {
        buildingTree = new BuildingTree(townId);
        unitTree = new UnitTree();
    }

    public bool HasBuiltThisRound
    {
        get
        {
            return hasBuiltThisRound;
        }

        set
        {
            hasBuiltThisRound = value;
        }
    }

    public bool canBuild(Building b)
    {
        for(int i=0; i<TOWNSIZE; i++)
        {
            if (b.GetRequirements()[i] && !buildingTree.GetBuildings()[i].IsBuilt())
                return false;
        }
        return true;
    }
}
