public class Unit	{

    string name;
    Element element;
    int tier;
    int faction;
    UnitStats unitstats;
    bool haveNotRetaliated;
    bool isRanged = false;
    int currentHealth;

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

    public int CurrentHealth
    {
        get
        {
            return currentHealth;
        }

        set
        {
            currentHealth = value;
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
        CurrentHealth = unitstats.Health;
    }

    public Unit()
    {
        HaveNotRetaliated = true;
    }

    public bool equals(Unit u)
    {
        return name.Equals(u.Name);
    }
}
