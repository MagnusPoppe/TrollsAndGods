using System;
using UnityEngine;
using MapGenerator;
using Buildings;

namespace Overworld
{
    
    public class Map : MonoBehaviour
	{
        public MapMaker mapmaker;
		public int widthXHeight = 128;
		int width, height;

        // Unity map objects
        public const float XRESOLUTION = 2598;
        public const float YRESOLUTION = 1299;
        public const float YOFFSET = YRESOLUTION / XRESOLUTION;
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

		[Range(0,20)] int buildingCount;

		public Sprite[] groundTiles;

        public GameObject[] heroPrefabs;
        void Awake()
        {
            heroPrefabs = UnityEngine.Resources.LoadAll<GameObject>("Heroes");
        }

        void Start()
		{
			width = widthXHeight;
			height = widthXHeight;

			mapmaker = new MapMaker(
				width, height,	groundTiles.Length,				// Map Properites
				seed, fillpercentWalkable, smoothIterations,	// BinaryMap Properities
				sites, relaxIterations,							// Voronoi Properties
				buildingCount
			);
			PlaceCamera();
			DrawMap(mapmaker.GetMap());

            Region[] regions = mapmaker.GetRegions();

            //for(int i=0; i<regions.Length; i++)
            //{
            //    SpawnHero(mapmaker.GetMap(), regions[i].GetCastle());
            //}

		}

		void SpawnHero(int[,] map, Castle castle)
		{
			Vector2 castlePos = castle.GetPosition();
			Vector2 heroPos = new Vector2((int)castlePos.x + 1, (int)castlePos.y/2 - 2);
			GameObject hero = heroPrefabs[UnityEngine.Random.Range(0, 2)];
            hero.transform.position = heroPos;
            Instantiate(hero);
        }

		/// <summary>
		/// Draws a given map.
		/// </summary>
		/// <param name="map">Map.</param>
		protected void DrawMap(int[,] map)
		{
			GameObject[,] objectsInBuildingLayer = new GameObject[width,height];

			// DRAWING THE MAP:
			tiles = new GameObject[width, height];
            float iy = 0;
			// Looping through all tile positions:
			for (int y = 0; y < height; y++)
			{

                for (int x = 0; x < width; x++)
                {
                    // Creating a new game object to place on the board:
                    tiles[x, y] = new GameObject();
                    tiles[x, y].name = "Tile (" + x + ", " + y + ")";
                    if (y % 2 == 0)
                        tiles[x, y].transform.position = new Vector2(x, iy / 2);
                    else
                        tiles[x, y].transform.position = new Vector2(x + 0.5f, iy / 2);

                    // Adding a sprite to the gameobject:
                    SpriteRenderer sr = tiles[x, y].AddComponent<SpriteRenderer>();

					int spriteID = map[x, height - 1 - y];
					if (spriteID >= 15) // TODO: MAKE CONSTANT!
					{
						objectsInBuildingLayer[x, y] = new GameObject();
						objectsInBuildingLayer[x, y].name = "objectsInBuildingLayer (" + x + ", " + y + ")";
						if (y % 2 == 0)
							objectsInBuildingLayer[x, y].transform.position = new Vector2(x, iy / 2);
						else
							objectsInBuildingLayer[x, y].transform.position = new Vector2(x + 0.5f, iy / 2);

						SpriteRenderer oibl = objectsInBuildingLayer[x, y].AddComponent<SpriteRenderer>();
						oibl.sprite = groundTiles[spriteID];
						oibl.sortingLayerName = "Buildings";
						sr.sprite = groundTiles[0]; // TODO: HARDKODET GRESS UNDER BYGNINGER.
					}
					else if (spriteID >= 0)
					{
						sr.sortingLayerName = "Ground";
						sr.sprite = groundTiles[spriteID];
					}


                    // Placing the tile on on the map within the board gameobject:
                    tiles[x, y].transform.parent = this.transform;
                    //iy += 0.576f;
                    if (x == 5 && y == 5)
                    {
                        GameObject hero = heroPrefabs[UnityEngine.Random.Range(0, 2)];
                        hero.transform.position = new Vector2(x + 0.5f, y / 2 + 0.5f);
                        Instantiate(hero);
                    }
                }
                iy += YOFFSET; // 0.57747603833865814696485623003195f;
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
