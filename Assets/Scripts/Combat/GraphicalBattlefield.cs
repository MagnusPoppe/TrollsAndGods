﻿using UnityEngine;
using System.Linq;
using System.Collections.Generic;

/// <summary>
/// Class handles the graphical side of combat
/// </summary>
public class GraphicalBattlefield : MonoBehaviour {

    BattleField battleField;
    int[,] canwalk;
    int width, height;
    bool inCombat;
    GameObject[,] field;
    GameObject[,] unitsOnField;
    GameObject parent;
    bool isWalking, finishedWalking;
    UnitGameObject[] initative;
    int whoseTurn;
    int livingAttackers, livingDefenders;
    private PossibleMovement possibleMovement;

    // Use this for initialization
    void Start () {
        InCombat = false;
        IsWalking = finishedWalking = false;
	}
	
	// Update is called once per frame
	void Update () {
		if (InCombat)
        {
            if (IsWalking)
            {
                //Todo keep moving
            }
            else if (finishedWalking)
            {
                //animation
                nextTurn();
            }
            else if (livingAttackers == 0 || livingDefenders == 0)
            {
                endCombat();
            }
        }
	}

    /// <summary>
    /// Readies battlefield between two heroes
    /// </summary>
    /// <param name="width">Width of battlefield</param>
    /// <param name="height">Height of battlefield</param>
    /// <param name="attacker">Attacking hero</param>
    /// <param name="defender">Defending hero</param>
    public void beginCombat(int width, int height, Hero attacker, Hero defender)
    {
        Width = width;
        Height = height;
        InCombat = true;
        Canwalk = new int[width, height];
        //todo add obstacles
        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                Canwalk[x, y] = MapGenerator.MapMaker.CANWALK;
            }
        }
        BattleField = new BattleField(width, height, attacker, defender, Canwalk);

        populateField();
        populateInitative(attacker, defender.Units);
        possibleMovement = new PossibleMovement(field,unitsOnField,canwalk,width,height);
        possibleMovement.flipReachable(Initative[whoseTurn].LogicalPos, Initative[whoseTurn].UnitTree.GetUnits()[Initative[whoseTurn].PosInUnitTree].Unitstats.Speed);
    }

    /// <summary>
    /// Readies battlefield between hero and neutral units
    /// </summary>
    /// <param name="width">Width of battlefield</param>
    /// <param name="height">Height of battlefield</param>
    /// <param name="attacker">Attacking hero</param>
    /// <param name="defender">Defending units</param>
    public void beginCombat(int width, int height, Hero attacker, UnitTree defender)
    {
        Width = width;
        Height = height;
        InCombat = true;
        Canwalk = new int[width, height];
        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                Canwalk[x, y] = MapGenerator.MapMaker.CANWALK;
            }
        }
        BattleField = new BattleField(width, height, attacker, defender, Canwalk);

        populateField();
        populateInitative(attacker, defender);
    }

    /// <summary>
    /// fills 2d array for groundtiles
    /// </summary>
    public void populateField()
    {
        field = new GameObject[width,height];
        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                GameObject go = new GameObject("ground x=" + x + ", y=" + y);
                go.AddComponent<GroundGameObject>();
                GroundGameObject ggo = go.GetComponent<GroundGameObject>();
                ggo.GraphicalBattlefield = this;
                ggo.LogicalPos = new Point(x, y);
            }
        }
    }

    /// <summary>
    /// Readies initative and populates 2d array for units
    /// </summary>
    /// <param name="attacker">Attacking hero</param>
    /// <param name="defender">Defending units</param>
    public void populateInitative(Hero attacker, UnitTree defender)
    {
        unitsOnField = new GameObject[width,height];
        Initative = new UnitGameObject[UnitTree.TREESIZE * 2];
        int logPos = 0;
        int increment = Height / UnitTree.TREESIZE;
        int place = 0;
        livingAttackers = livingDefenders = 0;
        // Adds attacking units
        UnitTree units = attacker.Units;
        for (int i = 0; i < UnitTree.TREESIZE; i++)
        {
            if (units.GetUnits()[i] != null)
            {
                GameObject go = new GameObject("a" + i);
                //todo add sprite
                go.AddComponent<UnitGameObject>();
                UnitGameObject ugo = go.GetComponent<UnitGameObject>();
                ugo.UnitTree = units;
                ugo.PosInUnitTree = i;
                ugo.GraphicalBattlefield = this;
                ugo.AttackingSide = true;
                ugo.Initative = units.GetUnits()[i].Unitstats.Initative;
                ugo.LogicalPos = new Point(0, place);
                Initative[logPos++] = ugo;
                //set correct graphical pos
                gameObject.transform.position = new Vector2(0, place);
                UnitsOnField[0, place] = go;
                field[0, place].GetComponent<GroundGameObject>().IsOccupied = true;
                livingAttackers++;
            }
            place += increment;
        }

        //Adds defending units
        units = defender;
        place = 0;
        for (int i = 0; i < UnitTree.TREESIZE; i++)
        {
            if (units.GetUnits()[i] != null)
            {
                GameObject go = new GameObject("d" + i);
                //todo add sprite
                go.AddComponent<UnitGameObject>();
                UnitGameObject ugo = go.GetComponent<UnitGameObject>();
                ugo.UnitTree = units;
                ugo.PosInUnitTree = i;
                ugo.GraphicalBattlefield = this;
                ugo.AttackingSide = false;
                ugo.Initative = units.GetUnits()[i].Unitstats.Initative;
                ugo.LogicalPos = new Point(width-1, place);
                Initative[logPos++] = ugo;
                //set correct graphical pos
                gameObject.transform.position = new Vector2(Width - 1, place);
                UnitsOnField[Width-1, place] = go;
                field[width-1, place].GetComponent<GroundGameObject>().IsOccupied = true;
                livingDefenders++;
            }
            place += increment;
        }
        // Sorts initative in descending order
        Initative = Initative.OrderByDescending(UnitGameObject => UnitGameObject.Initative).ToArray();
        WhoseTurn = 0;
        initative[whoseTurn].ItsTurn = true;
    }

    /// <summary>
    /// Ends combat
    /// </summary>
    public void endCombat()
    {
        battleField.endCombat();
        InCombat = false;
    }

    /// <summary>
    /// Initiates attack on unit
    /// </summary>
    /// <param name="defender">Unit thats being attacked</param>
    /// <param name="goal">Place on field attacking unit is going to</param>
    public void attackUnit(UnitGameObject defender, Point goal)
    {
        UnitGameObject activeUnit = initative[whoseTurn];
        Unit attackingUnit = activeUnit.UnitTree.GetUnits()[activeUnit.PosInUnitTree];
        //If unit does not need to move, call on method for attacking without moving
        if (activeUnit.LogicalPos.Equals(goal))
        {
            battleField.attackWithoutMoving(activeUnit.LogicalPos, defender.LogicalPos, false);
            //todo trigger animation
            nextTurn();
        }
        else
        {
            //checks if unit is ranged, and if it has ammo. if not move and attack
            if (attackingUnit.IsRanged)
            {
                Ranged r = (Ranged)attackingUnit;
                if (r.Ammo > 0)
                {
                    battleField.attackWithoutMoving(activeUnit.LogicalPos, defender.LogicalPos, true);
                    //todo trigger animation
                    nextTurn();
                }
                else
                {
                    List<Vector2> path = battleField.UnitMoveAndAttack(activeUnit.LogicalPos, goal, defender.LogicalPos);
                    BeginWalking(path);
                    //todo trigger animation
                }
            }
            else
            {
                List<Vector2> path = battleField.UnitMoveAndAttack(activeUnit.LogicalPos, goal, defender.LogicalPos);
                BeginWalking(path);
                //todo trigger animation
            }
        }
        //Updates living units counts
        if (defender.UnitTree.getUnitAmount(defender.PosInUnitTree) == 0)
        {
            if (defender.AttackingSide) livingAttackers--;
            else livingDefenders--;
        }
        if (activeUnit.UnitTree.getUnitAmount(activeUnit.PosInUnitTree) == 0)
        {
            if (activeUnit.AttackingSide) livingAttackers--;
            else livingDefenders--;
        }
    }

    /// <summary>
    /// Moves unit logically by calling on unitmove and graphically by calling on beginwalking
    /// </summary>
    /// <param name="goal">Destination</param>
    public void moveUnit(Point goal)
    {
        UnitGameObject activeUnit = initative[whoseTurn];
        List<Vector2> path = battleField.unitMove(activeUnit.LogicalPos, goal);
        BeginWalking(path);
    }

    /// <summary>
    /// Begins walking animation
    /// </summary>
    /// <param name="path">Path</param>
    public void BeginWalking(List<Vector2> path)
    {
        //todo begin walking
    }

    /// <summary>
    /// Moves to next in initative
    /// </summary>
    public void nextTurn()
    {
        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                field[x, y].GetComponent<GroundGameObject>().Reachable = false;
                if (unitsOnField[x, y] != null) unitsOnField[x, y].GetComponent<UnitGameObject>().Attackable = false;
            }
        }
        initative[whoseTurn].ItsTurn = false;
        whoseTurn++;
        if (whoseTurn == initative.Length) whoseTurn = 0;
        initative[whoseTurn].ItsTurn = true;
        possibleMovement.flipReachable(Initative[whoseTurn].LogicalPos, Initative[whoseTurn].UnitTree.GetUnits()[Initative[whoseTurn].PosInUnitTree].Unitstats.Speed);
    }

    public UnitGameObject getUnitWhoseTurnItIs()
    {
        return Initative[WhoseTurn];
    }

    public UnitGameObject[] Initative
    {
        get
        {
            return initative;
        }

        set
        {
            initative = value;
        }
    }

    public int WhoseTurn
    {
        get
        {
            return whoseTurn;
        }

        set
        {
            whoseTurn = value;
        }
    }

    public BattleField BattleField
    {
        get
        {
            return battleField;
        }

        set
        {
            battleField = value;
        }
    }

    public int[,] Canwalk
    {
        get
        {
            return canwalk;
        }

        set
        {
            canwalk = value;
        }
    }

    public int Width
    {
        get
        {
            return width;
        }

        set
        {
            width = value;
        }
    }

    public int Height
    {
        get
        {
            return height;
        }

        set
        {
            height = value;
        }
    }

    public bool InCombat
    {
        get
        {
            return inCombat;
        }

        set
        {
            inCombat = value;
        }
    }

    public GameObject[,] Field
    {
        get
        {
            return field;
        }

        set
        {
            field = value;
        }
    }

    public GameObject[,] UnitsOnField
    {
        get
        {
            return unitsOnField;
        }

        set
        {
            unitsOnField = value;
        }
    }

    public bool IsWalking
    {
        get
        {
            return isWalking;
        }

        set
        {
            isWalking = value;
        }
    }
}
