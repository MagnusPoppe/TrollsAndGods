using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player
{
    public const int X = 32;
    public const int Y = 32;
    Hero[] heroes;
    List<Town> towns;
    int color;
    bool[,] fogOfWar;

    public Player(int color)
    {
        heroes = new Hero[8];
        heroes[0] = new Hero();
        towns = new List<Town>();
        this.color = color;
        fogOfWar = new bool[X, Y];
    }
}
