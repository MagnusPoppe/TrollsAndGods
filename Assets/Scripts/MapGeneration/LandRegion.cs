﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using OverworldObjects;

namespace MapGenerator
{
    public class LandRegion : Region
    {

        List<OverworldBuilding> buildings;

        Economy economy;

        TileRating[] coordinateValue;
        Castle castle;

        /// <summary>
        /// Defines a region by a set of coordinates and its center position.
        /// Initializes a new instance of the <see cref="T:MapGenerator.Region"/> class.
        /// </summary>
        /// <param name="coordinateList">Coordinate list.</param>
        /// <param name="regionCenter">Castle position.</param>
        public LandRegion(List<Vector2> coordinateList, Vector2 regionCenter) 
            :base(coordinateList, regionCenter)
        {
            castle = new UnknownCastle(regionCenter, null); // TODO: Set player dynamically 
            coordinates = coordinateList;
            buildings = new List<OverworldBuilding>();
        }

       
        /// <summary>
        /// Gets the castle belonging to the region.
        /// </summary>
        /// <returns>The castle.</returns>
        public Castle GetCastle()
        {
            return castle;
        }

        /// <summary>
        /// Sets the type of the castle for this region.
        /// </summary>
        /// <param name="id">Identifier.</param>
        public void SetCastleType(int id)
        {
            castle.SetEnvironment(id);
        }

        /// <returns>Buildings list.</returns>
		public List<OverworldBuilding> GetBuildings()
        {
            return buildings;
        }

        /// <summary>
        /// Creates the entire region economy by placing the 
        /// given resource buildings.
        /// </summary>
        /// <param name="canWalk"></param>
        /// <param name="economy"></param>
		public void createEconomy(int[,] canWalk, Economy economy)
        {
            this.economy = economy;
            for (int i = 0; i < economy.oreMineCount; i++)
            {
                OreMine mine = new OreMine(null);
                PlaceResourceBuilding(mine, canWalk);
            }

            // TODO: COPY FOR ALL OTHER MINETYPES.
        }

        /// <summary>
        /// Checks if the position is part of this region.
        /// </summary>
        /// <returns><c>true</c>, if position in region, <c>false</c> otherwise.</returns>
        /// <param name="Position">Position.</param>
        public bool IsPositionInRegion(Vector2 Position)
        {
            foreach (Vector2 coordinate in coordinates)
            {
                if (coordinate.Equals(Position))
                    return true;
            }
            return false;
        }

        /// <summary>
        /// TODO! 
        /// SHOULD GIVE EACH TILE IN REGION A NEW RATING.
        /// </summary>
        /// <param name="canWalk"></param>
        /// <returns></returns>
		private Block[] RateRegionTiles(int[,] canWalk)
        {
            Block[] blocks = new Block[GetArea()];
            for (int i = 0; i < GetArea(); i++)
            {
                blocks[i] = new Block(GetCastle().GetPosition(), coordinates[i], canWalk);
            }
            return blocks;
        }

        /// <summary>
        /// Places a resource building inside the region.
        /// </summary>
        /// <param name="building"></param>
        /// <param name="canWalk"></param>
		public void PlaceResourceBuilding(ResourceBuilding building, int[,] canWalk)
        {
            Block[] blocks = RateRegionTiles(canWalk);

            // DEFINERER BYGGNINGSTYPEN TIL HVER TILE:
            for (int i = 0; i < economy.woodMineCount; i++)
            {
                float minDistance = building.MinDistFromTown;
                float maxDistance = building.MaxDistFromTown;

                for (int j = 0; j < GetArea(); j++)
                {
                    float distance = blocks[j].GetDistanceFromCastle();

                    if (blocks[j].CanPlaceBuilding(building.ShapeType))
                    {
                        if (distance >= minDistance && distance <= maxDistance)
                        {
                            // KLAR TIL Å PLASSERE
                            building.Origo = (blocks[j].GetPosition());
                            building.FlipCanWalk(canWalk);

                            // Plasserer bygning.
                            buildings.Add(building);
                        }
                        // TODO: CANT PLACE.
                    }
                }
            }
        }


    }
}

