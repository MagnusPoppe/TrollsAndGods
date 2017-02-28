using UnityEngine;
using OverworldObjects;
using Filter;
using System.Collections.Generic;

namespace MapGenerator
{
    public class MapMaker
    {
		// Mathematical game objects
		public int width, height;
        string seed;

		// Information about the map:
		Region[] regions;
		int[,] map;
		int[,] canWalk;
        int[,] initialMap;

        // Overworld objects
        Vector2[] regionCenterPoints;

		// Constants
		public const int  GROUND 					= 0;
		public const int  WALL 						= 1;
		public const int  REGION_CENTER 			= 2;
		public const bool KEEP_VORONOI_REGION_LINES = false;

		// Base sprites
        public const int GRASS_SPRITEID =   IngameObjectLibrary.GROUND_START + 0;
        public const int WATER_SPRITEID =   IngameObjectLibrary.GROUND_START + 1;

		// WATER->Grass Transition sprites:
		public const int GRASS_WATER_NORTH = IngameObjectLibrary.GROUND_START + 2;
		public const int GRASS_WATER_EAST = IngameObjectLibrary.GROUND_START + 3;
		public const int GRASS_WATER_SOUTH = IngameObjectLibrary.GROUND_START + 4;
		public const int GRASS_WATER_WEST = IngameObjectLibrary.GROUND_START + 5;

		public const int GRASS_WATER_NORTH_EAST_IN = IngameObjectLibrary.GROUND_START + 6;
		public const int GRASS_WATER_SOUTH_EAST_IN = IngameObjectLibrary.GROUND_START + 7;
		public const int GRASS_WATER_SOUTH_WEST_IN = IngameObjectLibrary.GROUND_START + 8;
		public const int GRASS_WATER_NORTH_WEST_IN = IngameObjectLibrary.GROUND_START + 9;

		public const int GRASS_WATER_NORTH_EAST_OUT = IngameObjectLibrary.GROUND_START + 10;
		public const int GRASS_WATER_SOUTH_EAST_OUT = IngameObjectLibrary.GROUND_START + 11;
		public const int GRASS_WATER_SOUTH_WEST_OUT = IngameObjectLibrary.GROUND_START + 12;
		public const int GRASS_WATER_NORTH_WEST_OUT = IngameObjectLibrary.GROUND_START + 13;

		// Environment sprites:
        public const int FOREST_SPRITEID = IngameObjectLibrary.ENVIRONMENT_START + 0;
        public const int MOUNTAIN1_SPRITEID = IngameObjectLibrary.ENVIRONMENT_START + 1;
        public const int MOUNTAIN2_SPRITEID = IngameObjectLibrary.ENVIRONMENT_START + 2;
        public const int MOUNTAIN3_SPRITEID = IngameObjectLibrary.ENVIRONMENT_START + 3;
        public const int MOUNTAIN4_SPRITEID = IngameObjectLibrary.ENVIRONMENT_START + 4;
        public const int MOUNTAIN5_SPRITEID = IngameObjectLibrary.ENVIRONMENT_START + 5;


        // CANWALK 
        public const int CANNOTWALK = 0;
		public const int CANWALK 	= 1;
		public const int TRIGGER 	= 2;


		/// <summary>
		/// Initializes a new instance of the <see cref="T:MapGenerator.MapMaker"/> class.
		/// </summary>
		/// <param name="width">Width of map.</param>
		/// <param name="height">Height of map.</param>
		/// <param name="seed">Seed used for random generation.</param>
		/// <param name="fill">Fillpercent for binary growth.</param>
		/// <param name="smooth">Smooth iterations for binary growth.</param>
		/// <param name="sites">number of sites/towns.</param>
		/// <param name="relax">Relax iterations used with Lloyds relaxation.</param>
		/// <param name="spritecount">Number of total available sprites.</param>
		public MapMaker(Player[] players, 
			int width, int height,
			int spritecount, string seed, 
			int fill, int smooth, 
			int sites, int relax, 
			int buildingCount
		)
		{
			this.width = width;
			this.height = height;
			this.seed = seed;

			this.map = GenerateMap(
				sites, relax, // Used for Voronoi algorithm
				fill, smooth, // Used for Binary map generation algorithm
				spritecount, players   // Used in castle creation

			);

            // PLACE TREES IN OCCUPIED AREAS:
            replaceWalls();
            CreateTransitions();

            Reaction[,] reactions = new Reaction[width, height];
            canWalk = CreateWalkableArea(initialMap);

            int i = 0;
            foreach (Region r in regions)
            {
                if (r.GetType().Equals(typeof(LandRegion)))
                {
                    LandRegion lr = (LandRegion)r;
                    lr.SetRegionGroundTileType(lr.GetCastle().EnvironmentTileType, map);


                    map[r.getX(), r.getY()] = lr.GetCastle().GetSpriteID();
                    InitBuildings(lr);
                }
            }

        QuailtyAssurance quality = new QuailtyAssurance();

		}

        private void printmap(int[,] map)
        {
            string msg = "";
            for (int y = 0; y < height; y++)
            {
                for (int x = 0; x < width; x++)
                {
                    msg += map[x, y] +" "; 
                }
                msg += "\n";
            }
            Debug.Log(msg);
        }

        /// <summary>
        /// Replaces walls with mountains/forests/unwalkables
        /// </summary>
        // TODO: Include mountains
        private void replaceWalls()
        {
            for (int y = 0; y < height; y++)
            {
                for (int x = 0; x < width; x++)
                {
                    if (map[x, y] == WALL)
                        map[x, y] = FOREST_SPRITEID;
                }
            }
        }

		private void InitBuildings(LandRegion r)
		{
			r.createEconomy(canWalk, new Economy(Economy.POOR));

			foreach (OverworldBuilding building in r.GetBuildings())
			{
				int x = (int)building.Origo.x;
				int y = (int)building.Origo.y;
				map[x, y] = building.GetSpriteID();
			}
		}

		private void CreateTransitions()
		{
			for (int y = 1; y < height - 1; y++)
			{
				for (int x = 1; x < width - 1; x++)
				{
					int direction = FindDirection(x, y);

					if (direction >= 0)
					{ 
						map[x, y] = 0 + direction;
					}
				}
			}
		}

		private int FindDirection(int x, int y)
		{
			if (Transition(CompassF.NORTH, x, y))
				return (int)GRASS_WATER_NORTH;

			//else if (Transition(CompassF.NORTH_EAST, x, y))
			//	return (int)GRASS_WATER_NORTH_EAST_OUT;

			//else if (Transition(CompassF.EAST, x, y))
			//	return (int)GRASS_WATER_EAST;

			//else if (Transition(CompassF.SOUTH_EAST, x, y))
			//	return (int)GRASS_WATER_SOUTH_EAST_OUT;

			//else if (Transition(CompassF.SOUTH, x, y))
			//	return (int)GRASS_WATER_SOUTH;

			//else if (Transition(CompassF.SOUTH_WEST, x, y))
			//	return (int)GRASS_WATER_SOUTH_WEST_OUT;

			//else if (Transition(CompassF.WEST, x, y))
			//	return (int)GRASS_WATER_WEST;

			//else if (Transition(CompassF.NORTH_WEST, x, y))
			//	return (int)GRASS_WATER_NORTH_WEST_OUT;
			
			return -1; // AREA IS HOMOGENUS
		}

		private bool Transition(int[,] filter, int realX, int realY)
		{
			int range = filter.GetLength(0) / 2;

			string debugOutput = "FOR ("+realX+", "+realY+"):\n";

			for (int y = 0; y < filter.GetLength(0); y++)
			{
				for (int x = 0; x < filter.GetLength(1); x++)
				{
					int fx = realX + (x - range);
					int fy = realY + (y - range);

					if (filter[x, y] == 1 && map[fx, fy] != WATER_SPRITEID) // THESE SHOULD BE WATER
					{
						return false;
					}

					if (filter[x, y] == 0 && map[fx, fy] == WATER_SPRITEID)
					{
						return false;
					}
					debugOutput += "("+x+","+y+") = "+ (filter[x, y] == 1 && map[fx, fy] == WATER_SPRITEID) + ",   ";
				}
				debugOutput += "\n";
			}
			Debug.Log(debugOutput);
			return true;
		}

		/// <summary>
		/// Returns the generated map.
		/// </summary>
		/// <returns>The map.</returns>
		public int[,] GetMap()
		{
			return map;
		}

		/// <summary>
		/// Gets the can walk map.
		/// </summary>
		/// <returns>The can walk map.</returns>
		public int[,] GetCanWalkMap()
		{
			return canWalk;
		}
        
        /// <summary>
        /// Gets the regions.
        /// </summary>
        /// <returns>The regions.</returns>
        public Region[] GetRegions()
		{
			return regions;
		}

		/// <summary>
		/// Generates the map using a set of algorithms. This is the 
		/// controller for the mapgenerator namespace.
		/// </summary>
		/// <returns>The newly generated map.</returns>
		private int[,] GenerateMap(int sites, int relaxItr, int fill, int smooth,int totalSprites, Player[] players)
		{ 
			// Using Voronoi Algorithm to make zones.
			VoronoiGenerator voronoi = VoronoiSiteSetup(sites, relaxItr, totalSprites);
			int[,] voronoiMap = voronoi.GetMap();

			// Converting zones to regions:
			RegionFill r = new RegionFill(voronoiMap, regionCenterPoints);
			int[,] generatedMap = r.GetMap();
			regions = r.GetRegions();

           List<Region> regionBySize = new List<Region>();

            foreach (Region region in regions)
			{
                regionBySize.Add(region);
				region.ResetRegionGroundTileType(generatedMap);
			}

            regionBySize.Sort();
      
            int waterRegionCount = (int) (regions.Length * 0.20);
            int playerID = 0;

        for (int i = 0; i < regions.Length; i++)
            {
                if (i <= waterRegionCount)
                    regionBySize[i] = new WaterRegion(
                        regionBySize[i].GetCoordinates(),
                        regionBySize[i].RegionCenter
                    );
                else
                {
                    Player player = null;
                    if (playerID < players.Length)
                    {
                        players[playerID] = player = new Player(playerID, 0);
                        playerID++;
                    }

                    regionBySize[i] = new LandRegion(
                        regionBySize[i].GetCoordinates(),
                        regionBySize[i].RegionCenter
                    );
                }

                for (int j = 0; j < regions.Length; j++)
                {
                    if (regionBySize[i].Equals(regions[j]))
                    {
                        regions[j] = regionBySize[i]; break;
                    }
                }
            }


            connectLostPointsToRegions(voronoiMap);

            // Creating randomness through procedural map generation.:
            BinaryMap binary = new BinaryMap(width,height,smooth,seed,fill,regions);
			int[,] binaryMap = binary.getMap();

			// Combining binary map and zone-devided maps:
			generatedMap = CombineMaps(binaryMap, generatedMap);
            canWalk = CreateWalkableArea(generatedMap);

           initialMap = new int[width, height];
            for (int y = 0; y < height; y++)
            {
                for (int x = 0; x < width; x++)
                {
                    initialMap[x, y] = generatedMap[x, y];
                }
            }
            // Setting the enviroment for each region:
            foreach (Region region in regions)
			{
                if (region.GetType().Equals(typeof(LandRegion)))
                {
                    LandRegion lr = (LandRegion)region;
                    lr.SetRegionGroundTileType(lr.GetCastle().EnvironmentTileType, generatedMap);

                }
                else if (region.GetType().Equals(typeof(WaterRegion)))
                {
                    WaterRegion wr = (WaterRegion)region;
                    wr.FillRegionWithWater(generatedMap, canWalk);
                }
            }

			return generatedMap;
		}

        public void initializePlayers(int[,] map, int[,] canWalk, Player[] players)
        {
            int i = 0;
            foreach(Region region in regions)
            {
                if (region.GetType().Equals(typeof(LandRegion)))
                {
                    LandRegion lr = (LandRegion)region;
                    // Place a castle at every landregion
                    lr.PlaceCastle(new UnknownCastle(null), map, canWalk);
                    if (i < players.Length)
                    {
                        // Create a player, set the castles owner as that player.
                        // Also place a corresponding hero.
                        players[i] = new Player(i, 0);
                        lr.GetCastle().Player = players[i];
                        lr.PlaceHero(players[i], map, canWalk);
                        i++;
                    }
                }
            }
        }

        /// <summary>
        /// Connects regionless tiles to the nearest region
        /// </summary>
        /// <param name="map"></param>
        public void connectLostPointsToRegions(int[,] map)
        {

            bool inRegion = false;

            Region prev = regions[0];
            Vector2 prevPos = Vector2.zero;
            for (int y = 0; y < height; y++)
            {
                for (int x = 0; x < width; x++)
                {
                    if (map[x, y] == MapMaker.WALL)
                    {
                        foreach (Region r in regions)
                        {
                            if (r.isPointInRegion(new Vector2(x, y)))
                            {
                                prevPos = new Vector2(x, y);
                                inRegion = true;
                                if (!typeof(WaterRegion).Equals(r.GetType()))
                                    prev = r;
                                break;
                            }
                        }
                        if (!inRegion)
                        {
                            prev.AddToRegion(new Vector2(x, y));
                            map[x, y] = MapMaker.FOREST_SPRITEID; // map[(int) prevPos.x, (int) prevPos.y];
                        }
                        inRegion = false;
                    }
                }
            }
        }

        /// <summary>
        /// Sets up the voronoi map + the castles/towns
        /// </summary>
        /// <returns>Already ran voronoi map.</returns>
        /// <param name="sites">Number of Sites/town.</param>
        /// <param name="relaxItr">Number of Relax itrations.</param>
        /// <param name="totalSprites">Total sprites.</param>
        private VoronoiGenerator VoronoiSiteSetup(int sites, int relaxItr, int totalSprites)
		{
			// DEFINING CASTLE POSITIONS ON THE MAP:
			Vector2[] sitelist = CreateRandomPoints(sites); // TODO: Place castles smart.

			// APPLYING VORONOI TO THE MAP ARRAY
			VoronoiGenerator voronoi = new VoronoiGenerator(width, height, sitelist, relaxItr);

			// Getting new positions after relaxing:
			regionCenterPoints = voronoi.GetNewSites();

			return voronoi;
		}

		/// <summary>
		/// Creates the walkable area through analysis of the map.
		/// </summary>
		/// <returns>The walkable area.</returns>
		/// <param name="map">Map.</param>
		private int[,] CreateWalkableArea(int[,] map)
		{
			int[,] canWalk = new int[width, height];
			for (int y = 0; y < height; y++)
			{
				for (int x = 0; x < width; x++)
				{
					if (map[x, y] == GROUND)
						canWalk[x, y] = 1;
					else
						canWalk[x, y] = 0;
				}
			}
			return canWalk;
		}

        /// <summary>
        /// Creates the walkable area through the other al.
        /// </summary>
        /// <returns>The walkable area.</returns>
        /// <param name="map">Map.</param>
        private int[,] CreateWalkableArea()
		{
			return CreateWalkableArea(map);
		}

		/// <summary>
		/// Combines the maps Binary Procedural generated map and Voronoi diagram map.
		/// </summary>
		/// <returns>Combined version of the two maps.</returns>
		/// <param name="binary">Binary map.</param>
		/// <param name="voronoi">Voronoi map.</param>
		private int[,] CombineMaps(int[,] binary, int[,] voronoi)
        {
			int[,] combinedMap = new int[width, height];
            for (int y = 0; y < height; y++)
            {
                for (int x = 0; x < width; x++)
                {
					if (voronoi[x, y] == WALL)
					if (KEEP_VORONOI_REGION_LINES)
						combinedMap[x, y] = WALL;
					else
						combinedMap[x, y] = binary[x, y];
					
					else if (voronoi[x, y] == REGION_CENTER)
						combinedMap[x, y] = WALL;
					
					else
						combinedMap[x, y] = binary[x, y];
                }
            }

            return combinedMap;
        }

        /// <summary>
        /// Creates the list of random points to create areas out of.
        /// </summary>
        /// <returns>The list of random points.</returns>
        /// <param name="width">Width.</param>
        /// <param name="height">Height.</param>
		private Vector2[] CreateRandomPoints(int sites) 
        {
            Vector2[] points = new Vector2[sites];

            // Pesuedo random number generator:
            System.Random prng = new System.Random(seed.GetHashCode());
            for (int i = 0; i < sites; i++) 
            {
				points[i] = new Vector2(prng.Next(0, width), prng.Next(0, height));
            }

            return points;
        }
    }
}
    