using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitGameObject : MonoBehaviour {

    bool itsTurn, attackingSide;
    int initative;
    UnitTree unitTree;
    int posInUnitTree;
    GraphicalBattlefield graphicalBattlefield;
    Point logicalPos;

    // Use this for initialization
    void Start () {
        AttackingSide = ItsTurn = false;
	}
	
	void OnMouseOver()
    {
        if (ItsTurn)
        {
            //Todo change cursor to defend
        }
        else if (AttackingSide != GraphicalBattlefield.getUnitWhoseTurnItIs().AttackingSide)
        {
            //todo determine wich direction attack is coming from
        }

    }

    void onMouseDown()
    {
        if (ItsTurn)
        {
            //defend
        }
    }

    public bool ItsTurn
    {
        get
        {
            return itsTurn;
        }

        set
        {
            itsTurn = value;
        }
    }

    public bool AttackingSide
    {
        get
        {
            return attackingSide;
        }

        set
        {
            attackingSide = value;
        }
    }

    public UnitTree UnitTree
    {
        get
        {
            return unitTree;
        }

        set
        {
            unitTree = value;
        }
    }

    public int PosInUnitTree
    {
        get
        {
            return posInUnitTree;
        }

        set
        {
            posInUnitTree = value;
        }
    }

    public int Initative
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

    public GraphicalBattlefield GraphicalBattlefield
    {
        get
        {
            return graphicalBattlefield;
        }

        set
        {
            graphicalBattlefield = value;
        }
    }

    public Point LogicalPos
    {
        get
        {
            return logicalPos;
        }

        set
        {
            logicalPos = value;
        }
    }
}
