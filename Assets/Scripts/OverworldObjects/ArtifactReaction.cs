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

    public ArtifactReaction(Item artifact, Vector2 pos, GameObject self, Reaction[,] reactionTab)
    {
        Artifact = artifact;
        Pos = pos;
        Self = self;
        ReactionTab = reactionTab;
    }

    public override bool React(Hero h) 
    {
        if (h.EquippedItems[artifact.SlotType] == null)
        {
            h.EquippedItems[artifact.SlotType] = artifact;
        }
        else h.Items.Add(artifact);

        Self.SetActive(false);
        GameObject.Destroy(Self);

        ReactionTab[(int)Pos.x, (int)Pos.y] = null;

        return true;
    }
}
