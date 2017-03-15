public class Ranged : Unit
{
    int ammo;
    bool meleePenalty;

    public int Ammo
    {
        get
        {
            return ammo;
        }

        set
        {
            ammo = value;
        }
    }

    public bool MeleePenalty
    {
        get
        {
            return meleePenalty;
        }

        set
        {
            meleePenalty = value;
        }
    }

    public Ranged(string name, Element element, int tier, int faction, UnitStats unitstats,
        int ammo, bool meleePenalty
        ) : base(name, element, tier, faction, unitstats)
    {
        Ammo = ammo;
        IsRanged = true;
        MeleePenalty = meleePenalty;
    }

    public Ranged()
    {
        IsRanged = true;
    }
}
