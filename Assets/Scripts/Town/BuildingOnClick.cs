using UnityEngine;
using TownView;
using UI;
using UnityEngine.UI;

namespace TownView
{

    public class BuildingOnClick : MonoBehaviour
    {
        public Building building;
        private Town town;
        Player player;
        SpriteRenderer spriteRenderer;
        float add = 1f;

        GameObject[] frames;

        IngameObjectLibrary libs;
        SpriteSystem spriteSystem;
        SpriteRenderer cardSpriteRenderer;
        Vector2 exitBtnPosition;
        Vector2 buyBtnPosition;
        TownHallOnClick townHallOnClick;
        GameObject cardWindow;
        GameObject exitButton;
        GameObject buyButtonObject;
        BuyButtonOnClick buyButton;
        PortraitOnClick portraitOnClick;
        GameObject[] buildingObjects;
        GameManager gm;
        int[] ratio = { 1, 100, 100, 200, 200 };
        Text textLeftResource, textRightResource;
        int selectedPayResource = 0;
        int selectedEarnResource = 1;

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

        // Inits variables used throughout the onClick chain
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

        // Reacts to mousedown effects on a buildings box collider in the town view
        void OnMouseDown()
        {
            OpenWindow(Building);
        }

        // Listens to escape presses to close the town view
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

            // Creates a building card game ojbect with a spriterenderer, sets its position, layer, name and parent
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

            // gets the positions for exit button and buy button if the window type requires it
            exitBtnPosition = getUpRightCorner(cardSpriteRenderer);
            buyBtnPosition = getDownRightCorner(cardSpriteRenderer);


            // Creates a town hall view if the building type matches it
            if (windowType == WindowTypes.TOWN_HALL_CARD)
            {
                CreateBuyButton(buyBtnPosition);
                CreateTownHallView();
            }
            // Creates a tavern view if the building type matches it
            else if (windowType == WindowTypes.TAVERN_CARD)
            {
                CreateBuyButton(buyBtnPosition);
                CreateTavernView();
            }
            // Creates a marketplace view if the building type matches it
            else if (windowType == WindowTypes.MARKETPLACE_CARD)
            {
                CreateBuyButton(buyBtnPosition);
                CreateMarketplaceView();
            }
            // Creates a dwelling view if the building type matches it
            else if (windowType == WindowTypes.DWELLING_CARD)
            {
                CreateBuyButton(buyBtnPosition);
                CreateDwellingView();
            }
            // Creates a general building view if the building type matches it
            else if (windowType == WindowTypes.BUILDING_CARD)
            {
                CreateBuildingView();
            }
            else if(windowType == WindowTypes.MARKETPLACE_CARD)
            {
                CreateMarketplaceView();
            }

            CreateExitButton(exitBtnPosition);
        }

        /// <summary>
        /// General view for a building with no specific function. Simply states its name and description
        /// </summary>
        private void CreateBuildingView()
        {
            // Add text below building
            GameObject textObject = new GameObject();
            textObject.transform.parent = GameObject.Find("TownCardPanel").transform;
            textObject.transform.localScale = GameObject.Find("Canvas").transform.localScale;
            textObject.name = Building.Name + " text";
            Text text = textObject.AddComponent<Text>();
            text.font = UnityEngine.Resources.Load<Font>("Fonts/ARIAL");
            text.fontSize = 18;
            text.text = Building.Description;
            text.alignment = TextAnchor.UpperCenter;
            text.color = Color.black;
            textObject.transform.position = new Vector2(cardWindow.transform.position.x, cardWindow.transform.position.y);
        }

        private void CreateDwellingView()
        {

        }

        private void CreateMarketplaceView()
        {
            Vector2 nextPositionPay = new Vector2(cardSpriteRenderer.transform.position.x, cardSpriteRenderer.transform.position.y + (cardSpriteRenderer.bounds.size.y / 4));
            Vector2 nextPositionEarn = new Vector2(nextPositionPay.x, (nextPositionPay.y - cardSpriteRenderer.bounds.size.y/5f));
            float startX = nextPositionPay.x;

            float leftX = startX;
            float rightX = startX;
            // Add each hero that you can buy in Tavern

            // holds all the frames to be disabled
            GameObject[] resourceFramesPay = new GameObject[System.Enum.GetNames(typeof(Resources.type)).Length];
            GameObject[] resourceFramesEarn = new GameObject[System.Enum.GetNames(typeof(Resources.type)).Length];
            
            for (int i = 0; i < System.Enum.GetNames(typeof(Resources.type)).Length; i++)
            {

                GameObject resourceFrameObjectPay = new GameObject();
                resourceFrameObjectPay.transform.parent = GameObject.Find("TownCardPanel").transform;
                resourceFrameObjectPay.name = player.Wallet.GetResourceName(i) + " pay frame";
                SpriteRenderer frameImagePay = resourceFrameObjectPay.AddComponent<SpriteRenderer>();
                frameImagePay.sprite = UnityEngine.Resources.Load<Sprite>("Sprites/UI/resource_frame");
                frameImagePay.sortingLayerName = "TownInteractive";
                resourceFramesPay[i] = resourceFrameObjectPay;
                resourceFrameObjectPay.SetActive(false);
                

                // Create the gameobject to see and click on
                GameObject resourceObjectPay = new GameObject();
                resourceObjectPay.transform.parent = GameObject.Find("TownCardPanel").transform;
                resourceObjectPay.name = "heroName";
                resourceObjectPay.tag = "toDestroy";

                // Add the picture of the resource
                SpriteRenderer sprPay = resourceObjectPay.AddComponent<SpriteRenderer>();

                // Quick and dirty switch
                switch (i)
                {
                    case 0: sprPay.sprite = UnityEngine.Resources.Load<Sprite>("Sprites/UI/gold"); break;
                    case 1: sprPay.sprite = UnityEngine.Resources.Load<Sprite>("Sprites/UI/wood"); break;
                    case 2: sprPay.sprite = UnityEngine.Resources.Load<Sprite>("Sprites/UI/ore"); break;
                    case 3: sprPay.sprite = UnityEngine.Resources.Load<Sprite>("Sprites/UI/crystal"); break;
                    case 4: sprPay.sprite = UnityEngine.Resources.Load<Sprite>("Sprites/UI/gem"); break;
                }

                sprPay.sortingLayerName = "GUI";

                // Add the collider to click on to build a building
                BoxCollider2D colliderPay = resourceObjectPay.AddComponent<BoxCollider2D>();
                colliderPay.size = sprPay.bounds.size;





                GameObject textNameObject = new GameObject();
                textNameObject.transform.parent = resourceObjectPay.transform;
                textNameObject.transform.localScale = GameObject.Find("Canvas").transform.localScale;
                textNameObject.name = player.Wallet.GetResource(i) + ", " + player.Wallet.GetResourceName(i);
                Text textResource = textNameObject.AddComponent<Text>();
                textResource.font = UnityEngine.Resources.Load<Font>("Fonts/ARIAL");
                textResource.fontSize = 18;
                textResource.text = player.Wallet.GetResource(i) + "";
                textResource.color = Color.black;
                textResource.alignment = TextAnchor.LowerCenter;







                GameObject resourceFrameObjectEarn = new GameObject();
                resourceFrameObjectEarn.transform.parent = GameObject.Find("TownCardPanel").transform;
                resourceFrameObjectEarn.name = player.Wallet.GetResourceName(i) + " earn frame";
                SpriteRenderer frameImageEarn = resourceFrameObjectEarn.AddComponent<SpriteRenderer>();
                frameImageEarn.sprite = UnityEngine.Resources.Load<Sprite>("Sprites/UI/resource_frame");
                frameImageEarn.sortingLayerName = "TownInteractive";
                resourceFramesEarn[i] = resourceFrameObjectEarn;
                resourceFrameObjectEarn.SetActive(false);

                // Create the gameobject to see and click on
                GameObject resourceObjectEarn = new GameObject();
                resourceObjectEarn.transform.parent = GameObject.Find("TownCardPanel").transform;
                resourceObjectEarn.name = "heroName";
                resourceObjectEarn.tag = "toDestroy";

                // Add the picture of the resource
                SpriteRenderer sprEarn = resourceObjectEarn.AddComponent<SpriteRenderer>();
                sprEarn.sprite = sprPay.sprite;
                sprEarn.sortingLayerName = "GUI";

                // Add the collider to click on to build a building
                BoxCollider2D colliderEarn = resourceObjectEarn.AddComponent<BoxCollider2D>();
                colliderEarn.size = sprEarn.bounds.size;
                




                resourceObjectPay.transform.position = nextPositionPay;
                resourceObjectEarn.transform.position = nextPositionEarn;
                resourceFrameObjectPay.transform.position = nextPositionPay;
                resourceFrameObjectEarn.transform.position = nextPositionEarn;
                //textNameObject.transform.position = new Vector2(nextPosition.x, (nextPosition.y - spr.bounds.size.y));
                //textDescriptionObject.transform.position = new Vector2(startX, (nextPosition.y - (spr.bounds.size.y*1.5f)));

                // Calculate the position for the next gameobject
                float newX = nextPositionPay.x;
                float newY = nextPositionPay.y;

                if (i % 2 == 0)
                {
                    newX = leftX -= (sprPay.bounds.size.x * 4f);
                }
                else
                {
                    newX = rightX += (sprPay.bounds.size.x * 4f);
                }
                nextPositionPay = new Vector2(newX, newY);
                nextPositionEarn = new Vector2(newX, nextPositionEarn.y);

                // Attaches onclick
                /*portraitOnClick = heroObject.AddComponent<PortraitOnClick>();
                portraitOnClick.BuyButton = buyButton;
                portraitOnClick.Hero = gm.heroes[i];
                portraitOnClick.NewHeroFrame = resourceFrameObject;
                portraitOnClick.AllFrames = heroFrames;*/

                // Turns last hero active
                if (i == 0)
                    resourceFrameObjectPay.SetActive(true);
                if(i == 1)
                    resourceFrameObjectEarn.SetActive(true);
            }






            string prefabPath = "Prefabs/Slider";
            GameObject sliderObject = Instantiate(UnityEngine.Resources.Load<GameObject>(prefabPath));
            sliderObject.transform.parent = cardSpriteRenderer.transform;
            sliderObject.transform.localScale = GameObject.Find("Canvas").transform.localScale;
            
            float bottomY = cardSpriteRenderer.transform.position.y * 0.5f;

            sliderObject.transform.position = new Vector2(cardSpriteRenderer.transform.position.x, bottomY);
            

            GameObject textLeftObject = new GameObject();
            textLeftObject.transform.parent = cardSpriteRenderer.transform;
            textLeftObject.transform.localScale = GameObject.Find("Canvas").transform.localScale;
            textLeftResource = textLeftObject.AddComponent<Text>();
            textLeftResource.font = UnityEngine.Resources.Load<Font>("Fonts/ARIAL");
            textLeftResource.fontSize = 16;
            textLeftResource.text = ratio[0] * ratio[1] + "";
            textLeftResource.color = Color.black;
            textLeftResource.alignment = TextAnchor.MiddleRight;
            textLeftObject.transform.position = new Vector2((cardSpriteRenderer.transform.position.x * 0.92f), bottomY);
            

            GameObject textRightObject = new GameObject();
            textRightObject.transform.parent = cardSpriteRenderer.transform;
            textRightObject.transform.localScale = GameObject.Find("Canvas").transform.localScale;
            textRightResource = textRightObject.AddComponent<Text>();
            textRightResource.font = UnityEngine.Resources.Load<Font>("Fonts/ARIAL");
            textRightResource.fontSize = 16;
            textRightResource.text = 1 + "";
            textRightResource.color = Color.black;
            textRightResource.alignment = TextAnchor.MiddleLeft;
            textRightResource.transform.position = new Vector2((cardSpriteRenderer.transform.position.x * 1.08f), bottomY);
            
            Slider slider = sliderObject.GetComponent<Slider>();
            slider.maxValue = player.Wallet.GetResource(0) / ratio[1];

            slider.onValueChanged.AddListener(adjustTradeAmount);
        }

        public void adjustTradeAmount(float value)
        {
            textRightResource.text = value + "";
            textLeftResource.text = value * ratio[selectedEarnResource] + "";
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
                frameImage.sprite = UnityEngine.Resources.Load<Sprite>("Sprites/UI/building_frame");
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
                townHallOnClick = buildingObject.AddComponent<TownHallOnClick>();
                townHallOnClick.BuyButton = buyButton;
                townHallOnClick.Building = town.Buildings[i];
                townHallOnClick.Town = Town;
                townHallOnClick.Player = Player;
                townHallOnClick.AllFrames = buildingFrames;
                townHallOnClick.NewHeroFrame = heroFrameObject;


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
                textObject.transform.position = new Vector2(buildingObject.transform.position.x, buildingObject.transform.position.y - spr.bounds.size.y);


                // Create text and image for every resource next to the icon
                Vector2 position = new Vector2(buildingObject.transform.position.x + (spr.bounds.size.x / 1.6f), nextPosition.y + spr.bounds.size.y / 2);
                for (int j = 0; j < town.Buildings[i].Cost.GetResourceTab().Length; j++)
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

        public void CreateTavernView()
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
                    heroFrameObject.SetActive(false);

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

                    // Sets the first hero as the active hero




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
                    portraitOnClick = heroObject.AddComponent<PortraitOnClick>();
                    portraitOnClick.BuyButton = buyButton;
                    portraitOnClick.Hero = gm.heroes[count];
                    portraitOnClick.NewHeroFrame = heroFrameObject;
                    portraitOnClick.AllFrames = heroFrames;

                    // Turns last hero active
                    if (i == gm.heroes.Length)
                    {
                        heroFrameObject.SetActive(true);
                        buyButton.Hero = gm.heroes[count];
                    }

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
            return new Vector2(sr.transform.position.x + (sr.bounds.size.x / 2.2f), sr.transform.position.y + (sr.bounds.size.y / 2.3f));
        }

        /// <summary>
        /// Gets the bottom right corner of the current building card.
        /// </summary>
        /// <param name="sr">The sprite renderer to get the bounds from</param>
        /// <returns></returns>
        private Vector2 getDownRightCorner(SpriteRenderer sr)
        {
            return new Vector2(sr.transform.position.x + (sr.bounds.size.x / 2.2f), sr.transform.position.y - (sr.bounds.size.y / 2.3f));
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
            sr.sprite = libs.GetUI(button.GetSpriteID() + 1);
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
            buyButton.Parent = this;
            buyButton.Player = Player;
        }

        private void updateFrames(GameObject frame)
        {
            foreach (GameObject t in frames)
            {
                if (t != null)
                    t.SetActive(false);
            }
            frame.SetActive(true);
            //buyButton.Hero = frame.hero;
        }
    }

}