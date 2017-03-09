using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GraphicalBattlefield : MonoBehaviour {

    BattleField battleField;
    int[,] canwalk;
    int width, height;
    bool inCombat;
    GameObject[,] field;
    GameObject[,] units;
    bool isWalking;
    UnitGameObject[] initative;
    int whoseTurn;

    // Use this for initialization
    void Start () {
        inCombat = false;
        isWalking = false;
	}
	
	// Update is called once per frame
	void Update () {
		if (inCombat)
        {
            if (isWalking)
            {
                //Todo keep moving
            }
        }
	}

    public void beginCombat(int width, int height)
    {
        //Todo ready and show combat
    }

    public void BeginWalking(Vector2 path)
    {
        //todo begin walking
    }

    public UnitGameObject getUnitWhoseTurnItIs()
    {
        return initative[whoseTurn];
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
}
