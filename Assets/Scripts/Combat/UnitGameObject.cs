using UnityEngine;

/// <summary>
/// Represents a unit on battlefield
/// </summary>
public class UnitGameObject : MonoBehaviour
{

    bool itsTurn, attackingSide, attackable;
    int initative;
    UnitTree unitTree;
    int posInUnitTree;
    GraphicalBattlefield graphicalBattlefield;
    Point logicalPos;

    private readonly Point[] HEXDIRSEVEN =
    {
        new Point(-1,-1), new Point(0,-1),
        new Point(-1,0), new Point(1,0),
        new Point(-1,1), new Point(0,1)      
    };

    private readonly Point[] HEXDIRSODD =
    {
        new Point(0,-1), new Point(1,-1),
        new Point(-1,0), new Point(1,0),
        new Point(0,1), new Point(1,1)
    };

    // Use this for initialization
    void Awake () {
        AttackingSide = ItsTurn = attackable = false;
	}
	
	public void MouseOver(int direction)
    {
        if (ItsTurn)
        {
            //Todo change cursor to defend
        }
        else if (AttackingSide != GraphicalBattlefield.getUnitWhoseTurnItIs().AttackingSide && attackable)
        {
            //todo show wich side your attacking from visually
        }

    }

    public void MouseDown(int direction)
    {
        if (ItsTurn)
        {
            //todo defend
        }
        else if(Attackable)
        {
            Point destination;
            if (logicalPos.y % 2 == 0)
            {
                destination = logicalPos.addition(HEXDIRSEVEN[direction]);
            }
            else
            {
                destination = logicalPos.addition(HEXDIRSODD[direction]);
            }
            graphicalBattlefield.attackUnit(this, destination);
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
