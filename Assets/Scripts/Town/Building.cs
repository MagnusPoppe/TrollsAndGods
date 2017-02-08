using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Building
{
    protected const int TOWNSIZE = 15;
    protected string name;
    protected bool isBuilt;
    protected bool[] requirements;
    protected Resources cost;
    protected SpriteRenderer spr;

    public Building()
    {
        requirements = new bool[TOWNSIZE];
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

    public bool[] GetRequirements()
    {
        return requirements;
    }

    public Resources GetCost()
    {
        return cost;
    }

    public string GetName()
    {
        return name;
    }

    public SpriteRenderer GetSpriteRenderer()
    {
        return spr;
    }
}
