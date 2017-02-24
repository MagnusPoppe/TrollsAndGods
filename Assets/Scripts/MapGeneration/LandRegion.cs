using System.Collections;
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
        Hero hero;

        /// <summary>
        /// Defines a region by a set of coordinates and its center position.
        /// Initializes a new instance of the <see cref="T:MapGenerator.Region"/> class.
        /// </summary>
        /// <param name="coordinateList">Coordinate list.</param>
        /// <param name="regionCenter">Castle position.</param>
        public LandRegion(List<Vector2> coordinateList, Vector2 regionCenter, Player player) 
            :base(coordinateList, regionCenter)
        {
            castle = new UnknownCastle(regionCenter, player); // TODO: Set player dynamically 
            if(castle.Player != null)
            {
                // Add hero below castle
                Vector2 heroOrigo = new Vector2((int)castle.Origo.x, (int)castle.Origo.y+2);
                hero = new TestHero(player, heroOrigo);
                hero.Player.addHero(hero);
            }
            coordinates = coordinateList;
            buildings = new List<OverworldBuilding>();
        }

        public void PlaceHero(Vector2 position, Player player, int[,] map)
        {
            
            hero = new TestHero(player, position);
        
            hero.Player.addHero(hero);
            map[(int)position.x, (int)position.y] = hero.GetSpriteID();
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
        /// Gets the hero belonging to the castle.
        /// </summary>
        /// <returns>The hero.</returns>
        public Hero GetHero()
        {
            return hero;
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

        public Reaction[,] makeReactions(Reaction[,] reaction)
        {
            foreach(OverworldBuilding b in buildings)
            {
                reaction[(int)b.Origo.x, (int)b.Origo.y] = b.makeReaction();
            }
            // TODO same with pickups AND heroes
            return reaction;
        }


    }
}


