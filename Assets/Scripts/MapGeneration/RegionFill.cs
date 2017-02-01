using System;
using System.Collections.Generic;
using UnityEngine;
using OverworldObjects;

namespace MapGenerator
{
    public class RegionFill
    {
        int width, height;
        int[,] map;
		Region[] regions;
		int regionindex;

		/// <summary>
		/// Takes the voronoi generated map and fills in the regions based
		/// on random colors.
		/// Initializes a new instance of the <see cref="T:MapGenerator.RegionFill"/> class.
		/// </summary>
		/// <param name="voronoiMap">Voronoi map.</param>
		/// <param name="seeds">Seeds.</param>
		public RegionFill(int[,] voronoiMap, Vector2[] seeds )
        {
            this.map = voronoiMap;

            this.height = map.GetLength(1);
            this.width = map.GetLength(0);

			regions = new Region[seeds.Length];
			regionindex = 0;

            int label = 3;
			for (int i = 0; i < seeds.Length; i++)
			{
				if (map[(int)seeds[i].x,(int)seeds[i].y] == 0) //UNMARKED
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

			for (int i = 0; i < castles.Length; i++)
				FloodFill(castles[i].GetPosition(), castles[i].GetEnvironment());
			
		}

		/// <summary>
		/// Starts a flooding by using a Vector2 seed. 
		/// </summary>
		/// <param name="seed">Seed.</param>
		/// <param name="label">Label for the point.</param>
		void FloodFill(Vector2 seed, int label)
		{
			Queue<Vector2> queue = new Queue<Vector2>();
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
            Queue<Vector2> queue = new Queue<Vector2>();
            queue.Enqueue(new Vector2(x, y));

			Fill(queue, label);
        }

		/// <summary>
		/// Fill the specified area around the seed found 
		/// in the initalized queue with a given label.
		/// </summary>
		/// <param name="queue">Queue.</param>
		/// <param name="label">Label.</param>
		private void Fill(Queue<Vector2> queue, int label)
		{
			List<Vector2> region = new List<Vector2>();
			Vector2 center = queue.Peek();

			while (queue.Count != 0)
			{
				
				Vector2 current = queue.Dequeue();
				int x = (int)current.x;
				int y = (int)current.y;

				// Checking if inbounds
				if (x >= 0 && x < width && y >= 0 && y < height)
				{
					// Checking if  ground or castle
					if (map[x, y] == MapMaker.GROUND || map[x, y] == MapMaker.CASTLE)
					{
						// Labeling:
						map[x, y] = label;

						// Creating region:
						region.Add(current);

						// Adding neighbours to queue
						queue.Enqueue(new Vector2(x - 1, y));
						queue.Enqueue(new Vector2(x + 1, y));
						queue.Enqueue(new Vector2(x, y - 1));
						queue.Enqueue(new Vector2(x, y + 1));
					}
				}
 			}
			Economy e = new Economy(Economy.ABUNDANT);
			regions[regionindex++] = new Region(region, center, e);
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

