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

		Economy economy;

		TileRating[] coordinateValue;
		Vector2[] coordinates;
		Castle castle;

		/// <summary>
		/// Defines a region by a set of coordinates and its center position.
		/// Initializes a new instance of the <see cref="T:MapGenerator.Region"/> class.
		/// </summary>
		/// <param name="coordinateList">Coordinate list.</param>
		/// <param name="castlePos">Castle position.</param>
		public Region( List<Vector2> coordinateList, Vector2 castlePos, Economy economy )
		{
			castle = new Castle(castlePos);

			coordinates = new Vector2[coordinateList.Count];
			int i = 0;
			foreach (Vector2 c in coordinateList)
				coordinates[i++] = c;

			this.economy = economy;
		}

		/// <summary>
		/// Defines a region by a set of coordinates and its center position.
		/// Initializes a new instance of the <see cref="T:MapGenerator.Region"/> class.
		/// </summary>
		/// <param name="area">Area.</param>
		/// <param name="castlePos">Castle position.</param>
		public Region( Vector2[] area, Vector2 castlePos)
		{
			castle = new Castle(castlePos);
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

		public void classifyRegionTiles( int[,] canWalk )
		{
			// DEFINERER BYGGNINGSTYPEN TIL HVER TILE:
			Block[] blocks = new Block[GetArea()];
			for (int i = 0; i < GetArea(); i++)
			{
				blocks[i] = new Block(GetCastle().GetPosition(), coordinates[i], canWalk);
			}
			for (int i = 0; i < economy.woodMineCount; i++)
			{
				float minDistance = 10;
				float maxDistance = 12;

				for (int j = 0; j < GetArea(); j++)
				{
					float distance = blocks[j].GetDistanceFromCastle();

					if (blocks[j].CanPlaceBuilding(Shapes.QUAD01x3))
					{ 
						if (distance >= minDistance && distance <= maxDistance)
						{
							// PLACE WOODMINE();
							if (i > 0) // FINNES WOODMINE FRA FØR
							{
								if (woodMine != null)
									if (woodMine.GetRating() < blocks[j].GetRating())
										woodMine = blocks[j];
							}
							else
								woodMine = blocks[j]; // TODO LAG EGEN INDEX FOR DENNE RESSURSE
						}
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