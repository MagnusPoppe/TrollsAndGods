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

        public ResourceBuilding(int shape, Player owner, int spriteID, IngameObjectLibrary.Category spriteCategory, Resources.type resourceID, int minDistFromTown, int maxDistFromTown)
            : base(shape, owner, spriteID, spriteCategory)
        {
            MinDistFromTown = minDistFromTown;
            MaxDistFromTown = maxDistFromTown;
            ResourceID = resourceID;
        }

        public override Reaction makeReaction()
        {
            Reaction = new ResourceBuildingReaction(this, Origo);
            return Reaction;
        }
    }
}