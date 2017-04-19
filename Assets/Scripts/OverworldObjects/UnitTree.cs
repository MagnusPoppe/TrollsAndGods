using System;
using System.Diagnostics;
using Debug = UnityEngine.Debug;

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

    public UnitTree(UnitTree unitTree)
    {
        this.units = unitTree.units;
        this.unitAmount = unitTree.unitAmount;
    }

    public UnitTree(Unit[] units)
    {
        this.units = units;
    }

    /// <summary>
    /// Sets troop amount troop
    /// </summary>
    /// <param name="pos">Which unit in array</param>
    /// <param name="amount">New stack amount</param>
    public void SetUnitAmount(int pos, int amount)
    {
        unitAmount[pos] = amount;
        // Remove unit if it's reduced to 0
        if (amount == 0)
            units[pos] = null;
        // Else set stackamount to input amount
        else if (units[pos] != null)
        {
            unitAmount[pos] = amount;
        }
    }


    public void SetUnitAmount(Unit Unit, int pos, int amount)
    {
        unitAmount[pos] = amount;
        // Remove unit if it's reduced to 0
        if (amount == 0)
        {
            units[pos] = null;
            unitAmount[pos] = amount;
        }
        // Else set stackamount to input amount
        else
        {
            unitAmount[pos] = amount;
            if(units[pos] == null)
                units[pos] = Unit;
        }
    }

    /// <summary>
    /// Changes amount of units at position
    /// </summary>
    /// <param name="amount">the amount to add or remove</param>
    /// <param name="pos">The position of the unit</param>
    public void changeAmount(int amount, int pos)
    {
        unitAmount[pos] += amount;
        if (unitAmount[pos] < 0) unitAmount[pos] = 0;
    }

    /// <summary>
    /// Called upon when a hero tries to enter a stationedTown
    /// </summary>
    /// <param name="units1">first army to check into</param>
    /// <param name="units2">the other army</param>
    /// <returns></returns>
    public bool CanMerge(UnitTree units2)
    {
        if (units != null || units2 == null || units2.units != null)
            return true;

        // Check if merge can be done
        int count = 0;
        for(int i=0; i<TREESIZE; i++)
        {
            if (units[i] != null)
            {
                count++;
                for(int j=0; j<TREESIZE; j++)
                {
                    if (units[i] != null && units[i].equals(units2.units[j]))
                        count--;
                }
            }
            if (units2.units[i] != null)
                count++;
        }
        if (count <= TREESIZE)
            return true;
        return false;

        /*
        
        UnitTree tmp1 = new UnitTree(this);
        UnitTree tmp2 = new UnitTree(units2);
        // Perform testMerge of tmp2 into tmp1
        tmp1.Merge(tmp2);

        // Can merge if count in the second unittree is less or same as first unittree's openspots
        if (tmp1.OpenSpots() >= tmp2.CountUnits())
        {
            return true;
        }
        return false;
        */
    }

    /// <summary>
    /// Count open spots in army
    /// </summary>
    /// <returns>Amount of open spots in army</returns>
    public int OpenSpots()
    {
        int count = 0;
        for (int i = 0; i < GetUnits().Length; i++)
        {
            if (GetUnits()[i] == null)
                count++;
        }
        return count;
    }

    /// <summary>
    /// Counts units in army
    /// </summary>
    /// <returns>Amount of units in army</returns>
    public int CountUnits()
    {
        int count = 0;
        for (int i = 0; i < GetUnits().Length; i++)
        {
            if (GetUnits()[i] != null)
                count++;
        }
        return count;
    }

    /// <summary>
    /// Merges input army2 into army1
    /// </summary>
    /// <param name="units2">Second input army</param>
    public void Merge(UnitTree units2)
    {
        // Merge duplicates units2 into units1
        for (int i = 0; i < GetUnits().Length; i++)
        {
            if(GetUnits()[i] != null)
            {
                for (int j = 0; j < GetUnits().Length; j++)
                {
                    if (units2.GetUnits()[j] != null && units2.GetUnits()[j].equals(GetUnits()[i]))
                    {
                        // Duplicate found in a bar, put one of them into the other one, and add amount
                        unitAmount[i] += units2.unitAmount[j];
                        units2.GetUnits()[j] = null;
                    }
                }
            }
        }

        // Merge duplicates in same bar
        for (int i = 0; i < GetUnits().Length; i++)
        {
            if(GetUnits()[i] != null)
            {
                for (int j = i+1; j < GetUnits().Length; j++)
                {
                    if (GetUnits()[j] != null && GetUnits()[i].equals(GetUnits()[j]))
                    {
                        // Duplicate found in a bar, put one of them into the other one, and add amount
                        unitAmount[i] += unitAmount[j];
                        units[j] = null;
                    }
                }
            }
        }

        // Merge units2 into units1
        for (int i = 0; i < GetUnits().Length; i++)
        {
            // If unit is found in units2
            if (units2.GetUnits()[i] != null)
            {
                // Check the units1 row to find next open spot to put it
                for (int j = 0; j < GetUnits().Length; j++)
                {
                    if (GetUnits()[j] == null)
                    {
                        GetUnits()[j] = units2.GetUnits()[i];
                        unitAmount[j] = units2.getUnitAmount(i);
                        units2.GetUnits()[i] = null;
                    }
                }
            }
        }
    }

    public bool addUnit(Unit unit, int amount)
    {
        if (amount < 1)
        {
            return false;
        }

        // checks for a unit of the same type to stack onto, choosing the first it finds
        for (int i = 0; i < units.Length; i++)
        {
            if (units[i] != null && units[i].GetType() == unit.GetType())
            {
                changeAmount(amount, i);
                return true;
            }
        }

        // if it fails to find a stack, look for empty spot
        for (int i = 0; i < units.Length; i++)
        {
            if (units[i] == null)
            {
                setUnit(unit, amount, i);
                return true;
            }
        }
        return false;
    }

    /*
    /// <summary>
    /// Merges input army2 into army1
    /// </summary>
    /// <param name="units1">First input army</param>
    /// <param name="units2">Second input army</param>
    public void Merge(UnitTree units1, UnitTree units2)
    {
        /*
        // Merge units1 into units2
        for (int i = 0; i < GetUnits().Length; i++)
        {
            for (int j = 0; j < GetUnits().Length; j++)
            {
                if (units1.GetUnits()[i] != null && units2.GetUnits()[i] != null && units1.GetUnits()[i].equals(units2.GetUnits()[j]))
                {
                    // Duplicate found in a bar, put one of them into the other one, and add amount
                    units2.unitAmount[i] += units2.unitAmount[j];
                    units1.GetUnits()[i] = null;
                }
            }
        }
        // Merge tmp2 duplicates
        for (int i = 0; i < GetUnits().Length; i++)
        {
            for (int j = 0; j < GetUnits().Length; j++)
            {
                if (units1.GetUnits()[i] != null && units2.GetUnits()[i] != null && units2.GetUnits()[i].equals(units2.GetUnits()[j]))
                {
                    // Duplicate found in a bar, put one of them into the other one, and add amount
                    units2.unitAmount[i] += units2.unitAmount[j];
                    units2.units[j] = null;
                }
            }
        }*/
    //}

    /// <summary>
    /// Sets unit at position
    /// </summary>
    /// <param name="unit">What unit</param>
    /// <param name="amount">How many</param>
    /// <param name="pos">It's position</param>
    public void setUnit(Unit unit, int amount, int pos)
    {
        units[pos] = unit;
        unitAmount[pos] = amount;
    }

    public void removeUnit(int pos)
    {
        units[pos] = null;
    }

    public Unit[] GetUnits()
    {
        return units;
    }

    public int getUnitAmount(int index)
    {
        return unitAmount[index];
    }

    public override string ToString()
    {
        string output = "";
        for (int i = 0; i < TREESIZE; i++)
        {
            if (units[i] != null)
            {
                output += i + " {Name=" + units[i].Name + ";Amount=" + unitAmount[i] +
                          "}";
            }
            else
            {
                output += i + "{NO UNIT FOUND}";
            }
        }
           
        return output;
    }
}