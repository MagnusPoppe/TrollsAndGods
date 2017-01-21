using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GenerateMap : MonoBehaviour	
{
    public bool[,] blockedSquare;
    public GameObject[] tiles;
    public GameObject tree;
    public GameObject hero;
    public const int X = 32;
    public const int Y = 32;

    void Start ()
    {
        hero.transform.position = new Vector2(4, 3);
        Instantiate(hero);
        fillTiles();
        fillObjects();
	}
	
	void Update ()	
    {
		
	}

    void fillTiles()
    {
        for (int x = 0; x < X; x++)
            for (int y = 0; y < Y; y++)
            {
                GameObject clone = tiles[0];
                clone.transform.position = new Vector2(x, y);
                clone = Instantiate(clone);
            }
    }

    void fillObjects()
    {
        blockedSquare = new bool[X, Y];
        for (int x = 0; x < X; x++)
            for (int y = 0; y < Y; y++)
            {
                if ((x + y) % 3 == 0 || y % 5 == 0)
                {
                    GameObject clone = tree;
                    clone.transform.position = new Vector2(x, y);
                    clone = Instantiate(clone);
                    blockedSquare[x, y] = true;
                }
            }
    }

    public bool GetBlocked(int x, int y)
    {
        return blockedSquare[x, y] && x >= 0 && x <= X && y >= 0 && y <= Y;
    }

    public int GetWidth()
    {
        return X;
    }

    public int GetHeight()
    {
        return Y;
    }
}
