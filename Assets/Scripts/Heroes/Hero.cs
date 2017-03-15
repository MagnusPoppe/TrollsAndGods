using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Class for the player's heroes
/// </summary>
public class Hero : SpriteSystem
{
    private string name;
    private string description;
    private int faction;
    private Player player;
    private UnitTree units;
    private List<Item> items;
    private Item[] equippedItems;
    private int movementSpeed;
    private int curMovementSpeed;
    private Point position;
    private int localSpriteID;
    private int portraitID;
    private const IngameObjectLibrary.Category SPRITECATEGORY = IngameObjectLibrary.Category.Heroes;
    private const IngameObjectLibrary.Category SPRITEPORTRAITCATEGORY = IngameObjectLibrary.Category.Portraits;
    private List<Vector2> path;
    private bool alive;
    protected Cost cost;

    /// <summary>
    /// Constructor that prepares unit, items, and equippeditems list for the hero
    /// </summary>
    /// <param name="color">id of which player gets the hero</param>
    public Hero(Player player, Point position, int localSpriteID, int portraitID, string name, string description, Cost cost) : base(localSpriteID, SPRITECATEGORY)
    {
        Player = player;
        Units = new UnitTree();
        Items = new List<Item>();
        EquippedItems = new Item[7];
        Position = position;
        CurMovementSpeed = MovementSpeed = 12;
        this.portraitID = portraitID;
        // Alive = true; TODO: dont set alive here?
        Name = name;
        Description = description;
        Cost = cost;
    }

    /// <summary>
    /// Constructor that prepares unit, items, and equippeditems list for the hero
    /// </summary>
    /// <param name="color">id of which player gets the hero</param>
    public Hero(int localSpriteID, int portraitID, string name, string description, Cost cost) : base(localSpriteID, SPRITECATEGORY)
    {
        Units = new UnitTree();
        Items = new List<Item>();
        EquippedItems = new Item[7];
        CurMovementSpeed = MovementSpeed = 12;
        this.portraitID = portraitID;
        Alive = false;
        Name = name;
        Description = description;
        Cost = cost;
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

    public UnitTree Units
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

    public Point Position
    {
        get
        {
            return position;
        }

        set
        {
            position = value;
        }
    }

    public int LocalSpriteID
    {
        get
        {
            return localSpriteID;
        }

        set
        {
            localSpriteID = value;
        }
    }

    public List<Vector2> Path
    {
        get
        {
            return path;
        }

        set
        {
            path = value;
        }
    }

    public string Description
    {
        get
        {
            return description;
        }

        set
        {
            description = value;
        }
    }

    public bool Alive
    {
        get
        {
            return alive;
        }

        set
        {
            alive = value;
        }
    }

    public Cost Cost
    {
        get
        {
            return cost;
        }
        set
        {
            cost = value;
        }
    }

    public int GetSpriteID()
    {
        return LocalSpriteID + IngameObjectLibrary.GetOffset(SPRITECATEGORY);
    }

    public int GetPortraitID()
    {
        return portraitID + IngameObjectLibrary.GetOffset(SPRITEPORTRAITCATEGORY);
    }
}
