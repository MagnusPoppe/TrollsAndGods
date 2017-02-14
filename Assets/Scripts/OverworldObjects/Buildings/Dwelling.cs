using System.Collections;
using System.Collections.Generic;
using Town;
using UnityEngine;

public class Dwelling {

    Town.Town town;
    Player owner;
    Unit unitType;
    int unitsPresent;
    int unitsPerWeek;

    public Town.Town Town
    {
        get
        {
            return town;
        }

        set
        {
            town = value;
        }
    }

    public Unit UnitType
    {
        get
        {
            return unitType;
        }

        set
        {
            unitType = value;
        }
    }

    public int UnitsPresent
    {
        get
        {
            return unitsPresent;
        }

        set
        {
            unitsPresent = value;
        }
    }

    public int UnitsPerWeek
    {
        get
        {
            return unitsPerWeek;
        }

        set
        {
            unitsPerWeek = value;
        }
    }

    public Player Owner
    {
        get
        {
            return owner;
        }

        set
        {
            owner = value;
        }
    }

    public Dwelling(Town.Town town,Unit unitType, int unitsPresent, int unitsPerWeek)
    {
        Town = town;
        UnitType = UnitType;
        UnitsPresent = unitsPresent;
        UnitsPerWeek = unitsPerWeek;
    }

    public void populate()
    {
        unitsPresent += unitsPerWeek;
    }

    public void populate(int more)
    {
        unitsPresent += unitsPerWeek * more;
    }
}
