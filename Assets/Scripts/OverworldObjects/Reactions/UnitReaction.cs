
using UnityEngine;

/// <summary>
/// Governs what happens when interacting with a unit on the map
/// </summary>
public class UnitReaction : Reaction {

    UnitTree units;
    private GameManager gm;

    public UnitReaction(UnitTree units, Point pos)
    {
        Units = units;
        Pos = pos;
        GameObject go = GameObject.Find("GameManager");
        gm = go.GetComponent<GameManager>();
    }

    public UnitTree Units
    {
        get
        {
            return units;
        }

        set
        {
            units = value;
        }
    }



    /// <summary>
    /// unit either flees, joins hero or fights.
    /// </summary>
    /// <param name="h">Hero interacting with unit</param>
    /// <returns>Returns False</returns>
    public override bool React(Hero h)
    {
        // Todo flee, join or fight
        // Returns true if hero remains. False if unit fleed, joined, or was defeated
        gm.enterCombat(15,11,h,units);
        return false;
    }
}
