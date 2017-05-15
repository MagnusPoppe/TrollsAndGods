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

    public Ranged(string name, int tier, int faction,int ammo, bool meleePenalty, int localID
        ) : base(name, tier, faction, localID)
    {
        Ammo = maxAmmo = ammo;
        IsRanged = true;
        MeleePenalty = meleePenalty;
        threatened = false;
    }

}
