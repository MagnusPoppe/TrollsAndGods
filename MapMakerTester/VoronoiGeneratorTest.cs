using NUnit.Framework;
using System;
using MapGenerator;
using UnityEngine;

namespace MapGeneratorTests
{
	[TestFixture()]
	public class VoronoiTests
	{
		[Test()]
		public void VoronoiGeneratorTest()
		{
			int width = 100;
			int height = 100;
			Vector2[] pkt = {
				new Vector2(25, 50),
				new Vector2(75, 50)
			};
			VoronoiGenerator generator = new VoronoiGenerator(
				width, height, pkt, 5
			);

			int[] unique = new int[100];

			RegionFill r = new RegionFill(generator.GetMap(), pkt);
			int[,] map = r.getMap();

			for (int y = 0; y < map.GetLength(1); y++) 
			{
				for (int x = 0; x < map.GetLength(0); x++)
				{
					unique[map[x,y]]++;
				}
			}

			int actual = 0;
			for (int i = 0; i < unique.Length; i++)
			{
				if (unique[i] > 0) actual++;
			}

			Console.WriteLine(unique);

			// Forventer å få en unik farge for hver sone, og en for border
			int expected = pkt.Length + 2;

			Assert.AreEqual(expected, actual);
		}
	}
}
