using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UseVoronoi : MonoBehaviour 
{

    // Mathematical game objects
    public int width, height;
    int[,] map;
    bool[,] canWalk;
    public string seed;
    [Range (0, 50)]
    public int numberOfPoints;

    [Range (0, 20)]
    public int relax;

    // Unity map objects
    GameObject[,] tiles;
    GameObject board;
    public Sprite[] groundTiles;

	// Use this for initialization
	void Start () 
    {
        Vector2[] castles = CreateRandomPoint(width, height, numberOfPoints);
        VoronoiGenerator voronoi = new VoronoiGenerator(width, height, castles, relax);	
        map = fillMap(voronoi.getTexture());


        tiles = new GameObject[width, height];
        canWalk = new bool[width, height];
        board = new GameObject();
        board.name = "Board";
        fillTiles();
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
