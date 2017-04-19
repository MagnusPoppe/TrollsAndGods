using NUnit.Framework;
using System;
using MapGenerator;
using UnityEngine;
using System.Collections.Generic;
using OverworldObjects;

namespace MapGeneratorTests
{
	[TestFixture()]
	public class RegionFillTests
	{
		[Test()]
		public void FloodFillFromSeedTester()
		{
			int[,] map = new int[20, 20];

			for (int y = 0; y < map.GetLength(0); y++)
			{
				map[15, y] = MapMaker.WALL;
			}

			Vector2[] seeds = {
				new Vector2( map.GetLength(0)/2, 5 ),
				new Vector2( map.GetLength(0)/2, 16),
			};

			RegionFill regionfill = new RegionFill(map, seeds);
			Region[] actual = regionfill.GetRegions();
			Region expected = TestTools.GenerateSquareDummyRegion(0, 0, 15, 20);

			Assert.AreEqual(expected.GetArea(), actual[0].GetArea());

		}

		[Test()]
		public void FloodFillFromCastleTester()
		{
			int[,] map = new int[20, 20];

			for (int y = 0; y < map.GetLength(0); y++)
			{
				map[15, y] = MapMaker.WALL;
			}

			Castle[] castles = {
				new Castle(new Vector2( map.GetLength(0)/2, 5 ), MapMaker.FIRST_AVAILABLE_SPRITE),
				new Castle(new Vector2( map.GetLength(0)/2, 16), MapMaker.FIRST_AVAILABLE_SPRITE)
			};

			RegionFill regionfill = new RegionFill(map, castles);
			Region[] actual = regionfill.GetRegions();
			Region expected = TestTools.GenerateSquareDummyRegion(0, 0, 15, 20);

			Assert.AreEqual(expected.GetArea(), actual[0].GetArea());
		}


    }
}
