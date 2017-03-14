/// <summary>
/// governs what happens when interacting with a resource pickup
/// </summary>
public class ResourceReaction : Reaction
{

    Resources.type resourceID;
    int amount;

    public Resources.type ResourceID
    {
        get
        {
            return resourceID;
        }

        set
        {
            resourceID = value;
        }
    }

    public int Amount
    {
        get
        {
            return amount;
        }

        set
        {
            amount = value;
        }
    }

    public ResourceReaction(Resources.type resID, int amount, Point pos)
    {
        ResourceID = resID;
        Amount = amount;
        Pos = pos;
    }

    /// <summary>
    /// Adds resource amount to player
    /// </summary>
    /// <param name="h">Hero interacting with resource</param>
    /// <returns>returns true to signal graphical change, true since resource is always picked up</returns>
    public override bool React(Hero h)
    {
        h.Player.Wallet.adjustResource(resourceID, amount);
        // Resource picked up, returned true
        return true;
    }
}
