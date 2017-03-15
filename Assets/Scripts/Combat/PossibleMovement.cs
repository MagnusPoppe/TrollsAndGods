using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PossibleMovement
{
    private Node[,] field;
    private GameObject[,] units;
    private int[,] canWalk;
    private int width, height;

    public PossibleMovement(GameObject[,] ground, GameObject[,] units, int[,] canWalk, int width, int height)
    {
        this.canWalk = canWalk;
        this.units = units;
        this.width = width;
        this.height = height;
        field = new Node[width,height];
        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                field[x,y] = new Node(ground[x,y].GetComponent<GroundGameObject>(),0,new Point(x,y));
            }
        }
    }

    public void flipReachable(Point startingPoint, int speed)
    {
        List<Node> openSet = new List<Node>();

        Node s = field[startingPoint.x, startingPoint.y];
        s.InOpenSet = true;
        openSet.Add(s);

        while (openSet.Count > 0)
        {
            Node cur = openSet[0];
            openSet.Remove(cur);
            cur.InOpenSet = false;
            cur.Evaluvated = true;
            if (cur.Ggo.IsOccupied && units[cur.Pos.x, cur.Pos.y] != null && cur.WalkedSteps <= speed+1)
                units[cur.Pos.x, cur.Pos.y].GetComponent<UnitGameObject>().Attackable = true;
            else if (cur.WalkedSteps <= speed) cur.Ggo.Reachable = true;

            Node[] neighbours = findNeighboursHex(cur.Pos);

            for (int i = 0; i < neighbours.Length && neighbours[i] != null; i++)
            {

                Node neighbour = neighbours[i];
                // If already evaluvated, skip node
                if (neighbour.Evaluvated)
                    continue;

                // If not in openSet, add to openSet, set where it came from and calculate pathCost
                if (!neighbour.InOpenSet)
                {
                    neighbour.WalkedSteps = cur.WalkedSteps + 1;
                    neighbour.InOpenSet = true;

                    //Inserts neighbour node into openset at sorted position
                    int index = Math.Abs(openSet.BinarySearch(neighbour));
                    if (index >= openSet.Count) openSet.Add(neighbour);
                    else openSet.Insert(index, neighbour);
                }
                // OpenSet contains node, then check if current path is better.
                else
                {
                    int newWalkedSteps = cur.WalkedSteps + 1;
                    if (neighbour.WalkedSteps > newWalkedSteps)
                    {
                        neighbour.WalkedSteps = newWalkedSteps;
                    }
                }
            }
        }
    }

    /// <summary>
    /// This method finds neighbours in a hex grid
    /// </summary>
    /// <param name="posX">Current position for x</param>
    /// <param name="posY">Current position for y</param>
    /// <param name="goal">goal to make it possible to walk to triggers</param>
    /// <returns>Array with neighbour nodes</returns>
    private Node[] findNeighboursHex(Point pos)
    {
        Node[] neighbours = new Node[6];
        int posX = pos.x;
        int posY = pos.y;
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
                    && (canWalk[posX + x - 1, posY + y - 1] == MapGenerator.MapMaker.CANWALK))
                {
                    neighbours[logPos] = field[posX + x - 1, posY + y - 1];
                    logPos++;
                }
            }
        }
        return neighbours;
    }

    private class Node : IComparable<Node>
    {
        private GroundGameObject ggo;
        private Point pos;
        private int walkedSteps;
        private bool inOpenSet, evaluvated;

        public Node(GroundGameObject ggo, int walkedSteps, Point pos)
        {
            this.ggo = ggo;
            this.pos = pos;
            this.walkedSteps = walkedSteps;
            inOpenSet = evaluvated = false;
        }

        public Point Pos
        {
            get { return pos; }
            set { pos = value; }
        }

        public bool InOpenSet
        {
            get { return inOpenSet; }
            set { inOpenSet = value; }
        }

        public bool Evaluvated
        {
            get { return evaluvated; }
            set { evaluvated = value; }
        }

        public GroundGameObject Ggo
        {
            get { return ggo; }
            set { ggo = value; }
        }

        public int WalkedSteps
        {
            get { return walkedSteps; }
            set { walkedSteps = value; }
        }

        public int CompareTo(Node other)
        {
            return walkedSteps-other.WalkedSteps;
        }
    }
}
