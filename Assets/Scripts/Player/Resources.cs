using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Class that holds resource values for heroes, resource costs for buildings and units.
/// It is used to check if the player can afford actions, and updates his values.
/// </summary>
public class Resources
{
    protected int[] resourceTab;


    protected int[] ResourceTab
    {
        get
        {
            return resourceTab;
        }

        set
        {
            resourceTab = value;
        }
    }

    public enum type { GOLD, WOOD, ORE, CRYSTAL, GEM };

    public Resources()
    {
        ResourceTab = new int[5];
    }

    /// <summary>
    /// Constructor for buildings or units that costs specific resources
    /// </summary>
    /// <param name="goldCost">gold</param>
    /// <param name="woodCost">wood</param>
    /// <param name="oreCost">ore</param>
    /// <param name="crystalCost">crystal</param>
    /// <param name="gemCost">gem</param>
    public Resources(int goldCost, int woodCost, int oreCost, int crystalCost, int gemCost)
    {
        ResourceTab = new int[] { goldCost, woodCost, oreCost, crystalCost, gemCost };
    }

    public int GetResource(type i)
    {
        return ResourceTab[(int)i];
    }

    public int GetResource(int i)
    {
        return ResourceTab[i];
    }
    public void adjustResource(type i, int amount)
    {
        ResourceTab[(int)i] += amount;
    }


}
