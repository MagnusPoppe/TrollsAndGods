using System;
using System.Collections.Generic;
using UnityEngine;


namespace MapGenerator
{
    public class RegionFill
    {
        int width, height;
        int[,] map;
        public RegionFill(int[,] voronoiMap)
        {
            this.width = voronoiMap.GetLength(0);
            this.height = voronoiMap.GetLength(1);

            this.map = voronoiMap;

            int label = 2;
            for (int y = 0; y < height; y++)
            {
                for (int x = 0; x < width; x++)
                {
                    if (map[x,y] == 0) //UNMARKED
                    {
                        floodFill(x, y, label++);
                    }
                }
            }
        }

        void floodFill( int x, int y, int label )
        {
            Queue<Vector2> queue = new Queue<Vector2>();
            queue.Enqueue(new Vector2(x, y));

            while (queue.Count != 0)
            {
                Vector2 current = queue.Dequeue();
                x = (int)current.x;
                y = (int)current.y;

                if (x >= 0 && x < width && y >= 0 && y < height)
                {
                    if (map[x, y] == 0)
                    {
                        map[x, y] = label;
                        queue.Enqueue(new Vector2(x - 1, y    ));
                        queue.Enqueue(new Vector2(x + 1, y    ));
                        queue.Enqueue(new Vector2(x    , y - 1));
                        queue.Enqueue(new Vector2(x    , y + 1));
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

