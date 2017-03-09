public abstract class Item
{
    int slotType;
    string description;

    public Item(int slotType, string description)
    {
        SlotType = slotType;
        Description = description;
    }

    public int SlotType
    {
        get
        {
            return slotType;
        }

        set
        {
            slotType = value;
        }
    }

    public string Description
    {
        get
        {
            return description;
        }

        set
        {
            description = value;
        }
    }

    public abstract bool effect();
}
