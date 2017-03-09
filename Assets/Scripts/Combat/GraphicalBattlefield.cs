using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class GraphicalBattlefield : MonoBehaviour {

    BattleField battleField;
    int[,] canwalk;
    int width, height;
    bool inCombat;
    GameObject[,] field;
    GameObject[,] unitsOnField;
    bool isWalking;
    UnitGameObject[] initative;
    int whoseTurn;
    int livingAttackers, livingDefenders;

    // Use this for initialization
    void Start () {
        InCombat = false;
        IsWalking = false;
	}
	
	// Update is called once per frame
	void Update () {
		if (InCombat)
        {
            if (IsWalking)
            {
                //Todo keep moving
            }
        }
	}

    public void beginCombat(int width, int height, Hero attacker, Hero defender)
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

        populateInitative(attacker, defender.Units);
    }

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

        populateInitative(attacker, defender);
    }

    public void populateInitative(Hero attacker, UnitTree defender)
    {
        Initative = new UnitGameObject[UnitTree.TREESIZE * 2];
        int logPos = 0;
        int increment = Height / UnitTree.TREESIZE;
        int place = 0;
        livingAttackers = livingDefenders = 0;
        UnitTree units = attacker.Units;
        for (int i = 0; i < UnitTree.TREESIZE; i++)
        {
            if (units.GetUnits()[i] != null)
            {
                GameObject go = new GameObject("a" + i);
                go.AddComponent<UnitGameObject>();
                UnitGameObject ugo = go.GetComponent<UnitGameObject>();
                ugo.UnitTree = units;
                ugo.PosInUnitTree = i;
                ugo.GraphicalBattlefield = this;
                ugo.AttackingSide = true;
                ugo.Initative = units.GetUnits()[i].Unitstats.Initative;
                Initative[logPos++] = ugo;
                gameObject.transform.position = new Vector2(0, place);
                UnitsOnField[0, place] = go;
                livingAttackers++;
            }
            place += increment;
        }

        units = defender;
        place = 0;
        for (int i = 0; i < UnitTree.TREESIZE; i++)
        {
            if (units.GetUnits()[i] != null)
            {
                GameObject go = new GameObject("d" + i);
                go.AddComponent<UnitGameObject>();
                UnitGameObject ugo = go.GetComponent<UnitGameObject>();
                ugo.UnitTree = units;
                ugo.PosInUnitTree = i;
                ugo.GraphicalBattlefield = this;
                ugo.AttackingSide = false;
                ugo.Initative = units.GetUnits()[i].Unitstats.Initative;
                Initative[logPos++] = ugo;
                gameObject.transform.position = new Vector2(Width - 1, place);
                UnitsOnField[Width-1, place] = go;
                livingDefenders++;
            }
            place += increment;
        }

        Initative = Initative.OrderByDescending(UnitGameObject => UnitGameObject.Initative).ToArray();
        WhoseTurn = 0;
        initative[whoseTurn].ItsTurn = true;
    }

    public void endCombat()
    {
        battleField.endCombat();
        InCombat = false;
    }

    public void attackUnit(UnitGameObject defender, Point goal)
    {
        UnitGameObject activeUnit = initative[whoseTurn];
        Unit attackingUnit = activeUnit.UnitTree.GetUnits()[activeUnit.PosInUnitTree];
        if (activeUnit.transform.position.Equals(goal))
        {
            battleField.attackWithoutMoving(activeUnit.LogicalPos, defender.LogicalPos, false);
            //todo trigger death animation if death occured
        }
        else
        {
            if (attackingUnit.IsRanged)
            {
                battleField.attackWithoutMoving(activeUnit.LogicalPos, defender.LogicalPos, true);
                //todo trigger death animation if death occured
            }
            else
            {
                battleField.UnitMoveAndAttack(activeUnit.LogicalPos, goal, defender.LogicalPos);
            }
        }
    }

    public void BeginWalking(Vector2 path)
    {
        //todo begin walking
    }

    public UnitGameObject getUnitWhoseTurnItIs()
    {
        return Initative[WhoseTurn];
    }

    public UnitGameObject[] Initative
    {
        get
        {
            return Initative;
        }

        set
        {
            Initative = value;
        }
    }

    public int WhoseTurn
    {
        get
        {
            return WhoseTurn;
        }

        set
        {
            WhoseTurn = value;
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
