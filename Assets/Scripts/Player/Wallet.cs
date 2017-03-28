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
            ResourceTab = new int[] { 15000, 20, 20, 10, 10, 10, 10 };
        else if (difficulty == 1)
            ResourceTab = new int[] { 10000, 15, 15, 5, 5, 5, 5 };
        else
            ResourceTab = new int[] { 5000, 10, 10, 0, 0, 0, 0 };
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
        return this.ResourceTab[0] >= gold 
            && this.ResourceTab[1] >= wood 
            && this.ResourceTab[2] >= ore 
            && this.ResourceTab[3] >= crystal 
            && this.ResourceTab[4] >= gem;
    }

    /// <summary>
    /// Checks if you can pay an amount of a specific resource index
    /// </summary>
    /// <param name="i">index</param>
    /// <param name="amount">amount to pay</param>
    /// <returns>true if you can pay for it</returns>
    public bool CanPay(int i, int amount)
    {
        return ResourceTab[i] >= amount;
    }

    /// <summary>
    /// Method that checks if parameter values is lower or equal to the objects resource values
    /// </summary>
    /// <returns><c>true</c> if this instance can pay the specified cost; otherwise, <c>false</c>.</returns>
    /// <param name="cost">Cost.</param>
    public bool CanPay(Cost cost)
    {
        return this.ResourceTab[(int)type.GOLD]     >= cost.GetResource(type.GOLD)
            && this.ResourceTab[(int)type.WOOD]     >= cost.GetResource(type.WOOD)
            && this.ResourceTab[(int)type.ORE]      >= cost.GetResource(type.ORE)
            && this.ResourceTab[(int)type.CRYSTAL]  >= cost.GetResource(type.CRYSTAL)
            && this.ResourceTab[(int)type.GEM]      >= cost.GetResource(type.GEM);
    }

    /// <summary>
    /// Checks canpay for multiples
    /// </summary>
    /// <param name="cost"></param>
    /// <param name="amount"></param>
    /// <returns></returns>
    public bool CanPayForMultiple(Cost cost, int amount)
    {
        return this.ResourceTab[(int)type.GOLD] * amount >= cost.GetResource(type.GOLD)
            && this.ResourceTab[(int)type.WOOD] * amount >= cost.GetResource(type.WOOD)
            && this.ResourceTab[(int)type.ORE] * amount >= cost.GetResource(type.ORE)
            && this.ResourceTab[(int)type.CRYSTAL] * amount >= cost.GetResource(type.CRYSTAL)
            && this.ResourceTab[(int)type.GEM] * amount >= cost.GetResource(type.GEM);
    }



    /// <summary>
    /// Pays the specified cost.
    /// </summary>
    /// <param name="cost">Cost.</param>
    public void Pay(Cost cost)
    {
        if (CanPay(cost))
        {
            ResourceTab[(int)type.GOLD] -= cost.GetResource(type.GOLD);
            ResourceTab[(int)type.WOOD] -= cost.GetResource(type.WOOD);
            ResourceTab[(int)type.ORE] -= cost.GetResource(type.ORE);
            ResourceTab[(int)type.CRYSTAL] -= cost.GetResource(type.CRYSTAL);
            ResourceTab[(int)type.GEM] -= cost.GetResource(type.GEM);
        }
    }
}

