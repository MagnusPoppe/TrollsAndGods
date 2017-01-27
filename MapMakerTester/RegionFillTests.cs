using NUnit.Framework;
using System;
using MapGenerator;
using UnityEngine;

namespace MapGeneratorTests
{
	[TestFixture()]
	public class RegionFillTests
	{
		[Test()]
		public void FloodFillTester()
		{
			int width = 10;
			int height = 10;
			int[,] splitMap = new int[width, height];

			for (int y = 0; y < splitMap.GetLength(1); y++)
				splitMap[5, y] = 1;

			int[,] expected = copy2DArray(splitMap);
			for (int y = 0; y < height; y++)
			{
				for (int x = 0; x < width; x++)
				{
					if (x < 5) expected[x, y] = 2;
					else if (x > 5) expected[x, y] = 3;
				}
			}

			Vector2[] seeds = {
				new Vector2(2, 5),
				new Vector2(7, 5)
			};

			RegionFill r = new RegionFill(splitMap, seeds);

			int[,] actual = r.getMap();

			Assert.AreEqual(expected, actual);
		}

		private int[,] copy2DArray(int[,] array)
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
	}
}
