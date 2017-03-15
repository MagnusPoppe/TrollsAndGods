using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

class PortraitOnClick : MonoBehaviour
{

    GameObject newHeroFrame;
    GameObject[] allFrames;
    Hero hero;

    public Hero Hero
    {
        get
        {
            return hero;
        }

        set
        {
            hero = value;
        }
    }

    public GameObject NewHeroFrame
    {
        get
        {
            return newHeroFrame;
        }

        set
        {
            newHeroFrame = value;
        }
    }

    public GameObject[] AllFrames
    {
        get
        {
            return allFrames;
        }

        set
        {
            allFrames = value;
        }
    }

    void OnMouseDown()
    {
        for (int i = 0; i < allFrames.Length; i++)
        {
            allFrames[i].SetActive(false);
        }
        newHeroFrame.SetActive(true);
        Debug.Log(hero.Name);
    }
}
