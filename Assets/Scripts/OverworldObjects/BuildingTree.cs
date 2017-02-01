using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildingTree
{
    protected Building[] buildings;
    protected const int TOWNSIZE = 15;
    public BuildingTree(int townId)
    {
        buildings = new Building[TOWNSIZE];

        for(int i=0; i<TOWNSIZE; i++)
        {
            buildings[i] = new Building("newname", new Sprite(), new Resources(5000, 10, 10, 0, 0, 0, 0));
            if (i > 0)
                buildings[i].AddParent(buildings[i-1]);
        }
    }

    public Building[] GetBuildings()
    {
        return buildings;
    }

    public class Building
    {
        protected string name;
        protected Sprite sprite;
        protected bool isBuilt;
        protected List<Building> parent;
        protected Resources cost;
        public Building(string name, Sprite sprite, Resources cost)
        {
            this.name = name;
            this.sprite = sprite;
            this.cost = cost;
            isBuilt = false;
            parent = null;
            parent = new List<Building>();
        }

        public bool IsBuilt()
        {
            return isBuilt;
        }

        public void SetBuilt(bool isBuilt)
        {
            this.isBuilt = isBuilt;
        }

        public void AddParent(Building parent)
        {
            this.parent.Add(parent);
        }

        public bool CanBuild()
        {
            foreach(Building b in parent)
            {
                if (!b.IsBuilt())
                    return false;
            }
            return true;
        }

        public Resources GetCost()
        {
            return cost;
        }
    }
}
