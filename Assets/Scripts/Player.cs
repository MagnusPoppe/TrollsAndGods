using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player
{
    Resources res;
    Hero[] heroes;
    List<Town> towns;
    int color;
    bool[,] fogOfWar;

    public Player(int color, int difficulty)
    {
        Res = new Resources(difficulty);
        Heroes = new Hero[8];
        Heroes[0] = new Hero();
        Towns = new List<Town>();
        this.Color = color;
        FogOfWar = new bool[32, 32]; // todo, link to map objects x y size
    }

    public Hero[] Heroes
    {
        get
        {
            return heroes;
        }

        set
        {
            heroes = value;
        }
    }

    public List<Town> Towns
    {
        get
        {
            return towns;
        }

        set
        {
            towns = value;
        }
    }

    public int Color
    {
        get
        {
            return color;
        }

        set
        {
            color = value;
        }
    }

    public bool[,] FogOfWar
    {
        get
        {
            return fogOfWar;
        }

        set
        {
            fogOfWar = value;
        }
    }

    public Resources Res
    {
        get
        {
            return res;
        }

        set
        {
            res = value;
        }
    }
}
