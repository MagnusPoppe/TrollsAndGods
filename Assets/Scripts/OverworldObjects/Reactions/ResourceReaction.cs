using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// governs what happens when interacting with a resource pickup
/// </summary>
public class ResourceReaction : Reaction
{

    Resources.type resourceID;
    UnitReaction unitReact;
    int amount;

    public Resources.type ResourceID
    {
        get
        {
            return resourceID;
        }

        set
        {
            resourceID = value;
        }
    }

    public int Amount
    {
        get
        {
            return amount;
        }

        set
        {
            amount = value;
        }
    }

    public UnitReaction UnitReact
    {
        get
        {
            return unitReact;
        }

        set
        {
            unitReact = value;
        }
    }

    public ResourceReaction(Resources.type resID, int amount, Vector2 pos)
    {
        ResourceID = resID;
        Amount = amount;
        Pos = pos;
    }

    /// <summary>
    /// Check's if there's a mob threatening the tile
    /// </summary>
    /// <returns>true if there's an reaction</returns>
    public override bool HasPreReact(Hero h)
    {
        return UnitReact != null;
    }

    /// <summary>
    /// If there's a mob threatening the tile, start their reaction
    /// </summary>
    /// <param name="h">Hero that initated the reaction</param>
    /// <returns>true if that hero won</returns>
    public override bool PreReact(Hero h)
    {
        return UnitReact.React(h);
    }

    /// <summary>
    /// Adds resource amount to player
    /// </summary>
    /// <param name="h">Hero interacting with resource</param>
    /// <returns>returns true to signal graphical change, true since resource is always picked up</returns>
    public override bool React(Hero h)
    {
        h.Player.Resources.adjustResource(resourceID, amount);
        // Resource picked up, returned true
        return true;
    }
}
