using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GroundGameObject : MonoBehaviour {

    bool isOccupied, reachable;
    Point logicalPos;
    GraphicalBattlefield graphicalBattlefield;

    // Use this for initialization
    void Start () {
        isOccupied = reachable = false;
	}
	
	void onMouseOver()
    {
        //todo highlight if you can walk
    }

    void onMouseDown()
    {
        //moves unit if space is not occupied
        if (!isOccupied)
        {
            graphicalBattlefield.moveUnit(logicalPos);
        }
    }

    public bool IsOccupied
    {
        get
        {
            return isOccupied;
        }

        set
        {
            isOccupied = value;
        }
    }

    public bool Reachable
    {
        get { return reachable; }
        set { reachable = value; }
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
}
