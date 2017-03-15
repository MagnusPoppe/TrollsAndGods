using UnityEngine;

public class UnitGameObject : MonoBehaviour {

    bool itsTurn, attackingSide, attackable;
    int initative;
    UnitTree unitTree;
    int posInUnitTree;
    GraphicalBattlefield graphicalBattlefield;
    Point logicalPos;

    // Use this for initialization
    void Start () {
        AttackingSide = ItsTurn = attackable = false;
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
            //todo defend
        }
        else
        {
            //todo determine direction
            Point goal = logicalPos;
            goal.x -= 1;
            if (goal.x < 0) goal.x += 2;
            graphicalBattlefield.attackUnit(this,goal);
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

    public bool Attackable
    {
        get { return attackable; }
        set { attackable = value; }
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
