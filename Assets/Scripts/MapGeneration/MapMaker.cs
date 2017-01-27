using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using OverworldObjects;

namespace MapGenerator
{
    public class MapMaker : MonoBehaviour 
    {

        // Mathematical game objects
        public int width, height;
        int[,] map;
        bool[,] canWalk;
        public string seed;
        [Range(0,100)] public int fillPercent;

        // VORONOI varables:
		[Range (0, 50)] public int sites;
        [Range (0, 20)] public int relaxItr;
        [Range (0, 20)] public int smoothItr;

        // Unity map objects
        GameObject[,] tiles;
        GameObject board;
        public Sprite[] groundTiles;

		// Overworld objects
		Castle[] castles;

		public MapMaker()
		{	
			
		}

        // Use this for initialization
        void Start () 
        {
			VoronoiGenerator voronoi = CastleSetup();
			int[,] voronoiMap = voronoi.GetMap();

            // APPLYING PROCEDURAL MAP GENERATOR TO MAP ARRAY:
            BinaryMap binary = new BinaryMap(width, height, smoothItr, seed, fillPercent);
            int[,] binaryMap = binary.getMap();



			RegionFill r = new RegionFill(voronoiMap, castles);
			map = r.getMap();
			//map = CombineMaps(binaryMap, voronoiMap);

			DrawMap();
            // FLIPPING THE CANWALK TABLE ELEMENTS ACCORDING TO THE MAP
            CreateWalkableArea();
        }

		VoronoiGenerator CastleSetup()
		{
			// DEFINING CASTLE POSITIONS ON THE MAP:
			Vector2[] sitelist = CreateRandomPoints();

			// APPLYING VORONOI TO THE MAP ARRAY
			VoronoiGenerator voronoi = new VoronoiGenerator(width, height, sitelist, relaxItr);

			// Getting new positions after relaxing:
			sitelist = voronoi.GetNewSites();

			castles = new Castle[sites];

			for (int i = 0; i < castles.Length; i++)
			{
				int color = Random.Range(3, groundTiles.Length);
				castles[i] = new Castle(sitelist[i], color);
			}

			return voronoi;
		}

		void CreateWalkableArea()
        {
            for (int y = 0; y < height; y++)
            {
                for (int x = 0; x < width; x++)
                {
                    if (map[x, y] == 1)
                        canWalk[x, y] = false;
                    else
                        canWalk[x, y] = true;
                }
            }
        }

		protected int[,] CombineMaps(int[,] binary, int[,] voronoi)
        {
            for (int y = 0; y < height; y++)
            {
                for (int x = 0; x < width; x++)
                {
                    if (voronoi[x, y] == 1)
                        map[x, y] = 2;
                    else
                        map[x, y] = binary[x, y];
                }
            }

			foreach (Castle site in castles)
			{
				map[(int)site.GetPosition().x, (int)site.GetPosition().x] = 2;
			}

            return map;
        }

		protected void DrawMap()
		{
            // DRAWING THE MAP:
            tiles = new GameObject[width, height];
			canWalk = new bool[width, height];
			board = new GameObject();
			board.name = "Board";
			fillTiles();
		}



        /// <summary>
        /// Creates the list of random points to create areas out of.
        /// </summary>
        /// <returns>The list of random points.</returns>
        /// <param name="width">Width.</param>
        /// <param name="height">Height.</param>
		private Vector2[] CreateRandomPoints() 
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

        /// <summary>
        /// Fills the tiles with game objects and sprites.
        /// </summary>
        void fillTiles()
        {
            // Looping through all tile positions:
            for (int y = 0; y < height; y++)
            {
                for (int x = 0; x < width; x++)
                {
                    // Creating a new game object to place on the board:
                    tiles[x, y] = new GameObject();
                    tiles[x, y].name = "Tile (" + x + ", " + y + ")";
                    tiles[x, y].transform.position = new Vector2(x, y);

                    // Adding a sprite to the gameobject:
                    SpriteRenderer sr = tiles[x, y].AddComponent<SpriteRenderer>();
                    sr.sprite = groundTiles[ map[x, y] ];

                    // Placing the tile on on the map within the board gameobject:
                    tiles[x, y].transform.parent = board.transform;
                }
            }
        }
    }   
}
    