using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Governs what happens when you interact with an artifact pickup on the map
/// </summary>
public class ArtifactReaction : Reaction {

    Item artifact;

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

    public ArtifactReaction(Item artifact, Vector2 pos)
    {
        Artifact = artifact;
        Pos = pos;
    }

    /// <summary>
    /// Adds artifact to hero inventory. Equips if room.
    /// </summary>
    /// <param name="h">Hero interacting with artifact</param>
    /// <returns>returns true to signal graphical change</returns>
    public override bool React(Hero h) 
    {
        if (h.EquippedItems[artifact.SlotType] == null)
        {
            h.EquippedItems[artifact.SlotType] = artifact;
        }
        else h.Items.Add(artifact);

        return true;
    }
}
