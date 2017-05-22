/// <summary>
/// Holds stats specific to units
/// </summary>
public class UnitStats : BaseStats {

    int minDamage;
    int maxDamage;
    int health;
    private int bonusHealth;
    private int baseHealth;
    int initative;
    private int bonusInitative;
    private int baseInitative;
    int effectiveRange;

    /// <summary>
    /// Default constructor with base stats
    /// </summary>
    /// <param name="baseAttack">The units base attack stat</param>
    /// <param name="baseDefence">The units base defence stat</param>
    /// <param name="baseSpeed">The units base speed stat</param>
    /// <param name="baseMoral">The units base moral stat</param>
    /// <param name="baseLuck">The units base luck stat</param>
    /// <param name="minDamage">Minimum auto attack damage</param>
    /// <param name="maxDamage">Maximum auto attack daamge</param>
    /// <param name="baseHealth">The units base health stat</param>
    /// <param name="baseInitative">The units base initiative stat</param>
    /// <param name="effectiveRange">The units effective attack range</param>
    public UnitStats(int baseAttack, int baseDefence, int baseSpeed, int baseMoral, int baseLuck, int minDamage, int maxDamage, int baseHealth, int baseInitative, int effectiveRange) : base(baseAttack, baseDefence, baseSpeed, baseMoral, baseLuck)
    {
        this.minDamage = minDamage;
        this.maxDamage = maxDamage;
        this.BaseHealth = baseHealth;
        this.baseInitative = baseInitative;
        this.effectiveRange = effectiveRange;
        updateStats();
    }

    /// <summary>
    /// Consturctor with bonus stats
    /// </summary>
    /// <param name="baseAttack">The units base attack stat</param>
    /// <param name="bonusAttack">The extra attack given by other sources</param>
    /// <param name="baseDefence">The units base defence stat</param>
    /// <param name="bonusDefence">The extra units defence given by other sources</param>
    /// <param name="baseSpeed">The units base speed stat</param>
    /// <param name="bonusSpeed">The units extra speed given by other sources</param>
    /// <param name="baseMoral">The units base moral stat</param>
    /// <param name="bonusMoral">The units extra moral given by other sources</param>
    /// <param name="baseLuck">The units base luck stat</param>
    /// <param name="bonusLuck">The units extra luck given by other sources</param>
    /// <param name="minDamage">Minimum auto attack damage</param>
    /// <param name="maxDamage">Maximum auto attack daamge</param
    /// <param name="bonusHealth">The units extra health given by other sources</param>
    /// <param name="baseHealth">The units base health stat</param>
    /// <param name="bonusInitative">The units extra initiative given by other sources</param>
    /// <param name="baseInitative">The units base initiative stat</param>
    /// <param name="effectiveRange">The units effective attack range</param>
    public UnitStats(int bonusAttack, int baseAttack, int bonusDefence, int baseDefence, int bonusSpeed, int baseSpeed, int bonusMoral, int baseMoral, int bonusLuck, int baseLuck, int minDamage, int maxDamage, int bonusHealth, int baseHealth, int bonusInitative, int baseInitative, int effectiveRange) : base(bonusAttack, baseAttack, bonusDefence, baseDefence, bonusSpeed, baseSpeed, bonusMoral, baseMoral, bonusLuck, baseLuck)
    {
        this.minDamage = minDamage;
        this.maxDamage = maxDamage;
        this.BonusHealth = bonusHealth;
        this.BaseHealth = baseHealth;
        this.bonusInitative = bonusInitative;
        this.baseInitative = baseInitative;
        this.effectiveRange = effectiveRange;
        updateStats();
    }

    /// <summary>
    /// Function to update a units stats
    /// </summary>
    public new void updateStats()
    {
        base.updateStats();
        initative = baseInitative + bonusInitative;
        health = BaseHealth + BonusHealth;
    }

    public int MinDamage
    {
        get
        {
            return minDamage;
        }

        set
        {
            minDamage = value;
        }
    }

    public int MaxDamage
    {
        get
        {
            return maxDamage;
        }

        set
        {
            maxDamage = value;
        }
    }

    public int Health
    {
        get
        {
            return health;
        }

        set
        {
            health = value;
        }
    }

    public int Initative
    {
        get
        {
            return initative;
        }

        set
        {
            initative = value;
        }
    }


    public int BaseInitative
    {
        get { return baseInitative; }
        set { baseInitative = value; }
    }
    public int BonusInitiative
    {
        get { return bonusInitative; }
        set { bonusInitative = value; }
    }

    public int EffectiveRange
    {
        get
        {
            return effectiveRange;
        }

        set
        {
            effectiveRange = value;
        }
    }

    public int BonusHealth
    {
        get
        {
            return bonusHealth;
        }

        set
        {
            bonusHealth = value;
        }
    }

    public int BaseHealth
    {
        get
        {
            return baseHealth;
        }

        set
        {
            baseHealth = value;
        }
    }
}
