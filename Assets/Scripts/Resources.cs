using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Resources : MonoBehaviour
{
    protected int gold;
    protected int wood;
    protected int ore;
    protected int gem;
    protected int sulfur;
    protected int mercury;
    protected int crystal;

    public Resources(int difficulty)
    {
        if (difficulty == 0)
            ResourcesInit(15000, 20, 20, 10, 10, 10, 10);
        else if (difficulty == 1)
            ResourcesInit(10000, 15, 15, 5, 5, 5, 5);
        else
            ResourcesInit(5000, 10, 10, 0, 0, 0, 0);
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
