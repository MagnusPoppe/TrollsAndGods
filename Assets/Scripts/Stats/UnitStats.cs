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

    public UnitStats(int baseAttack, int baseDefence, int baseSpeed, int baseMoral, int baseLuck, int minDamage, int maxDamage, int baseHealth, int baseInitative, int effectiveRange) : base(baseAttack, baseDefence, baseSpeed, baseMoral, baseLuck)
    {
        this.minDamage = minDamage;
        this.maxDamage = maxDamage;
        this.BaseHealth = baseHealth;
        this.baseInitative = baseInitative;
        this.effectiveRange = effectiveRange;
        updateStats();
    }

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
