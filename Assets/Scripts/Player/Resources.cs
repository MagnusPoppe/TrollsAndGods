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

    public enum type { GOLD, WOOD, ORE, CRYSTAL, GEM };

    /// <summary>
    /// Consctructor that sets resource values according to set difficulty
    /// </summary>
    /// <param name="difficulty">easy, normal, hard level of difficulty</param>
    public Resources(int difficulty)
    {
        if (difficulty == 0)
            resourceTab = new int[] { 15000, 20, 20, 10, 10, 10, 10 };
        else if (difficulty == 1)
            resourceTab = new int[] { 10000, 15, 15, 5, 5, 5, 5 };
        else
            resourceTab = new int[] { 5000, 10, 10, 0, 0, 0, 0 };
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
        resourceTab = new int[] { goldCost, woodCost, oreCost, crystalCost, gemCost };
    }

    public int GetResource(type i)
    {
        return resourceTab[(int)i];
    }
    public void adjustResource(type i, int amount)
    {
        resourceTab[(int)i] += amount;
    }
}
