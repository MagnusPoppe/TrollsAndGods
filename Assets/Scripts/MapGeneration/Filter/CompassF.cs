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

        const int FILTERLENGTH_EVEN = 5;
        const int FILTERLENGTH_ODD  = 6;

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
        private static bool CorrectDirection(int[,] map,int filterlength, int[,] compass, int initX, int initY, int environment, int otherEnvironment)
        {
            int offset = filterlength / 2;

            bool failed = false; // If a filter does not match the specs, it will fail.

            for (int iy = 0; iy < filterlength; iy++)
            {
                for (int ix = 0; ix < filterlength; ix++)
                {
                    int x = initX + (ix - offset);
                    int y = initY + (iy - offset);

                    if (0 <= x && x < map.GetLength(0) && 0 <= y && y < map.GetLength(0))
                    {

                        // This test checks if the underlaying enviroment is the correct type where is should be.
                        if (compass[ix, iy] == 1) if (map[x, y] != environment) return false;
                        if (compass[ix, iy] == 2) if (map[x, y] != otherEnvironment) return false;
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

            if (y % 2 == 0)
            {
//                if (CorrectDirection(map,FILTERLENGTH_EVEN, NORTH, x, y, environment, environmentOther))
//                    return Direction.north;
//                else if (CorrectDirection(map,FILTERLENGTH_EVEN, NORTH_EAST, x, y, environment, environmentOther))
//                    return Direction.northEast;
//                else if (CorrectDirection(map,FILTERLENGTH_EVEN, EAST, x, y, environment, environmentOther))
//                    return Direction.east;
//                else if (CorrectDirection(map,FILTERLENGTH_EVEN,  SOUTH_EAST, x, y, environment, environmentOther))
//                    return Direction.southEast;
                if (CorrectDirection(map,FILTERLENGTH_EVEN,  SOUTH, x, y, environment, environmentOther))
                    return Direction.south;
//                else if (CorrectDirection(map,FILTERLENGTH_EVEN, SOUTH_WEST, x, y, environment, environmentOther))
//                    return Direction.southWest;
//                else if (CorrectDirection(map,FILTERLENGTH_EVEN,  WEST, x, y, environment, environmentOther))
//                    return Direction.west;
//                else if (CorrectDirection(map,FILTERLENGTH_EVEN, NORTH_WEST, x, y, environment, environmentOther))
//                    return Direction.northWest;
                else
                    return Direction.noDirection;
            }
            else
            {
//                if (CorrectDirection(map,FILTERLENGTH_EVEN, NORTH_ODD, x, y, environment, environmentOther))
//                    return Direction.north;
//                else if (CorrectDirection(map,FILTERLENGTH_EVEN, NORTH_EAST_ODD, x, y, environment, environmentOther))
//                    return Direction.northEast;
//                else if (CorrectDirection(map,FILTERLENGTH_EVEN, EAST_ODD, x, y, environment, environmentOther))
//                    return Direction.east;
//                else if (CorrectDirection(map,FILTERLENGTH_EVEN, SOUTH_EAST_ODD, x, y, environment, environmentOther))
//                    return Direction.southEast;
//                else if (CorrectDirection(map,FILTERLENGTH_EVEN, SOUTH_ODD, x, y, environment, environmentOther))
//                    return Direction.south;
//                else if (CorrectDirection(map,FILTERLENGTH_EVEN, SOUTH_WEST_ODD, x, y, environment, environmentOther))
//                    return Direction.southWest;
//                else if (CorrectDirection(map,FILTERLENGTH_EVEN, WEST_ODD, x, y, environment, environmentOther))
//                    return Direction.west;
//                else if (CorrectDirection(map,FILTERLENGTH_EVEN, NORTH_WEST_ODD, x, y, environment, environmentOther))
//                    return Direction.northWest;
//                else
                    return Direction.noDirection;
            }
        }

        const int ENVIRONMENT_TYPE_1 = 1;
        const int ENVIRONMENT_TYPE_2 = 2;

        // EVEN
		public static int[,] SOUTH = {
            {0,0,0,0,0},
            {0,2,0,0,0},
            {0,1,2,0,0},
            {0,1,2,0,0},
            {0,0,1,0,0}
		};
		public static int[,] SOUTH_WEST = {
            {0,0,0,0,0},
            {0,1,2,0,0},
            {0,1,2,0,0},
            {0,1,2,0,0},
            {0,0,0,0,0}
		};
		public static int[,] WEST = {
            {0,0,1,0,0},
            {0,1,2,0,0},
            {0,1,2,0,0},
            {0,2,0,0,0},
            {0,0,0,0,0}
		};
		public static int[,] NORTH_WEST = {
            {0,0,1,0,0},
            {0,1,1,0,0},
            {0,0,2,0,0},
            {0,2,2,0,0},
            {0,0,0,0,0}
		};
		public static int[,] NORTH = {
            {0,0,1,0,0},
            {0,2,1,0,0},
            {0,2,2,1,0},
            {0,2,2,0,0},
            {0,0,2,0,0}
		};
		public static int[,] NORTH_EAST = {
            {0,0,0,0,0},
            {0,2,1,0,0},
            {0,0,2,1,0},
            {0,2,1,0,0},
            {0,0,0,0,0}
		};
		public static int[,] EAST = {
            {0,0,0,0,0},
            {0,0,2,0,0},
            {0,0,2,1,0},
            {0,2,1,0,0},
            {0,0,1,0,0}
		};
		public static int[,] SOUTH_EAST = {
            {0,0,0,0,0},
            {0,2,2,0,0},
            {0,0,2,0,0},
            {0,1,1,0,0},
            {0,0,1,0,0}
		};

        // ODD
        public static int[,] SOUTH_ODD =
        {
            {0,0,0,0,0,0},
            {0,0,0,0,0,0},
            {0,0,2,0,0,0},
            {0,1,2,0,0,0},
            {0,0,1,2,0,0},
            {0,0,1,0,0,0}
        };
        public static int[,] SOUTH_WEST_ODD =
        {
            {0,0,0,0,0,0},
            {0,0,0,0,0,0},
            {0,0,1,0,0,0},
            {0,1,2,2,0,0},
            {0,0,1,2,0,0},
            {0,0,0,0,0,0}
        };
        public static int[,] WEST_ODD =
        {
            {0,0,0,0,0,0},
            {0,0,1,0,0,0},
            {0,0,1,2,0,0},
            {0,1,2,0,0,0},
            {0,0,2,0,0,0},
            {0,0,0,0,0,0}
        };
        public static int[,] NORTH_WEST_ODD =
        {
            {0,0,0,0,0,0},
            {0,0,1,0,0,0},
            {0,0,1,1,0,0},
            {0,0,2,0,0,0},
            {0,0,2,2,0,0},
            {0,0,0,0,0,0}
        };
        public static int[,] NORTH_ODD =
        {
            {0,0,0,0,0,0},
            {0,0,1,0,0,0},
            {0,0,2,1,0,0},
            {0,0,2,1,0,0},
            {0,0,0,2,0,0},
            {0,0,0,0,0,0}
        };
        public static int[,] NORTH_EAST_ODD =
        {
            {0,0,0,0,0,0},
            {0,0,0,0,0,0},
            {0,0,2,1,0,0},
            {0,0,2,1,0,0},
            {0,0,2,1,0,0},
            {0,0,0,0,0,0}
        };
        public static int[,] EAST_ODD =
        {
            {0,0,0,0,0,0},
            {0,0,0,0,0,0},
            {0,0,0,2,0,0},
            {0,0,2,1,0,0},
            {0,0,2,1,0,0},
            {0,0,1,0,0,0}
        };
        public static int[,] SOUTH_EAST_ODD =
        {
            {0,0,0,0,0,0},
            {0,0,0,0,0,0},
            {0,0,2,2,0,0},
            {0,0,2,0,0,0},
            {0,0,1,1,0,0},
            {0,0,1,0,0,0}
        };



//        // EVEN
//        public static int[,] SOUTH = {
//            {0,0,0,0,0},
//            {0,2,0,0,0},
//            {0,1,2,0,0},
//            {0,1,2,0,0},
//            {0,0,1,0,0}
//        };
//        public static int[,] SOUTH_WEST = {
//            {0,0,0,0,0},
//            {0,1,2,0,0},
//            {0,1,2,0,0},
//            {0,1,2,0,0},
//            {0,0,0,0,0}
//        };
//        public static int[,] WEST = {
//            {0,0,1,0,0},
//            {0,1,2,0,0},
//            {0,1,2,0,0},
//            {0,2,0,0,0},
//            {0,0,0,0,0}
//        };
//        public static int[,] NORTH_WEST = {
//            {0,0,1,0,0},
//            {0,1,1,0,0},
//            {0,0,2,0,0},
//            {0,2,2,0,0},
//            {0,0,0,0,0}
//        };
//        public static int[,] NORTH = {
//            {0,0,1,0,0},
//            {0,2,1,0,0},
//            {0,0,2,1,0},
//            {0,0,2,0,0},
//            {0,0,0,0,0}
//        };
//        public static int[,] NORTH_EAST = {
//            {0,0,0,0,0},
//            {0,2,1,0,0},
//            {0,0,2,1,0},
//            {0,2,1,0,0},
//            {0,0,0,0,0}
//        };
//        public static int[,] EAST = {
//            {0,0,0,0,0},
//            {0,0,2,0,0},
//            {0,0,2,1,0},
//            {0,2,1,0,0},
//            {0,0,1,0,0}
//        };
//        public static int[,] SOUTH_EAST = {
//            {0,0,0,0,0},
//            {0,2,2,0,0},
//            {0,0,2,0,0},
//            {0,1,1,0,0},
//            {0,0,1,0,0}
//        };
//
//        // ODD
//        public static int[,] SOUTH_ODD =
//            {
//                {0,0,0,0,0,0},
//                {0,0,0,0,0,0},
//                {0,0,2,0,0,0},
//                {0,1,2,0,0,0},
//                {0,0,1,2,0,0},
//                {0,0,1,0,0,0}
//            };
//        public static int[,] SOUTH_WEST_ODD =
//            {
//                {0,0,0,0,0,0},
//                {0,0,0,0,0,0},
//                {0,0,1,0,0,0},
//                {0,1,2,2,0,0},
//                {0,0,1,2,0,0},
//                {0,0,0,0,0,0}
//            };
//        public static int[,] WEST_ODD =
//            {
//                {0,0,0,0,0,0},
//                {0,0,1,0,0,0},
//                {0,0,1,2,0,0},
//                {0,1,2,0,0,0},
//                {0,0,2,0,0,0},
//                {0,0,0,0,0,0}
//            };
//        public static int[,] NORTH_WEST_ODD =
//            {
//                {0,0,0,0,0,0},
//                {0,0,1,0,0,0},
//                {0,0,1,1,0,0},
//                {0,0,2,0,0,0},
//                {0,0,2,2,0,0},
//                {0,0,0,0,0,0}
//            };
//        public static int[,] NORTH_ODD =
//            {
//                {0,0,0,0,0,0},
//                {0,0,1,0,0,0},
//                {0,0,2,1,0,0},
//                {0,0,2,1,0,0},
//                {0,0,0,2,0,0},
//                {0,0,0,0,0,0}
//            };
//        public static int[,] NORTH_EAST_ODD =
//            {
//                {0,0,0,0,0,0},
//                {0,0,0,0,0,0},
//                {0,0,2,1,0,0},
//                {0,0,2,1,0,0},
//                {0,0,2,1,0,0},
//                {0,0,0,0,0,0}
//            };
//        public static int[,] EAST_ODD =
//            {
//                {0,0,0,0,0,0},
//                {0,0,0,0,0,0},
//                {0,0,0,2,0,0},
//                {0,0,2,1,0,0},
//                {0,0,2,1,0,0},
//                {0,0,1,0,0,0}
//            };
//        public static int[,] SOUTH_EAST_ODD =
//            {
//                {0,0,0,0,0,0},
//                {0,0,0,0,0,0},
//                {0,0,2,2,0,0},
//                {0,0,2,0,0,0},
//                {0,0,1,1,0,0},
//                {0,0,1,0,0,0}
//            };
	}

}
