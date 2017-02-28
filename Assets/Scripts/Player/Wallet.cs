using System;

public class Wallet : Resources
{

    public Wallet(int gold, int wood, int ore, int crystal, int gem)
        :base(gold, wood, ore, crystal, gem)
    {
    }

    /// <summary>
    /// Method that checks if parameter values is lower or equal to the objects resource values
    /// </summary>
    /// <param name="gold">gold</param>
    /// <param name="wood">wood</param>
    /// <param name="ore">ore</param>
    /// <param name="crystal">crystal</param>
    /// <param name="gem">gem</param>
    /// <returns>true if there is enough resources</returns>
    public bool CanPay(int gold, int wood, int ore, int crystal, int gem)
    {
        return this.resourceTab[0] >= gold 
            && this.resourceTab[1] >= wood 
            && this.resourceTab[2] >= ore 
            && this.resourceTab[3] >= crystal 
            && this.resourceTab[4] >= gem;
    }

    public bool CanPay(Cost cost)
    {
        return this.resourceTab[(int)type.GOLD]     >= cost.GetResource(type.GOLD)
            && this.resourceTab[(int)type.WOOD]     >= cost.GetResource(type.WOOD)
            && this.resourceTab[(int)type.ORE]      >= cost.GetResource(type.ORE)
            && this.resourceTab[(int)type.CRYSTAL]  >= cost.GetResource(type.CRYSTAL)
            && this.resourceTab[(int)type.GEM]      >= cost.GetResource(type.GEM);
    }

    public void Pay(Cost cost)
    {
        if (CanPay(cost))
        {
            resourceTab[(int)type.GOLD] -= cost.GetResource((int)type.GOLD);
            resourceTab[(int)type.WOOD] -= cost.GetResource((int)type.WOOD);
            resourceTab[(int)type.ORE] -= cost.GetResource((int)type.ORE);
            resourceTab[(int)type.CRYSTAL] -= cost.GetResource((int)type.CRYSTAL);
            resourceTab[(int)type.GEM] -= cost.GetResource((int)type.GEM);
        }
        else
        {
            return null;
        }
    }
}

