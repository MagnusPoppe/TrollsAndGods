using UnityEngine;
using OverworldObjects;

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
		bool[,] canWalk;

		// Overworld objects
		Castle[] castles;

		// Constants
		public const int  GROUND 					= 0;
		public const int  WALL 						= 1;
		public const int  CASTLE 					= 2;
		public const int  FIRST_AVAILABLE_SPRITE	= 3;
		public const bool KEEP_VORONOI_REGION_LINES = false;

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
			canWalk = CreateWalkableArea(map);
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
		public bool[,] GetCanWalkMap()
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
				region.SetRegionGroundTileType(region.GetCastle().GetEnvironment(), generatedMap);
			}

			return generatedMap;
		}

		private Vector2[] SetResourcesSpawnpoint(Region region)
		{


			return null;
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
				int color = Random.Range(3, totalSprites);
				castles[i] = new Castle(sitelist[i], color);
			}

			return voronoi;
		}

		/// <summary>
		/// Creates the walkable area through analysis of the map.
		/// </summary>
		/// <returns>The walkable area.</returns>
		/// <param name="map">Map.</param>
		private bool[,] CreateWalkableArea(int[,] map)
		{
			bool[,] canWalk = new bool[width, height];
			for (int y = 0; y < height; y++)
			{
				for (int x = 0; x < width; x++)
				{
					if (map[x, y] == GROUND)
						canWalk[x, y] = true;
					else
						canWalk[x, y] = false;
				}
			}
			return canWalk;
		}

		/// <summary>
		 /// Creates the walkable area through the other al.
		 /// </summary>
		 /// <returns>The walkable area.</returns>
		 /// <param name="map">Map.</param>
		private bool[,] CreateWalkableArea()
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
    