using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResourceBuildingReaction : Reaction {

    ResourceBuilding resourceBuilding;

    public ResourceBuildingReaction(ResourceBuilding resourceBuilding, Vector2 pos, GameObject self, Reaction[,] reactionTab)
    {
        ResourceBuilding = resourceBuilding;
        Pos = pos;
        Self = self;
        ReactionTab = reactionTab;
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
