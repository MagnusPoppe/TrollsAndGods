using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResourceBuilding {

    Player player;
    Vector2 pos;
    int resourceID;

    public Player Player
    {
        get
        {
            return player;
        }

        set
        {
            player = value;
        }
    }

    public Vector2 Pos
    {
        get
        {
            return pos;
        }

        set
        {
            pos = value;
        }
    }

    public int ResourceID
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

    public ResourceBuilding(Player player, Vector2 pos, int resourceID)
    {
        Player = player;
        Pos = pos;
        ResourceID = resourceID;
    }
}
