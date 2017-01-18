using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Map maker. Creates a random map based on mediann filter.
/// </summary>
public class MapMaker : MonoBehaviour 
{
    // Mathematical game objects
    public int width, height;
    int[,] map;
    bool[,] canWalk;
    public string seed;
    [Range (0, 5)]
    public int smoothing;

    // Unity map objects
    GameObject[,] tiles;
    GameObject board;
    public Sprite[] groundTiles;


    /// <summary>
    /// Start this instance. Also instansiates all objects needed.
    /// </summary>
	void Start () 
    {
        MultiTileMapGenerator mapGenerator = new MultiTileMapGenerator(width, height, seed, smoothing);
        map = mapGenerator.GetMap();
        tiles = new GameObject[width, height];
        canWalk = new bool[width, height];
        board = new GameObject();
        board.name = "Board";
        fillTiles();
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
            
                if (map[x, y] == 2) // snø
                    canWalk[x, y] = false;
                else
                    canWalk[x, y] = true;
            }
        }
    }
}
