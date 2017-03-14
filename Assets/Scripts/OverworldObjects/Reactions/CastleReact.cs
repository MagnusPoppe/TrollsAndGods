using UnityEngine;
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
