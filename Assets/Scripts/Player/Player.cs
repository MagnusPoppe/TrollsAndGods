using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using OverworldObjects;
using TownView;

/// <summary>
/// Player class that holds everything corresponding to the players values and actions.
/// </summary>
public class Player
{
    private Resources resources;
    private Hero[] heroes;
    private int nextEmptyHero;
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
        ResourceBuildings = new List<ResourceBuilding>();
        nextEmptyHero = 0;
    }

    public void GatherIncome()
    {
        foreach (ResourceBuilding rb in ResourceBuildings)
        {
            Resources.adjustResource(rb.ResourceID, 1);
        }
    }

    // override object.Equals
    public bool equals(Player player)
    {
        //       
        // See the full list of guidelines at
        //   http://go.microsoft.com/fwlink/?LinkID=85237  
        // and also the guidance for operator== at
        //   http://go.microsoft.com/fwlink/?LinkId=85238
        //

        if (player == null || GetType() != player.GetType())
        {
            return false;
        }

        // TODO: write your implementation of Equals() here
        return playerID == player.PlayerID;    
    }

    // override object.GetHashCode
    public override int GetHashCode()
    {
        // TODO: write your implementation of GetHashCode() here
        throw new System.NotImplementedException();
        return base.GetHashCode();
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

    public bool addHero(Hero h)
    {
        if(nextEmptyHero < MAXHEROES)
        {
            heroes[nextEmptyHero++] = h;
            return true;
        }
        return false;
    }

    public bool removeHero(int pos)
    {
        if (heroes[pos] != null)
        {
            heroes[pos] = null;
            nextEmptyHero--;

            // fill empty space in herotable
            for(int i=pos+1; i<MAXHEROES-1; i++)
            {
                heroes[i] = heroes[i + 1];
                // at last increment, clear last position in table
                if (i == MAXHEROES - 1)
                    heroes[MAXHEROES] = null;
            }
            return true;
        }
        return false;
    }
}
