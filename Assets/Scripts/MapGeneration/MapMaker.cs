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

        private float percentWater = 0.20f;
        string seed;

        // Information about the map:
        Region[] regions;

        int[,] map;
        int[,] canWalk;
        int[,] initialMap;

        // Overworld objects
        Point[] regionCenterPoints;

        // Constants
        public const int GROUND = 0;

        public const int WALL = 1;
        public const int REGION_CENTER = 2;
        public const bool KEEP_VORONOI_REGION_LINES = false;

        // Base sprites
        public const int GRASS_SPRITEID = IngameObjectLibrary.GROUND_START + 13;

        public const int GRASS2_SPRITEID = IngameObjectLibrary.GROUND_START + 14;
        public const int GRASS3_SPRITEID = IngameObjectLibrary.GROUND_START + 15;
        public const int GRASS4_SPRITEID = IngameObjectLibrary.GROUND_START + 16;

        public const int WATER_SPRITEID = IngameObjectLibrary.GROUND_START + 0;

        // WATER->Grass Transition sprites:
        public const int GRASS_WATER_NORTH = IngameObjectLibrary.GROUND_START + 1;

        public const int GRASS_WATER_EAST = IngameObjectLibrary.GROUND_START + 2;
        public const int GRASS_WATER_SOUTH = IngameObjectLibrary.GROUND_START + 3;
        public const int GRASS_WATER_WEST = IngameObjectLibrary.GROUND_START + 4;

        public const int GRASS_WATER_NORTH_EAST_IN = IngameObjectLibrary.GROUND_START + 5;
        public const int GRASS_WATER_SOUTH_EAST_IN = IngameObjectLibrary.GROUND_START + 6;
        public const int GRASS_WATER_SOUTH_WEST_IN = IngameObjectLibrary.GROUND_START + 7;
        public const int GRASS_WATER_NORTH_WEST_IN = IngameObjectLibrary.GROUND_START + 8;

        public const int GRASS_WATER_NORTH_EAST_OUT = IngameObjectLibrary.GROUND_START + 9;
        public const int GRASS_WATER_SOUTH_EAST_OUT = IngameObjectLibrary.GROUND_START + 10;
        public const int GRASS_WATER_SOUTH_WEST_OUT = IngameObjectLibrary.GROUND_START + 11;
        public const int GRASS_WATER_NORTH_WEST_OUT = IngameObjectLibrary.GROUND_START + 12;

        // Environment sprites:
        public const int FOREST_SPRITEID = IngameObjectLibrary.ENVIRONMENT_START + 0;

        public const int MOUNTAIN1_SPRITEID = IngameObjectLibrary.ENVIRONMENT_START + 1;
        public const int MOUNTAIN2_SPRITEID = IngameObjectLibrary.ENVIRONMENT_START + 2;
        public const int MOUNTAIN3_SPRITEID = IngameObjectLibrary.ENVIRONMENT_START + 3;
        public const int MOUNTAIN4_SPRITEID = IngameObjectLibrary.ENVIRONMENT_START + 4;
        public const int MOUNTAIN5_SPRITEID = IngameObjectLibrary.ENVIRONMENT_START + 5;


        // CANWALK 
        public const int CANNOTWALK = 0;

        public const int CANWALK = 1;
        public const int TRIGGER = 2;

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
            int buildingCount,
            bool coast
        )
        {
            this.width = width;
            this.height = height;
            this.seed = seed;

            this.map = GenerateMap(
                sites, relax, // Used for Voronoi algorithm
                fill, smooth, // Used for Binary map generation algorithm
                spritecount, players // Used in castle creation

            );

            // PLACE TREES IN OCCUPIED AREAS:
            replaceWalls();

            if (coast)
                CreateTransitions();

            QuailtyAssurance quality = new QuailtyAssurance();

        }


        /// <summary>
        /// Replaces walls with mountains/forests/unwalkables
        /// </summary>
        // TODO: Include mountains
        private void replaceWalls()
        {
            Mountain mountain = new Mountain();
            Grass grass = new Grass();
            // Trying mountains first:
            for (int y = 0; y < height; y++)
            {
                for (int x = 0; x < width; x++)
                {
                    if (map[x, y] == WALL)
                    {
                        if (Shapes.CanFitShapeOver(WALL, new Point(x, y), Shapes.GetShape(Shapes.TRIPLEx3_LEFT), map))
                        {
                            PlaceMountain(new Point(x, y), mountain.GetSpriteID(), GRASS2_SPRITEID, mountain.Shape);
                        }
                    }
                }
            }

            // Filling with forests where mountains do not fill.
            for (int y = 0; y < height; y++)
            {
                for (int x = 0; x < width; x++)
                {
                    if (map[x, y] == WALL)
                        FloodFillWall(new Point(x, y), FOREST_SPRITEID);
                }
            }
        }

        /// <summary>
        /// Places a mountain using a filtering algoritm. Places mountain at
        /// the origo point of the shape, and environment tiles everywhere else.
        /// </summary>
        /// <param name="pos"> Position to place mountain</param>
        /// <param name="spriteID"> ID of mountain sprite</param>
        /// <param name="environment"> ID of environment underneath the mountain</param>
        /// <param name="shape"> Shape of the mountain</param>
        void PlaceMountain(Point pos, int spriteID, int environment, int[,] shape)
        {
            for (int iy = 0; iy < shape.GetLength(1); iy++)
            {
                for (int ix = 0; ix < shape.GetLength(0); ix++)
                {

                    int x = pos.x + (ix - (shape.GetLength(1)/2));
                    int y = pos.y + (iy - (shape.GetLength(0)/2));
                    Point p = new Point(x,y);
                    if (p.InBounds(map))
                    {
                        if (shape[ix, iy] == 1)
                            map[x, y] = spriteID;
                        else if (shape[ix, iy] == 2)
                            map[x, y] = environment;
                    }
                }
            }
        }

        /// <summary>
        /// Floodfills all map values where the value matches the constant WALL
        /// from a given seed.
        /// </summary>
        /// <param name="initial">Initial Position</param>
        /// <param name="spriteID">spriteID</param>
        private void FloodFillWall( Point initial, int spriteID )
        {
            Queue<Point> queue = new Queue<Point>();
            queue.Enqueue(initial);

            Point center = queue.Peek();

            while (queue.Count != 0)
            {      
                Point here = queue.Dequeue();

                // Checking if inbounds
                if (here.x >= 0 && here.x < width && here.y >= 0 && here.y < height)
                {
                    // Checking if wall
                    if (map[here.x, here.y] == WALL)
                    {
                        // Labeling:
                        map[here.x, here.y] = spriteID;

                        // Adding neighbours to queue
                        queue.Enqueue(new Point(here.x - 1, here.y));
                        queue.Enqueue(new Point(here.x + 1, here.y));
                        queue.Enqueue(new Point(here.x, here.y - 1));
                        queue.Enqueue(new Point(here.x, here.y + 1));
                    }
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
            int[,] tempMap = HandyMethods.Copy2DArray(map);
			for (int y = 1; y < height - 1; y++)
			{
				for (int x = 1; x < width - 1; x++)
				{
					int direction = FindDirection(x, y);

                    if (direction >= 0)
                    { 
                        tempMap[x, y] = direction;
                    }
                    else
                    {
                        tempMap[x, y] = map[x, y];
                    }
				}
			}
            map = tempMap;
		}

		private int FindDirection(int x, int y)
		{
            CompassF.Direction direction = CompassF.GetDirection(map, x, y, WATER_SPRITEID, GRASS_SPRITEID);

            if (direction == CompassF.Direction.north)
				return (int)GRASS_WATER_NORTH;

            else if (direction == CompassF.Direction.northEast)
				return (int)GRASS_WATER_NORTH_EAST_OUT;

            else if (direction == CompassF.Direction.east)
				return (int)GRASS_WATER_EAST;

            else if (direction == CompassF.Direction.southEast)
				return (int)GRASS_WATER_SOUTH_EAST_OUT;

            else if (direction == CompassF.Direction.south)
				return (int)GRASS_WATER_SOUTH;

            else if (direction == CompassF.Direction.southWest)
				return (int)GRASS_WATER_SOUTH_WEST_OUT;

            else if (direction == CompassF.Direction.west)
				return (int)GRASS_WATER_WEST;

            else if (direction == CompassF.Direction.northWest)
				return (int)GRASS_WATER_NORTH_WEST_OUT;
			
			return -1; // AREA IS HOMOGENUS, NO CHANGES.
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
      
            int waterRegionCount = (int) (regions.Length * percentWater);
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
                    lr.SetRegionGroundTileType(generatedMap);

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
                    lr.PlaceCastle(map, canWalk);
                    if (i < players.Length)
                    {
                        // Create a player, set the castles owner as that player.
                        // Also place a corresponding hero.
                        players[i] = new Player(i, 0);
                        lr.GetCastle().Player = players[i];
                        lr.GetCastle().Town.Owner = players[i];
                        players[i].Castle.Add(lr.GetCastle());
                        i++;
                    }
                }
            }
        }

        /// <summary>
        /// Uses the placement class to place buildings onto the map.
        /// This is done through a given region.
        /// </summary>
        public Placement PlaceBuildings(Player[] players, Reaction[,] reactions)
        {
            Placement placements = new Placement(map, canWalk);
            return placements;
            // PLACEMENT OF ALL BUILDINGS:
            // TODO: GIVE THE MINES TO PLAYERS, NOT JUST players[0]
            /*
            Player ownerOfMines = players[0];
            foreach (Region r in regions)
            {
                ResourceBuilding mine = new OreMine(ownerOfMines);
                placements.Place( r, mine );
                ownerOfMines.ResourceBuildings.Add(mine);

                mine = new GemMine(ownerOfMines);
                placements.Place( r, mine);
                ownerOfMines.ResourceBuildings.Add(mine);

                mine = new CrystalMine(ownerOfMines);
                placements.Place( r, mine);
                ownerOfMines.ResourceBuildings.Add(mine);

                mine = new GoldMine(ownerOfMines);
                placements.Place( r, mine);
                ownerOfMines.ResourceBuildings.Add(mine);
            }
            */
        }

        /// <summary>
        /// Connects regionless tiles to the nearest region
        /// </summary>
        /// <param name="map"></param>
        public void connectLostPointsToRegions(int[,] map)
        {
            bool inRegion = false;

            Region prev = regions[0];
            Point prevPos = new Point(0,0);
            for (int y = 0; y < height; y++)
            {
                for (int x = 0; x < width; x++)
                {
                    if (map[x, y] == MapMaker.WALL)
                    {
                        foreach (Region r in regions)
                        {
                            if (r.isPointInRegion(new Point(x, y)))
                            {
                                prevPos = new Point(x, y);
                                inRegion = true;
                                if (!typeof(WaterRegion).Equals(r.GetType()))
                                    prev = r;
                                break;
                            }
                        }
                        if (!inRegion)
                        {
                            prev.AddToRegion(new Point(x, y));
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
			Vector2[] sitelist = CreateRandomPoints(sites);

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
    