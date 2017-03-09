public class HeroStat : BaseStats
{

    int wisdom;
    int magicPower;
    int mana;
    int maxMana;

    public int Wisdom
    {
        get
        {
            return wisdom;
        }

        set
        {
            wisdom = value;
        }
    }

    public int MagicPower
    {
        get
        {
            return magicPower;
        }

        set
        {
            magicPower = value;
        }
    }

    public int Mana
    {
        get
        {
            return mana;
        }

        set
        {
            mana = value;
        }
    }

    public int MaxMana
    {
        get
        {
            return maxMana;
        }

        set
        {
            maxMana = value;
        }
    }

    public HeroStat(int attack, int defence, int speed, int moral, int luck,
        int wisdom, int magicPower, int mana, int maxMana
        ) : base(attack, defence, speed, moral, luck)
    {
        this.Wisdom = wisdom;
        this.MagicPower = magicPower;
        this.Mana = mana;
        this.MaxMana = maxMana;
    }
}
