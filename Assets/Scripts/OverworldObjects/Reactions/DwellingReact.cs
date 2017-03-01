using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using OverworldObjects;

/// <summary>
/// governs what happens when you interact with a dwelling
/// </summary>
public class DwellingReact : Reaction {

    Dwelling dwelling;
    UnitReaction unitReact;

    public DwellingReact(Dwelling dwelling, Vector2 pos)
    {
        Dwelling = dwelling;
        Pos = pos;
    }

    public Dwelling Dwelling
    {
        get
        {
            return dwelling;
        }

        set
        {
            dwelling = value;
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
    /// If you don't own the dwelling, change owner. Hire units that are at dwelling.
    /// </summary>
    /// <param name="h">The hero interacting with the dwelling</param>
    /// <returns>True if you do not own dwelling, else false.</returns>
    public override bool React(Hero h)
    {
        if (!dwelling.Owner.equals(h.Player))
        {
            if (dwelling.Owner != null) dwelling.Owner.DwellingsOwned.Remove(dwelling);
            dwelling.Owner = h.Player;
            h.Player.DwellingsOwned.Add(dwelling);
            //dwelling.Town.updateDwellingOwnerChange(dwelling);
            //todo inital hiring of units
            return true;
        }
        
        // todo hire units
        return false;
    }
}
