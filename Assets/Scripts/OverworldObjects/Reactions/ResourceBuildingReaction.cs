﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using OverworldObjects;

/// <summary>
/// Governs what happens when interacting with a resource generating building
/// </summary>
public class ResourceBuildingReaction : Reaction {

    ResourceBuilding resourceBuilding;
    HeroMeetReact heroReact;

    public ResourceBuildingReaction(ResourceBuilding resourceBuilding, Vector2 pos)
    {
        ResourceBuilding = resourceBuilding;
        Pos = pos;
        HeroReact = null;
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

    public HeroMeetReact HeroReact
    {
        get
        {
            return heroReact;
        }

        set
        {
            heroReact = value;
        }
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
