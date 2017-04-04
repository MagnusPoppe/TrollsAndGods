using MapGenerator;

namespace OverworldObjects
{
    public class Shapes
    {
        public const int NOTHING = -1;
        public const int SINGLE = 0;
        public const int DOUBLE_RIGHT = 1;
        public const int DOUBLE_LEFT = 2;
        public const int CUBE = 3;
        public const int TRIPLE = 4;
        public const int TRIPLEx2_RIGHT = 5;
        public const int TRIPLEx2_LEFT = 6;
        public const int TRIPLEx3_RIGHT = 7;
        public const int TRIPLEx3_LEFT = 8;
        public const int QUAD_LEFT = 9;
        public const int QUAD_RIGHT = 10;
        public const int QUADx2_RIGHT = 11;
        public const int QUADx2_LEFT = 12;

        public const int SHAPE_COUNT = 12;

        public static int[] dx = {-2, -1, 0, 1, 2};
        public static int[] dy = {-1, 0, 1, 2, 3};

        const int FILTER_SIZE = 5;

        /// <summary>
        /// Tests if the building fits inside the grid.
        /// </summary>
        /// <returns>The building fit.</returns>
        /// <param name="Position">Position.</param>
        /// <param name="canWalk">Can walk.</param>
        public static bool[] GetBuildingFit(Point Position, int[,] canWalk)
        {
            bool[] BuildingTypesFit = new bool[SHAPE_COUNT];

            if ((Position.x >= FILTER_SIZE / 2 && Position.x < canWalk.GetLength(1) - FILTER_SIZE / 2)
                && (Position.y >= FILTER_SIZE / 2 && Position.y < canWalk.GetLength(0) - FILTER_SIZE / 2))
            {
                for (int i = 0; i < SHAPE_COUNT; i++)
                    BuildingTypesFit[i] = fits(i, Position.x, Position.y, canWalk);
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
                    Point dxdy = new Point(dxx, dyy);

                    if (dxdy.InBounds(canWalk) && shape[x, y] == 1)
                        if (canWalk[dxx, dyy] == MapGenerator.MapMaker.CANNOTWALK)
                            return false;
                }
            }
            return true;
        }

        /// <summary>
        /// Determines whether a shape can fit over the specified Environment in a given map.
        /// </summary>
        /// <returns><c>true</c> if this instance can fit shape over the specified Environment initial shape map; otherwise, <c>false</c>.</returns>
        /// <param name="Environment">Environment.</param>
        /// <param name="initial">Initial.</param>
        /// <param name="shape">Shape.</param>
        /// <param name="map">Map.</param>
        public static bool CanFitShapeOver(int Environment, Point initial, int[,] shape, int[,] map)
        {
            for (int iy = 0; iy < shape.GetLength(0); iy++)
            {
                for (int ix = 0; ix < shape.GetLength(1); ix++)
                {
                    int x = initial.x + (ix - (shape.GetLength(1) / 2));
                    int y = initial.y + (iy - (shape.GetLength(0) / 2));

                    if (0 <= x && x < map.GetLength(0) && 0 <= y && y < map.GetLength(0))
                    {
                        if (shape[ix, iy] == 1)
                        {
                            if (map[x, y] != Environment)
                                return false;
                        }
                    }
                }
            }
            return true;
        }

        /// <summary>
        /// Creates an outline of a shape.
        /// </summary>
        /// <param name="shape">Shape to outline</param>
        /// <returns>2D array containing the outline.</returns>
        public static int[,] GetOutline(int[,] shape)
        {
            int[,] outline = new int[shape.GetLength(0),shape.GetLength(1)];

            for (int y = 0; y < shape.GetLength(0); y++)
            {
                for (int x = 0; x < shape.GetLength(1); x++)
                {
                    Point here = new Point(x, y);

                    if (IsEdge(here, shape)) outline[x, y] = 1;
                }
            }
            return outline;
        }

        /// <summary>
        /// Tests a spesific point for neighbours. If it finds a position
        /// where center is outside the shape, but there is a neighbour that is
        /// of the shape, it returns true.
        /// </summary>
        /// <param name="outer">Point to check for neighbours</param>
        /// <param name="shape">Underlying shape.</param>
        /// <returns>true if neighbour is found. else otherwise.</returns>
        private static bool IsEdge(Point outer, int[,] shape)
        {
            int[,] structure = new int[,]
            {
                {1, 1, 1},
                {1, 0, 1},
                {1, 1, 1}
            };

            // Using the structure filter:
            for (int innerY = 0; innerY < structure.GetLength(0); innerY++)
            {
                for (int innerX = 0; innerX < structure.GetLength(1); innerX++)
                {
                    if (!outer.InBounds(shape, structure)) continue;
                    if (shape[outer.x, outer.y] == 1) continue;// Inside shape. No good.

                    // Calculating the dx, dy for this point.
                    int dx = outer.x + (innerX - (structure.GetLength(1) / 2));
                    int dy = outer.y + (innerY - (structure.GetLength(1) / 2));

                    // Looking for edges:
                    if (shape[dx, dy] == 1 && structure[innerX, innerY] == 1)
                    {
                        return true; // Marking found edge.
                    }
                }
            }
            return false; // This point does not have neightbours.
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
                    {0,0,1,0,0},
                    {0,0,0,0,0},
                    {0,0,0,0,0},
                    {0,0,0,0,0}
                },{ // DOUBLE RIGHT
                    {0,0,0,0,0},
                    {0,0,1,0,0},
                    {0,0,0,1,0},
                    {0,0,0,0,0},
                    {0,0,0,0,0}
                },{ // DOUBLE LEFT
                    {0,0,0,0,0},
                    {0,0,1,0,0},
                    {0,0,1,0,0},
                    {0,0,0,0,0},
                    {0,0,0,0,0}
                }, { // CUBE
                    {0,0,0,0,0},
                    {0,0,1,0,0},
                    {0,0,1,1,0},
                    {0,0,1,0,0},
                    {0,0,0,0,0}
                }, { // TRIPLE
                    {0,0,0,0,0},
                    {0,0,1,0,0},
                    {0,0,1,0,0},
                    {0,0,1,0,0},
                    {0,0,0,0,0}
                }, { // TRIPLE x2 RIGHT
                    {0,0,0,0,0},
                    {0,0,1,0,0},
                    {0,0,1,1,0},
                    {0,0,1,1,0},
                    {0,0,0,1,0}
                }, { // TRIPLE x2 LEFT
                    {0,0,0,0,0},
                    {0,0,1,0,0},
                    {0,0,1,1,0},
                    {0,1,1,0,0},
                    {0,0,1,0,0}
                }, { // TRIPLE x3 RIGHT
                    {0,0,1,0,0},
                    {0,1,1,0,0},
                    {0,1,1,1,0},
                    {0,1,1,0,0},
                    {0,0,1,0,0}
                }, { // TRIPLE x3 LEFT
                    {0,0,0,1,0},
                    {0,0,1,1,0},
                    {0,0,1,1,1},
                    {0,0,1,1,0},
                    {0,0,0,1,0}
                }, { // QUAD LEFT
                    {0,0,0,0,0},
                    {0,0,0,1,0},
                    {0,0,0,1,0},
                    {0,0,1,0,0},
                    {0,0,1,0,0}
                }, { // QUAD RIGHT
                    {0,0,0,0,0},
                    {0,1,0,0,0},
                    {0,0,1,0,0},
                    {0,0,1,0,0},
                    {0,0,0,1,0}
                }, { // QUAD x2 RIGHT
                    {0,0,1,0,0},
                    {0,1,1,0,0},
                    {0,0,1,1,0},
                    {0,0,1,1,0},
                    {0,0,0,1,0}
                }, { // QUAD x2 LEFT
                    {0,0,0,1,0},
                    {0,0,1,1,0},
                    {0,0,1,1,0},
                    {0,1,1,0,0},
                    {0,0,1,0,0}
                }
            };
	}
}
