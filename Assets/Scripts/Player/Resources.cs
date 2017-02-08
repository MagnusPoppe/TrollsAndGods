using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Class that holds resource values for heroes, resource costs for buildings and units.
/// It is used to check if the player can afford actions, and updates his values.
/// </summary>
public class Resources
{
    protected int gold;
    protected int wood;
    protected int ore;
    protected int gem;
    protected int sulfur;
    protected int mercury;
    protected int crystal;

    /// <summary>
    /// Consctructor that sets resource values according to set difficulty
    /// </summary>
    /// <param name="difficulty">easy, normal, hard level of difficulty</param>
    public Resources(int difficulty)
    {
        if (difficulty == 0)
            ResourcesInit(15000, 20, 20, 10, 10, 10, 10);
        else if (difficulty == 1)
            ResourcesInit(10000, 15, 15, 5, 5, 5, 5);
        else
            ResourcesInit(5000, 10, 10, 0, 0, 0, 0);
    }

    public Resources(int gold, int wood, int ore, int gem, int sulfur, int mercury, int crystal)
    {
        this.gold = gold;
        this.wood = wood;
        this.ore = ore;
        this.gem = gem;
        this.sulfur = sulfur;
        this.mercury = mercury;
        this.crystal = crystal;
    }

    protected void ResourcesInit(int gold, int wood, int ore, int gem, int sulfur, int mercury, int crystal)
    {
        this.gold = gold;
        this.wood = wood;
        this.ore = ore;
        this.gem = gem;
        this.sulfur = sulfur;
        this.mercury = mercury;
        this.crystal = crystal;
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
    /// <returns></returns>
    public bool CanPay(int gold, int wood, int ore, int gem, int sulfur, int mercury, int crystal)
    {
        return this.gold >= gold && this.wood >= wood && this.ore >= ore && this.gem >= gem && this.sulfur >= sulfur && this.mercury >= mercury && this.crystal >= crystal;
    }

    public int GetGold()
    {
        return gold;
    }
    public int GetWood()
    {
        return wood;
    }
    public int GetOre()
    {
        return ore;
    }
    public int GetGem()
    {
        return gem;
    }
    public int GetMercury()
    {
        return mercury;
    }
    public int GetSulfur()
    {
        return sulfur;
    }
    public int GetCrystal()
    {
        return crystal;
    }
    public void adjustGold(int i)
    {
        gold += i;
    }
    public void adjustWood(int i)
    {
        wood += i;
    }
    public void adjustOre(int i)
    {
        ore += i;
    }
    public void adjustGem(int i)
    {
        gem += i;
    }
    public void adjustMercury(int i)
    {
        mercury += i;
    }
    public void adjustSulfur(int i)
    {
        sulfur += i;
    }
    public void adjustCrystal(int i)
    {
        crystal += i;
    }
}
