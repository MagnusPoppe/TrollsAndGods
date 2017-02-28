using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildingOnClick : MonoBehaviour {

	void OnMouseDown()
    {
        Debug.Log("Du har klikket på " + gameObject.name);
    }
}
