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
    /// Method that checks if parameter values is lower or equal to the objects resource values
    /// </summary>
    /// <param name="gold">gold</param>
    /// <param name="wood">wood</param>
    /// <param name="ore">ore</param>
    /// <param name="gem">gem</param>
    /// <param name="sulfur">sulfur</param>
    /// <param name="mercury">mercury</param>
    /// <param name="crystal">crystal</param>
    /// <returns>true if there is enough resources</returns>
    public bool CanPay(int gold, int wood, int ore, int gem, int sulfur, int mercury, int crystal)
    {
        return this.resourceTab[0] >= gold && this.resourceTab[1] >= wood && this.resourceTab[2] >= ore && this.resourceTab[3] >= gem && this.resourceTab[4] >= sulfur && this.resourceTab[5] >= mercury && this.resourceTab[6] >= crystal;
    }

    public int GetResource(int i)
    {
        return resourceTab[i];
    }
    public void adjustResource(int i, int amount)
    {
        resourceTab[i] += amount;
    }
}
