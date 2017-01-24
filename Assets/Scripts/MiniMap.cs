using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MiniMap : MonoBehaviour {

	// Use this for initialization
	void Start () 
    {
        GameObject map = GameObject.Find("useVoronoi");

        Vector2 pos = new Vector2(map.transform.position.x/2, map.transform.position.y/2) ;
        this.transform.position = pos;

        Camera cam = this.GetComponent<Camera>();
        cam.orthographicSize = map.transform.position.x / 2;
	}
	
	// Update is called once per frame
	void Update () 
    {
		
	}
}
