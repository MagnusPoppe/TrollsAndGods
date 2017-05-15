using Units;

/// <summary>
/// Superclass for ranged units
/// </summary>
public class Ranged : Unit
{
    int ammo, maxAmmo;
    bool meleePenalty, threatened;

    public int MaxAmmo
    {
        get { return maxAmmo; }
        set { maxAmmo = value; }
    }

    /// <summary>
    /// Bool for the unit if it's in a threatened combat state or not
    /// </summary>
    public bool Threatened
    {
        get { return threatened; }
        set { threatened = value; }
    }

    public int Ammo
    {
        get { return ammo; }

        set { ammo = value; }
    }

    /// <summary>
    /// Bool for the unit if it has a melee penalty or not
    /// </summary>
    public bool MeleePenalty
    {
        get { return meleePenalty; }

        set { meleePenalty = value; }
    }

    /// <summary>
    /// Default constructor
    /// </summary>
    /// <param name="name">Name of the unit</param>
    /// <param name="tier">The units tier</param>
    /// <param name="faction">The faction this unit belongs to</param>
    /// <param name="ammo">How much ammo this unit carries</param>
    /// <param name="meleePenalty">If the ranged unit has a melee penalty</param>
    /// <param name="localID">The units sprite ID</param>
    public Ranged(string name, int tier, int faction, int ammo, bool meleePenalty, int localID
    ) : base(name, tier, faction, localID)
    {
        Ammo = maxAmmo = ammo;
        IsRanged = true;
        MeleePenalty = meleePenalty;
        threatened = false;
    }
}