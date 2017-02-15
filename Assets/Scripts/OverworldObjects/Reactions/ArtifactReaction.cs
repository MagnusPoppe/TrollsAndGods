using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
