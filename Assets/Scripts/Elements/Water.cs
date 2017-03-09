using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Water : Element {

	public Water()
    {
        resistances[WATER] = MIN;
        element = WATER;
    }
}
