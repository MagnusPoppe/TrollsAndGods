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
    private int color;
    private Unit[] units;
    private List<Item> items;
    private List<Item> equippedItems;

    /// <summary>
    /// Constructor that prepares unit, items, and equippeditems list for the hero
    /// </summary>
    /// <param name="color">id of which player gets the hero</param>
    public Hero(int color)
    {
        this.color = color;
        Units = new Unit[7];
        Items = new List<Item>();
        EquippedItems = new List<Item>();
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

    public int Color
    {
        get
        {
            return color;
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

    public List<Item> EquippedItems
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
}
