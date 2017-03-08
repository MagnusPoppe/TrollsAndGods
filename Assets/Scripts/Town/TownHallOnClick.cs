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
        Debug.Log("Buy " + Building.Name);

        // CHECKLIST
        if (!Town.HasBuiltThisRound && Player.Wallet.CanPay(Building.Cost))
        {
            Debug.Log("Du kan kjøpe!");
            //town.Buildings[i].Build();
            Debug.Log("Du kjøpte...");
            Player.Wallet.Pay(Building.Cost);
            Debug.Log(building.Cost.GetResource(0));
            town.HasBuiltThisRound = true;
            gm.updateResourceText();
        }
        else
        {
            Debug.Log("Du kan ikke kjøpe :(");
        }
        // check if town has built this turn
        // check if player can pay
            

    }
}