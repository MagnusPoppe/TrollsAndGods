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

		public RegionFill(int[,] voronoiMap, Vector2[] seeds )
        {
            this.map = voronoiMap;

            this.height = map.GetLength(1);
            this.width = map.GetLength(0);

            int label = 3;
			for (int i = 0; i < seeds.Length; i++)
            {
                if (map[(int)seeds[i].x,(int)seeds[i].y] == 0) //UNMARKED
                {
                     floodFill(seeds[i], label++);
                }
            }
        }

		public RegionFill(int[,] voronoiMap, Castle[] castles)
		{
			this.map = voronoiMap;

            this.height = map.GetLength(1);
            this.width = map.GetLength(0);

			for (int i = 0; i < castles.Length; i++)
				floodFill(castles[i].GetPosition(), castles[i].GetEnvironment());
			
		}

		void floodFill(Vector2 seed, int label)
		{
			Queue<Vector2> queue = new Queue<Vector2>();
			queue.Enqueue(seed);

			fill(queue, label);
		}

        void floodFill( int x, int y, int label )
        {
            Queue<Vector2> queue = new Queue<Vector2>();
            queue.Enqueue(new Vector2(x, y));

			fill(queue, label);
        }

		private void fill(Queue<Vector2> queue, int label)
		{
			int debug_queue_size = 0;
			Vector2 saved = queue.Peek();

			while (queue.Count != 0)
			{
				
				Vector2 current = queue.Dequeue();
				int x = (int)current.x;
				int y = (int)current.y;

				if (x >= 0 && x < width && y >= 0 && y < height)
				{
					if (map[x, y] == 0)
					{
						debug_queue_size++;
						map[x, y] = label;
						queue.Enqueue(new Vector2(x - 1, y));
						queue.Enqueue(new Vector2(x + 1, y));
						queue.Enqueue(new Vector2(x, y - 1));
						queue.Enqueue(new Vector2(x, y + 1));
					}
					else if (map[x, y] == 2)
					{
						debug_queue_size++;
						queue.Enqueue(new Vector2(x - 1, y));
						queue.Enqueue(new Vector2(x + 1, y));
						queue.Enqueue(new Vector2(x, y - 1));
						queue.Enqueue(new Vector2(x, y + 1));
					}
				}
 			}
		}

        public int[,] getMap()
        {
            return map;
        }
    }
}

