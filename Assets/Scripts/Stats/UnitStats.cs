using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitStats : BaseStats {

    int damage;
    int health;
    int initative;
    int accuracy;

    public UnitStats(int attack, int defence, int speed, int moral, int luck,
        int damage, int health, int initative, int accuracy
        ) : base(attack, defence, speed, moral, luck)
    {
        Damage = damage;
        Health = health;
        Initative = initative;
        Accuracy = accuracy;
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
