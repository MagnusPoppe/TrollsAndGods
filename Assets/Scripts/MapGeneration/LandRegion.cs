using System.Collections.Generic;
using System.Diagnostics;
using OverworldObjects;
using UnityEngine;
using Debug = UnityEngine.Debug;

namespace MapGenerator
{
    public class LandRegion : Region
    {

        List<OverworldBuilding> buildings;

        Economy economy;

        TileRating[] coordinateValue;
        Castle castle;
        Hero hero;

        /// <summary>
        /// Defines a region by a set of coordinates and its center position.
        /// Initializes a new instance of the <see cref="T:MapGenerator.Region"/> class.
        /// </summary>
        /// <param name="coordinateList">Coordinate list.</param>
        /// <param name="regionCenter">Castle position.</param>
        public LandRegion(List<Point> coordinateList, Point regionCenter) 
            :base(coordinateList, regionCenter)
        {
            // TODO: alt skal ikke være viking town
            castle = new VikingCastle(regionCenter, null);
            coordinates = coordinateList;
            buildings = new List<OverworldBuilding>();
        }

        public void PlaceCastle(int[,] map, int[,] canWalk)
        {
            castle.FlipCanWalk(canWalk);
            map[RegionCenter.x, RegionCenter.y] = castle.GetSpriteID();
        }


        /// <summary>
        /// Sets all ground tiles to a given type of tile.
        /// Definition of groundtile is found in MapMaker.GROUND
        /// </summary>
        /// <returns>The region ground tile type.</returns>
        /// <param name="groundTile">Ground tile.</param>
        /// <param name="map">Map.</param>
        public int[,] SetRegionGroundTileType(int[,] map)
        {
            foreach (Point v in coordinates)
            {
                if (map[v.x, v.y] == MapMaker.GROUND)
                    map[v.x, v.y] = castle.Environment.GetSpriteID();
            }
            return map;
        }

        /// <summary>
        /// Gets the castle belonging to the region.
        /// </summary>
        /// <returns>The castle.</returns>
        public Castle GetCastle()
        {
            return castle;
        }

        public void SetCastle(Castle castle)
        {
            this.castle = castle;
        }


        /// <summary>
        /// Gets the hero belonging to the castle.
        /// </summary>
        /// <returns>The hero.</returns>
        public Hero GetHero()
        {
            return hero;
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
        public bool IsPositionInRegion(Point Position)
        {
            foreach (Point coordinate in coordinates)
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
							break;
                        }
                        // TODO: CANT PLACE.
                    }
                }
            }
        }

        public Reaction[,] makeReactions(Reaction[,] reaction)
        {
            castle.flipReactions(reaction, hero);
            foreach(OverworldBuilding b in buildings)
            {
                reaction[(int)b.Origo.x, (int)b.Origo.y] = b.makeReaction();
            }
            // TODO same with pickups AND heroes
            return reaction;
        }


    }
}


