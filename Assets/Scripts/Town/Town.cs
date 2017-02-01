using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Town class for the players towns. Creates the tree of builings, and holds 
/// the purchase variable that is flipped every time you buy something, and for every new day.
/// </summary>
public class Town
{
    protected bool hasBuiltThisRound;
    protected Building[] buildings;
    private Hero stationedHero;
    private Hero visitingHero;
    protected UnitTree unitTree;

    /// <summary>
    /// Constructor that builds the buildingtree with corresponding town according to townId variable
    /// </summary>
    /// <param name="townId">which town shall be built</param>
    public Town(int townId, Building[] buildings)
    {
        this.buildings = buildings;
        unitTree = new UnitTree();
    }
    public bool canBuild(Building b)
    {
        for (int i = 0; i < buildings.Length; i++)
        {
            if (b.GetRequirements()[i] && !buildings[i].IsBuilt())
                return false;
        }
        return true;
    }

    public void Build(Building b)
    {
        b.SetBuilt(true);
        hasBuiltThisRound = true;
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

    protected Hero StationedHero
    {
        get
        {
            return stationedHero;
        }

        set
        {
            stationedHero = value;
        }
    }

    protected Hero VisitingHero
    {
        get
        {
            return visitingHero;
        }

        set
        {
            visitingHero = value;
        }
    }
}
