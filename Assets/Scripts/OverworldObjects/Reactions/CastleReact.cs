﻿using UnityEngine;
using OverworldObjects;

/// <summary>
/// Governs what happens when you interact with a castle/town
/// </summary>
public class CastleReact : Reaction {

    Castle castle;
    GameManager gm;

    public CastleReact(Castle castle, Point pos)
    {
        Castle = castle;
        Pos = pos;
        GameObject go = GameObject.Find("GameManager");
        gm = go.GetComponent<GameManager>();
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

    /// <summary>
    /// Hero either visits or attacks castle
    /// </summary>
    /// <param name="h">Hero interacting with castle</param>
    /// <returns>returns false</returns>
    public override bool React(Hero h)
    {
        if (castle.Player.Equals(h.Player))
        {
            gm.EnterTown(castle.Town);
        }
        else
        {
            if (castle.Town.VisitingUnits != null)
            {
                //todo combine with stationed units and battle
                return false;
            }
            else if (castle.Town.StationedUnits != null)
            {
                //battle
                gm.enterCombat(10, 10, h, castle.Town.StationedUnits);
                return false;
            }
            return true;
        }
        return false;
    }

    /// <summary>
    /// player clicked on a castle, opens town window if player owns it.
    /// </summary>
    /// <param name="player">Wich player clicked</param>
    /// <returns>false, no graphical change</returns>
    public bool React(Player player)
    {
        if (player.equals(castle.Player))
        {
            gm.EnterTown(castle.Town);
        }
        return false;
    }
}
