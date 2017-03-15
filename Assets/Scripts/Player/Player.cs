using System.Collections.Generic;
using OverworldObjects;
using UnityEngine;

/// <summary>
/// Player class that holds everything corresponding to the players values and actions.
/// </summary>
public class Player
{
    private Wallet wallet;
    private Hero[] heroes;
    private int nextEmptyHero;
    private List<Castle> castle;
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
        wallet = new Wallet(difficulty);
        Heroes = new Hero[MAXHEROES];
        Castle = new List<Castle>();
        PlayerID = playerID;
        FogOfWar = new bool[32, 32]; // todo, link to map objects x y size
        DwellingsOwned = new List<Dwelling>();
        ResourceBuildings = new List<ResourceBuilding>();
        nextEmptyHero = 0;
    }

    public void GatherIncome()
    {
        string debug = "WAS : " + wallet;
        foreach (ResourceBuilding building in ResourceBuildings)
        {
            Debug.Log("Gathering resources from ResourceBuilding" + building);
            wallet = building.Earnings.adjustResources(wallet);
        }
        foreach (Castle c in castle)
        {
            foreach (TownView.Building building in c.Town.Buildings)
            {
                if (building.GetType().BaseType == typeof(TownView.ResourceBuilding))
                {
                    TownView.ResourceBuilding b = (TownView.ResourceBuilding) building;
                    wallet = b.Earnings.adjustResources(wallet);
                    Debug.Log("Gathering resources from TownBuilding " + b);
                }
            }
        }

        debug += "\nIS : " + wallet;
        Debug.Log(debug);
    }

    // override object.Equals
    public bool equals(Player player)
    {

        if (player == null || GetType() != player.GetType())
        {
            return false;
        }
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

    public List<Castle> Castle
    {
        get
        {
            return castle;
        }

        set
        {
            castle = value;
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

    public Wallet Wallet
    {
        get
        {
            return wallet;
        }

        set
        {
            wallet = value;
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
        if(nextEmptyHero < MAXHEROES && !h.Alive)
        {
            h.Alive = true;
            heroes[nextEmptyHero++] = h;
            return true;
        }
        return false;
    }

    public bool removeHero(int pos)
    {
        if (heroes[pos] != null && heroes[pos].Alive)
        {
            heroes[pos].Alive = false;
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
