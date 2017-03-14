using UnityEngine;
using TownView;
using UI;
using UnityEngine.UI;

public class BuildingOnClick : MonoBehaviour {

    public Building building;
    private Town town;
    Player player;
    SpriteRenderer spriteRenderer;
    float add = 1f;
    
    IngameObjectLibrary libs;
    SpriteSystem spriteSystem;
    SpriteRenderer cardSpriteRenderer;
    Vector2 exitBtnPosition;
    Vector2 buyBtnPosition;
    TownHallOnClick onClick;
    GameObject cardWindow;
    GameObject exitButton;
    GameObject buyButtonObject;
    BuyButtonOnClick buyButton;
    GameObject[] buildingObjects;
    GameManager gm;

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

    void Start()
    {
        spriteRenderer = gameObject.GetComponent<SpriteRenderer>();
        libs = GameManager.libs;
        GameObject go = GameObject.Find("GameManager");
        gm = go.GetComponent<GameManager>();

    }

    // Fade effet when hovering a building in the town view
    private void OnMouseOver()
    {
        if (add > 0.6f)
            spriteRenderer.color = new Color(255, 255, 255, add -= 0.02f);
    }
    private void OnMouseExit()
    {
        add = 1f;
        spriteRenderer.color = new Color(255, 255, 255, add);
    }

    void OnMouseDown()
    {
        OpenWindow(Building);
    }
    
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            foreach (GameObject go in GameObject.FindGameObjectsWithTag("toDestroy"))
                Destroy(go);
            gm.EnterTown(town);
        }
    }
    
    /// <summary>
    /// Creates the game objects to open a building window
    /// </summary>
    /// <param name="b">The building that was clicked on in the town window</param>
    void OpenWindow(Building b)
    {
        // Gets the type of window associated with the given building
        int windowType = b.UIType();

        // TODO: Make less specific
        BuildingCard card = new BuildingCard(windowType, IngameObjectLibrary.Category.UI);

        // Creates an object with a spriterenderer, sets its position, layer, name and parent
        cardWindow = new GameObject();
        cardWindow.transform.parent = GameObject.Find("Canvas").transform;
        cardWindow.name = "TownCardPanel";
        cardWindow.tag = "toDestroy";
        cardWindow.transform.position = cardWindow.transform.parent.position;
        cardSpriteRenderer = cardWindow.AddComponent<SpriteRenderer>();
        cardSpriteRenderer.sprite = libs.GetUI(card.GetSpriteID());
        cardSpriteRenderer.sortingLayerName = "TownInteractive";

        // disables colliders in other layers when window is open
        for (int i = 0; i < BuildingObjects.Length; i++)
        {
            // TODO: make into list so we dont have to check for null?
            if (BuildingObjects[i] != null)
                BuildingObjects[i].GetComponent<PolygonCollider2D>().enabled = false;
        }


        exitBtnPosition = getUpRightCorner(cardSpriteRenderer);
        buyBtnPosition = getDownRightCorner(cardSpriteRenderer);


        // Lage townhall building
        if (windowType == WindowTypes.TOWN_HALL_CARD)
        {
            CreateBuyButton(buyBtnPosition);
            CreateTownHallView();
        }
        // Lage tavern bygning
        else if (windowType == WindowTypes.TAVERN_CARD)
        {
            CreateBuyButton(buyBtnPosition);
            CreateTavernView();
        }

        CreateExitButton(exitBtnPosition);
    }

    /// <summary>
    /// General method for all TownHall-type buildings to build their view, consisitng of all the buildings this tonw type can build
    /// and displaying whether they're buildable, unbuildable or already built.
    /// </summary>
    private void CreateTownHallView()
    {
        
        // TOOD: get parents order and i++
        float offsetX = cardSpriteRenderer.transform.position.x - (cardSpriteRenderer.bounds.size.x / 2.5f);
        float offsetY = cardSpriteRenderer.transform.position.y + (cardSpriteRenderer.bounds.size.y / 4);
        Vector2 nextPosition = new Vector2(offsetX, offsetY);
        float startX = nextPosition.x;
        // Create the object for each building in Town Hall view

        // Array for all building frames frames
        GameObject[] buildingFrames = new GameObject[town.Buildings.Length];
        for (int i = 0; i < town.Buildings.Length; i++)
        {

            // sets teh fram behind the object
            GameObject heroFrameObject = new GameObject();
            heroFrameObject.transform.parent = GameObject.Find("TownCardPanel").transform;
            heroFrameObject.name = town.Buildings[i].Name;
            SpriteRenderer frameImage = heroFrameObject.AddComponent<SpriteRenderer>();
            frameImage.sprite = UnityEngine.Resources.Load<Sprite>("Sprites/UI/hero_frame");
            frameImage.sortingLayerName = "TownInteractive";
            frameImage.sortingOrder = 1;
            buildingFrames[i] = heroFrameObject;
            if (i != 0)
                heroFrameObject.SetActive(false);

            // Create the gameobject to see and click on
            GameObject buildingObject = new GameObject();
            buildingObject.transform.parent = GameObject.Find("TownCardPanel").transform;
            buildingObject.transform.position = GameObject.Find("TownCardPanel").transform.position;
            buildingObject.name = town.Buildings[i].Name;
            buildingObject.tag = "toDestroy";


            // Add components to the gameobject window
            onClick = buildingObject.AddComponent<TownHallOnClick>();
            onClick.BuyButton = buyButton;
            onClick.Building = town.Buildings[i];
            onClick.Town = Town;
            onClick.Player = Player;
            onClick.AllFrames = buildingFrames;
            onClick.NewHeroFrame = heroFrameObject;
            

            // If it's already purchased or you can't build it, use another sprite, set by an offset
            int offset = 0;
            if (town.Buildings[i].Built)
                offset = town.Buildings.Length;
            else if (!Player.Wallet.CanPay(town.Buildings[i].Cost) || town.HasBuiltThisRound || !town.Buildings[i].MeetsRequirements(town))
                offset = town.Buildings.Length * 2;

            // Add the picture of the building
            SpriteRenderer spr = buildingObject.AddComponent<SpriteRenderer>();
            spr.sprite = libs.GetTown(town.Buildings[i].GetSpriteBlueprintID() + offset);
            spr.sortingLayerName = "GUI";

            // Add the collider to click on to build a building
            BoxCollider2D collider = buildingObject.AddComponent<BoxCollider2D>();
            collider.size = spr.bounds.size;

            // Add text below building
            GameObject textObject = new GameObject();
            textObject.transform.parent = GameObject.Find(town.Buildings[i].Name).transform;
            textObject.transform.localScale = GameObject.Find("Canvas").transform.localScale;
            textObject.name = town.Buildings[i].Name + " text";
            Text text = textObject.AddComponent<Text>();
            text.font = UnityEngine.Resources.Load<Font>("Fonts/ARIAL");
            text.fontSize = 18;
            text.text = town.Buildings[i].Name;
            text.alignment = TextAnchor.UpperCenter;
            text.color = Color.black;

            // Position icon and text
            buildingObject.transform.position = nextPosition;
            heroFrameObject.transform.position = buildingObject.transform.position;
            textObject.transform.position = new Vector2(buildingObject.transform.position.x, buildingObject.transform.position.y-spr.bounds.size.y);


            // Create text and image for every resource next to the icon
            Vector2 position = new Vector2(buildingObject.transform.position.x + (spr.bounds.size.x / 1.6f), nextPosition.y + spr.bounds.size.y / 2);
            for (int j = 0; j<town.Buildings[i].Cost.GetResourceTab().Length; j++)
            {
                if (town.Buildings[i].Cost.GetResourceTab()[j] != 0)
                {
                    GameObject imageObject = new GameObject();
                    imageObject.transform.parent = GameObject.Find(town.Buildings[i].Name).transform;
                    imageObject.name = town.Buildings[i].Cost.ResourceToString(i) + "image";
                    SpriteRenderer sprResource = imageObject.AddComponent<SpriteRenderer>();
                    string spritePath = "Sprites/UI/gold"; // TODO add to IngameObjectLibrary
                    if (j == 1)
                        spritePath = "Sprites/UI/wood";
                    else if (j == 2)
                        spritePath = "Sprites/UI/ore";
                    else if (j == 3)
                        spritePath = "Sprites/UI/crystal";
                    else if (j == 4)
                        spritePath = "Sprites/UI/gem";
                    sprResource.sprite = UnityEngine.Resources.Load<Sprite>(spritePath);
                    sprResource.sortingLayerName = "GUI";
                    
                    GameObject textCostObject = new GameObject();
                    textCostObject.transform.parent = GameObject.Find(town.Buildings[i].Name).transform;
                    textCostObject.transform.localScale = GameObject.Find("Canvas").transform.localScale;
                    textCostObject.name = town.Buildings[i].Cost.ToString(j);
                    Text textCost = textCostObject.AddComponent<Text>();
                    textCost.font = UnityEngine.Resources.Load<Font>("Fonts/ARIAL");
                    textCost.text = town.Buildings[i].Cost.CostToString(j);
                    textCost.color = Color.black;
                    textCost.alignment = TextAnchor.MiddleLeft;

                    imageObject.transform.position = position;
                    textCostObject.transform.position = new Vector2(position.x + (sprResource.bounds.size.x * 2.3f), position.y);
                    position = new Vector2(buildingObject.transform.position.x + (spr.bounds.size.x / 1.6f), position.y - sprResource.bounds.size.y);

                }
            }
            // Calculate the position for the next gameobject
            float newX = nextPosition.x;
            float newY = nextPosition.y;

            if (nextPosition.x > cardSpriteRenderer.transform.position.x + spr.bounds.size.x * 1.3f)
            {
                newX = startX;
                newY -= (spr.bounds.size.y * 1.5f);
            }
            else
                newX += (spr.bounds.size.x * 1.45f);
            nextPosition = new Vector2(newX, newY);
        }
    }

    private void CreateTavernView()
    {
        Vector2 nextPosition = new Vector2(cardSpriteRenderer.transform.position.x, cardSpriteRenderer.transform.position.y + (cardSpriteRenderer.bounds.size.y / 4));
        float startX = nextPosition.x;

        float leftX = startX;
        float rightX = startX;
        // Add each hero that you can buy in Tavern
        int count = 0;

        // holds all the frames to be disabled
        GameObject[] heroFrames = new GameObject[gm.heroes.Length];

        for (int i = 0; i < gm.heroes.Length; i++)
        {
            if (gm.heroes[i] != null && !gm.heroes[i].Alive)
            {

                GameObject heroFrameObject = new GameObject();
                heroFrameObject.transform.parent = GameObject.Find("TownCardPanel").transform;
                heroFrameObject.name = gm.heroes[i].Name + "'s frame";
                SpriteRenderer frameImage = heroFrameObject.AddComponent<SpriteRenderer>();
                frameImage.sprite = UnityEngine.Resources.Load<Sprite>("Sprites/UI/hero_frame");
                frameImage.sortingLayerName = "TownInteractive";
                heroFrames[count] = heroFrameObject;

                // Create the gameobject to see and click on
                GameObject heroObject = new GameObject();
                heroObject.transform.parent = GameObject.Find("TownCardPanel").transform;
                heroObject.name = "heroName";
                heroObject.tag = "toDestroy";

                // Add the picture of the building
                SpriteRenderer spr = heroObject.AddComponent<SpriteRenderer>();
                spr.sprite = libs.GetPortrait(gm.heroes[i].GetPortraitID());// TODO libs.GetPortrait(town.Owner.Heroes[0].GetPortraitID());
                spr.sortingLayerName = "GUI";

                // Add the collider to click on to build a building
                BoxCollider2D collider = heroObject.AddComponent<BoxCollider2D>();
                collider.size = spr.bounds.size;

                GameObject textNameObject = new GameObject();
                textNameObject.transform.parent = heroObject.transform;
                textNameObject.transform.localScale = GameObject.Find("Canvas").transform.localScale;
                textNameObject.name = gm.heroes[i].Name;
                Text textName = textNameObject.AddComponent<Text>();
                textName.font = UnityEngine.Resources.Load<Font>("Fonts/ARIAL");
                textName.fontSize = 18;
                textName.text = gm.heroes[i].Name;
                textName.color = Color.black;
                //textCost.alignment = TextAnchor.MiddleCenter;


                /*
                GameObject textDescriptionObject = new GameObject();
                textDescriptionObject.transform.parent = heroObject.transform;
                textDescriptionObject.transform.localScale = GameObject.Find("Canvas").transform.localScale;
                textDescriptionObject.name = gm.heroes[i].Name + "'s description";
                Text textDescription = textDescriptionObject.AddComponent<Text>();
                textDescription.font = UnityEngine.Resources.Load<Font>("Fonts/ARIAL");
                textDescription.fontSize = 18;
                textDescription.text = gm.heroes[i].Description;
                textDescription.color = Color.black;
                */
                if (i != 0)
                    heroFrameObject.SetActive(false);


                heroObject.transform.position = nextPosition;
                heroFrameObject.transform.position = nextPosition;
                textNameObject.transform.position = new Vector2(nextPosition.x, (nextPosition.y - spr.bounds.size.y));
                //textDescriptionObject.transform.position = new Vector2(startX, (nextPosition.y - (spr.bounds.size.y*1.5f)));

                // Calculate the position for the next gameobject
                float newX = nextPosition.x;
                float newY = nextPosition.y;

                if (count % 2 == 0)
                    newX = leftX -= (spr.bounds.size.x * 1.2f);
                else
                    newX = rightX += (spr.bounds.size.x * 1.2f);
                nextPosition = new Vector2(newX, newY);

                // Attaches onclick
                PortraitOnClick onClick = heroObject.AddComponent<PortraitOnClick>();
                onClick.Hero = gm.heroes[count];
                onClick.NewHeroFrame = heroFrameObject;
                onClick.AllFrames = heroFrames;

                count++;
            }
        }
    }

    /// <summary>
    /// Gets the upper right corner of the current building card.
    /// </summary>
    /// <param name="sr">The sprite renderer to get the bounds from</param>
    /// <returns></returns>
    private Vector2 getUpRightCorner(SpriteRenderer sr)
    {
        return new Vector2(sr.transform.position.x + (sr.bounds.size.x / 2.15f), sr.transform.position.y + (sr.bounds.size.y / 2.2f));
    }

    /// <summary>
    /// Gets the bottom right corner of the current building card.
    /// </summary>
    /// <param name="sr">The sprite renderer to get the bounds from</param>
    /// <returns></returns>
    private Vector2 getDownRightCorner(SpriteRenderer sr)
    {
        return new Vector2(sr.transform.position.x + (sr.bounds.size.x / 2.15f), sr.transform.position.y - (sr.bounds.size.y / 2.2f));
    }

    /// <summary>
    /// Creates a gameobject exitbutton to exit the building screen
    /// </summary>
    void CreateExitButton(Vector2 position)
    {
        // creates object and sets its name and position
        exitButton = new GameObject();
        exitButton.name = "ExitButton";
        exitButton.tag = "toDestroy";
        exitButton.transform.position = position;

        // Attaches a sprite renderer, sets spriet, sorting layer and sorting order
        SpriteRenderer sr = exitButton.AddComponent<SpriteRenderer>();
        ExitButton button = new ExitButton();
        sr.sprite = libs.GetUI(button.GetSpriteID());
        sr.sortingLayerName = "GUI";
        sr.sortingOrder = cardWindow.GetComponent<SpriteRenderer>().sortingOrder + 1;  

        // sets a box collider trigger around the button
        BoxCollider2D collider = exitButton.AddComponent<BoxCollider2D>();
        collider.isTrigger = true;

        // sends game objects through to the button so it knows what to destroy
        exitButton.AddComponent<ExitButtonOnClick>();
        exitButton.GetComponent<ExitButtonOnClick>().CardWindow = cardWindow;
        exitButton.GetComponent<ExitButtonOnClick>().ExitButton = exitButton;
        exitButton.GetComponent<ExitButtonOnClick>().BuildingObjects = buildingObjects;
    }

    /// <summary>
    /// Creates a gameobject exitbutton to exit the building screen
    /// </summary>
    void CreateBuyButton(Vector2 position)
    {
        // creates object and sets its name and position
        buyButtonObject = new GameObject();
        buyButtonObject.name = "BuyButton";
        buyButtonObject.tag = "toDestroy";
        buyButtonObject.transform.position = position;

        // Attaches a sprite renderer, sets spriet, sorting layer and sorting order
        SpriteRenderer sr = buyButtonObject.AddComponent<SpriteRenderer>();
        ExitButton button = new ExitButton();
        sr.sprite = libs.GetUI(button.GetSpriteID()+1);
        sr.sortingLayerName = "GUI";
        sr.sortingOrder = cardWindow.GetComponent<SpriteRenderer>().sortingOrder + 1;

        // sets a box collider trigger around the button
        BoxCollider2D collider = buyButtonObject.AddComponent<BoxCollider2D>();
        collider.isTrigger = true;

        // sends game objects through to the button so it knows what to destroy
        buyButton = buyButtonObject.AddComponent<BuyButtonOnClick>();
        buyButton.CardWindow = cardWindow;
        buyButton.BuyButton = buyButtonObject;
        buyButton.BuildingObjects = buildingObjects;


        buyButton.Town = Town;
        buyButton.Player = Player;
    }

}
