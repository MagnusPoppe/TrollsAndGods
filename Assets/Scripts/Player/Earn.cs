/// <summary>
/// Handles the weekly and daily earnings of a player
/// </summary>
public class Earn : Resources
{

    /// <summary>
    /// This takes a set of resources and creates the earnings. 
    /// if you want a resource not to be earned, use the value "0".
    /// Initializes a new instance of the <see cref="Earn"/> class.
    /// </summary>
    /// <param name="goldEarnings">Gold earnings.</param>
    /// <param name="woodEarnings">Wood earnings.</param>
    /// <param name="oreEarnings">Ore earnings.</param>
    /// <param name="crystalEarnings">Crystal earnings.</param>
    /// <param name="gemEarnings">Gem earnings.</param>
    public Earn(int goldEarnings, int woodEarnings, int oreEarnings, int crystalEarnings, int gemEarnings) 
        :base(goldEarnings, woodEarnings, oreEarnings, crystalEarnings, gemEarnings)
    { 
    }

    /// <summary>
    /// Adjusts the resources according to this Earn.
    /// </summary>
    /// <returns>The wallet, complete with earnings.</returns>
    /// <param name="wallet">A given heros wallet.</param>
    public Wallet adjustResources(Wallet wallet)
    {
        wallet.adjustResource(type.GOLD,    GetResource((int)type.GOLD));
        wallet.adjustResource(type.WOOD,    GetResource((int)type.WOOD));
        wallet.adjustResource(type.ORE,     GetResource((int)type.ORE));
        wallet.adjustResource(type.GEM,     GetResource((int)type.GEM));
        wallet.adjustResource(type.CRYSTAL, GetResource((int)type.CRYSTAL));
        return wallet;
    }

    public override string ToString()
    {
        string text = "";
        for (int i = 0; i < TYPES; i++)
        {
            if (GetResource(i) != 0)
                text += GetResource(i) + " " + GetResourceName(i);
        }
        return text;
    }
}