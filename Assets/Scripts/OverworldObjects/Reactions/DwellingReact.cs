using OverworldObjects;

/// <summary>
/// governs what happens when you interact with a dwelling
/// </summary>
public class DwellingReact : Reaction {

    DwellingBuilding dwellingBuilding;

    public DwellingReact(DwellingBuilding dwellingBuilding, Point pos)
    {
        DwellingBuilding = dwellingBuilding;
        Pos = pos;
    }

    public DwellingBuilding DwellingBuilding
    {
        get
        {
            return dwellingBuilding;
        }

        set
        {
            dwellingBuilding = value;
        }
    }

    /// <summary>
    /// If you don't own the dwelling, change owner. Hire units that are at dwelling.
    /// </summary>
    /// <param name="h">The hero interacting with the dwelling</param>
    /// <returns>True if owner changed</returns>
    public override bool React(Hero h)
    {
        if (!dwellingBuilding.Owner.equals(h.Player))
        {
            if (dwellingBuilding.Owner != null) dwellingBuilding.Owner.DwellingsOwned.Remove(dwellingBuilding);
            dwellingBuilding.Owner = h.Player;
            h.Player.DwellingsOwned.Add(dwellingBuilding);
            //dwelling.Town.updateDwellingOwnerChange(dwelling);
            //todo inital hiring of units
            return true;
        }
        
        // todo hire units
        return false;
    }
}
