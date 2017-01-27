using System;
using UnityEngine;
using MapGenerator;

public class Map : MonoBehaviour
{
	public int widthOfMap = 128, heightOfMap = 128;

	// Unity map objects
	GameObject[,] tiles;
	public string seed = "Angelica";
 	[Range(0, 100)] public int fillpercentWalkable = 57;

	// VORONOI varables:
	[Range(0, 50)] public int sites = 8;
	[Range(1, 20)] public int relaxIterations = 3;
	[Range(0, 20)] public int smoothIterations = 5;
	
	public Sprite[] groundTiles;

	void Start()
	{
		MapMaker mapmaker = new MapMaker(
			widthOfMap, heightOfMap,
			seed, fillpercentWalkable, smoothIterations,
			sites, relaxIterations,
			groundTiles.Length
		);
		PlaceCamera();
		DrawMap(mapmaker.GetMap());

	}

	/// <summary>
	/// Draws a given map.
	/// </summary>
	/// <param name="map">Map.</param>
	protected void DrawMap(int[,] map)
	{
		// DRAWING THE MAP:
		tiles = new GameObject[widthOfMap, heightOfMap];
		// Looping through all tile positions:
		for (int y = 0; y < heightOfMap; y++)
		{
			for (int x = 0; x < widthOfMap; x++)
			{
				// Creating a new game object to place on the board:
				tiles[x, y] = new GameObject();
				tiles[x, y].name = "Tile (" + x + ", " + y + ")";
				tiles[x, y].transform.position = new Vector2(x, y);

				// Adding a sprite to the gameobject:
				SpriteRenderer sr = tiles[x, y].AddComponent<SpriteRenderer>();
				sr.sprite = groundTiles[map[x, y]];

				// Placing the tile on on the map within the board gameobject:
				tiles[x, y].transform.parent = this.transform;
			}
		}
	}

	/// <summary>
	/// Places a given camera perfectly over the map.
	/// </summary>
	public void PlaceCamera()
	{
		GameObject miniMapCamera = new GameObject();
		miniMapCamera.name = "MiniMapCamera";
		int x = (int)this.transform.position.x + (widthOfMap / 2);
		int y = (int)this.transform.position.y + (heightOfMap / 2);

		miniMapCamera.transform.position = new Vector3(x, y, -1);
		miniMapCamera.transform.parent = this.transform;
		miniMapCamera.tag = "MainCamera";

		Camera cam = miniMapCamera.AddComponent<Camera>();
		cam.orthographic = true;
		cam.orthographicSize = widthOfMap;
		cam.farClipPlane = 2;

	}

	public int GetHeightOfMap()
	{
		return heightOfMap;
	}

	public int GetWidthOfMap()
	{
		return widthOfMap;
	}

}