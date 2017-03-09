
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

    public ArtifactReaction(Item artifact, Point pos)
    {
        Artifact = artifact;
        Pos = pos;
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
