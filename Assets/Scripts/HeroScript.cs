using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Linq;

public class HeroScript : MonoBehaviour
{
    bool pointerActive;
    bool legalPath;
    int toX, toY;
    GenerateMap gm;
    GameObject g;
    public GameObject pathDestYes;
    public GameObject pathDestNo;
    public GameObject pathYes;
    public GameObject pathNo;
    List<GameObject> pathList = new List<GameObject>();
    Vector2 curPos;
    int heroSpeed = 8;
    bool heroWalking;
    List<Vector2> positions;
    int curSpeed;
    int i;
    float move;
    bool walking;
    void Start ()
    {
        g = GameObject.Find("MapGenerator");
        gm = g.GetComponent<GenerateMap>();
    }
	
	void Update ()
    {
        if (Input.GetMouseButtonDown(0))
        {

            curPos = this.transform.position;
            Vector2 pos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            pos.x = (int)(pos.x + 0.5);
            pos.y = (int)(pos.y + 0.5);
            if (heroWalking)
            {
                walking = false;
            }
            // Hero's own position is clicked
            else if (curPos.Equals(pos))
            {
                // Todo, open hero menu
            }
            else if (!gm.blockedSquare[(int)pos.x, (int)pos.y])
            {
                if (pointerActive && pos.x == toX && pos.y == toY)
                {
                    foreach (GameObject go in pathList)
                        Destroy(go);
                    curSpeed = Math.Min(positions.Count, heroSpeed);
                    i = 0;
                    move = 0;
                    walking = true;
                    heroWalking = true;
                    pointerActive = false;
                    toX = -1;
                    toY = -1;
                }
                else
                {
                    positions = aStar(curPos, pos);
                    foreach(GameObject go in pathList)
                        Destroy(go);

                    pathList.Clear();
                    curSpeed = Math.Min(positions.Count, heroSpeed);
                    foreach (Vector2 no in positions)
                    {
                        GameObject clone;
                        if (pos == no && curSpeed > 0)
                        {
                            clone = pathDestYes;
                        }
                        else if (pos == no)
                            clone = pathDestNo;
                        else if (curSpeed > 0)
                            clone = pathYes;
                        else
                            clone = pathNo;
                        curSpeed--;
                        clone.transform.position = no;
                        clone = Instantiate(clone);
                        pathList.Add(clone);
                    }
                    pointerActive = true;
                    toX = (int)pos.x;
                    toY = (int)pos.y;
                }
            }
        }
        if (heroWalking)
        {
            move += Time.deltaTime;
            transform.position = Vector2.Lerp(transform.position, positions[i], move);
            Vector2 pos = transform.position;
            
            if (pos.Equals(positions[i]))
            {
                i++;
                move = 0f;
                if (i == curSpeed || !walking)
                {
                    heroWalking = false;
                }
            }
        }
    }

    /// <summary>
    /// This method calculates the shortest possible path from start to goal
    /// using the a* pathfinding algorithm
    /// </summary>
    /// <param name="start">Start position</param>
    /// <param name="goal">Goal position</param>
    /// <returns>A vector2 List containing the shortest path</returns>
    List<Vector2> aStar(Vector2 start, Vector2 goal)
    {
        // Return variable
        List<Vector2> path = new List<Vector2>();

        // Contains evaluvated nodes
        List<Node> closedSet = new List<Node>();

        // Contains nodes that are to be evaluvated
        List<Node> openSet = new List<Node>();

        // Generates 2d array of nodes matching the map in size
        Node[,] n = new Node[gm.GetWidth(), gm.GetHeight()];

        for (int i = 0; i < gm.GetWidth(); i++)
        {
            for (int j = 0; j < gm.GetHeight(); j++)
            {
                n[i, j] = new Node(new Vector2(i, j));
            }
        }

        // Creates start node at start position and adds to openSet
        Node s = n[(int)start.x, (int)start.y];
        s.calculateH(start, goal);
        s.calculateF();
        openSet.Add(s);

        // Starts loop that continues until openset is empty or a path has been found
        while (openSet.Count != 0)
        {
            // Fetches node from openSet
            Node cur = openSet[0];

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
                    if (!gm.GetBlocked((int)cur.Getpos().x, (int)cur.Getpos().y) 
                        && (int)cur.Getpos().x + x - 1 >= 0 && (int)cur.Getpos().x + x - 1 < gm.GetWidth() 
                        && (int)cur.Getpos().y + y - 1 >= 0 && (int)cur.Getpos().y + y - 1 < gm.GetHeight())
                    {
                        neighbours[logPos] = n[(int)cur.Getpos().x + x - 1, (int)cur.Getpos().y + y - 1];
                        logPos++;
                    }
                }
            }

            // Calculates pathcost to neighbor nodes
            for (int i = 0; i < logPos; i++)
            {
                Node node = neighbours[i];

                // If already evaluvated, skip node
                if (closedSet.Contains(node))
                    continue;
                
                // If not in openSet, add to openSet, set where it came from and calculate pathCost
                if (!openSet.Contains(node))
                {
                    openSet.Insert(0,node);
                    node.SetGScore(cur.GetGScore() + 1);
                    node.calculateH(node.Getpos(), goal);
                    node.calculateF();
                    node.SetCameFrom(cur);
                }
                // OpenSet contains node, then check if current path is better.
                else
                {
                    int f = node.calcNewF(cur.GetGScore() + 1);
                    if (f < node.GetF())
                    {
                        node.SetGScore(cur.GetGScore() + 1);
                        node.calculateF();
                        node.SetCameFrom(cur);
                    }
                }
            }

            // If openSet contains goal node, generate path by backtracking and break loop.
            if (openSet.Contains(n[(int)goal.x, (int)goal.y]))
            {
                n[(int)goal.x, (int)goal.y].backTrack(path);
                break;
            }

            // Sorts openSet by cost
            openSet = openSet.OrderBy(Node=>Node.GetF()).ToList();
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
