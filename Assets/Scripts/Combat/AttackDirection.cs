using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackDirection : MonoBehaviour
{
    private int direction;
    private UnitGameObject unitGameObject;

	// Use this for initialization
	void Start () {
	    switch (name)
	    {
            case "NorthEast":
	            direction = 1;
                break;
            case "East":
                direction = 3;
                break;
            case "SouthEast":
                direction = 5;
                break;
            case "SouthWest":
                direction = 4;
                break;
            case "West":
                direction = 2;
                break;
            case "NorthWest":
                direction = 0;
                break;
        }
	    unitGameObject = GetComponentInParent<UnitGameObject>();
	}

    private void OnMouseOver()
    {
        unitGameObject.MouseOver(direction);
    }

    private void OnMouseDown()
    {
        unitGameObject.MouseDown(direction);
        Debug.Log(name);
    }
}
