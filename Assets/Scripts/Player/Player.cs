using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Player class that holds everything corresponding to the players values and actions.
/// </summary>
public class Player
{
    private Resources res;
    private Hero[] heroes;
    private List<Town.Town> towns;
    private int color;
    private bool[,] fogOfWar;
    private const int MAXHEROES = 8;
    private List<ResourceBuilding> resourceBuildings;

    /// <summary>
    /// Constructor that creates a new hero for the player, prepares fog of war, resources and towns
    /// </summary>
    /// <param name="color">Which color the player will get</param>
    /// <param name="difficulty">How difficult the game is set to be</param>
    public Player(int color, int difficulty)
    {
        Res = new Resources(difficulty);
        Heroes = new Hero[MAXHEROES];
        Heroes[0] = new Hero(this);
        Towns = new List<Town.Town>();
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

    public List<Town.Town> Towns
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

    public List<ResourceBuilding> ResourceBuildings
    {
        get
        {
            return resourceBuildings;
        }

        set
        {
            resourceBuildings = value;
        }
    }
}
