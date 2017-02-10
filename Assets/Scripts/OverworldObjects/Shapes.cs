using System;
using UnityEngine;
namespace OverworldObjects
{
	public class Shapes
	{
		public const int NOTHING 	= -1;
		public const int SINGLE 	= 0;
		public const int DOUBLE01 	= 1;
		public const int DOUBLE02 	= 2;
		public const int CUBE01 	= 3;
		public const int CUBE02 	= 4;
		public const int TRIPLE 	= 5;
		public const int TRIPLEx2 	= 6;
		public const int TRIPLEx3 	= 7;
		public const int QUAD01 	= 8;
		public const int QUAD02 	= 9;
		public const int QUAD01x2 	= 10;
		public const int QUAD02x2 	= 11;
		public const int QUAD01x3 	= 12;
		public const int QUAD02x3 	= 13;

		public static int[] dx = { -2, -1, 0, 1, 2 };
		public static int[] dy = { 1, 0, -1, -2, -3 };

		const int FILTER_SIZE = 5;

		/// <summary>
		/// Tests if the building fits inside the grid.
		/// </summary>
		/// <returns>The building fit.</returns>
		/// <param name="Position">Position.</param>
		/// <param name="canWalk">Can walk.</param>
		public static bool[] GetBuildingFit(Vector2 Position, int[,] canWalk)
		{
			int x = (int)Position.x;
			int y = (int)Position.y;
			bool[] BuildingTypesFit = new bool[QUAD02x3];


			if ((x >= FILTER_SIZE/2 && x < canWalk.GetLength(1)-FILTER_SIZE/2) 
			&&  (y >= FILTER_SIZE/2 && y < canWalk.GetLength(0)-FILTER_SIZE/2))
			{
				for (int i = 0; i < QUAD02x3; i++)
					BuildingTypesFit[i] = fits(i, x, y, canWalk);
			}
			return BuildingTypesFit;
		}

		/// <summary>
		/// Fits a building within the area.
		/// </summary>
		/// <param name="shapeType">The index.</param>
		/// <param name="x">The x coordinate.</param>
		/// <param name="y">The y coordinate.</param>
		/// <param name="canWalk">Can walk.</param>
		private static bool fits(int shapeType, int ekteX, int ekteY, int[,] canWalk)
		{
			int[,] shape = GetShape(shapeType);

			for (int y = 0; y < FILTER_SIZE; y++)
			{
				for (int x = 0; x < FILTER_SIZE; x++)
				{
					int dxx = ekteX + dx[x];
					int dyy = ekteY + dy[y];

					if (shape[x, y] == 1)
						if (canWalk[dxx, dyy] == MapGenerator.MapMaker.CANNOTWALK)
							return false;
				}
			}
			return true;
		}

		/// <summary>
		/// Gets the shape from the "ALL SHAPES" table using a given index.
		/// </summary>
		/// <returns>The shape.</returns>
		/// <param name="shapeType">Shape type (CONSTANTS).</param>
		public static int[,] GetShape(int shapeType)
		{
			int[,] a = new int[FILTER_SIZE, FILTER_SIZE];

			for (int y = 0; y < FILTER_SIZE; y++)
			{
				for (int x = 0; x < FILTER_SIZE; x++)
				{
					a[x, y] = ALL_SHAPES[shapeType, x, y];
				}
			}

			// TODO FIKS TABELLEN SLIK AT DU IKKE TRENGER ROTERE.
			return rotateGrid90(a);
		}


		private static int[,] rotateGrid90(int[,] grid)
		{
			int maxX = grid.GetLength(0);
			int maxY = grid.GetLength(1);
			int[,] copy = new int[maxX, maxY];

			for (int y = 0; y < maxY; y++)
			{
				for (int x = 0; x < maxX; x++)
				{
					copy[x, maxY - 1 - y] = grid[y, x];
				}
			}
			return copy;
		}

		private static int[,,] ALL_SHAPES =
		{
			{ // SINGLE
				{0,0,0,0,0},
				{0,0,0,0,0},
				{0,0,0,0,0},
				{0,0,1,0,0},
				{0,0,0,0,0}
			},{ // DOUBLE 01
				{0,0,0,0,0},
				{0,0,0,0,0},
				{0,0,0,0,0},
				{0,0,1,1,0},
				{0,0,0,0,0}
			},{ // DOUBLE 02
				{0,0,0,0,0},
				{0,0,0,0,0},
				{0,0,0,0,0},
				{0,1,1,0,0},
				{0,0,0,0,0}
			},{ // CUBE 01
				{0,0,0,0,0},
				{0,0,0,0,0},
				{0,1,1,0,0},
				{0,1,1,0,0},
				{0,0,0,0,0}
			}, { // CUBE 02
				{0,0,0,0,0},
				{0,0,0,0,0},
				{0,0,1,1,0},
				{0,0,1,1,0},
				{0,0,0,0,0}
			}, { // TRIPLE
				{0,0,0,0,0},
				{0,0,0,0,0},
				{0,0,0,0,0},
				{0,1,1,1,0},
				{0,0,0,0,0}
			}, { // TRIPLE x2
				{0,0,0,0,0},
				{0,0,0,0,0},
				{0,1,1,1,0},
				{0,1,1,1,0},
				{0,0,0,0,0}
			}, { // TRIPLE x3
				{0,0,0,0,0},
				{0,1,1,1,0},
				{0,1,1,1,0},
				{0,1,1,1,0},
				{0,0,0,0,0}
			}, { // QUAD 01
				{0,0,0,0,0},
				{0,0,0,0,0},
				{0,0,0,0,0},
				{1,1,1,1,0},
				{0,0,0,0,0}
			}, { // QUAD 02
				{0,0,0,0,0},
				{0,0,0,0,0},
				{0,0,0,0,0},
				{0,1,1,1,1},
				{0,0,0,0,0}
			}, { // QUAD 01 x2
				{0,0,0,0,0},
				{0,0,0,0,0},
				{1,1,1,1,0},
				{1,1,1,1,0},
				{0,0,0,0,0}
			}, { // QUAD 02 x2
				{0,0,0,0,0},
				{0,0,0,0,0},
				{0,1,1,1,1},
				{0,1,1,1,1},
				{0,0,0,0,0}
			}, { // QUAD 01 x3
				{0,0,0,0,0},
				{1,1,1,1,0},
				{1,1,1,1,0},
				{1,1,1,1,0},
				{0,0,0,0,0}
			}, { // QUAD 02 x3
				{0,0,0,0,0},
				{0,1,1,1,1},
				{0,1,1,1,1},
				{0,1,1,1,1},
				{0,0,0,0,0}
			}
		};
	}
}
