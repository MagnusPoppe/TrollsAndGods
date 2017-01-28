using System;
using MapGenerator;
using Overworld;
using UnityEngine;
using NUnit.Framework;

namespace MapGeneratorTests
{
	[TestFixture()]
	public class MapMakerTests
	{
		[Test()]
		public void ConstructorSquareMapTest()
		{
			int widthXHeight 			= 128;
			string seed 				= "Angelica";
			int fillpercentWalkable 	= 57;
			int sites 					= 5;
			int relaxIterations 		= 1;
			int smoothIterations 		= 5;
			int NumberOFSprites 		= 8;

			int width  = widthXHeight;
			int height = widthXHeight;

			MapMaker mapmaker = new MapMaker(
				width, height, NumberOFSprites,              	// Map Properites
				seed, fillpercentWalkable, smoothIterations,    // BinaryMap Properities
				sites, relaxIterations                          // Voronoi Properties
			);

			int[,] map = mapmaker.GetMap();

			int groundCount = 0;
			int wallCount 	= 0;
			int castleCount = 0;
			int other 		= 0;

			for (int y = 0; y < height; y++)
			{
				for (int x = 0; x < width; x++)
				{
					if (map[x, y] < 0)
						Assert.Fail("Failed due to illegal map value. Value in map below zero(0).");
					else if (map[x, y] == MapMaker.GROUND)
						groundCount++;
					else if (map[x, y] == MapMaker.WALL)
						wallCount++;
					else if (map[x, y] == MapMaker.CASTLE)
						castleCount++;
					else
						other++;
				}
			}

			// Tester for om hele kartet kun består av en type tile:
			Assert.AreNotEqual(groundCount, map.GetLength(0) * map.GetLength(1));
			Assert.AreNotEqual(wallCount, 	map.GetLength(0) * map.GetLength(1));
			Assert.AreNotEqual(castleCount, map.GetLength(0) * map.GetLength(1));
		}



	}
}
