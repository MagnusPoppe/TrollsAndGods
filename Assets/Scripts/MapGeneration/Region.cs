using System;
using UnityEngine;
using OverworldObjects;
using System.Collections.Generic;
using OverworldObjects;

namespace MapGenerator
{
	public class Region
	{
		Block woodMine;
		List<OverworldBuilding> buildings;

		Economy economy;

		TileRating[] coordinateValue;
		Vector2[] coordinates;
		Castle castle;

		/// <summary>
		/// Defines a region by a set of coordinates and its center position.
		/// Initializes a new instance of the <see cref="T:MapGenerator.Region"/> class.
		/// </summary>
		/// <param name="coordinateList">Coordinate list.</param>
		/// <param name="regionCenter">Castle position.</param>
		public Region( List<Vector2> coordinateList, Vector2 regionCenter )
		{
            castle = new UnknownCastle(regionCenter, 0); // TODO: Set player dynamically 

            coordinates = new Vector2[coordinateList.Count];
			int i = 0;
			foreach (Vector2 c in coordinateList)
				coordinates[i++] = c;
				
			buildings = new List<OverworldBuilding>();
		}

		/// <summary>
		/// Defines a region by a set of coordinates and its center position.
		/// Initializes a new instance of the <see cref="T:MapGenerator.Region"/> class.
		/// </summary>
		/// <param name="area">Area.</param>
		/// <param name="regionCenter">Castle position.</param>
		public Region( Vector2[] area, Vector2 regionCenter)
		{
			castle = new UnknownCastle(regionCenter , 0);
			coordinates = area;
		}

		public Vector2[] GetCoordinates()
		{
			return coordinates;
		}

		/// <summary>
		/// Gets the area of the region. (AREAL)
		/// </summary>
		/// <returns>The area.</returns>
		public int GetArea()
		{
			return coordinates.Length;
		}

		/// <summary>
		/// Gets the castle belonging to the region.
		/// </summary>
		/// <returns>The castle.</returns>
		public Castle GetCastle()
		{
			return castle;
		}

		/// <summary>
		/// Sets the type of the castle for this region.
		/// </summary>
		/// <param name="id">Identifier.</param>
		public void SetCastleType(int id)
		{
			castle.SetEnvironment(id);
		}

		public List<OverworldBuilding> GetBuildings()
		{
			return buildings;
		}

		public void createEconomy(int[,] canWalk, Economy economy)
		{
			this.economy = economy;
			for (int i = 0; i < economy.oreMineCount; i++)
			{
				OreMine mine = new OreMine(0);
				PlaceResourceBuilding(mine, canWalk);
			}

			// TODO: COPY FOR ALL OTHER MINETYPES.
		}


		/// <summary>
		/// Resets the type of groundtile to the standard ground
		/// tile. This includes all tiles bigger than the reserved
		/// numbers.
		/// Definition of groundtile is found in MapMaker.GROUND
		/// </summary>
		/// <returns>Reset map</returns>
		/// <param name="map">Map.</param>
		public int[,] ResetRegionGroundTileType(int[,] map)
		{
			foreach (Vector2 v in coordinates)
			{
				if ( map[(int)v.x, (int)v.y] >= MapMaker.DIRT
				   || map[(int)v.x, (int)v.y] == MapMaker.GROUND)
					map[(int)v.x, (int)v.y] = MapMaker.GROUND;
			}
			return map;
		}

		/// <summary>
		/// Sets all ground tiles to a given type of tile.
		/// Definition of groundtile is found in MapMaker.GROUND
		/// </summary>
		/// <returns>The region ground tile type.</returns>
		/// <param name="groundTile">Ground tile.</param>
		/// <param name="map">Map.</param>
		public int[,] SetRegionGroundTileType(int groundTile, int[,] map)
		{
			foreach (Vector2 v in coordinates)
			{
				if (map[(int)v.x, (int)v.y] == MapMaker.GROUND )
					map[(int)v.x, (int)v.y] = groundTile;
			}
			return map;
		}

		/// <summary>
		/// Checks if the position is part of this region.
		/// </summary>
		/// <returns><c>true</c>, if position in region, <c>false</c> otherwise.</returns>
		/// <param name="Position">Position.</param>
		public bool IsPositionInRegion(Vector2 Position)
		{
			foreach (Vector2 coordinate in coordinates)
			{
				if (coordinate.Equals(Position))
					return true;
			}
			return false;
		}

		private Block[] RateRegionTiles(int[,] canWalk)
		{
			Block[] blocks = new Block[GetArea()];
			for (int i = 0; i < GetArea(); i++)
			{
				blocks[i] = new Block(GetCastle().GetPosition(), coordinates[i], canWalk);
			}
			return blocks;
		}


		public void PlaceResourceBuilding(ResourceBuilding building, int[,] canWalk )
		{
			Block[] blocks = RateRegionTiles(canWalk);

			// DEFINERER BYGGNINGSTYPEN TIL HVER TILE:
			for (int i = 0; i < economy.woodMineCount; i++)
			{
                float minDistance = building.MinDistFromTown;
				float maxDistance = building.MaxDistFromTown;

				for (int j = 0; j < GetArea(); j++)
				{
					float distance = blocks[j].GetDistanceFromCastle();

					if (blocks[j].CanPlaceBuilding(building.ShapeType))
					{
						if (distance >= minDistance && distance <= maxDistance)
						{
							// KLAR TIL Å PLASSERE
							building.Origo = (blocks[j].GetPosition());
							building.FilpCanWalk(canWalk);

							// Plasserer bygning.
							buildings.Add(building);
						}
						// TODO: CANT PLACE.
					}
				}
			}
		}

		/// <summary>
		/// Fills the region with water.
		/// Replaces walkable area with water and 
		/// not walkable area turns into land (regular ground).
		/// </summary>
		/// <param name="map">Map.</param>
		public void FillRegionWithWater(int[,] map)
		{
			for (int i = 0; i < coordinates.Length; i++)
			{
				int x = (int)coordinates[i].x;
				int y = (int)coordinates[i].y;

				if (map[x, y] == MapMaker.GROUND)
				{
					map[x, y] = MapMaker.WATER;
				}
				else if (map[x, y] == MapMaker.CASTLE)
				{
					map[x, y] = MapMaker.WATER;
				}
				else if (map[x, y] == MapMaker.BUILDING)
				{
					map[x, y] = MapMaker.WATER;
				}

				else if (map[x, y] == MapMaker.WOODS)
				{
					map[x, y] = MapMaker.GROUND;
				}
			}
		}

		/// <summary>
		/// Gets the wood mine.
		/// </summary>
		/// <returns>The wood mine.</returns>
		public Block GetWoodMine()
		{
			return woodMine;
		}

	}
}