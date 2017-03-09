public class BaseStats {

    int attack;
    int defence;
    int speed;
    int moral;
    int luck;

    public int Attack
    {
        get
        {
            return attack;
        }

        set
        {
            attack = value;
        }
    }

    public int Defence
    {
        get
        {
            return defence;
        }

        set
        {
            defence = value;
        }
    }

    public int Speed
    {
        get
        {
            return speed;
        }

        set
        {
            speed = value;
        }
    }

    public int Moral
    {
        get
        {
            return moral;
        }

        set
        {
            moral = value;
        }
    }

    public int Luck
    {
        get
        {
            return luck;
        }

        set
        {
            luck = value;
        }
    }

    public BaseStats(int attack, int defence, int speed, int moral, int luck)
    {
        this.Attack = attack;
        this.Defence = defence;
        this.Speed = speed;
        this.Moral = moral;
        this.Luck = luck;
    }
	
	
}
