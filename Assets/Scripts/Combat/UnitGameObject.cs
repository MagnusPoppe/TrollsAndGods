using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitGameObject : MonoBehaviour {

    bool isActive, attackingSide;
    GraphicalBattlefield graphicalBattlefield;

    // Use this for initialization
    void Start () {
        AttackingSide = IsActive = false;
	}
	
	void OnMouseOver()
    {
        if (IsActive)
        {
            //Todo change cursor to defend
        }
        else if (AttackingSide != graphicalBattlefield.getUnitWhoseTurnItIs().AttackingSide)
        {
            //todo determine wich direction attack is coming from
        }

    }

    public bool IsActive
    {
        get
        {
            return isActive;
        }

        set
        {
            isActive = value;
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
}
