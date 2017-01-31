using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitStats : BaseStats {

    int damage;
    int health;
    int initative;

    public UnitStats(int attack, int defence, int speed, int moral, int luck,
        int damage, int health, int initative
        ) : base(attack, defence, speed, moral, luck)
    {
        this.Damage = damage;
        this.Health = health;
        this.Initative = initative;
    }

    public int Damage
    {
        get
        {
            return damage;
        }

        set
        {
            damage = value;
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
}
