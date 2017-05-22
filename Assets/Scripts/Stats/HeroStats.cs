/// <summary>
/// Class with stats specific to heroes
/// </summary>
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

    /// <summary>
    /// Default constructor
    /// </summary>
    /// <param name="attack">Heros attack</param>
    /// <param name="defence">Heros defence</param>
    /// <param name="speed">Heros speed</param>
    /// <param name="moral">Heros morale</param>
    /// <param name="luck">Heroes luck</param>
    /// <param name="wisdom">Heros wisdom</param>
    /// <param name="magicPower">Heroes magic pwoer</param>
    /// <param name="mana">Heroes current mana</param>
    /// <param name="maxMana">Heroes maximum mana</param>
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
