using System.Collections;
using System.Collections.Generic;
using UnityEngine;

abstract public class Reaction {

    Vector2 pos;
    GameObject self;
    Reaction[,] reactionTab;

    public Vector2 Pos
    {
        get
        {
            return pos;
        }

        set
        {
            pos = value;
        }
    }

    public GameObject Self
    {
        get
        {
            return self;
        }

        set
        {
            self = value;
        }
    }

    public Reaction[,] ReactionTab
    {
        get
        {
            return reactionTab;
        }

        set
        {
            reactionTab = value;
        }
    }

    abstract public bool React(Hero h);
}
