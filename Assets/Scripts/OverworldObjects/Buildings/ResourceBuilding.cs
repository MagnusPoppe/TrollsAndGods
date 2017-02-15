using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace OverworldObjects
{
    public class ResourceBuilding : OverworldBuilding
    {


        Resources.type resourceID;
        private int minDistFromTown;
        private int maxDistFromTown;

        public Resources.type ResourceID
        {
            get
            {
                return resourceID;
            }

            set
            {
                resourceID = value;
            }
        }

        public int MinDistFromTown
        {
            get
            {
                return minDistFromTown;
            }

            set
            {
                minDistFromTown = value;
            }
        }

        public int MaxDistFromTown
        {
            get
            {
                return maxDistFromTown;
            }

            set
            {
                maxDistFromTown = value;
            }
        }

        public ResourceBuilding(int shape, int owner, int spriteID, Resources.type resourceID, int minDistFromTown, int maxDistFromTown)
            : base(shape, owner, spriteID)
        {
            MinDistFromTown = minDistFromTown;
            MaxDistFromTown = maxDistFromTown;
            ResourceID = resourceID;
        }

        public override void makeReaction(int x, int y)
        {
            Reaction = new ResourceBuildingReaction(this, new Vector2(x, y));
        }
    }
}