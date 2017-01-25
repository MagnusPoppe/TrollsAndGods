using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Linq;

public class AStarAlgo {

    bool[,] canWalk;
    Node[,] nodes;
    int width, height;

    public AStarAlgo(bool[,] canWalk, int w, int h)
    {
        this.canWalk = canWalk;
        width = w;
        height = h;
    }

    /// <summary>
    /// This method calculates the shortest possible path from start to goal
    /// using the a* pathfinding algorithm, in a square grid
    /// </summary>
    /// <param name="start">Start position</param>
    /// <param name="goal">Goal position</param>
    /// <returns>A vector2 List containing the shortest path</returns>
    List<Vector2> calculate(Vector2 start, Vector2 goal)
    {
        // Return variable
        List<Vector2> path = new List<Vector2>();

        // Contains evaluvated nodes
        List<Node> closedSet = new List<Node>();

        // Contains nodes that are to be evaluvated
        List<Node> openSet = new List<Node>();

        // Generates 2d array of nodes matching the map in size
        nodes = new Node[width, height];

        for (int i = 0; i < width; i++)
        {
            for (int j = 0; j < height; j++)
            {
                nodes[i, j] = new Node(new Vector2(i, j));
            }
        }

        // Creates start node at start position and adds to openSet
        Node s = nodes[(int)start.x, (int)start.y];
        s.calculateH(start, goal);
        s.calculateF();
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

            // Fetches all walkable neighbor nodes
            Node[] neighbours = new Node[8];
            int logPos = 0;
            for (int x = 0; x < 3; x++)
            {
                for (int y = 0; y < 3; y++)
                {
                    if (x == 1 && y == 1)
                        continue;
                    if (posX + x - 1 >= 0 && posX + x - 1 < width
                        && posY + y - 1 >= 0 && posY + y - 1 < height
                        && canWalk[posX + x - 1, posY + y - 1])
                    {
                        neighbours[logPos] = nodes[posX + x - 1, posY + y - 1];
                        logPos++;
                    }
                }
            }

            // Calculates pathcost to neighbor nodes
            for (int i = 0; i < logPos; i++)
            {
                Node neighbour = neighbours[i];

                // If already evaluvated, skip node
                if (closedSet.Contains(neighbour))
                    continue;

                // If not in openSet, add to openSet, set where it came from and calculate pathCost
                if (!openSet.Contains(neighbour))
                {
                    openSet.Insert(0, neighbour);
                    neighbour.SetGScore(cur.GetGScore() + 1);
                    neighbour.calculateH(neighbour.Getpos(), goal);
                    neighbour.calculateF();
                    neighbour.SetCameFrom(cur);
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
            if (openSet.Contains(nodes[(int)goal.x, (int)goal.y]))
            {
                nodes[(int)goal.x, (int)goal.y].backTrack(path);
                break;
            }

            // Sorts openSet by cost
            openSet = openSet.OrderBy(Node => Node.GetF()).ToList();
        }

        // Returns path array that contains the shortest path
        return path;
    }

    /// <summary>
    /// The node class contains it's position, a reference to the node you came from,
    /// it's gScore(the cost to walk to this node), hScore(the estimated cost to reach the goal, ignoring obstacles)
    /// and it's f wich is g+h. f is refferred to as pathCost in other commentraries.
    /// </summary>
    public class Node
    {
        Node cameFrom;
        Vector2 pos;
        int gScore, hScore, f;

        public Node(Vector2 pos)
        {
            cameFrom = this;
            this.pos = pos;
            gScore = hScore = f = 0;
        }

        // Calculates the estimated cost of moving to goal from this node, ignoring obstacles, as hScore
        public void calculateH(Vector2 start, Vector2 goal)
        {
            hScore = (int)(Math.Abs(goal.x - start.x) + Math.Abs(goal.y - start.y));
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
                n.Add(pos);
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

        public Vector2 Getpos()
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
        public void SetPos(Vector2 p)
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
    }
}
