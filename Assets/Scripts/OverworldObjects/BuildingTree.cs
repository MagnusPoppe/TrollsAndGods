using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Class for all the buildings in the town
/// </summary>
public class BuildingTree
{
    protected Building[] buildings;
    protected const int TOWNSIZE = 15;
    /// <summary>
    /// Constructor that fills the buildings array with test values
    /// </summary>
    /// <param name="townId">Which type of town</param>
    public BuildingTree(int townId)
    {
        buildings = new Building[TOWNSIZE];
        for(int i=0; i<TOWNSIZE; i++)
        {
            buildings[i] = new Building("newname" + i, new Sprite(), new Resources(500 * i, i, i, 0, 0, 0, 0));
            if (i > 0)
                buildings[i].AddParent(buildings[i-1]);
        }
    }

    public Building[] GetBuildings()
    {
        return buildings;
    }

    /// <summary>
    /// Inner building class that holds all the values of the building
    /// </summary>
    public class Building
    {
        protected string name;
        protected Sprite sprite;
        protected bool isBuilt;
        protected List<Building> parent;
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
            parent = new List<Building>();
        }

        /// <summary>
        /// Checks if a parent is not built
        /// </summary>
        /// <returns>true if no parent is needed</returns>
        public bool CanBuild()
        {
            foreach (Building b in parent)
            {
                if (!b.IsBuilt())
                    return false;
            }
            return true;
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
    }
}
