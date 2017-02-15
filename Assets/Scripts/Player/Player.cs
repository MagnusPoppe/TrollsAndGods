﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using OverworldObjects;

/// <summary>
/// Player class that holds everything corresponding to the players values and actions.
/// </summary>
public class Player
{
    private Resources resources;
    private Hero[] heroes;
    private List<Town> towns;
    private int playerID;
    private bool[,] fogOfWar;
    private const int MAXHEROES = 8;
    private List<ResourceBuilding> resourceBuildings;
    private List<Dwelling> dwellingsOwned;

    /// <summary>
    /// Constructor that creates a new hero for the player, prepares fog of war, resources and towns
    /// </summary>
    /// <param name="playerID">Which color the player will get</param>
    /// <param name="difficulty">How difficult the game is set to be</param>
    public Player(int playerID, int difficulty)
    {
        Resources = new Resources(difficulty);
        Heroes = new Hero[MAXHEROES];
        Towns = new List<Town>();
        PlayerID = playerID;
        FogOfWar = new bool[32, 32]; // todo, link to map objects x y size
        DwellingsOwned = new List<Dwelling>();
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

    public int PlayerID
    {
        get
        {
            return playerID;
        }

        set
        {
            playerID = value;
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

    public Resources Resources
    {
        get
        {
            return resources;
        }

        set
        {
            resources = value;
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

    public List<Dwelling> DwellingsOwned
    {
        get
        {
            return dwellingsOwned;
        }

        set
        {
            dwellingsOwned = value;
        }
    }
}
