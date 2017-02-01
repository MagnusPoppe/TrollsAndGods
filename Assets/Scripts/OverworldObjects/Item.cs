using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item
{
    int slotType;

    public Item(int slotType)
    {
        this.slotType = slotType;
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
}
