using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Class of the units that are kept in a town, or a hero
/// </summary>
public class UnitTree
{
    protected Unit[] units;
    protected const int TREESIZE = 7;

    public UnitTree()
    {
        units = new Unit[TREESIZE];
    }

    public UnitTree(Unit[] units)
    {
        this.units = units;
    }

    /// <summary>
    /// Swaps the two units in the positions of the unitarray according to int parameters
    /// </summary>
    /// <param name="pos1">position of first unit</param>
    /// <param name="pos2">position of second unit</param>
    public void swapUnits(int pos1, int pos2)
    {
        Unit tmp = units[pos1];
        units[pos1] = units[pos2];
        units[pos2] = tmp;
    }

    public void setUnit(Unit unit, int pos)
    {
        units[pos] = unit;
    }

    public Unit[] GetUnits()
    {
        return units;
    }
}
