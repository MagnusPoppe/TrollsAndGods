using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TownView;

public class BuildingOnClick : MonoBehaviour {

    Building building;

    public Building Building
    {
        get
        {
            return building;
        }

        set
        {
            building = value;
        }
    }

    void OnMouseDown()
    {
        Debug.Log("Du har klikket på " + building.Name);

        OpenWindow(Building);
    }

    void OpenWindow(Building b)
    {
        //b-
    }
}
