using System;
using MapGenerator;
using UnityEngine;
using System.Collections.Generic;


namespace MapGeneratorTests
{
	public class TestTools
	{

		public static int[,] copy2DArray(int[,] array)
		{
			int[,] copy = new int[array.GetLength(0), array.GetLength(1)];
			for (int y = 0; y < array.GetLength(1); y++)
			{
				for (int x = 0; x < array.GetLength(0); x++)
				{
					copy[x, y] = array[x, y];
				}
			}
			return copy;
		}

		public static Region GenerateSquareDummyRegion(int startX, int startY, int width, int height)
		{
			List<Vector2> coordinates = new List<Vector2>();
			for (int y = startY; y < startY + height; y++)
				for (int x = startX; x < startX + width; x++)
					coordinates.Add(new Vector2(x, y));

			return new Region(coordinates, new Vector2(startX + (width / 2), startY + (height / 2)));
		}

		public static int[,] GenerateMapWithWall(int wallPosition, int width, int height)
		{
			int[,] map = new int[width, height];

			for (int y = 0; y < height; y++)
			{
				for (int x = 0; x < width; x++)
				{
					if (x == wallPosition)
						map[x, y] = MapMaker.WALL;
					else
						map[x, y] = MapMaker.GROUND;
				}
			}

			return map;
		}
	}
}
