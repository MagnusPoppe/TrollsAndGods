using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TownView;

public class DwellingOnClick : MonoBehaviour {

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
        OpenWindow(Building);
    }

    void OpenWindow(Building b)
    {

    }
}
