using System;
using NUnit.Framework;
using MapGenerator;
using UnityEngine;
using System.Collections.Generic;

namespace MapGeneratorTests
{
	[TestFixture()]
	public class RegionTests
	{
		[Test()]
		public void ListConstructorTest()
		{
			Region r1 = TestTools.GenerateSquareDummyRegion(30, 30, 20, 20);
			Region r2 = TestTools.GenerateSquareDummyRegion(30, 30, 20, 20);

			Assert.AreNotSame(r2, r1);
			Assert.AreEqual(r2.GetArea(), r1.GetArea());
			Assert.AreEqual(r1.GetCastle().GetPosition(), r2.GetCastle().GetPosition());
		}

		[Test()]
		public void ResetRegionGroundTileTypeTest()
		{
			int[,] map = TestTools.GenerateMapWithWall(10, 20, 20);

			for (int y = 0; y < 20; y++)
			{
				for (int x = 0; x < 20; x++)
				{
					if (x > 10) map[x, y] = MapMaker.FIRST_AVAILABLE_SPRITE;
				}
			}

			int[,] otherMap = TestTools.copy2DArray(map);

			Region r1 = TestTools.GenerateSquareDummyRegion(0, 0, 20, 20);

			r1.ResetRegionGroundTileType(otherMap);

			for (int y = 0; y < 20; y++)
			{
				for (int x = 0; x < 20; x++)
				{
					if (x <= 10)
						Assert.AreEqual(map[x, y], otherMap[x, y]);
					else if (x > 10)
						Assert.AreNotEqual(map[x, y], otherMap[x, y]);
				}
			}

		}

		[Test()]
		public void SetRegionGroundTileTypeTest()
		{
			int[,] map = TestTools.GenerateMapWithWall(10, 20, 20);

			for (int y = 0; y < 20; y++)
			{
				for (int x = 0; x < 20; x++)
				{
					if (x > 10) map[x, y] = MapMaker.FIRST_AVAILABLE_SPRITE;
				}
			}

			int[,] otherMap = TestTools.copy2DArray(map);

			Region r1 = TestTools.GenerateSquareDummyRegion(5, 5, 5, 5);

			r1.SetRegionGroundTileType(MapMaker.FIRST_AVAILABLE_SPRITE+1, otherMap);

			for (int y = 0; y < 20; y++)
			{
				for (int x = 0; x < 20; x++)
				{
					if (x >= 5 && x < 10 && y >= 5 && y < 10)
					{
						Assert.AreNotEqual(map[x, y], otherMap[x, y]);
					}
				}
			}

			Assert.AreEqual(map[2, 2], otherMap[2, 2]);

		}

		[Test()]
		public void IsPositionInRegionTest()
		{
			Region r = TestTools.GenerateSquareDummyRegion(10, 10, 50, 50);

			Vector2 position = new Vector2(15, 15);
			Assert.AreEqual(true, r.IsPositionInRegion(position));

			position = new Vector2(10, 10);
			Assert.AreEqual(true, r.IsPositionInRegion(position));

			position = new Vector2(9, 9);
			Assert.AreNotEqual(true, r.IsPositionInRegion(position));
		}
	}
}
