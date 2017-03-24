using Units;

public class Unit : SpriteSystem	{

    string name;
    Element element;
    int tier;
    int faction;
    UnitStats unitstats;
    bool haveNotRetaliated;
    bool isRanged = false;
    int currentHealth;
    private Move[] moves;
    private Ability[] abilities;
    Cost price;

    private const IngameObjectLibrary.Category CATEGORY = IngameObjectLibrary.Category.Unit;

    public Move[] Moves
    {
        get { return moves; }
        set { moves = value; }
    }

    public Ability[] Abilities
    {
        get { return abilities; }
        set { abilities = value; }
    }

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

    public Cost Price
    {
        get { return price; }
        set { price = value; }
    }

    public Unit(string name,int tier, int faction, int localID) : base(localID, CATEGORY)
    {
        Name = name;
        Tier = tier;
        Faction = faction;
        HaveNotRetaliated = true;
        CurrentHealth = unitstats.Health;
        Price = price;
    }


    public bool equals(Unit u)
    {
        return name.Equals(u.Name);
    }
}
