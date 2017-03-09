using System;
namespace Filter
{
	public class CompassF
	{
		public enum Direction
		{
			north,
			northEast,
			east,
			southEast,
			south,
			southWest,
			west,
			northWest,
            noDirection
		};

        const int FILTERLENGTH = 5;

        /// <summary>
        /// Checks if this compass matches a given direction between to tilesets. 
        /// 
        /// It uses to environment types in the form of their "Global spriteID" to check
        /// if there is a clear divide between them matching the filter type.
        /// </summary>
        /// <returns><c>true</c>, if the direction matched, <c>false</c> otherwise.</returns>
        /// <param name="map">Main map</param>
        /// <param name="compass">Compass to check with</param>
        /// <param name="oppositeCompass">Compass opposite of the other compass, example: opposite of "north" is "south"</param>
        /// <param name="initX">Initial x.</param>
        /// <param name="initY">Initial y.</param>
        /// <param name="environment1">Environment type1, this is tested against.</param>
        private static bool CorrectDirection(int[,] map, int[,] compass, int[,] oppositeCompass, int initX, int initY, int environment, int otherEnvironment)
        {
            int offset = FILTERLENGTH / 2;

            bool failed = false; // If a filter does not match the specs, it will fail.

            for (int iy = 0; iy < FILTERLENGTH; iy++)
            {
                for (int ix = 0; ix < FILTERLENGTH; ix++)
                {
                    int x = initX + (ix - offset);
                    int y = initY + (iy - offset);

                    if (0 <= x && x < map.GetLength(0) && 0 <= y && y < map.GetLength(0))
                    {

                        // This test checks if the underlaying enviroment is the correct type where is should be.
                        if (compass[ix, iy] == 1)         if (map[x, y] != environment) return false;
                        if (oppositeCompass[ix, iy] == 1) if (map[x, y] != otherEnvironment) return false;
                    }
                }
            }
            return true;
        }

        /// <summary>
        /// Finds the correct direction.
        /// </summary>
        /// <returns>The direction.</returns>
        /// <param name="map">Map.</param>
        /// <param name="x">The x coordinate.</param>
        /// <param name="y">The y coordinate.</param>
        /// <param name="environment">Environment.</param>
        /// <param name="environmentOther">Environment other.</param>
        public static Direction GetDirection(int[,] map, int x, int y, int environment, int environmentOther)
        {
            if (CorrectDirection(map, NORTH, SOUTH, x, y, environment, environmentOther))
                return Direction.north;
            else if (CorrectDirection(map, NORTH_EAST,SOUTH_WEST, x, y, environment, environmentOther))
                return Direction.northEast;
            else if (CorrectDirection(map, EAST,WEST, x, y, environment, environmentOther))
                return Direction.east;
            else if (CorrectDirection(map, SOUTH_EAST,NORTH_WEST, x, y, environment, environmentOther))
                return Direction.southEast;
            else if (CorrectDirection(map, SOUTH,NORTH, x, y, environment, environmentOther))
                return Direction.south;
            else if (CorrectDirection(map, SOUTH_WEST, NORTH_EAST, x, y, environment, environmentOther))
                return Direction.southWest;
            else if (CorrectDirection(map, WEST, EAST, x, y, environment, environmentOther))
                return Direction.west;
            else if (CorrectDirection(map, NORTH_WEST,SOUTH_EAST, x, y, environment, environmentOther))
                return Direction.northWest;
            else
                return Direction.noDirection;
        }

        const int ENVIRONMENT_TYPE_1 = 1;
        const int ENVIRONMENT_TYPE_2 = 2;

		public static int[,] SOUTH = {
            {0,0,0,0,0},
            {0,0,0,0,0},
            {0,1,0,0,0},
            {0,1,0,0,0},
            {0,0,1,0,0}
		};
		public static int[,] SOUTH_WEST = {
            {0,0,0,0,0},
            {0,1,0,0,0},
            {0,1,0,0,0},
            {0,1,0,0,0},
            {0,0,0,0,0}
		};
		public static int[,] WEST = {
            {0,0,1,0,0},
            {0,1,0,0,0},
            {0,1,0,0,0},
            {0,0,0,0,0},
            {0,0,0,0,0}
		};
		public static int[,] NORTH_WEST = {
            {0,0,1,0,0},
            {0,1,0,1,0},
            {0,0,0,0,0},
            {0,0,0,0,0},
            {0,0,0,0,0}
		};
		public static int[,] NORTH = {
            {0,0,1,0,0},
            {0,0,1,0,0},
            {0,2,0,1,0},
            {0,2,0,0,0},
            {0,0,2,0,0}
		};
		public static int[,] NORTH_EAST = {
            {0,0,0,0,0},
            {0,0,1,0,0},
            {0,0,0,1,0},
            {0,0,1,0,0},
            {0,0,0,0,0}
		};
		public static int[,] EAST = {
            {0,0,0,0,0},
            {0,0,0,0,0},
            {0,0,0,1,0},
            {0,0,1,0,0},
            {0,0,1,0,0}
		};
		public static int[,] SOUTH_EAST = {
            {0,0,0,0,0},
            {0,0,0,0,0},
            {0,0,0,0,0},
            {0,1,1,0,0},
            {0,0,1,0,0}
		};
	}

}
