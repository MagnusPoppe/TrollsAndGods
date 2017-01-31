using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GenerateMap : MonoBehaviour	
{
    public bool[,] canWalk;
    public GameObject[] tiles;
    public GameObject tree;
    public const int X = 32;
    public const int Y = 32;
    GameObject[] heroPrefabs;

    void Awake()
    {
        heroPrefabs = UnityEngine.Resources.LoadAll<GameObject>("Heroes");
    }

    void Start ()
    {
        //SpriteRenderer sr = hero.AddComponent<SpriteRenderer>();
        //sr.sprite = UnityEngine.Resources.Load("hero") as Sprite;
        //hero.AddComponent<Sprite>();
        //hero.GetComponent<SpriteRenderer>().sprite = sprites[0];
        //hero.GetComponent<SpriteRenderer>().sprite = UnityEngine.Resources.Load("hero") as Sprite;

        //hero = UnityEngine.Resources.Load("hero") as GameObject;
        //SpriteRenderer spr = hero.AddComponent<SpriteRenderer>();
        //hero.transform.position = new Vector2(4, 3);
        GameObject[] hero = new GameObject[2];
        for(int i=0; i<hero.Length; i++)
        {
            hero[i] = heroPrefabs[i];
            hero[i].transform.position = new Vector2(i, -1);
            Instantiate(hero[i]);

        }
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

    public bool isNotBlocked(int x, int y)
    {
        return  x >= 0 && x <= X && y >= 0 && y <= Y && canWalk[x, y];
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
