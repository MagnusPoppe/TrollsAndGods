﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// parent class for Reactions, wich governs what happens when objects are triggered
/// </summary>
abstract public class Reaction {

    Vector2 pos;
    Reaction preReaction;

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

    public Reaction PreReaction
    {
        get
        {
            return preReaction;
        }

        set
        {
            preReaction = value;
        }
    }

    /// <summary>
    /// Initiates the additional unit or hero reaction
    /// </summary>
    /// <param name="h">Hero that triggers it</param>
    /// <returns>result of preReacts react</returns>
    public bool PreReact(Hero h) {
        return (preReaction.React(h));
    }

    /// <summary>
    /// abstract method to handle what happens when you interact with object
    /// </summary>
    /// <param name="h">The hero that is interacting with the object</param>
    /// <returns>returns true if graphic change, false if not</returns>
    abstract public bool React(Hero h);

    /// <summary>
    /// Checks if there's a reaction that needs to trigger before the normal reaction
    /// </summary>
    /// <returns>true if there is an additional reaction there</returns>
    public bool HasPreReact(Hero h)
    {
        return (preReaction != null);
    }

}
