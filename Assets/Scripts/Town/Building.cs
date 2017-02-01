using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Building
{
    protected string name;
    protected Sprite sprite;
    protected bool isBuilt;
    protected bool[] requirements;
    protected Resources cost;

    /// <summary>
    /// Constructor, sets isbuilt to false upon creation and prepares a list of 
    /// building parents that must be already built to built it
    /// </summary>
    /// <param name="name">Name of building</param>
    /// <param name="sprite">The image it contains</param>
    /// <param name="cost">What it costs to build it</param>
    public Building(string name, Sprite sprite, Resources cost)
    {
        this.name = name;
        this.sprite = sprite;
        this.cost = cost;
        isBuilt = false;
    }

    public bool IsBuilt()
    {
        return isBuilt;
    }

    public void SetBuilt(bool isBuilt)
    {
        this.isBuilt = isBuilt;
    }

    public Resources GetCost()
    {
        return cost;
    }


    override public string ToString()
    {
        return name;
    }

    public Sprite GetSprite()
    {
        return sprite;
    }

    public bool[] GetRequirements()
    {
        return requirements;
    }
}
