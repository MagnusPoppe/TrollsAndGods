using System.Collections;
using System.Collections.Generic;
using UnityEngine;

abstract public class Reaction {

    Vector2 pos;
    GameObject self;
    Reaction[,] reactionTab;

	abstract public bool React(Hero h);
}
