using UnityEngine;
using TownView;

class TownHallOnClick : MonoBehaviour
{

    Building building;
    Town town;
    Player player;
    GameManager gm;
    BuyButtonOnClick buyButton;
    GameObject newHeroFrame;
    GameObject[] allFrames;

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

    public BuyButtonOnClick BuyButton
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

    public void Start()
    {
        GameObject go = GameObject.Find("GameManager");
        gm = go.GetComponent<GameManager>();
    }


    // Destroys all the given game objects and returns to the town screen
    void OnMouseDown()
    {
        for (int i = 0; i < allFrames.Length; i++)
        {
            allFrames[i].SetActive(false);
        }
        newHeroFrame.SetActive(true);
        buyButton.Building = Building;

        /*
        // Build building if town has not already built that day, player can pay, and building is not built already
        if (!Town.HasBuiltThisRound && Player.Wallet.CanPay(Building.Cost) && !Building.Built && Building.MeetsRequirements(town))
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
        }*/
    }
}