using System.Collections.Generic;
using OverworldObjects;

namespace MapGenerator
{
    public class RegionFill
    {
        int width, height;
        int[,] map;
		Region[] regions;
		int regionindex;
        public const int DEFAULT_LABEL_START = 3;
		/// <summary>
		/// Takes the voronoi generated map and fills in the regions based
		/// on random colors.
		/// Initializes a new instance of the <see cref="T:MapGenerator.RegionFill"/> class.
		/// </summary>
		/// <param name="voronoiMap">Voronoi map.</param>
		/// <param name="seeds">Seeds.</param>
		public RegionFill(int[,] voronoiMap, Point[] seeds)
        {
            this.map = voronoiMap;

            this.height = map.GetLength(1);
            this.width = map.GetLength(0);

			regions = new Region[seeds.Length];
			regionindex = 0;

            int label = DEFAULT_LABEL_START;
            for (int i = 0; i < seeds.Length; i++)
			{
				if (map[seeds[i].x, seeds[i].y] == 0) //UNMARKED
                {
                     FloodFill(seeds[i], label++);
                }
            }
        }

		/// <summary>
		/// Takes the voronoi generated map and fills in the regions based
		/// on the castles own color.
		/// Initializes a new instance of the <see cref="T:MapGenerator.RegionFill"/> class.
		/// </summary>
		/// <param name="voronoiMap">Voronoi map.</param>
		/// <param name="castles">castles.</param>
		public RegionFill(int[,] voronoiMap, Castle[] castles)
		{
			this.map = voronoiMap;

            this.height = map.GetLength(1);
            this.width = map.GetLength(0);

			regions = new Region[castles.Length];
			regionindex = 0;

			int label = DEFAULT_LABEL_START;
			for (int i = 0; i < castles.Length; i++)
			{
				FloodFill(castles[i].GetPosition(), label++);
			}
		}

		/// <summary>
		/// Starts a flooding by using a Vector2 seed. 
		/// </summary>
		/// <param name="seed">Seed.</param>
		/// <param name="label">Label for the point.</param>
        void FloodFill(Point seed, int label)
		{
			Queue<Point> queue = new Queue<Point>();
			queue.Enqueue(seed);

			Fill(queue, label);
		}

		/// <summary>
		/// Starts a flooding by using an x, y coordinate set. 
		/// </summary>
		/// <param name="x">Seed coordnate x.</param>
		/// <param name="y">Seed coordnate y.</param>
		/// <param name="label">Label for the point.</param>
		void FloodFill( int x, int y, int label )
        {
            Queue<Point> queue = new Queue<Point>();
            queue.Enqueue(new Point(x, y));

			Fill(queue, label);
        }

		/// <summary>
		/// Fill the specified area around the seed found 
		/// in the initalized queue with a given label.
		/// </summary>
		/// <param name="queue">Queue.</param>
		/// <param name="label">Label.</param>
		private void Fill(Queue<Point> queue, int label)
		{
			List<Point> region = new List<Point>();
			Point center = queue.Peek();

			while (queue.Count != 0)
			{				
                Point here = queue.Dequeue();

				// Checking if inbounds
				if (here.x >= 0 && here.x < width && here.y >= 0 && here.y < height)
				{
					// Checking if  ground or castle
					if (map[here.x, here.y] == MapMaker.GROUND || map[here.x, here.y] == MapMaker.REGION_CENTER)
					{
						// Labeling:
						map[here.x, here.y] = label;

						// Creating region:
						region.Add(here);

						// Adding neighbours to queue
                        queue.Enqueue(new Point(here.x - 1, here.y));
                        queue.Enqueue(new Point(here.x + 1, here.y));
                        queue.Enqueue(new Point(here.x, here.y - 1));
                        queue.Enqueue(new Point(here.x, here.y + 1));
					}
				}
 			}
			regions[regionindex++] = new Region(region, center);
		}

		/// <summary>
		/// Returns the map created by this algorithm.
		/// </summary>
		/// <returns>The map.</returns>
        public int[,] GetMap()
        {
            return map;
        }

		/// <summary>
		/// Gets the regions generated.
		/// </summary>
		/// <returns>The regions.</returns>
		public Region[] GetRegions()
		{
			return regions;
		}
    }
}

