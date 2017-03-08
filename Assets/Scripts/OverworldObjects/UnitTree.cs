using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Class of the units that are kept in a town, or a hero
/// </summary>
public class UnitTree
{
    private Unit[] units;
    private int[] unitAmount;
    public static readonly int TREESIZE = 7;

    public UnitTree()
    {
        units = new Unit[TREESIZE];
        unitAmount = new int[TREESIZE];
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
    /// <param name="amount1">amount of first stack</param>
    /// <param name="amount2">amount of second stack</param>
    public void swapUnits(int pos1, int amount1, int pos2, int amount2)
    {
        Unit tmp = units[pos1];
        units[pos1] = units[pos2];
        units[pos2] = tmp;

        int tmpAmount = amount1;
        unitAmount[amount1] = amount2;
        unitAmount[amount2] = tmpAmount;
    }

    /// <summary>
    /// Changes amount of units at position
    /// </summary>
    /// <param name="amount">the amount to add or remove</param>
    /// <param name="pos">The position of the unit</param>
    public void changeAmount(int amount, int pos)
    {
        unitAmount[pos] += amount;
    }

    /// <summary>
    /// Sets unit at position
    /// </summary>
    /// <param name="unit">What unit</param>
    /// <param name="amount">How many</param>
    /// <param name="pos">It's position</param>
    public void setUnit(Unit unit, int amount, int pos)
    {
        units[pos] = unit;
    }

    public Unit[] GetUnits()
    {
        return units;
    }

    public int getUnitAmount(int index)
    {
        return unitAmount[index];
    }
}
