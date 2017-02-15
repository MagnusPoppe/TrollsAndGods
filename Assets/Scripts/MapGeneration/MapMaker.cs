using UnityEngine;
using OverworldObjects;
using Filter;

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
        Reaction[,] reactions;

        // Overworld objects
        Castle[] castles;

		// Constants
		public const int  GROUND 					= 0;
		public const int  WALL 						= 1;
		public const int  CASTLE 					= 2;
		public const int  WATER 					= 3;
		public const int  DIRT						= 4;
		public const int  WOODS 					= 5;
		public const int  BUILDING 					= 6;
		public const int GRASS_WATER = 7;
		public const int GRASS_TO_WATER_DIRECTION_END = 15;
		public const bool KEEP_VORONOI_REGION_LINES = false;

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
		public MapMaker(
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
				spritecount   // Used in castle creation
			);

			// PLACE TREES IN OCCUPIED AREAS:
			for (int y = 0; y < height; y++)
			{
				for (int x = 0; x < width; x++)
				{
					if (map[x, y] == WALL)
						map[x, y] = WOODS;
				}
			}
			FillRandomRegionsWithWater();

			//CreateTransitions();

			canWalk = CreateWalkableArea();

            reactions = new Reaction[width, height];

			foreach (Region r in regions)
			{ 
				InitBuildings(r);
			}

			QuailtyAssurance quality = new QuailtyAssurance();

		}

        private void AddReaction(int x, int y, OverworldBuilding building)
        {
            building.makeReaction(x, y);
            reactions[x, y] = building.Reaction;
        }

		private void InitBuildings(Region r)
		{
			r.createEconomy(canWalk, new Economy(Economy.POOR));

			foreach (OverworldBuilding building in r.GetBuildings())
			{
				int x = (int)building.Origo.x;
				int y = (int)building.Origo.y;
				map[x, y] = building.SpriteID;
			}
		}

		private void FillRandomRegionsWithWater()
		{
			System.Random prng = new System.Random(seed.GetHashCode());

			for (int i = 0; i < castles.Length/2; i++)
			{
				// Pesuedo random number generator:
				int r = prng.Next(0, castles.Length);
    			regions[r].FillRegionWithWater(map);
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
						Debug.Log("Direction " + direction + " found for (" + x + "," + y + ")");
						map[x, y] = GRASS_WATER + direction;
					}
				}
			}
		}

		private int FindDirection(int x, int y)
		{
			if (Transition(CompassF.NORTH, x, y))
				return (int)CompassF.Directions.north;

			//else if (Transition(CompassF.NORTH_EAST, x, y))
			//	return (int)CompassF.Directions.northEast;

			//else if (Transition(CompassF.EAST, x, y))
			//	return (int)CompassF.Directions.east;

			//else if (Transition(CompassF.SOUTH_EAST, x, y))
			//	return (int)CompassF.Directions.southEast;

			//else if (Transition(CompassF.SOUTH, x, y))
			//	return (int)CompassF.Directions.south;

			//else if (Transition(CompassF.SOUTH_WEST, x, y))
			//	return (int)CompassF.Directions.southWest;

			//else if (Transition(CompassF.WEST, x, y))
			//	return (int)CompassF.Directions.west;

			//else if (Transition(CompassF.NORTH_WEST, x, y))
			//	return (int)CompassF.Directions.northWest;
			
			return -1; // AREA IS HOMOGENUS
		}

		private bool Transition(int[,] filter, int x, int y)
		{
			int range = filter.GetLength(0) / 2;

			for (int fy = 0; fy < filter.GetLength(0); fy++)
			{
				for (int fx = 0; fx < filter.GetLength(0); fx++)
				{

					if (filter[fx, fy] == 1) // HER SKAL DET VÆRE VANN
					{
						if (map[x + (fx - range), y + (fy - range)] != WATER)
							return false;
					}
					else
					{
						if (map[x + (fx - range), y + (fy - range)] == WATER)
							return false;
					}

				}
			}
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
		private int[,] GenerateMap(int sites, int relaxItr, int fill, int smooth,int totalSprites)
		{ 
			// Using Voronoi Algorithm to make zones.
			VoronoiGenerator voronoi = VoronoiSiteSetup(sites, relaxItr, totalSprites);
			int[,] voronoiMap = voronoi.GetMap();

			// Converting zones to regions:
			RegionFill r = new RegionFill(voronoiMap, castles);
			int[,] generatedMap = r.GetMap();
			regions = r.GetRegions();

			foreach (Region region in regions)
			{
				region.ResetRegionGroundTileType(generatedMap);
			}

			// Creating randomness through procedural map generation.:
			BinaryMap binary = new BinaryMap(width,height,smooth,seed,fill,regions);
			int[,] binaryMap = binary.getMap();

			// Combining binary map and zone-devided maps:
			generatedMap = CombineMaps(binaryMap, generatedMap);

			// Setting the enviroment for each region:
			foreach (Region region in regions)
			{
				region.SetRegionGroundTileType(region.GetCastle().EnvironmentTileType, generatedMap);
			}

			return generatedMap;
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
			sitelist = voronoi.GetNewSites();

			castles = new Castle[sites];

			for (int i = 0; i < castles.Length; i++)
			{
                castles[i] = new UnknownCastle(sitelist[i], 0);
			}

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
					
					else if (voronoi[x, y] == CASTLE)
						combinedMap[x, y] = WALL;
					
					else
						combinedMap[x, y] = binary[x, y];
                }
            }

			foreach (Castle site in castles)
			{
				combinedMap[(int)site.GetPosition().x, (int)site.GetPosition().y] = CASTLE;
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
    