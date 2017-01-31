using System;
using UnityEngine;
using MapGenerator;

namespace Overworld
{


    public class Map : MonoBehaviour
	{
		public int widthXHeight = 128;
		int width, height;

		// Unity map objects
		GameObject[,] tiles;
		public string seed = "Angelica";
		[Range(0, 100)]
		public int fillpercentWalkable = 57;

		// VORONOI varables:
		[Range(0, 50)]
		public int sites = 8;
		[Range(1, 20)]
		public int relaxIterations = 3;
		[Range(0, 20)]
		public int smoothIterations = 5;

		public Sprite[] groundTiles;

        GameObject[] heroPrefabs;
        void Awake()
        {
            heroPrefabs = UnityEngine.Resources.LoadAll<GameObject>("Heroes");
        }

        void Start()
		{
			width = widthXHeight;
			height = widthXHeight;

			MapMaker mapmaker = new MapMaker(
				width, height,	groundTiles.Length,				// Map Properites
				seed, fillpercentWalkable, smoothIterations,	// BinaryMap Properities
				sites, relaxIterations							// Voronoi Properties
			);
			PlaceCamera();
			DrawMap(mapmaker.GetMap());

			SpawnHero(mapmaker.GetMap(), mapmaker.GetRegions());

		}

		void SpawnHero(int[,] map, Region[] regions)
		{
			Vector2 castlePos = regions[0].GetCastle().GetPosition();
			Vector2 heroPos = new Vector2(castlePos.x + 1, castlePos.y - 2);

			GameObject hero = new GameObject();
            hero = heroPrefabs[UnityEngine.Random.Range(0, 1)];
            hero.transform.position = heroPos;
            Instantiate(hero);
        }

		/// <summary>
		/// Draws a given map.
		/// </summary>
		/// <param name="map">Map.</param>
		protected void DrawMap(int[,] map)
		{
			// DRAWING THE MAP:
			tiles = new GameObject[width, height];
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
			int x = (int)this.transform.position.x + (width / 2);
			int y = (int)this.transform.position.y + (height / 2);

			miniMapCamera.transform.position = new Vector3(x, y, -1);
			miniMapCamera.transform.parent = this.transform;
			miniMapCamera.tag = "MainCamera";

			Camera cam = miniMapCamera.AddComponent<Camera>();
			cam.orthographic = true;
			cam.orthographicSize = width/2;
			cam.farClipPlane = 2;

		}

		public int GetHeightOfMap()
		{
			return height;
		}

		public int GetWidthOfMap()
		{
			return width;
		}

	}
}
