using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Building
{
    protected const int TOWNSIZE = 15;
    protected string name;
    protected bool isBuilt;
    protected bool[] requirements;
    private bool producesCreatures;
    private Unit producedUnit;
    private int amountOfCreatures;
    private int creaturesPerWeek;
    protected Resources cost;
    protected SpriteRenderer spr;

    public bool ProducesCreatures
    {
        get
        {
            return producesCreatures;
        }

        set
        {
            producesCreatures = value;
        }
    }

    public Unit ProducedUnit
    {
        get
        {
            return producedUnit;
        }

        set
        {
            producedUnit = value;
        }
    }

    public int AmountOfCreatures
    {
        get
        {
            return amountOfCreatures;
        }

        set
        {
            amountOfCreatures = value;
        }
    }

    public int CreaturesPerWeek
    {
        get
        {
            return creaturesPerWeek;
        }

        set
        {
            creaturesPerWeek = value;
        }
    }

    public Building()
    {
        requirements = new bool[TOWNSIZE];
        isBuilt = false;
        ProducesCreatures = false;
    }

    public bool IsBuilt()
    {
        return isBuilt;
    }

    public void populate(int extra)
    {
        AmountOfCreatures += CreaturesPerWeek + extra;
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
