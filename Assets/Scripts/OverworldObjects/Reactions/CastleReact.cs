using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using OverworldObjects;

public class CastleReact : Reaction {

    Castle castle;

    public CastleReact(Castle castle, Vector2 pos)
    {
        Castle = castle;
        Pos = pos;
    }

    public Castle Castle
    {
        get
        {
            return castle;
        }

        set
        {
            castle = value;
        }
    }

    public override bool React(Hero h)
    {
        //todo check if owner matches heroes owner
        if (castle.Player == h.PlayerID)
        {
            //Todo visit town
            return false;
        }
        else
        {
            //Todo battle
        }
        return true;
    }

    public bool React(Player player)
    {
        //todo check if player owns town
        if (true)
        {
            //todo open town window
            return true;
        }
        return false;
    }
}
