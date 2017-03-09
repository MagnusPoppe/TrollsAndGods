public class Wallet : Resources
{

    public Wallet(int gold, int wood, int ore, int crystal, int gem)
        :base(gold, wood, ore, crystal, gem)
    {
    }


    /// <summary>
    /// Consctructor that sets resource values according to set difficulty
    /// </summary>
    /// <param name="difficulty">easy, normal, hard level of difficulty</param>
    public Wallet(int difficulty)
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

    /// <summary>
    /// Method that checks if parameter values is lower or equal to the objects resource values
    /// </summary>
    /// <returns><c>true</c> if this instance can pay the specified cost; otherwise, <c>false</c>.</returns>
    /// <param name="cost">Cost.</param>
    public bool CanPay(Cost cost)
    {
        return this.resourceTab[(int)type.GOLD]     >= cost.GetResource(type.GOLD)
            && this.resourceTab[(int)type.WOOD]     >= cost.GetResource(type.WOOD)
            && this.resourceTab[(int)type.ORE]      >= cost.GetResource(type.ORE)
            && this.resourceTab[(int)type.CRYSTAL]  >= cost.GetResource(type.CRYSTAL)
            && this.resourceTab[(int)type.GEM]      >= cost.GetResource(type.GEM);
    }

    /// <summary>
    /// Pays the specified cost.
    /// </summary>
    /// <param name="cost">Cost.</param>
    public void Pay(Cost cost)
    {
        if (CanPay(cost))
        {
            resourceTab[(int)type.GOLD] -= cost.GetResource(type.GOLD);
            resourceTab[(int)type.WOOD] -= cost.GetResource(type.WOOD);
            resourceTab[(int)type.ORE] -= cost.GetResource(type.ORE);
            resourceTab[(int)type.CRYSTAL] -= cost.GetResource(type.CRYSTAL);
            resourceTab[(int)type.GEM] -= cost.GetResource(type.GEM);
        }
    }
}

