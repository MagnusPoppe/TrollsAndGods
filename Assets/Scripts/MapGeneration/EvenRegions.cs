using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class EvenRegions : MonoBehaviour 
{
 
    int delta;
    const int NEIGHBOURS = 4;
    const int RANGE = 1;
    const int X = 0;
    const int Y = 1;

    int bounds;
    int area;
    int width, height;
    int[,] map;

    public EvenRegions(int delta, int width, int height, int bounds, int area, int[,] map) 
    {
        this.delta  = delta;
        this.area   = area;
        this.width  = width;
        this.height = height;
        this.bounds = bounds;
        this.map    = map;
    }

    public void Grow(int seedX, int seedY, int marker)
    {
        Queue<int[]> queue = new Queue<int[]>();
        int[] pos = { seedX, seedY };
        int[] last = pos;

        queue.Enqueue(pos);
        int currentArea = 0;

        while (queue.Count > 0) // isEmpty();
        {
            // DEQUEUE();
            pos = queue.Dequeue();

            // TESTING
            if (currentArea > area)
                break;
            if (map[pos[X], pos[Y]] == marker)
                continue;
            if ( !compareArea (last, pos, marker) )
                continue;

            map[pos[X], pos[Y]] = marker;
            currentArea++;

            // LEGGER TIL NABOER: 
            int[] right = { pos[X] + 1, pos[Y]     };
            int[] left  = { pos[X] - 1, pos[Y]     };
            int[] up    = { pos[X],     pos[Y] + 1 };
            int[] down  = { pos[X],     pos[Y] - 1 };

            queue.Enqueue(right);
            queue.Enqueue(left);
            queue.Enqueue(up);
            queue.Enqueue(down);
   
        }
    }

    bool testBounds( int[] pos )
    {
        return pos[Y] != 0 && pos[Y] != height - 1 && pos[X] != width - 1 && pos[X] != 0;
    }

    public void Grow1(int seedX, int seedY, int marker)
    {
        Queue<int[]> queue = new Queue<int[]>();
        int[] pos = { seedX, seedY };
        queue.Enqueue(pos);
        int currentArea = 0;

        while (queue.Count > 0) // isEmpty();
        {
            // TESTING AREA FOR SIZE:
            if (currentArea > area)
            {
                break;
            }

            // DEQUEUE();
            pos = queue.Dequeue();
            if (map[pos[X], pos[Y]] == marker)
                continue;
            
            map[pos[X], pos[Y]] = marker;
            currentArea++;

            // HVIS NABO ER BRA NOK FOR PREDIKATET, LEGG TIL:
            if (pos[X] != width - 1)
            {
                int[] next = { pos[X] + 1, pos[Y] };
                Debug.Log("pos = ("+pos[X]+","+pos[Y]+"), next = ("+next[X]+","+next[Y]+")");  
                if ( compareArea(pos, next, marker) )
                    queue.Enqueue(next);
            }

            if (pos[X] != 0)
            {
                int[] next = { pos[X] - 1, pos[Y] };
                if ( compareArea(pos, next, marker) )
                    queue.Enqueue(next);

            }
            if (pos[Y] != height-1)
            {
                int[] next = { pos[X], pos[Y] + 1};
                if ( compareArea(pos, next, marker) )
                    queue.Enqueue(next);

            }
            if (pos[Y] != 0)
            {
                int[] next = { pos[X], pos[Y] - 1 };
                if ( compareArea(pos, next, marker) )
                    queue.Enqueue(next);

            }


        }
    }



    bool compareArea( int[] here, int[] next, int marker )
    {
        return map[next[X], next[Y]] != marker && Mathf.Abs(getArea(here[X], here[Y]) - getArea(next[X], next[Y])) <= delta;
    }

    int getArea( int x, int y )
    {
        int sum = 0;

        if (map[x - 1, y] < 0|| map[x-1, y] >= bounds)  // LEFT
            sum += map[x -1, y];

        if (map[x + 1, y] < 0|| map[x+1, y] >= bounds)  // RIGHT
            sum += map[x +1, y];

        if (map[x, y-1]  < 0 || map[x, y-1] >= bounds)    // DOWN
            sum += map[x, y-1];

        if (map[x, y+1]  < 0 || map[x, y+1] >= bounds)    // UP
            sum += map[x, y+1]; 

        return sum / NEIGHBOURS;
    }
}