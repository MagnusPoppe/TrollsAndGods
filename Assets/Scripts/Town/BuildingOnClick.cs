using System;
using UnityEngine;
using TownView;
using UI;
using UnityEngine.UI;
using System.Collections.Generic;
using Units;

namespace TownView
{
    public class BuildingOnClick : MonoBehaviour
    {
        GameManager gm;

        private Town town;
        Player player;
        Building building;
        SpriteRenderer spriteRenderer;
        float add = 1f;

        private UnitBuilding currentUnitBuilding; // Clear after exit dwelling screen;

        GameObject canvas;

        GameObject frame;
        GameObject resourceFrame;
        SpriteRenderer frameImage;
        SpriteRenderer resourceFrameImage;

        IngameObjectLibrary libs;
        SpriteSystem spriteSystem;
        SpriteRenderer cardSpriteRenderer;
        GameObject cardWindow;
        GameObject exitButtonObject;
        GameObject buyButtonObject;
        GameObject[] buildingObjects;

        // Selected actions in town
        System.Object toBuyObject;
        Text[] textResource;
        int[] ratio = {1, 100, 100, 200, 200};
        Slider slider;
        int payAmount;
        int earnAmount;
        Text textLeftResource, textRightResource;
        int selectedPayResource;
        int selectedEarnResource;
        int unitAmount;

        private Font FONT;

        private Text unitTotalCountText;
        private Text unitToBuyCountText;
        private Text textCost;
        private Text[] totalCostTexts;

        public GameObject[] BuildingObjects
        {
            get { return buildingObjects; }

            set { buildingObjects = value; }
        }

        public Building Building
        {
            get { return building; }

            set { building = value; }
        }

        public Town Town
        {
            get { return town; }

            set { town = value; }
        }

        public Player Player
        {
            get { return player; }

            set { player = value; }
        }

        // Inits variables used throughout the onClick chain
        void Start()
        {
            spriteRenderer = gameObject.GetComponent<SpriteRenderer>();
            libs = GameManager.libs;
            GameObject go = GameObject.Find("GameManager");
            gm = go.GetComponent<GameManager>();
            FONT = UnityEngine.Resources.Load<Font>("Fonts/ARIAL");
            totalCostTexts = new Text[5];
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
                gm.ExitTown();
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

            gm.swapObject = null;

            // Default set resources to nothing
            selectedEarnResource = selectedPayResource = -1;

            // Initialize frame
            frame = new GameObject();
            frame.name = "frame";
            frameImage = frame.AddComponent<SpriteRenderer>();
            frameImage.sortingLayerName = "TownInteractive";

            // TODO: Make less specific
            BuildingCard card = new BuildingCard(windowType, IngameObjectLibrary.Category.UI);
            /*
            canvas = GameObject.Find("TownCanvas");

            
            // Creates a building card game ojbect with a spriterenderer, sets its position, layer, name and parent
            cardWindow = new GameObject();
            cardWindow.transform.parent = canvas.transform;
            cardWindow.name = "TownCardPanel";
            cardWindow.tag = "toDestroy";
            cardWindow.transform.position = cardWindow.transform.parent.position;
            cardSpriteRenderer = cardWindow.AddComponent<SpriteRenderer>();
            cardSpriteRenderer.sprite = libs.GetUI(card.GetSpriteID());
            cardSpriteRenderer.sortingLayerName = "TownInteractive";
            
            frame.transform.parent = cardWindow.transform;
            */

            // disables colliders in other layers when window is open
            for (int i = 0; i < BuildingObjects.Length; i++)
            {
                // TODO: make into list so we dont have to check for null?
                if (BuildingObjects[i] != null)
                    BuildingObjects[i].GetComponent<PolygonCollider2D>().enabled = false;
            }

            // Creates a town hall view if the building type matches it
            if (windowType == WindowTypes.TOWN_HALL_CARD)
            {
                //CreateBuyButton();
                CreateTownHallView();
            }
            // Creates a tavern view if the building type matches it
            else if (windowType == WindowTypes.TAVERN_CARD)
            {
                //CreateBuyButton();
                CreateTavernView();
            }
            // Creates a marketplace view if the building type matches it
            else if (windowType == WindowTypes.MARKETPLACE_CARD)
            {
                //CreateBuyButton();
                CreateMarketplaceView();
            }
            // Creates a dwelling view if the building type matches it
            else if (windowType == WindowTypes.DWELLING_CARD)
            {
                //CreateBuyButton();
                CreateDwellingView();
            }
            // Creates a general building view if the building type matches it
            else if (windowType == WindowTypes.BUILDING_CARD)
            {
                CreateBuildingView();
            }


            //CreateExitButton();
            //frame.transform.parent = GameObject.Find("TownCardPanel").transform;
        }

        /// <summary>
        /// General view for a building with no specific function. Simply states its name and description
        /// </summary>
        private void CreateBuildingView()
        {
            // Add text below building
            /*
            GameObject textObject = new GameObject();
            textObject.transform.parent = cardWindow.transform;
            textObject.transform.localScale = canvas.transform.localScale;
            textObject.name = Building.Name + " text";
            Text text = textObject.AddComponent<Text>();
            text.font = FONT;
            text.fontSize = 18;
            text.text = Building.Description;
            text.alignment = TextAnchor.UpperCenter;
            text.color = Color.black;
            textObject.transform.position = new Vector2(cardWindow.transform.position.x, cardWindow.transform.position.y);
            */



            // Get the ContentPanel and set description
            GameObject buildingContentPanel = gm.buildingPanel.transform.GetChild(0).gameObject;
            buildingContentPanel.transform.GetChild(0).GetComponent<Text>().text = Building.Description;

            // Set Buildingname Text
            GameObject buildingNameObject = gm.buildingPanel.transform.GetChild(1).gameObject;
            buildingNameObject.GetComponent<Text>().text = Building.Name;

            // Set exit button
            GameObject exitButtonObject = gm.buildingPanel.transform.GetChild(2).gameObject;
            exitButtonObject.GetComponent<Button>().onClick.AddListener(DestroyObjects);

            gm.buildingPanel.SetActive(true);
            //CreateExitButton();
        }



        /// <summary>
        /// General method for all TownHall-type buildings to build their view, consisitng of all the buildings this tonw type can build
        /// and displaying whether they're buildable, unbuildable or already built.
        /// </summary>
        private void CreateTownHallView()
        {

            GameObject townHallPanel = gm.townHallPanel.transform.GetChild(0).gameObject;

            // Set Buildingname Text
            GameObject buildingNameObject = gm.townHallPanel.transform.GetChild(1).gameObject;
            buildingNameObject.GetComponent<Text>().text = Building.Name;

            // Set exit button
            GameObject exitButtonObject = gm.townHallPanel.transform.GetChild(2).gameObject;
            exitButtonObject.GetComponent<Button>().onClick.AddListener(DestroyObjects);

            // Array for all building frames frames, except Workshop
            int buildingCount = town.Buildings.Length - 1;
            Building[] buildingArray = town.Buildings;


            GameObject[] buildingFrames = new GameObject[buildingCount];
            for (int i = 0; i < buildingCount; i++)
            {
                // If it's already purchased or you can't build it, use another sprite, set by an offset, -1 because Workshop has no buysprites
                int offset = 0;
                if (buildingArray[i].Built)
                    offset = buildingCount;
                else if (!Player.Wallet.CanPay(buildingArray[i].Cost) || town.HasBuiltThisRound || !buildingArray[i].MeetsRequirements(town))
                    offset = buildingCount * 2;

                // Finding the panel that holds the gameobject with button and textchild, and resourcepanel
                GameObject buildingPanel = townHallPanel.transform.GetChild(i).gameObject;
                // Building imagebutton with listener
                GameObject buildingObject = buildingPanel.transform.GetChild(0).gameObject;

                Building selectedBuilding = buildingArray[i];
                // Set building image
                buildingObject.GetComponent<Image>().sprite = libs.GetTown(selectedBuilding.GetSpriteBlueprintID() + offset);
                Button button = buildingObject.GetComponent<Button>();
                // Set buy listener to building
                button.onClick.AddListener(() => setBuy(selectedBuilding, buildingObject.transform.position));

                // Add text below building
                buildingObject.transform.GetChild(0).GetComponent<Text>().text = buildingArray[i].Name;
                // Finding resourcepanel
                GameObject resourcePanel = buildingPanel.transform.GetChild(1).gameObject;

                // Create text and image for every resource next to the icon
                for (int j = 0; j < buildingArray[i].Cost.GetResourceTab().Length; j++)
                {
                    if (buildingArray[i].Cost.GetResourceTab()[j] != 0)
                    {
                        // Finding resourcepanel gameobject child with buttonimage and text
                        GameObject resourceObject = resourcePanel.transform.GetChild(j).gameObject;
                        Button resourceButton = resourceObject.GetComponent<Button>();
                        string spritePath = "Sprites/UI/gold"; // TODO add to IngameObjectLibrary
                        if (j == 1)
                            spritePath = "Sprites/UI/wood";
                        else if (j == 2)
                            spritePath = "Sprites/UI/ore";
                        else if (j == 3)
                            spritePath = "Sprites/UI/crystal";
                        else if (j == 4)
                            spritePath = "Sprites/UI/gem";

                        // Setting resource mage
                        resourceButton.GetComponent<Image>().sprite = UnityEngine.Resources.Load<Sprite>(spritePath);
                        // Setting resourcecost text
                        resourceButton.transform.GetChild(0).GetComponent<Text>().text =
                            buildingArray[i].Cost.CostToString(j);
                    }
                }
            }
            gm.townHallPanel.SetActive(true);
        }

        /// <summary>
        /// Creates the tavern view for buying heroes
        /// </summary>
        public void CreateTavernView()
        {
            GameObject heroPanel = gm.tavernPanel.transform.GetChild(0).gameObject.transform.GetChild(0).gameObject;

            // Set Buildingname Text
            GameObject buildingNameObject = gm.tavernPanel.transform.GetChild(1).gameObject;
            buildingNameObject.GetComponent<Text>().text = Building.Name;

            // Set exit button
            GameObject exitButtonObject = gm.tavernPanel.transform.GetChild(2).gameObject;
            exitButtonObject.GetComponent<Button>().onClick.AddListener(DestroyObjects);

            // Add each hero that you can buy in Tavern
            int count = 0;
            for (int i = 0; i < gm.heroes.Length; i++)
            {
                if (gm.heroes[i] != null && !gm.heroes[i].Alive || count < 5)
                {
                    // Hero imagebutton with listener
                    GameObject heroObject = heroPanel.transform.GetChild(count).gameObject;
                    
                    Hero selectedHero = gm.heroes[i];
                    heroObject.GetComponent<Image>().sprite = libs.GetPortrait(selectedHero.GetPortraitID());
                    Button button = heroObject.GetComponent<Button>();
                    button.onClick.AddListener(() => setBuy(selectedHero, heroObject.transform.position));

                    button.transform.GetChild(0).GetComponent<Text>().text = selectedHero.Name;
                    // TODO listener to popup hero panel
                    count++;
                }
            }
            gm.tavernPanel.SetActive(true);
        }


        /// <summary>
        /// Creates the marketplace view for trading resources
        /// </summary>
        private void CreateMarketplaceView()
        {
            GameObject marketplacePanel = gm.marketplacePanel.transform.GetChild(0).gameObject;

            // Set Buildingname Text
            GameObject buildingNameObject = gm.marketplacePanel.transform.GetChild(1).gameObject;
            buildingNameObject.GetComponent<Text>().text = Building.Name;

            // Set exit button
            GameObject exitButtonObject = gm.marketplacePanel.transform.GetChild(2).gameObject;
            exitButtonObject.GetComponent<Button>().onClick.AddListener(DestroyObjects);


            GameObject payPanel = marketplacePanel.transform.GetChild(0).gameObject;
            GameObject earnPanel = marketplacePanel.transform.GetChild(1).gameObject;
            GameObject bottomPanel = marketplacePanel.transform.GetChild(2).gameObject;

            /*
            resourceFrame = new GameObject();
            resourceFrame.transform.parent = cardWindow.transform;
            resourceFrameImage = resourceFrame.AddComponent<SpriteRenderer>();
            resourceFrameImage.sortingLayerName = "TownInteractive";
            resourceFrameImage.sprite = UnityEngine.Resources.Load<Sprite>("Sprites/UI/resource_frame");
            resourceFrame.name = "frame";
            resourceFrame.tag = "toDestroy";
            */

            // Prepare global text for your resources, if you trade they will be updated
            textResource = new Text[5];

            for (int i = 0; i < System.Enum.GetNames(typeof(Resources.type)).Length; i++)
            {
                // Top resource imagebutton with listener
                GameObject resourceObjectPay = payPanel.transform.GetChild(i).gameObject;
                int selectedResource = i;
                //resourceObjectPay.GetComponent<Image>().sprite = libs.GetPortrait(selectedHero.GetPortraitID());
                Button buttonPay = resourceObjectPay.GetComponent<Button>();
                buttonPay.onClick.AddListener(() => setTrade(true, selectedResource, resourceObjectPay.transform.position));

                // Quick and dirty switch
                switch (i)
                {
                    case 0:
                        resourceObjectPay.GetComponent<Image>().sprite =
                            UnityEngine.Resources.Load<Sprite>("Sprites/UI/gold");
                        break;
                    case 1:
                        resourceObjectPay.GetComponent<Image>().sprite =
                            UnityEngine.Resources.Load<Sprite>("Sprites/UI/wood");
                        break;
                    case 2:
                        resourceObjectPay.GetComponent<Image>().sprite =
                            UnityEngine.Resources.Load<Sprite>("Sprites/UI/ore");
                        break;
                    case 3:
                        resourceObjectPay.GetComponent<Image>().sprite =
                            UnityEngine.Resources.Load<Sprite>("Sprites/UI/crystal");
                        break;
                    case 4:
                        resourceObjectPay.GetComponent<Image>().sprite =
                            UnityEngine.Resources.Load<Sprite>("Sprites/UI/gem");
                        break;
                }

                textResource[i] = resourceObjectPay.transform.GetChild(0).gameObject.GetComponent<Text>();
                textResource[i].text = player.Wallet.GetResource(i) + "";


                // Bottom resource with listener
                GameObject resourceObjectEarn = earnPanel.transform.GetChild(i).gameObject;

                Button buttonEarn = resourceObjectEarn.GetComponent<Button>();
                buttonEarn.onClick.AddListener(() => setTrade(false, selectedResource, resourceObjectEarn.transform.position));
                resourceObjectEarn.GetComponent<Image>().sprite = resourceObjectPay.GetComponent<Image>().sprite;
                
            }
            

            GameObject leftTextObject = bottomPanel.transform.GetChild(0).gameObject;
            GameObject sliderObject = bottomPanel.transform.GetChild(1).gameObject;
            GameObject rightTextObject = bottomPanel.transform.GetChild(2).gameObject;

            textLeftResource = leftTextObject.GetComponent<Text>();
            textRightResource = rightTextObject.GetComponent<Text>();
            
            slider = sliderObject.GetComponent<Slider>();
            slider.enabled = false;
            slider.maxValue = player.Wallet.GetResource(0) / ratio[1];

            slider.onValueChanged.AddListener(adjustTradeAmount);
            textLeftResource.text = "";
            textRightResource.text = "";
            //textLeftResource.text = ratio[0] * ratio[1] + "";
            //textRightResource.text = 1 + "";

            gm.marketplacePanel.SetActive(true);
        }

        private void CreateDwellingView()
        {
            // Gets the unit to create a view for 
            currentUnitBuilding = (UnitBuilding) building;

            toBuyObject = currentUnitBuilding.Unit;
            unitAmount = -1;

            gm.CreateUnitCard(cardWindow, canvas, currentUnitBuilding);

            // Creates the purchase slider
            string prefabPath = "Prefabs/Slider";
            GameObject sliderObject = Instantiate(UnityEngine.Resources.Load<GameObject>(prefabPath));
            sliderObject.transform.parent = cardSpriteRenderer.transform;
            sliderObject.transform.localScale = canvas.transform.localScale;
            float bottomY = 1.5f;
            sliderObject.transform.position = new Vector2(cardSpriteRenderer.transform.position.x, bottomY);
            slider = sliderObject.GetComponent<Slider>();
            //slider.maxValue = currentUnitBuilding.UnitsPresent;
            slider.maxValue = player.Wallet.CanAffordCount(currentUnitBuilding.Unit, currentUnitBuilding.UnitsPresent);
            slider.onValueChanged.AddListener(adjustUnits);

            // Text for total available units
            GameObject unitTotalCountObject = new GameObject();
            unitTotalCountObject.name = "Units available object";
            unitTotalCountObject.transform.parent = cardSpriteRenderer.transform;
            unitTotalCountObject.transform.localScale = canvas.transform.localScale;
            unitTotalCountObject.transform.position = new Vector2(sliderObject.transform.position.x + 1.8f,
                sliderObject.transform.position.y - 1f);
            unitTotalCountText = unitTotalCountObject.AddComponent<Text>();
            unitTotalCountText.color = Color.black;
            unitTotalCountText.alignment = TextAnchor.UpperLeft;
            unitTotalCountText.font = FONT;

            // Text for how many units to buy
            GameObject unitToBuyCountObject = new GameObject();
            unitToBuyCountObject.name = "Units to buy object";
            unitToBuyCountObject.transform.parent = cardSpriteRenderer.transform;
            unitToBuyCountObject.transform.localScale = canvas.transform.localScale;
            unitToBuyCountObject.transform.position = new Vector2(sliderObject.transform.position.x - 0.3f,
                sliderObject.transform.position.y - 1f);
            unitToBuyCountText = unitToBuyCountObject.AddComponent<Text>();
            unitToBuyCountText.color = Color.black;
            unitToBuyCountText.alignment = TextAnchor.UpperLeft;
            unitToBuyCountText.font = FONT;

            unitTotalCountText.text = currentUnitBuilding.UnitsPresent + "";
            unitToBuyCountText.text = slider.value + "";

            // positions for resource images and text
            Vector2 position = new Vector2(slider.transform.position.x - 3f, slider.transform.position.y + 2f);
            float offset = 0.6f;

            for (int i = 0; i < currentUnitBuilding.Unit.Price.GetResourceTab().Length; i++)
            {
                if (currentUnitBuilding.Unit.Price.GetResourceTab()[i] > 0)
                {
                    GameObject imageObject = new GameObject();
                    imageObject.transform.parent = cardWindow.transform;
                    imageObject.transform.position = position;
                    imageObject.name = town.Buildings[i].Cost.ResourceToString(i) + "image";
                    SpriteRenderer sprResource = imageObject.AddComponent<SpriteRenderer>();
                    string spritePath = "Sprites/UI/gold"; // TODO add to IngameObjectLibrary
                    if (i == 1)
                        spritePath = "Sprites/UI/wood";
                    else if (i == 2)
                        spritePath = "Sprites/UI/ore";
                    else if (i == 3)
                        spritePath = "Sprites/UI/crystal";
                    else if (i == 4)
                        spritePath = "Sprites/UI/gem";
                    sprResource.sprite = UnityEngine.Resources.Load<Sprite>(spritePath);
                    sprResource.sortingLayerName = "TownGUI";

                    position = new Vector2(position.x + offset, position.y);

                    GameObject textCostObject = new GameObject();
                    textCostObject.transform.parent = cardWindow.transform;
                    textCostObject.transform.position = position;
                    textCostObject.transform.localScale = canvas.transform.localScale;
                    textCostObject.name = currentUnitBuilding.Unit.Price.ToString(i);
                    Text textCost = textCostObject.AddComponent<Text>();
                    textCost.font = FONT;
                    textCost.text = currentUnitBuilding.Unit.Price.CostToString(i);
                    textCost.color = Color.black;
                    textCost.fontSize = 16;
                    textCost.alignment = TextAnchor.MiddleCenter;

                    position = new Vector2(position.x - offset, position.y - offset);
                }
            }

            // resets positions
            position = new Vector2(slider.transform.position.x + 2.5f, slider.transform.position.y + 2f);

            for (int i = 0; i < currentUnitBuilding.Unit.Price.GetResourceTab().Length; i++)
            {
                if (currentUnitBuilding.Unit.Price.GetResourceTab()[i] > 0)
                {
                    GameObject imageObject = new GameObject();
                    imageObject.transform.parent = cardWindow.transform;
                    imageObject.transform.position = position;
                    imageObject.name = town.Buildings[i].Cost.ResourceToString(i) + "image";
                    SpriteRenderer sprResource = imageObject.AddComponent<SpriteRenderer>();
                    string spritePath = "Sprites/UI/gold"; // TODO add to IngameObjectLibrary
                    if (i == 1)
                        spritePath = "Sprites/UI/wood";
                    else if (i == 2)
                        spritePath = "Sprites/UI/ore";
                    else if (i == 3)
                        spritePath = "Sprites/UI/crystal";
                    else if (i == 4)
                        spritePath = "Sprites/UI/gem";
                    sprResource.sprite = UnityEngine.Resources.Load<Sprite>(spritePath);
                    sprResource.sortingLayerName = "TownGUI";

                    position = new Vector2(position.x + offset, position.y);

                    GameObject textCostObject = new GameObject();
                    textCostObject.transform.parent = cardWindow.transform;
                    textCostObject.transform.localScale = canvas.transform.localScale;
                    textCostObject.transform.position = position;
                    textCostObject.name = currentUnitBuilding.Unit.Price.ToString(i);
                    textCost = textCostObject.AddComponent<Text>();
                    textCost.font = FONT;
                    textCost.text = (currentUnitBuilding.Unit.Price.GetResourceTab()[i] * slider.value) + "";
                    textCost.color = Color.black;
                    textCost.fontSize = 16;
                    textCost.alignment = TextAnchor.MiddleCenter;

                    position = new Vector2(position.x - offset, position.y - offset);

                    totalCostTexts[i] = textCost;
                }
            }


            gm.dwellingPanel.SetActive(true);
        }

        /// <summary>
        /// Function for unit slider to update the numbers on the UI
        /// </summary>
        /// <param name="value">Value of slider</param>
        private void adjustUnits(float value)
        {
            unitAmount = (int) value;
            unitTotalCountText.text = (currentUnitBuilding.UnitsPresent - unitAmount) + "";
            unitToBuyCountText.text = unitAmount + "";
            AdjustTotalCost();
        }

        /// <summary>
        /// Adjusts the texts for total price to pay
        /// </summary>
        private void AdjustTotalCost()
        {
            for (int i = 0; i < totalCostTexts.Length; i++)
            {
                if (currentUnitBuilding.Unit.Price.GetResourceTab()[i] > 0)
                    totalCostTexts[i].text = (currentUnitBuilding.Unit.Price.GetResourceTab()[i] * slider.value) + "";
            }
        }

        /// <summary>
        /// Gets the upper right corner of the current building card.
        /// </summary>
        /// <param name="sr">The sprite renderer to get the bounds from</param>
        /// <returns></returns>
        private Vector2 getUpRightCorner(SpriteRenderer sr)
        {
            return new Vector2(sr.transform.position.x + (sr.bounds.size.x / 2.2f),
                sr.transform.position.y + (sr.bounds.size.y / 2.3f));
        }

        /// <summary>
        /// Gets the bottom right corner of the current building card.
        /// </summary>
        /// <param name="sr">The sprite renderer to get the bounds from</param>
        /// <returns></returns>
        private Vector2 getDownRightCorner(SpriteRenderer sr)
        {
            return new Vector2(sr.transform.position.x + (sr.bounds.size.x / 2.2f),
                sr.transform.position.y - (sr.bounds.size.y / 2.3f));
        }
        
        void CreateExitButton()
        {
            GameObject.Find("ExitButton").GetComponent<Button>().onClick.AddListener(DestroyObjects);
        }

        /*
        void CreateBuyButton()
        {
            // gets the positions for exit button and buy button if the window type requires it
            buyButtonObject = Instantiate(UnityEngine.Resources.Load<GameObject>("Prefabs/Button"));
            buyButtonObject.transform.parent = cardWindow.transform;
            buyButtonObject.GetComponent<Image>().sprite = libs.GetUI(new ExitButton().GetSpriteID() + 1);
            buyButtonObject.name = "BuyButton";
            buyButtonObject.tag = "toDestroy";
            RectTransform rect = buyButtonObject.GetComponent<RectTransform>();
            rect.sizeDelta = new Vector2(buyButtonObject.GetComponent<Image>().sprite.bounds.size.x,
                buyButtonObject.GetComponent<Image>().sprite.bounds.size.y);
            Button button = buyButtonObject.GetComponent<Button>();
            button.onClick.AddListener(purchase);
            buyButtonObject.transform.position = getDownRightCorner(cardSpriteRenderer);
        }
        */

        /// <summary>
        /// Sets which object to be bought by buybutton.
        /// </summary>
        /// <param name="obj"></param>
        private void setBuy(System.Object obj, Vector2 position)
        {
            toBuyObject = obj;

            if (toBuyObject.GetType().BaseType.Name.Equals("Hero"))
                frameImage.sprite = UnityEngine.Resources.Load<Sprite>("Sprites/UI/hero_frame");
            else if (toBuyObject.GetType().BaseType.Name.Equals("Building"))
                frameImage.sprite = UnityEngine.Resources.Load<Sprite>("Sprites/UI/building_frame");

            frame.transform.position = position;
        }


        /// <summary>
        /// Sets which object to be bought by buybutton.
        /// </summary>
        /// <param name="obj"></param>
        private void setTrade(bool top, int type, Vector2 position)
        {
            if ((top && type != selectedEarnResource) || (!top && type != selectedPayResource))
            {
                //frameImage.sprite = UnityEngine.Resources.Load<Sprite>("Sprites/UI/resource_frame");
                if (top)
                {
                    selectedPayResource = type;
                    //frame.transform.position = position;
                }
                else
                {
                    selectedEarnResource = type;
                    //resourceFrame.transform.position = position;
                }
                if (selectedEarnResource >= 0 && selectedPayResource >= 0)
                {
                    if (!slider.isActiveAndEnabled)
                        slider.enabled = true;
                    slider.maxValue = (player.Wallet.GetResource(selectedPayResource) * ratio[selectedPayResource]) /
                                      ratio[selectedEarnResource];
                    Debug.Log(player.Wallet.GetResource(selectedPayResource));
                    adjustTradeAmount(1);
                    slider.value = 1;
                }
            }
        }

        /// <summary>
        /// Sets the amount to be traded and the texts of what you must pay for it
        /// </summary>
        /// <param name="value"></param>
        private void adjustTradeAmount(float value)
        {
            payAmount = (int) (value * ratio[selectedEarnResource]) / ratio[selectedPayResource];
            earnAmount = (int) value;
            textLeftResource.text = payAmount + "";
            textRightResource.text = earnAmount + "";
        }


        /// <summary>
        /// When clicked on buybutton, try to purchase active object
        /// </summary>
        private void purchase()
        {
            if (toBuyObject != null)
            {
                if (toBuyObject.GetType().BaseType.Name.Equals("Hero"))
                {
                    Hero buyHero = (Hero) toBuyObject;
                    // checks if the player can afford the hero and if the hero is alive
                    if (Player.Wallet.CanPay(buyHero.Cost) && (town.StationedHero == null || town.VisitingHero == null))
                    {
                        // If theres stationed and visitingunits there, check if you can merge with the existing troops
                        if ((town.StationedUnits == null || buyHero.Units.CanMerge(town.StationedUnits)) || (town.VisitingUnits == null || buyHero.Units.CanMerge(town.VisitingUnits)))
                        {

                            if (town.VisitingHero == null)
                            {
                                town.VisitingHero = buyHero;
                                town.VisitingHero.Units.Merge(town.VisitingUnits);
                                town.VisitingUnits = town.VisitingHero.Units;
                            }
                            else if (town.StationedHero == null)
                            {
                                town.StationedHero = buyHero;
                                town.StationedHero.Units.Merge(town.StationedUnits);
                                town.StationedUnits = town.StationedHero.Units;
                            }

                            Player.Wallet.Pay(buyHero.Cost);
                            Debug.Log("Bought the hero" + buyHero.Name);

                            // TODO PLACE HERO LOGICALLY AND VISUALLY
                            gm.PlaceHero(Player, buyHero, town.Position);
                            DestroyObjects();
                            gm.ReDrawArmyInTown(town);
                            gm.updateResourceText();
                            // Update herolist and townlist UI
                            gm.updateOverworldUI(player);
                        }
                        else
                            Debug.Log("Could not merge with existing troops");
                    }
                    else
                        Debug.Log("Not enough gold");
                    return;
                }
                else if (toBuyObject.GetType().BaseType.Name.Equals("Building"))
                {
                    Building buyBuilding = (Building) toBuyObject;

                    // Build building if town has not already built that day, player can pay, and building is not built already
                    if (!Town.HasBuiltThisRound && Player.Wallet.CanPay(buyBuilding.Cost) && !buyBuilding.Built && buyBuilding.MeetsRequirements(town))
                    {
                        // Player pays
                        Player.Wallet.Pay(buyBuilding.Cost);
                        town.HasBuiltThisRound = true;
                        gm.updateResourceText();

                        // Find the building in the town's list, build it and draw it in the view
                        for (int i = 0; i < town.Buildings.Length; i++)
                        {
                            if (town.Buildings[i].Equals(buyBuilding))
                            {
                                Debug.Log("YOU BOUGHT: " + buyBuilding.Name); // TODO remove
                                town.Buildings[i].Build();
                                gm.DrawBuilding(town, buyBuilding, i);
                                DestroyObjects();
                            }
                        }
                    }
                    else
                    {
                        Debug.Log("YOU DO NOT HAVE THE SUFFICIENT ECONOMICAL WEALTH TO PRODUCE THE STRUCTURE OF CHOICE: " + buyBuilding.Name); // TODO remove
                        return;
                        // TODO: what's the graphic feedback for trying to purchase something unpurchasable?
                    }
                }
                else if (toBuyObject.GetType().BaseType.BaseType.Name.Equals("Unit"))
                {
                    
                    Unit unit = (Unit) toBuyObject;
                    {
                        if (Player.Wallet.CanPayForMultiple(unit.Price, unitAmount) && town.StationedUnits.addUnit(unit, unitAmount) && currentUnitBuilding.AdjustPresentUnits((int)-slider.value))
                        {
                            // Add to hero list if theres a stationed hero
                            if (town.StationedHero != null)
                                town.StationedHero.Units = town.StationedUnits;
                            for (int i = 0; i < (int)slider.value; i++)
                            {
                                Player.Wallet.Pay(unit.Price);
                            }

                            unitTotalCountText.text = currentUnitBuilding.UnitsPresent + "";
                            unitToBuyCountText.text = slider.value + "";
                            slider.maxValue = player.Wallet.CanAffordCount(currentUnitBuilding.Unit, currentUnitBuilding.UnitsPresent);
                            gm.ReDrawArmyInTown(town);
                            gm.updateResourceText();
                        }
                    }
                }
            }
            else if (selectedPayResource >= 0 && selectedEarnResource >= 0)
            {
                if (Player.Wallet.CanPay(selectedPayResource, payAmount))
                {
                    Player.Wallet.adjustResource(selectedPayResource, -payAmount);
                    Player.Wallet.adjustResource(selectedEarnResource, earnAmount);
                    gm.updateResourceText();

                    for (int i = 0; i < 5; i++)
                    {
                        textResource[i].text = player.Wallet.GetResource(i) + "";
                        slider.maxValue = player.Wallet.GetResource(selectedPayResource) / ratio[selectedEarnResource];
                        //textLeftResource.text = "";
                        //textRightResource.text = "";
                        //slider.value = 1;
                    }
                }
            }
        }

        /// <summary>
        /// Remove all gameobjects that has the tag "toDestroy"
        /// </summary>
        private void DestroyObjects()
        {
            foreach (GameObject t in BuildingObjects)
            {
                // TODO: make into list so we dont have to check for null?
                if (t != null)
                    t.GetComponent<PolygonCollider2D>().enabled = true;
            }

            gm.DeactivateTownPanels(true);
        }
    }
}