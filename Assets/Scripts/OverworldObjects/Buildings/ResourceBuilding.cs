using System;
using System.Collections.Generic;
using MapGenerator;
using UnityEngine;

namespace OverworldObjects
{
    public class ResourceBuilding : OverworldBuilding
    {


        Resources.type resourceID;
        private Earn earnings;

        private int minDistFromTown;
        private int maxDistFromTown;

        private Environment sceneryType;

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

        public Earn Earnings
        {
            get { return earnings; }
            set { earnings = value; }
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

        public override void Place(Region region, int[,] map, int[,] canWalk)
        {
            // Creating all datastructures:
            Point[] coordinates = region.GetCoordinatesArray();
            Block[] all = new Block[coordinates.Length];

            // Finding all suitable positions for this building.
            for (int i = 0; i < coordinates.Length; i++)
            {
                all[i] = new Block(
                    coordinates[i],
                    shapeType,
                    new int[] { MinDistFromTown, MaxDistFromTown}
                );
                float distanceFromTown = Vector2.Distance(Origo.ToVector2(), coordinates[i].ToVector2());

                // Checking if the distance from region center is good:
                if (MinDistFromTown < distanceFromTown && distanceFromTown < MaxDistFromTown)
                {
                    all[i].Rate.Distance(minDistFromTown, distanceFromTown, MaxDistFromTown);
                    // Checking if the building fits here:
                    if (Shapes.isSpecialShape(shapeType))
                    {
                        if (Shapes.FitSpecial(coordinates[i], GetSpriteID(), sceneryType.GetBelow(),
                            Shapes.GetShape(shapeType),map, canWalk))
                        {
                            all[i].Rate.Scenery(10);
                        }
                        else
                        {
                            all[i] = null;
                            continue;
                        }
                    }
                    else if (all[i].isSuitable(canWalk))
                    {
                        if (sceneryType.GetBelow().GetType() == typeof(Grass))
                        {
                            all[i].Rate.Scenery(10);
                        }
                        else if (Shapes.CanFitShapeOver(sceneryType, coordinates[i], Shapes.GetShape(shapeType), map))
                        {
                            all[i].Rate.Scenery(10);
                        }
                        else
                        {
                            all[i].Rate.Scenery(5);
                        }
                    }
                }
            }

            Array.Sort(all);

            Origo = all[all.Length - 1].Position;
        }

        public ResourceBuilding(int shape, Player owner, int spriteID, IngameObjectLibrary.Category spriteCategory, Environment sceneryType, Resources.type resourceID, Earn amountPerWeek, int minDistFromTown, int maxDistFromTown)
            : base(shape, owner, spriteID, spriteCategory)
        {
            MinDistFromTown = minDistFromTown;
            MaxDistFromTown = maxDistFromTown;
            ResourceID = resourceID;
            Earnings = amountPerWeek;
            this.sceneryType = sceneryType;
        }

        public override Reaction makeReaction()
        {
            Reaction = new ResourceBuildingReaction(this, Origo);
            return Reaction;
        }
    }
}