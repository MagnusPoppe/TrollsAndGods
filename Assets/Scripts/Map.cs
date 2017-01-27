using System;
using UnityEngine;
using MapGenerator;

	public class Map : MonoBehaviour
	{
		public int widthOfMap = 128, heightOfMap = 128;

		// Unity map objects
		GameObject[,] tiles;
		public string seed;
		[Range(0, 100)] public int fillpercentWalkable;

		// VORONOI varables:
		[Range(0, 50)] public int sites;
		[Range(1, 20)] public int relaxIterations;
		[Range(0, 20)] public int smoothIterations;
		
		public Sprite[] groundTiles;

		void Start()
		{
			MapMaker mapmaker = new MapMaker(
				widthOfMap, heightOfMap,
				seed, fillpercentWalkable, smoothIterations,
				sites, relaxIterations,
				groundTiles.Length
			);

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

}