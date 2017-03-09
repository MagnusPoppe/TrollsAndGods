using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using TownView;

class TownHallOnClick : MonoBehaviour
{

    Building building;
    Town town;
    Player player;
    GameManager gm;

    public Building Building
    {
        get
        {
            return building;
        }

        set
        {
            building = value;
        }
    }

    public Town Town
    {
        get
        {
            return town;
        }

        set
        {
            town = value;
        }
    }

    public Player Player
    {
        get
        {
            return player;
        }

        set
        {
            player = value;
        }
    }

    public void Start()
    {
        GameObject go = GameObject.Find("GameManager");
        gm = go.GetComponent<GameManager>();
    }


    // Destroys all the given game objects and returns to the town screen
    void OnMouseDown()
    {
        // Build building if town has not already built that day, player can pay, and building is not built already
        if (!Town.HasBuiltThisRound && Player.Wallet.CanPay(Building.Cost) && !Building.Built)
        {
            // Player pays
            Player.Wallet.Pay(Building.Cost);
            town.HasBuiltThisRound = true;
            gm.updateResourceText();
        
            // Find the building in the town's list, build it and draw it in the view
            for(int i=0; i<town.Buildings.Length; i++)
            {
                if (town.Buildings[i].Equals(Building))
                {
                    Debug.Log("YOU BOUGHT: " + building.Name); // TODO remove
                    town.Buildings[i].Build();
                    gm.DrawBuilding(town, building, i);
                    return;
                }
            }
        }
        else
        {
            Debug.Log("YOU DO NOT HAVE THE SUFFICIENT ECONOMICAL WEALTH TO PRODUCE THE STRUCTURE OF CHOICE: " + building.Name); // TODO remove
        }
    }
}