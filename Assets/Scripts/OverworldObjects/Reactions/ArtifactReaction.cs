using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Governs what happens when you interact with an artifact pickup on the map
/// </summary>
public class ArtifactReaction : Reaction {

    Item artifact;
    UnitReaction unitReact;

    public Item Artifact
    {
        get
        {
            return artifact;
        }

        set
        {
            artifact = value;
        }
    }

    public UnitReaction UnitReact
    {
        get
        {
            return unitReact;
        }

        set
        {
            unitReact = value;
        }
    }

    public ArtifactReaction(Item artifact, Vector2 pos)
    {
        Artifact = artifact;
        Pos = pos;
    }

    /// <summary>
    /// Check's if there's a mob threatening the tile
    /// </summary>
    /// <returns>true if there's an reaction</returns>
    public override bool HasPreReact(Hero h)
    {
        return UnitReact != null;
    }

    /// <summary>
    /// If there's a mob threatening the tile, start their reaction
    /// </summary>
    /// <param name="h">Hero that initated the reaction</param>
    /// <returns>true if that hero won</returns>
    public override bool PreReact(Hero h)
    {
        return UnitReact.React(h);
    }

    /// <summary>
    /// Adds artifact to hero inventory. Equips if room.
    /// </summary>
    /// <param name="h">Hero interacting with artifact</param>
    /// <returns>returns true to signal graphical change, artifact always picked up</returns>
    public override bool React(Hero h) 
    {
        if (h.EquippedItems[artifact.SlotType] == null)
        {
            h.EquippedItems[artifact.SlotType] = artifact;
        }
        else h.Items.Add(artifact);
        // Artifact picked up, returns true
        return true;
    }
}
