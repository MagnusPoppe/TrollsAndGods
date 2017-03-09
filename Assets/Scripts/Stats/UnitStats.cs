public class UnitStats : BaseStats {

    int minDamage;
    int maxDamage;
    int health;
    int initative;
    int accuracy;

    public UnitStats(int attack, int defence, int speed, int moral, int luck,
        int minDamage,int maxDamage, int health, int initative, int accuracy
        ) : base(attack, defence, speed, moral, luck)
    {
        MinDamage = minDamage;
        MaxDamage = maxDamage;
        Health = health;
        Initative = initative;
        Accuracy = accuracy;
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

    public int Accuracy
    {
        get
        {
            return accuracy;
        }

        set
        {
            accuracy = value;
        }
    }
}
