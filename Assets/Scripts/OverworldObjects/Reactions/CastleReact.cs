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
    /// <returns>returns false</returns>
    public override bool React(Hero h)
    {
        if (castle.Player.Equals(h.Player))
        {
            gm.EnterTown(castle.Town);
        }
        else
        {
            if (castle.Town.VisitingUnits.CountUnits() > 0)
            {
                if (castle.Town.StationedHero != null)
                {
                    gm.enterCombat(15,11,h,castle.Town.VisitingHero);
                }
                else
                {
                    castle.Town.StationedHero = castle.Town.VisitingHero;
                    castle.Town.VisitingHero = null;
                    if (castle.Town.StationedUnits.CanMerge(castle.Town.VisitingUnits))
                    {
                        castle.Town.StationedUnits.Merge(castle.Town.VisitingUnits);
                    }
                    gm.enterCombat(15,11,h,castle.Town.StationedHero);
                }
                return false;
            }
            else if (castle.Town.StationedUnits.CountUnits() > 0)
            {
                //battle
                if (castle.Town.StationedHero != null)
                {
                    gm.enterCombat(15, 11, h, castle.Town.StationedHero);
                }
                else
                {
                    gm.enterCombat(15, 11, h, castle.Town.StationedUnits);
                }
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
