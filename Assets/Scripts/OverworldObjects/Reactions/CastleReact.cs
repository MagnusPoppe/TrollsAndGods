using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using OverworldObjects;

/// <summary>
/// Governs what happens when you interact with a castle/town
/// </summary>
public class CastleReact : Reaction {

    Castle castle;
    UnitReaction unitReact;
    GameManager gm;

    public CastleReact(Castle castle, Vector2 pos)
    {
        Castle = castle;
        Pos = pos;
        HeroMeetReact = null;
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
    /// Hero either visits or attacks castle
    /// </summary>
    /// <param name="h">Hero interacting with castle</param>
    /// <returns>returns false if visiting, true if attacking</returns>
    public override bool React(Hero h)
    {
        if (castle.Player.Equals(h.Player))
        {
            gm.EnterTown(castle.Town);
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
            gm.EnterTown(castle.Town);
        }
        return false;
    }
}
