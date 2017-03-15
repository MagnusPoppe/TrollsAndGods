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

    BuyButtonOnClick buyButton;

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

    public BuyButtonOnClick BuyButton
    {
        get { return buyButton; }
        set { buyButton = value; }
    }

    void OnMouseDown()
    {
        foreach (GameObject t in allFrames)
        {
            if (t != null)
                t.SetActive(false);
        }
        newHeroFrame.SetActive(true);
        buyButton.Hero = Hero;
    }
}
