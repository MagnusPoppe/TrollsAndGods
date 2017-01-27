using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hero : MonoBehaviour
{
    Sprite portrait;
    string name;
    int faction;
    Unit[] units;
    List<Item> items;
    List<Item> equippedItems;

    public Hero()
    {
        units = new Unit[7];
        items = new List<Item>();
        equippedItems = new List<Item>();
    }
}
