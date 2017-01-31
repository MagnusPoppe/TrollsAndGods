using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Town
{
    bool canPurchase;

    public Town()
    {
        CanPurchase = true;
    }

    public bool CanPurchase
    {
        get
        {
            return canPurchase;
        }

        set
        {
            canPurchase = value;
        }
    }
}
