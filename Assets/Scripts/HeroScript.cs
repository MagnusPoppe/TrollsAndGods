using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

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
    Vector2 curPos;
    //public GameObject clone;
    int i = 0;

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
            curPos = this.transform.position;
            Vector2 pos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            pos.x = (int)(pos.x + 0.5);
            pos.y = (int)(pos.y + 0.5);
            //Debug.Log(curPos.x + " " + curPos.y);
            if (!gm.blockedSquare[(int)pos.x, (int)pos.y])
            {
                if (pointerActive && pos.x == toX && pos.y == toY)
                {
                    //clone.SetActive(false);
                    //transform.position = new Vector2(pos.x, pos.y);
                    pointerActive = false;
                    toX = 0;
                    toY = 0;
                }
                else
                {
                    // stjernealgoritme (startpos, tilpos), returnere tabell som vi legger gameobject i posisjonene
                    List<Vector2> positions = aStar(curPos, pos);

                    //Debug.Log(curPos.x + " " + curPos.y);
                    foreach (Vector2 no in positions)
                    {
                        GameObject clone;
                        if (pos == no)
                            clone = pathDestYes;
                        else
                            clone = pathYes;
                        clone.transform.position = no;
                        clone = Instantiate(clone);
                    }

                    /*for (int i = 0; i < gm.GetWidth(); i++)
                        for (int j = 0; j < gm.GetWidth(); j++)
                        {
                            if ((Vector2)positions)
                            {
                                GameObject clone;
                                if (i == pos.x && j == pos.y)
                                    clone = pathDestYes;
                                else
                                    clone = pathYes;
                                clone.transform.position = new Vector2(i, j);
                                clone = Instantiate(clone);
                            }
                        }*/


                    //clone.transform.position = new Vector2(pos.x, pos.y);
                    if (i == 0)
                    {
                        i++;
                        //clone = Instantiate(clone);
                    }
                    //clone.SetActive(true);
                    pointerActive = true;
                    toX = (int)pos.x;
                    toY = (int)pos.y;
                }
            }
        }
    }

    List<Vector2> aStar(Vector2 start, Vector2 goal)
    {
        List<Vector2> path = new List<Vector2>();

        List<Node> closedSet = new List<Node>();

        List<Node> openSet = new List<Node>();

        Node[,] n = new Node[gm.GetWidth(), gm.GetHeight()];

        for (int i = 0; i < gm.GetWidth(); i++)
        {
            for (int j = 0; j < gm.GetHeight(); j++)
            {
                n[i, j] = new Node(new Vector2(i, j));
            }
        }

        Node s = n[(int)start.x, (int)start.y];
        s.calculateH(start, goal);
        s.calculateF();

        openSet.Add(s);

        while (openSet.Count != 0)
        {
            Node cur = openSet[0];

            openSet.Remove(cur);
            closedSet.Add(cur);

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

            for (int i = 0; i < logPos; i++)
            {
                Node node = neighbours[i];
                if (closedSet.Contains(node))
                    continue;
                if (!openSet.Contains(node))
                {
                    openSet.Add(node);
                    node.SetGScore(cur.GetGScore() + 1);
                    node.calculateH(node.Getpos(), goal);
                    node.calculateF();
                    node.SetCameFrom(cur);
                }
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

            if (openSet.Contains(n[(int)goal.x, (int)goal.y]))
            {
                n[(int)goal.x, (int)goal.y].backTrack(path);
                break;
            }

        }

        return path;

        //for (int i = 0; i < 5; i++)
        //    path[i, i] = true;
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
            return 0;
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
