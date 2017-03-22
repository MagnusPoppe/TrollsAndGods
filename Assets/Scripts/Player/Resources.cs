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

    public string GetResourceName(int i)
    {
        string name = "";
        if (i == 0)
            name = "Gold";
        else if (i == 1)
            name = "Wood";
        else if (i == 2)
            name = "Ore";
        else if (i == 3)
            name = "Crystal";
        else if (i == 4)
            name = "Gem";
        return name;
    }

    public Resources()
    {
        resourceTab = new int[5];
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

    public int GetResource(int i)
    {
        return resourceTab[i];
    }
    public void adjustResource(type i, int amount)
    {
        resourceTab[(int)i] += amount;
    }

    public override string ToString()
    {
        return "{ " +
                   "Gold : " + GetResource(type.GOLD) + ", " +
                   "Wood : " + GetResource(type.WOOD) + ", " +
                   "ORE : " + GetResource(type.ORE) + ", " +
                   "GEM : " + GetResource(type.GEM) + ", " +
                   "CRYSTAL : " + GetResource(type.CRYSTAL) +
               " }";
    }

}
