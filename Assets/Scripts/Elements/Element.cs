using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Element {

    public static readonly int MIN = 0;
    public static readonly int NEUTRAL = 1;
    public readonly int MAX = 2;
    public static readonly int NRELEMENTS = 7;

    public static readonly int FIRE = 0;
    public static readonly int WATER = 1;
    public static readonly int AIR = 2;
    public static readonly int EARTH = 3;
    public static readonly int PIERCING = 4;
    public static readonly int BLUDGEONING = 5;
    public static readonly int SLASH = 6;
    
    protected int[] resistances = new int[NRELEMENTS];

    public Element()
    {
        for (int i = 0; i < NRELEMENTS; i++) resistances[i] = NEUTRAL;
    }

    public int getElementResistance (int index)
    {
        return resistances[index];
    }
    
}
