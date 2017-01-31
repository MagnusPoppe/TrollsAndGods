using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player
{
    private const int x = 32;
    private const int y = 32;
    Hero[] heroes;
    List<Town> towns;
    int color;
    bool[,] fogOfWar;

    public Player(int color)
    {
        Heroes = new Hero[8];
        Heroes[0] = new Hero();
        Towns = new List<Town>();
        this.Color = color;
        FogOfWar = new bool[X, Y];
    }

    public static int X
    {
        get
        {
            return x;
        }
    }

    public static int Y
    {
        get
        {
            return y;
        }
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

}
