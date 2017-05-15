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

    private Sprite reachableSprite, attackableSprite, defaultSprite;

    // Use this for initialization
    void Awake () {
        isOccupied = reachable = false;
        reachableSprite = UnityEngine.Resources.Load<Sprite>(SPRITE_PATH_REACHABLE);
        attackableSprite = UnityEngine.Resources.Load<Sprite>(SPRITE_PATH_ATTACKABLE);
        defaultSprite = UnityEngine.Resources.Load<Sprite>(SPRITE_PATH_DEFAULT);
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

    /// <summary>
    /// Method changes graphic to indicate if hexes are attackable, reachable or neither.
    /// </summary>
    /// <param name="attackable">If you can attack this hex</param>
    public void MarkReachable( bool attackable )
    {
        SpriteRenderer sr = GetComponent<SpriteRenderer>();
        if (attackable)
            sr.sprite = attackableSprite;
        else if (Reachable)
            sr.sprite = reachableSprite;
        else
            sr.sprite = defaultSprite;
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
