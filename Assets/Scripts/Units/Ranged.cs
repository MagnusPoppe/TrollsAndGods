using Units;

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

    public Ranged(string name, int tier, int faction,int ammo, bool meleePenalty, int localID
        ) : base(name, tier, faction, localID)
    {
        Ammo = ammo;
        IsRanged = true;
        MeleePenalty = meleePenalty;
    }

}
