using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TownView
{


    public class Building
    {

        protected string name;
        private bool built;
        protected bool[] requirements;
        protected Resources cost;
        private Vector2 placement;

        protected bool Built
        {
            get
            {
                return built;
            }

            set
            {
                built = value;
            }
        }

        protected Vector2 Placement
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

        public void Build()
        {
            // TODO: implementer metoden med cost
            Built = true;
        }

        public Building(string name, bool[] requirements, Resources cost)
        {

        }
    }
}