using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GenerateMap : MonoBehaviour	
{
    public bool[,] canWalk;
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
        canWalk = new bool[X, Y];
        for (int x = 0; x < X; x++)
            for (int y = 0; y < Y; y++)
            {
                GameObject clone = tiles[0];
                clone.transform.position = new Vector2(x, y);
                clone = Instantiate(clone);
                canWalk[x, y] = true;
            }
    }

    void fillObjects()
    {
        for (int x = 0; x < X; x++)
            for (int y = 0; y < Y; y++)
            {
                if ((x + y) % 4 == 0 || y % 5 == 0)
                {
                    GameObject clone = tree;
                    clone.transform.position = new Vector2(x, y);
                    clone = Instantiate(clone);
                    canWalk[x, y] = false;
                }
            }
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
