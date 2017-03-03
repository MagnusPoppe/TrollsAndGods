using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Linq;

/// <summary>
/// The AstarAlgo class exists to calculate the shortest possible path from start to end
/// </summary>
public class AStarAlgo {


    int[,] canWalk;
    Node[,] nodes;
    int width, height;
    protected bool hex;
    readonly Point[] evenIsometricDirections = {
                        new Point(0,2),
        new Point(-1,0),                 new Point(1,0),
                        new Point(0,-2),
        new Point(-1,1)                ,new Point(0,1),
        new Point(-1,-1)                ,new Point(0,-1)
    };
    readonly Point[] oddIsometricDirections = {
                        new Point(0,2),
        new Point(-1,0),                 new Point(1,0),
                        new Point(0,-2),
        new Point(0,1)                 ,new Point(1,1),
        new Point(0,-1)                  ,new Point(1,-1)
    };

    /// <summary>
    /// Constructor
    /// </summary>
    /// <param name="canWalk">2D int array with 1 where you can walk and 2 where triggers are</param>
    /// <param name="w">The width of the map</param>
    /// <param name="h">The height of the map</param>
    /// <param name="hex">If the map is hex based or Isometric based</param>
    public AStarAlgo(int[,] canWalk, int w, int h, bool hex)
    {
        this.canWalk = canWalk;
        width = w;
        height = h;
        this.hex = hex;

        // Generates 2d array of nodes matching the map in size
        nodes = new Node[width, height];

        for (int i = 0; i < width; i++)
        {
            for (int j = 0; j < height; j++)
            {
                nodes[i, j] = new Node(new Point(i, j));
            }
        }
    }

    /// <summary>
    /// This method calculates the shortest possible path from start to goal
    /// using the a* pathfinding algorithm, in a square grid
    /// </summary>
    /// <param name="startPos">Start position</param>
    /// <param name="goalPos">Goal position</param>
    /// <returns>A vector2 List containing the shortest path</returns>
    public List<Vector2> calculate(Vector2 startPos, Vector2 goalPos)
    {
        Point start = new Point((int)startPos.x, (int)startPos.y);
        Point goal = new Point((int)goalPos.x, (int)goalPos.y);


        // Return variable
        List<Vector2> path = new List<Vector2>();

        // Contains evaluvated nodes
        List<Node> closedSet = new List<Node>();

        // Contains nodes that are to be evaluvated
        List<Node> openSet = new List<Node>();

        // Creates start node at start position and adds to openSet
        Node s = nodes[(int)start.x, (int)start.y];
        s.calculateH(goal, hex);
        s.calculateF();
        s.inOpenSet = true;
        s.SetCameFrom(s);
        openSet.Add(s);

        // Starts loop that continues until openset is empty or a path has been found
        while (openSet.Count != 0)
        {
            
            // Fetches node from openSet
            Node cur = openSet[0];
            int posX = (int)cur.Getpos().x;
            int posY = (int)cur.Getpos().y;

            // Removes node from openSet and adds to closedSet
            openSet.Remove(cur);
            closedSet.Add(cur);
            cur.inOpenSet = false;
            cur.evaluvated = true;

            // Fetches all walkable neighbor nodes
            Node[] neighbours;
            if (hex)
                neighbours = findNeighboursHex(posX, posY, goal);
            else 
                neighbours = findNeighboursIso(posX, posY, goal);
            // Calculates pathcost to neighbor nodes
            for (int i = 0; i < neighbours.Length && neighbours[i] != null; i++)
            {

                Node neighbour = neighbours[i];
                // If already evaluvated, skip node
                if (neighbour.evaluvated)
                    continue;

                // If not in openSet, add to openSet, set where it came from and calculate pathCost
                if (!neighbour.inOpenSet)
                {
                    neighbour.SetGScore(cur.GetGScore() + 1);
                    neighbour.calculateH(goal, hex);
                    neighbour.calculateF();
                    neighbour.SetCameFrom(cur);
                    neighbour.inOpenSet = true;

                    int index = Math.Abs(openSet.BinarySearch(neighbour));
                    //Debug.Log(index + " " + neighbour.GetF() + " " + openSet.Count);
                    if (index >= openSet.Count) openSet.Add(neighbour);
                    else
                    {
                        while (index > 0 && openSet[index-1].GetF() == neighbour.GetF())
                        {
                            index--;
                        }
                        openSet.Insert(index, neighbour);
                    }
                }
                // OpenSet contains node, then check if current path is better.
                else
                {
                    int f = neighbour.calcNewF(cur.GetGScore() + 1);
                    if (f < neighbour.GetF())
                    {
                        neighbour.SetGScore(cur.GetGScore() + 1);
                        neighbour.calculateF();
                        neighbour.SetCameFrom(cur);
                    }
                }
            }

            // If openSet contains goal node, generate path by backtracking and break loop.
            if (nodes[(int)goal.x, (int)goal.y].inOpenSet)
            {
                nodes[(int)goal.x, (int)goal.y].backTrack(path);
                break;
            }

            // Sorts openSet by cost
            //openSet = openSet.OrderByDescending(Node => Node.GetF()).ToList();
        }

        // prepares nodes for new run
        for (int i = 0; i < width; i++)
        {
            for (int j = 0; j < height; j++)
            {
                nodes[i, j].inOpenSet = false;
                nodes[i, j].evaluvated = false;
            }
        }

        // Returns path array that contains the shortest path
        return path;
    }

    /// <summary>
    /// This method finds neighbours in a isometric grid
    /// </summary>
    /// <param name="posX">Current position for x</param>
    /// <param name="posY">Current position for y</param>
    /// <param name="goal">goal to make it possible to walk to triggers</param>
    /// <returns>Array with neighbour nodes</returns>
    private Node[] findNeighboursIso(int posX, int posY, Point goal)
    {
        Node[] neighbours = new Node[8];
        int logPos = 0;
        // checks if your at en even or odd place in the y direction and uses the correct
        // array for directions based on that
        if (posY % 2 == 0)
        {
            foreach (Point v in evenIsometricDirections)
            {
                if (posX + v.x >= 0 && posX + v.x < width
                    && posY + v.y >= 0 && posY + v.y < height
				    && (canWalk[posX + (int)v.x, posY + (int)v.y] == MapGenerator.MapMaker.CANWALK
                    || (canWalk[posX + (int)v.x, posY + (int)v.y] == MapGenerator.MapMaker.TRIGGER
                    && posX + (int)v.x == goal.x && posY + (int)v.y == goal.y)))
                {
                    neighbours[logPos] = nodes[posX + (int)v.x, posY + (int)v.y];
                    logPos++;
                }
            }
        }
        else
        {
            foreach (Point v in oddIsometricDirections)
            {
                if (posX + v.x >= 0 && posX + v.x < width
                    && posY + v.y >= 0 && posY + v.y < height
                    && (canWalk[posX + (int)v.x, posY + (int)v.y] == MapGenerator.MapMaker.CANWALK
                    || (canWalk[posX + (int)v.x, posY + (int)v.y] == MapGenerator.MapMaker.TRIGGER
                    && posX + (int)v.x == goal.x && posY + (int)v.y == goal.y)))
                {
                    neighbours[logPos] = nodes[posX + (int)v.x, posY + (int)v.y];
                    logPos++;
                }
            }
        }
        return neighbours;
    }

    /// <summary>
    /// This method finds neighbours in a hex grid
    /// </summary>
    /// <param name="posX">Current position for x</param>
    /// <param name="posY">Current position for y</param>
    /// <param name="goal">goal to make it possible to walk to triggers</param>
    /// <returns>Array with neighbour nodes</returns>
    private Node[] findNeighboursHex(int posX, int posY, Point goal)
    {
        Node[] neighbours = new Node[6];
        int logPos = 0;
        for (int x = 0; x < 3; x++)
        {
            for (int y = 0; y < 3; y++)
            {
                //ignore self
                if (x == 1 && y == 1)
                    continue;
                //ignores two positions based on if y is odd or even
                //this is to simulate the hex grid
                else if (posY % 2 == 0 && x == 2 && (y == 0 || y == 2))
                    continue;
                else if (posY % 2 == 1 && x == 0 && (y == 0 || y == 2))
                    continue;
                //adds neighbour if you can walk there
                if (posX + x - 1 >= 0 && posX + x - 1 < width
                    && posY + y - 1 >= 0 && posY + y - 1 < height
                    && (canWalk[posX + x - 1, posY + y - 1] == MapGenerator.MapMaker.CANWALK
                    || (canWalk[posX + x - 1, posY + y - 1] == MapGenerator.MapMaker.TRIGGER
                    && posX + x - 1 == goal.x && posY + y - 1 == goal.y)))
                {
                    neighbours[logPos] = nodes[posX + x - 1, posY + y - 1];
                    logPos++;
                }
            }
        }
        return neighbours;
    }

    /// <summary>
    /// The node class contains it's position, a reference to the node you came from,
    /// it's gScore(the cost to walk to this node), hScore(the estimated cost to reach the goal, ignoring obstacles)
    /// and it's f wich is g+h. f is refferred to as pathCost in other commentraries.
    /// Also contains booleans to check if node is evaluvated or in openSet list.
    /// </summary>
    public class Node : IComparable<Node>
    {
        Node cameFrom;
        Point pos;
        int gScore, hScore, f;
        public bool evaluvated, inOpenSet;

        public Node(Point pos)
        {
            cameFrom = this;
            this.pos = pos;
            gScore = hScore = f = 0;
            evaluvated = false;
            inOpenSet = false;
        }

        // Calculates the estimated cost of moving to goal from this node, ignoring obstacles, as hScore
        public void calculateH(Point goal, bool hex)
        {
            if (hex)
                hScore = DistanceHex(pos, goal);
            else
                hScore = (int)Math.Sqrt(Math.Abs(goal.x - pos.x) + Math.Abs(goal.y - pos.y));
        }

        // Transelates offset cordinates to cube cordinates
        private Vector3 oddROffsetToCube(Point pos)
        {
            int x = (pos.x - ((pos.y - 1 * (pos.y & 1)) / 2));
            int z = pos.y;
            int y = -x - z;
            return new Vector3(x, y, z);
        }

        // returns distance to target in a offset grid, ignoring obstacles
        private int DistanceHex(Point a, Point b)
        {
            Vector3 s = oddROffsetToCube(a);
            Vector3 g = oddROffsetToCube(b);
            return cubeDistance(s, g);
        }

        // returns distance to target in a cube grid, ignoring obstacles
        private int cubeDistance(Vector3 a, Vector3 b)
        {
            return (int)Math.Max(Math.Abs(a.x - b.x), Math.Max(Math.Abs(a.y - b.y), Math.Abs(a.z - b.z)));
        }

        // Calculates the estimated cost of this path wich is gScore + hScore.
        public void calculateF()
        {
            f = gScore + hScore;
        }

        // Calculates the estimated cost of a path trou this node based on given g
        public int calcNewF(int g)
        {
            return g + hScore;
        }

        // Recursive method that travels from node to node using the cameFrom reference, to construct a path
        // result is stored in the given list
        public void backTrack(List<Vector2> n)
        {
            if (!cameFrom.Equals(this))
            {
                cameFrom.backTrack(n);
                n.Add(new Vector2(pos.x,pos.y));
            }
        }



        // Returns true if this.pos equals n.pos
        public bool equals(Node n)
        {
            return (n.pos.Equals(pos));
        }

        public Node GetCameFrom()
        {
            return cameFrom;
        }

        public Point Getpos()
        {
            return pos;
        }

        public int GetGScore()
        {
            return gScore;
        }
        public int GetHScore()
        {
            return hScore;
        }
        public int GetF()
        {
            return f;
        }
        public void SetCameFrom(Node cm)
        {
            cameFrom = cm;
        }
        public void SetPos(Point p)
        {
            pos = p;
        }
        public void SetGScore(int g)
        {
            gScore = g;
        }
        public void SetHScore(int h)
        {
            hScore = h;
        }
        public void SetF(int f)
        {
            this.f = f;
        }

        public int CompareTo(Node n)
        {
            return f-n.f;
        }
    }

    /// <summary>
    /// simple class to handle x,y coordinates.
    /// </summary>
    public class Point
    {
        public int x,y;

        public Point(int x, int y)
        {
            this.x = x;
            this.y = y;
        }
    }
}
