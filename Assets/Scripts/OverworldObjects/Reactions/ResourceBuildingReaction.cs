﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using OverworldObjects;

/// <summary>
/// Governs what happens when interacting with a resource generating building
/// </summary>
public class ResourceBuildingReaction : Reaction {

    ResourceBuilding resourceBuilding;
    UnitReaction unitReact;

    public ResourceBuildingReaction(ResourceBuilding resourceBuilding, Vector2 pos)
    {
        ResourceBuilding = resourceBuilding;
        Pos = pos;
    }

    public ResourceBuilding ResourceBuilding
    {
        get
        {
            return resourceBuilding;
        }

        set
        {
            resourceBuilding = value;
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

    /// <summary>
    /// Check's if there's a mob or hero threatening the tile
    /// </summary>
    /// <returns>true if there's an reaction</returns>
    public override bool HasPreReact(Hero h)
    {
        return (UnitReact != null || HeroMeetReact != null) && !h.Player.equals(HeroMeetReact.Hero.Player);
    }

    /// <summary>
    /// If there's a mob or hero threatening the tile, start their reaction
    /// </summary>
    /// <param name="h">Hero that initated the reaction</param>
    /// <returns>true if that hero won</returns>
    public override bool PreReact(Hero h)
    {
        if (UnitReact != null)
            return UnitReact.React(h);
        else if (HeroMeetReact != null)
            return HeroMeetReact.React(h);
        return false;
    }

    /// <summary>
    /// Changes owner of building if you don't already own it
    /// </summary>
    /// <param name="h">The hero interacting with reaource building</param>
    /// <returns>Returns true if owner changed, else false</returns>
    public override bool React(Hero h)
    {
        if (!h.Player.ResourceBuildings.Contains(ResourceBuilding))
        {
            ResourceBuilding.Player.ResourceBuildings.Remove(ResourceBuilding);
            ResourceBuilding.Player = h.Player;
            h.Player.ResourceBuildings.Add(ResourceBuilding);
            return true;
        }
        
        return false;
    }
}
