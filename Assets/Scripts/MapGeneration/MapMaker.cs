using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
        [Range (0, 50)] public int numberOfPoints;
        [Range (0, 20)] public int relaxItr;
        [Range (0, 20)] public int smoothItr;

        // Unity map objects
        GameObject[,] tiles;
        GameObject board;
        public Sprite[] groundTiles;

        // Use this for initialization
        void Start () 
        {
            map = new int[width, height];

            // APPLYING VORONOI TO THE MAP ARRAY
            Vector2[] castles = CreateRandomPoint(width, height, numberOfPoints);
            VoronoiGenerator voronoi = new VoronoiGenerator(width, height, castles, relaxItr); 
            int[,] voronoiMap = fillMap(voronoi.getTexture());

            // APPLYING PROCEDURAL MAP GENERATOR TO MAP ARRAY:
            BinaryMap binary = new BinaryMap(width, height, smoothItr, seed, fillPercent);
            int[,] binaryMap = binary.getMap();

            // CHOOSING MAP TO USE:
            map = combineMaps(binaryMap, voronoiMap);

            // DRAWING THE MAP:
            tiles = new GameObject[width, height];
            canWalk = new bool[width, height];
            board = new GameObject();
            board.name = "Board";
            fillTiles();

            // FLIPPING THE CANWALK TABLE ELEMENTS ACCORDING TO THE MAP
            createWalkableArea();
        }

        void createWalkableArea()
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

        int[,] combineMaps(int[,] binary, int[,] voronoi)
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
            return map;
        }


        /// <summary>
        /// Fills the map with the values for the voronoi zones. 
        ///     Lines     == 1.
        ///     EmptyArea == 0.
        /// 
        /// </summary>
        /// <returns>The map.</returns>
        /// <param name="tx">Tx.</param>
        int[,] fillMap( Texture2D tx )
        {
            int[,] map = new int[width,height];

            for (int y = 0; y < height; y++)
            {
                for (int x = 0; x < width; x++)
                {
                    Color here = tx.GetPixel(x, y);

                    if (here == Color.blue) // IF WALL
                    {
                        map[x, y] = 1;
                    }
                    else if (here == Color.red) // IF THE CASTLE
                    {
                        map[x, y] = 0;
                    }
                    else // IF EMPTY TILE
                    {
                        map[x, y] = 0;
                    }
                }
            }
            return map;
        }

        /// <summary>
        /// Creates the list of random points to create areas out of.
        /// </summary>
        /// <returns>The list of random points.</returns>
        /// <param name="width">Width.</param>
        /// <param name="height">Height.</param>
        private Vector2[] CreateRandomPoint(int width, int height, int numberOfPoints) 
        {
            Vector2[] points = new Vector2[numberOfPoints];
            for (int i = 0; i < numberOfPoints; i++) 
            {
                points[i] = new Vector2(Random.Range(0,width), Random.Range(0,height));
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
    