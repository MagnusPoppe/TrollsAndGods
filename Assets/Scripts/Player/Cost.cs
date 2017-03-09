using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
        return new Cost(ResourceTab[(int) type.GOLD] * amount, ResourceTab[(int)type.WOOD] * amount, ResourceTab[(int)type.ORE] * amount,
            ResourceTab[(int)type.CRYSTAL] * amount, ResourceTab[(int)type.GEM] * amount);
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
        int goldPrUnit = ResourceTab[(int)type.GOLD];

        int woodOwned = wallet.GetResource(type.WOOD);
        int woodCost = 0;
        int woodPrUnit = ResourceTab[(int)type.WOOD];

        int oreOwned = wallet.GetResource(type.ORE);
        int oreCost = 0;
        int orePrUnit = ResourceTab[(int)type.ORE];

        int crystalOwned = wallet.GetResource(type.CRYSTAL);
        int crystalCost = 0;
        int crystalPrUnit = ResourceTab[(int)type.CRYSTAL];

        int gemOwned = wallet.GetResource(type.GEM);
        int gemCost = 0;
        int gemPrUnit = ResourceTab[(int)type.GEM];

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

    public int[] GetResourceTab()
    {
        return resourceTab;
    }

    public string CostToString(int i)
    {
        return GetResourceTab()[i] + "";
    }

    public string ResourceToString(int i)
    {
        String text = " ";
        if ((int)type.GOLD == i)
            text += "Gold";
        else if ((int)type.WOOD == i)
            text += "Wood";
        else if ((int)type.ORE == i)
            text += "Ore";
        else if ((int)type.CRYSTAL == i)
            text += "Crystal";
        else if ((int)type.GEM == i)
            text += "Gem";
        return text;
    }

    public string ToString(int i)
    {
        return CostToString(i) + ResourceToString(i);
    }
}
