using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using OverworldObjects;

/// <summary>
/// Governs what happens when you interact with a castle/town
/// </summary>
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

    /// <summary>
    /// Hero either visits or attacks castle
    /// </summary>
    /// <param name="h">Hero interacting with castle</param>
    /// <returns>returns false if visiting, true if attacking</returns>
    public override bool React(Hero h)
    {
        
        if (castle.Player.Equals(h.Player))
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

    /// <summary>
    /// player clicked on a castle, opens town window if player owns it.
    /// </summary>
    /// <param name="player">Wich player clicked</param>
    /// <returns>false, no graphical change</returns>
    public bool React(Player player)
    {
        if (player.equals(castle.Player))
        {
            //todo open town window
        }
        return false;
    }
}
