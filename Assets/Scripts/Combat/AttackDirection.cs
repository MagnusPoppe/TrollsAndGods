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
	            direction = 0;
                break;
            case "East":
                direction = 1;
                break;
            case "SouthEast":
                direction = 2;
                break;
            case "SouthWest":
                direction = 3;
                break;
            case "West":
                direction = 4;
                break;
            case "NorthWest":
                direction = 5;
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
        Debug.Log("Cliked on");
    }
}
