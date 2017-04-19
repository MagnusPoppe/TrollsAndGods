using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Represents a hex tile on battlefield
/// </summary>
public class GroundGameObject : MonoBehaviour {

    bool isOccupied, reachable;
    Point logicalPos;
    GraphicalBattlefield graphicalBattlefield;

    private const String SPRITE_PATH_REACHABLE = "Sprites/Combat/HexagonTrimmedReachable";
    private const String SPRITE_PATH_ATTACKABLE = "Sprites/Combat/HexagonTrimmedAttackable";
    private const String SPRITE_PATH_DEFAULT   = "Sprites/Combat/HexagonTrimmed";

    // Use this for initialization
    void Awake () {
        isOccupied = reachable = false;
	}
	
	void OnMouseOver()
    {
        //todo highlight if you can walk
    }

    void OnMouseDown()
    {
        //moves unit if space is not occupied
        if (!isOccupied && reachable)
        {
            graphicalBattlefield.moveUnit(logicalPos);
        }
    }

    public void MarkReachable( bool attackable )
    {
        SpriteRenderer sr = GetComponent<SpriteRenderer>();
        if (attackable)
            sr.sprite = UnityEngine.Resources.Load<Sprite>(SPRITE_PATH_ATTACKABLE);
        else if (Reachable)
            sr.sprite = UnityEngine.Resources.Load<Sprite>(SPRITE_PATH_REACHABLE);
        else
            sr.sprite = UnityEngine.Resources.Load<Sprite>(SPRITE_PATH_DEFAULT);
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
