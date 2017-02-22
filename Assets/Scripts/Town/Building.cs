using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TownView
{


    public class Building : SpriteSystem
    {

        private float scale;
        private string name;
        private bool built;
        protected bool[] requirements;
        protected Resources cost;
        private Vector2 placement;
        private const IngameObjectLibrary.Category CATEGORY = IngameObjectLibrary.Category.Town; // TODO: ny kategori for town buildings

        public bool Built
        {
            get
            {
                return built;
            }
        }

        public Vector2 Placement
        {
            get
            {
                return placement;
            }

            set
            {
                placement = value;
            }
        }

        public string Name
        {
            get
            {
                return name;
            }

            set
            {
                name = value;
            }
        }

        public float Scale
        {
            get
            {
                return scale;
            }
            set
            {
                scale = value;
            }
        }

        public void Build()
        {
            // TODO: implementer metoden med cost
            built = true;
        }

        public Building(string name, bool[] requirements, Resources cost, int localID, float scale) :base(localID, CATEGORY)
        {
            Name = name;
            this.requirements = requirements;
            Placement = placement;
            Scale = scale;
            this.cost = cost;
        }
    }
}