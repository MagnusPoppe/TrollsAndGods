using UnityEngine;
using TownView;

public class BuyButtonOnClick : MonoBehaviour  {

    GameObject cardWindow;
    GameObject buyButton;
    GameObject[] buildingObjects;

    Hero hero;
    Building building;
    Town town;
    Player player;
    GameManager gm;

    public GameObject CardWindow
    {
        get
        {
            return cardWindow;
        }

        set
        {
            cardWindow = value;
        }
    }

    public GameObject BuyButton
    {
        get
        {
            return buyButton;
        }

        set
        {
            buyButton = value;
        }
    }

    public GameObject[] BuildingObjects
    {
        get
        {
            return buildingObjects;
        }

        set
        {
            buildingObjects = value;
        }
    }



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

    public void Start()
    {
        GameObject go = GameObject.Find("GameManager");
        gm = go.GetComponent<GameManager>();
    }


    // Destroys all the given game objects and returns to the town screen
    void OnMouseDown()
    {
        if(building != null)
        {
            // Build building if town has not already built that day, player can pay, and building is not built already
            if (!Town.HasBuiltThisRound && Player.Wallet.CanPay(Building.Cost) && !Building.Built && Building.MeetsRequirements(town))
            {
                // Player pays
                Player.Wallet.Pay(Building.Cost);
                town.HasBuiltThisRound = true;
                gm.updateResourceText();

                // Find the building in the town's list, build it and draw it in the view
                for (int i = 0; i < town.Buildings.Length; i++)
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
        else if(Hero != null)
        {
            Cost cost = new Cost(2000, 0, 0, 0, 0);
            if (Player.Wallet.CanPay(cost))
            {
                Debug.Log("Bought the hero" + hero.Name); // TODO remove
                player.Wallet.Pay(cost);
            }
            else
                Debug.Log("Not enough gold");
        }





        for (int i = 0; i < BuildingObjects.Length; i++)
        {
            // TODO: make into list so we dont have to check for null?
            if (BuildingObjects[i] != null)
                BuildingObjects[i].GetComponent<PolygonCollider2D>().enabled = true;
        }

        foreach (GameObject go in GameObject.FindGameObjectsWithTag("toDestroy"))
            Destroy(go);
    }
}