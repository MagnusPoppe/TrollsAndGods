using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Unit	{

    string name;
    Element element;
    int tier;
    int faction;
    UnitStats unitstats;
    bool haveNotRetaliated;
    bool isRanged = false;

    public Element Element
    {
        get
        {
            return element;
        }

        set
        {
            element = value;
        }
    }

    public int Tier
    {
        get
        {
            return tier;
        }

        set
        {
            tier = value;
        }
    }

    public int Faction
    {
        get
        {
            return faction;
        }

        set
        {
            faction = value;
        }
    }

    public UnitStats Unitstats
    {
        get
        {
            return unitstats;
        }

        set
        {
            unitstats = value;
        }
    }

    public bool HaveNotRetaliated
    {
        get
        {
            return haveNotRetaliated;
        }

        set
        {
            haveNotRetaliated = value;
        }
    }

    public bool IsRanged
    {
        get
        {
            return isRanged;
        }

        set
        {
            isRanged = value;
        }
    }

    public string Name
    {
        get
        {
            return name;
        }

        set
        {
            name = value;
        }
    }

    public Unit(string name,Element element, int tier, int faction, UnitStats unitstats)
    {
        Name = name;
        Element = element;
        Tier = tier;
        Faction = faction;
        Unitstats = unitstats;
        HaveNotRetaliated = true;
    }

    public int attack()
    {
        return unitstats.Damage;
    }

    public int retaliate()
    {
        if (HaveNotRetaliated)
        {
            return attack();
            HaveNotRetaliated = false;
        }
        else return 0;
    }

    public bool equals(Unit u)
    {
        return name.Equals(u.Name);
    }
}
