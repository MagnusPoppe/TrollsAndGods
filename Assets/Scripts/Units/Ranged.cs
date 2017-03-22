using Units;

public class Ranged : Unit
{
    int ammo, maxAmmo;
    bool meleePenalty, threatened;

    public int MaxAmmo
    {
        get { return maxAmmo; }
        set { maxAmmo = value; }
    }

    public bool Threatened
    {
        get { return threatened; }
        set { threatened = value; }
    }

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
        Move[] moves, Ability[] abilities,int ammo, bool meleePenalty
        ) : base(name, element, tier, faction, unitstats,moves,abilities)
    {
        Ammo = maxAmmo = ammo;
        IsRanged = true;
        MeleePenalty = meleePenalty;
        threatened = false;
    }

    public Ranged()
    {
        IsRanged = true;
    }
}
