using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Piercing : Element {

	public Piercing()
    {
        resistances[PIERCING] = MIN;
        element = PIERCING;
    }
}
