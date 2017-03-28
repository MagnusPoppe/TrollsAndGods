using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// this class registers all possible places you can move to, as well as registering what you can attack
/// </summary>
public class PossibleMovement
{
    private Node[,] field;
    private GameObject[,] units;
    private int[,] canWalk;
    private int width, height;

    /// <summary>
    /// Constructor
    /// </summary>
    /// <param name="ground">GameObjects constituting the ground</param>
    /// <param name="units">GameObjects constituting the units on the field</param>
    /// <param name="canWalk">2d array over where you can walk</param>
    /// <param name="width">Width of battlefield</param>
    /// <param name="height">Height of battlefield</param>
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

    /// <summary>
    /// Checks in a growing circular fashion what you can reach and flips the appropriate bool
    /// </summary>
    /// <param name="startingPoint">Position of unit</param>
    /// <param name="speed">How many steps the unit can take</param>
    public void flipReachable(Point startingPoint, int speed)
    {
        // List for all nodes that are to be evaluvated
        List<Node> openSet = new List<Node>();

        UnitGameObject aktiveUnit = units[startingPoint.x, startingPoint.y].GetComponent<UnitGameObject>();
        Unit u = aktiveUnit.UnitTree.GetUnits()[aktiveUnit.PosInUnitTree];
        Ranged r = null;
        if (u.IsRanged)
        {
            r = (Ranged)u;
            r.Threatened = false;
        }
        //Adds starting node to openSet
        Node s = field[startingPoint.x, startingPoint.y];
        s.WalkedSteps = 0;
        s.InOpenSet = true;
        openSet.Add(s);

        //Loops as long as there are unevaluvated nodes
        while (openSet.Count > 0)
        {
            //Fetches current node from openset
            Node cur = openSet[0];
            //Removes cur from openset and marks as evaluvated
            openSet.Remove(cur);
            cur.InOpenSet = false;
            cur.Evaluvated = true;

            //Flips bool for reachable or attackable if unit can reach cur
            if (cur.Ggo.IsOccupied && units[cur.Pos.x, cur.Pos.y] != null && cur.WalkedSteps <= speed + 1
                && aktiveUnit.AttackingSide != units[cur.Pos.x, cur.Pos.y].GetComponent<UnitGameObject>().AttackingSide)
            {
                units[cur.Pos.x, cur.Pos.y].GetComponent<UnitGameObject>().Attackable = true;
                if (u.IsRanged && cur.WalkedSteps == 1)
                {
                    r.Threatened = true;
                }
            } 
            else if (cur.WalkedSteps <= speed)
            {
                cur.Ggo.Reachable = true;
            }

            //Finds walkable neighbours
            Node[] neighbours = findNeighboursHex(cur.Pos);

            //Loops trou neighbours
            for (int i = 0; i < neighbours.Length && neighbours[i] != null; i++)
            {

                Node neighbour = neighbours[i];
                // If already evaluvated, skip node
                if (neighbour.Evaluvated)
                    continue;

                // If not in openSet, add to openSet and calculate WalkedSteps
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
            //Breaks loop if next node to be checked is unreachable
            if (cur.WalkedSteps == speed + 2) break;
        }
        //Readies for new run
        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                field[x, y].Evaluvated = false;
                field[x, y].InOpenSet = false;
            }
        }
    }

    /// <summary>
    /// This method finds neighbours in a hex grid
    /// </summary>
    /// <param name="pos">Current position</param>
    /// <param name="goal">Goal</param>
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
                //adds neighbour if inside bounds
                if (posX + x - 1 >= 0 && posX + x - 1 < width
                    && posY + y - 1 >= 0 && posY + y - 1 < height)
                {
                    neighbours[logPos] = field[posX + x - 1, posY + y - 1];
                    logPos++;
                }
            }
        }
        return neighbours;
    }

    /// <summary>
    /// Inner class for nodes, implements Comparable
    /// </summary>
    private class Node : IComparable<Node>
    {
        private GroundGameObject ggo;
        private Point pos;
        private int walkedSteps;
        private bool inOpenSet, evaluvated;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="ggo">GroundGameObject</param>
        /// <param name="walkedSteps">WalkedSteps</param>
        /// <param name="pos">Position</param>
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
