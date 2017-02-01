using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Class for all the buildings in the town
/// </summary>
public class BuildingTree
{
    protected Building[] buildings;
    protected int[] built;
    protected const int TOWNSIZE = 15;
    /// <summary>
    /// Constructor that fills the buildings array with test values
    /// </summary>
    /// <param name="townId">Which type of town</param>
    public BuildingTree(int townId)
    {
        buildings = new Building[TOWNSIZE];
        for(int i=0; i<TOWNSIZE; i++)
        {
            buildings[i] = new Building("newname" + i, new Sprite(), new Resources(500 * i, i, i, 0, 0, 0, 0));
        }
    }

    public Building[] GetBuildings()
    {
        return buildings;
    }
}
