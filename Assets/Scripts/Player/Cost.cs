/// <summary>
/// class for handlig how much a unit or a building costs.
/// </summary>
public class Cost : Resources {

    public Cost(int goldCost, int woodCost, int oreCost, int crystalCost, int gemCost) 
        :base(goldCost, woodCost, oreCost, crystalCost, gemCost){ }

    /// <summary>
    /// Returns the combined cost for an amount of units
    /// </summary>
    /// <param name="amount">The amount you want</param>
    /// <returns>Cost*amount</returns>
    public Cost BuyAmount(int amount)
    {
        return new Cost(resourceTab[(int) type.GOLD] * amount, resourceTab[(int)type.WOOD] * amount, resourceTab[(int)type.ORE] * amount,
            resourceTab[(int)type.CRYSTAL] * amount, resourceTab[(int)type.GEM] * amount);
    }

    /// <summary>
    /// This return the max amount you can buy based on your wallet.
    /// </summary>
    /// <param name="wallet">The wallet your paying with</param>
    /// <returns>The amount you can buy</returns>
    public int MaxAmountYouCanBuy(Wallet wallet)
    {
        int goldOwned = wallet.GetResource(type.GOLD);
        int goldCost = 0;
        int goldPrUnit = resourceTab[(int)type.GOLD];

        int woodOwned = wallet.GetResource(type.WOOD);
        int woodCost = 0;
        int woodPrUnit = resourceTab[(int)type.WOOD];

        int oreOwned = wallet.GetResource(type.ORE);
        int oreCost = 0;
        int orePrUnit = resourceTab[(int)type.ORE];

        int crystalOwned = wallet.GetResource(type.CRYSTAL);
        int crystalCost = 0;
        int crystalPrUnit = resourceTab[(int)type.CRYSTAL];

        int gemOwned = wallet.GetResource(type.GEM);
        int gemCost = 0;
        int gemPrUnit = resourceTab[(int)type.GEM];

        int amount = 0;
        bool canPay = true;

        while (canPay)
        {
            goldCost += goldPrUnit;
            woodCost += woodPrUnit;
            oreCost += orePrUnit;
            crystalCost += crystalPrUnit;
            gemCost += gemPrUnit;

            if (goldCost < goldOwned && woodCost < woodOwned && oreCost < oreOwned 
                && crystalCost < crystalOwned && gemCost < gemOwned)
            {
                amount++;
            }
            else
            {
                canPay = false;
            }
        }

        return amount;
    }
}
