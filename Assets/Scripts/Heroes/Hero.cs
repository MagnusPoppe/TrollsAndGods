using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Class for the player's heroes
/// </summary>
public class Hero
{
    private Sprite portrait;
    private string name;
    private int faction;
    private Player player;
    private Unit[] units;
    private List<Item> items;
    private Item[] equippedItems;
    private GameObject self;
    private int movementSpeed;
    private int curMovementSpeed;

    /// <summary>
    /// Constructor that prepares unit, items, and equippeditems list for the hero
    /// </summary>
    /// <param name="color">id of which player gets the hero</param>
    public Hero(Player player, GameObject self, int movementSpeed)
    {
        Self = self;
        Player = player;
        Units = new Unit[7];
        Items = new List<Item>();
        EquippedItems = new Item[7];
        CurMovementSpeed = MovementSpeed = movementSpeed;
    }

    public Sprite GetPortrait()
    {
        return portrait;
    }
    public string Name
    {
        get
        {
            return name;
        }

        set
        {
            name = value;
        }
    }

    public int Faction
    {
        get
        {
            return faction;
        }

        set
        {
            faction = value;
        }
    }

    public Unit[] Units
    {
        get
        {
            return units;
        }

        set
        {
            units = value;
        }
    }

    public List<Item> Items
    {
        get
        {
            return items;
        }

        set
        {
            items = value;
        }
    }

    public Item[] EquippedItems
    {
        get
        {
            return equippedItems;
        }

        set
        {
            equippedItems = value;
        }
    }

    public Player Player
    {
        get
        {
            return player;
        }

        set
        {
            player = value;
        }
    }

    public GameObject Self
    {
        get
        {
            return self;
        }

        set
        {
            self = value;
        }
    }

    public int MovementSpeed
    {
        get
        {
            return movementSpeed;
        }

        set
        {
            movementSpeed = value;
        }
    }

    public int CurMovementSpeed
    {
        get
        {
            return curMovementSpeed;
        }

        set
        {
            curMovementSpeed = value;
        }
    }
}
