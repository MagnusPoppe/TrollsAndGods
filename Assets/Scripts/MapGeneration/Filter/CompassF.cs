using System;
namespace Filter
{
	public class CompassF
	{
		public enum Directions
		{
			north,
			northEast,
			east,
			southEast,
			south,
			southWest,
			west,
			northWest
		};

		public static int[,] SOUTH = {
			{1,1,1},
			{0,0,0},
			{0,0,0}
		};
		public static int[,] SOUTH_WEST = {
			{0,1,1},
			{0,0,1},
			{0,0,0}
		};
		public static int[,] WEST = {
			{0,0,1},
			{0,0,1},
			{0,0,1}
		};
		public static int[,] NORTH_WEST = {
			{0,0,0},
			{0,0,1},
			{0,1,1}
		};
		public static int[,] NORTH = {
			{0,0,0},
			{0,0,0},
			{1,1,1}
		};
		public static int[,] NORTH_EAST = {
			{0,0,0},
			{1,0,0},
			{1,1,0}
		};
		public static int[,] EAST = {
			{1,0,0},
			{1,0,0},
			{1,0,0}
		};
		public static int[,] SOUTH_EAST = {
			{1,1,0},
			{1,0,0},
			{0,0,0}
		};
	}

}
