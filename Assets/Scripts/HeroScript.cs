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

    void Start ()
    {
        g = GameObject.Find("MapGenerator");
        gm = g.GetComponent<GenerateMap>();
        //clone = pathDestYes;
    }
	
	void Update ()
    {
        if (Input.GetMouseButtonDown(0))
        {
            heroWalking = false;
            curPos = this.transform.position;
            Vector2 pos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            pos.x = (int)(pos.x + 0.5);
            pos.y = (int)(pos.y + 0.5);
            //Debug.Log(curPos.x + " " + curPos.y);
            if (!gm.blockedSquare[(int)pos.x, (int)pos.y])
            {
                if (pointerActive && pos.x == toX && pos.y == toY)
                {
                    foreach (GameObject go in pathList)
                        Destroy(go);
                    curSpeed = Math.Min(positions.Count, heroSpeed);
                    //transform.position = new Vector2(pos.x, pos.y);
                    i = 0;
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
                    Debug.Log(positions.Count);
                    foreach (Vector2 no in positions)
                    {
                        GameObject clone;
                        if (pos == no && curSpeed > 0)
                        {
                            clone = pathDestYes;
                        }
                        else if(pos == no)
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
        else if (heroWalking)
        {
            if (i % 24 == 0)
            {
                Debug.Log(curSpeed);
                transform.position = positions[i / 24];
                curSpeed--;
            }
            i++;

            if (curSpeed == 0)
                heroWalking = false;
        }
    }

    /// <summary>
    /// this method calculates the shortest possible path from start to goal
    /// using the a* pathfinding algorithm
    /// </summary>
    /// <param name="start">start position</param>
    /// <param name="goal">goal position</param>
    /// <returns>a vector2 List containing the shortest path</returns>
    List<Vector2> aStar(Vector2 start, Vector2 goal)
    {
        // return variable
        List<Vector2> path = new List<Vector2>();

        // contains evaluvated nodes
        List<Node> closedSet = new List<Node>();

        // contains nodes that are to be evaluvated
        List<Node> openSet = new List<Node>();

        // generates 2d array of nodes matching the map in size
        Node[,] n = new Node[gm.GetWidth(), gm.GetHeight()];

        for (int i = 0; i < gm.GetWidth(); i++)
        {
            for (int j = 0; j < gm.GetHeight(); j++)
            {
                n[i, j] = new Node(new Vector2(i, j));
            }
        }

        // creates start node at start position and adds to openSet
        Node s = n[(int)start.x, (int)start.y];
        s.calculateH(start, goal);
        s.calculateF();
        openSet.Add(s);

        // starts loop that continues until openset is empty or a path has been found
        while (openSet.Count != 0)
        {
            // fetches node from openSet
            Node cur = openSet[0];

            // removes node from openSet and adds to closedSet
            openSet.Remove(cur);
            closedSet.Add(cur);

            // fetches all walkable neighbor nodes
            Node[] neighbours = new Node[8];
            int logPos = 0;
            for (int x = 0; x < 3; x++)
            {
                for (int y = 0; y < 3; y++)
                {
                    if (x == 1 && y == 1)
                        continue;
                    if (!gm.GetBlocked((int)cur.Getpos().x, (int)cur.Getpos().y) && (int)cur.Getpos().x + x - 1 >= 0 && (int)cur.Getpos().x + x - 1 < gm.GetWidth() && (int)cur.Getpos().y + y - 1 >= 0 && (int)cur.Getpos().y + y - 1 < gm.GetHeight())
                    {
                        neighbours[logPos] = n[(int)cur.Getpos().x + x - 1, (int)cur.Getpos().y + y - 1];
                        logPos++;
                    }
                }
            }

            // calculates pathcost to neighbor nodes
            for (int i = 0; i < logPos; i++)
            {
                Node node = neighbours[i];

                // if already evaluvated, skip node
                if (closedSet.Contains(node))
                    continue;
                
                // if not in openSet, add to openSet, set where it came from and calculate pathCost
                if (!openSet.Contains(node))
                {
                    openSet.Insert(0,node);
                    node.SetGScore(cur.GetGScore() + 1);
                    node.calculateH(node.Getpos(), goal);
                    node.calculateF();
                    node.SetCameFrom(cur);
                }
                // openSet contains node, then check if current path is better.
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

            // if openSet contains goal node, generate path by backtracking and break loop.
            if (openSet.Contains(n[(int)goal.x, (int)goal.y]))
            {
                n[(int)goal.x, (int)goal.y].backTrack(path);
                break;
            }

            // sorts openSet by cost
            openSet = openSet.OrderBy(Node=>Node.GetF()).ToList();
        }

        // returns path array that contains the shortest path
        return path;
    }

    public class Node : IComparable<Node>
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

        public void calculateH(Vector2 start, Vector2 goal)
        {
            hScore = (int)(Math.Abs(goal.x - start.x) + Math.Abs(goal.y - start.y));
        }


        public void calculateF()
        {
            f = gScore + hScore;
        }

        public int calcNewF(int g)
        {
            return g + hScore;
        }

        public void backTrack(List<Vector2> n)
        {
            if (!cameFrom.Equals(this))
            {
                cameFrom.backTrack(n);
                n.Add(pos);
            }
        }

        public bool equals(Node n)
        {
            return (n.pos.Equals(pos));
        }

        public int CompareTo(Node n)
        {
            return f-n.GetF();
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
