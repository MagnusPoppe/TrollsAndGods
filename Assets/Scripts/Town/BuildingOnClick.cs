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
        Text[] textResource;
        int[] ratio = {1, 100, 100, 200, 200};
        Slider tradeSlider, unitSlider;
        int payAmount;
        int earnAmount;
        Text textLeftResource, textRightResource;
        Text textLeftUnit, textRightUnit;
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

            GameObject townHallContentPanel = gm.townHallPanel.transform.GetChild(0).gameObject;

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
                GameObject buildingPanel = townHallContentPanel.transform.GetChild(i).gameObject;
                // Building imagebutton with listener
                GameObject buildingObject = buildingPanel.transform.GetChild(0).gameObject;

                Building selectedBuilding = buildingArray[i];
                // Set building image
                buildingObject.GetComponent<Image>().sprite = libs.GetTown(selectedBuilding.GetSpriteBlueprintID() + offset);
                Button button = buildingObject.GetComponent<Button>();
                // Set buy listener to building
                button.onClick.AddListener(() => SetBuilding(selectedBuilding, buildingObject.transform.position));

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
                        resourceObject.GetComponent<Image>().sprite = UnityEngine.Resources.Load<Sprite>(spritePath);
                        // Setting resourcecost text
                        resourceObject.transform.GetChild(0).GetComponent<Text>().text = buildingArray[i].Cost.CostToString(j);
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
            GameObject heroContentPanel = gm.tavernPanel.transform.GetChild(0).gameObject.transform.GetChild(0).gameObject;

            // Set Buildingname Text
            GameObject buildingNameObject = gm.tavernPanel.transform.GetChild(1).gameObject;
            buildingNameObject.GetComponent<Text>().text = Building.Name;

            // Set exit button
            GameObject exitButtonObject = gm.tavernPanel.transform.GetChild(2).gameObject;
            exitButtonObject.GetComponent<Button>().onClick.AddListener(DestroyObjects);

            // Add each hero that you can buy in Tavern
            int count = 0;

            for (int i = 0; i < gm.heroes.Length && count < 5; i++)
            {
                if (gm.heroes[i] != null && !gm.heroes[i].Alive)
                {
                    // Hero imagebutton with listener
                    GameObject heroObject = heroContentPanel.transform.GetChild(count).gameObject;
                    heroObject.SetActive(true);

                    Hero selectedHero = gm.heroes[i];
                    heroObject.GetComponent<Image>().sprite = libs.GetPortrait(selectedHero.GetPortraitID());
                    Button button = heroObject.GetComponent<Button>();
                    button.onClick.AddListener(() => SetHero(selectedHero, heroObject.transform.position));

                    button.transform.GetChild(0).GetComponent<Text>().text = selectedHero.Name;
                    // TODO listener to popup hero panel
                    count++;
                }
            }
            // Hide unused panels
            while(count < 5)
            {
                heroContentPanel.transform.GetChild(count).gameObject.SetActive(false);
                count++;
            }
            gm.tavernPanel.SetActive(true);
        }


        /// <summary>
        /// Creates the marketplace view for trading resources
        /// </summary>
        private void CreateMarketplaceView()
        {
            GameObject marketplaceContentPanel = gm.marketplacePanel.transform.GetChild(0).gameObject;

            // Set Buildingname Text
            GameObject buildingNameObject = gm.marketplacePanel.transform.GetChild(1).gameObject;
            buildingNameObject.GetComponent<Text>().text = Building.Name;

            // Set exit button
            GameObject exitButtonObject = gm.marketplacePanel.transform.GetChild(2).gameObject;
            exitButtonObject.GetComponent<Button>().onClick.AddListener(DestroyObjects);


            GameObject payPanel = marketplaceContentPanel.transform.GetChild(0).gameObject;
            GameObject earnPanel = marketplaceContentPanel.transform.GetChild(1).gameObject;
            GameObject bottomPanel = marketplaceContentPanel.transform.GetChild(2).gameObject;

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

                // Set buy button onclick
                gm.purchaseButton.GetComponent<Button>().onClick.RemoveAllListeners();
                gm.purchaseButton.GetComponent<Button>().onClick.AddListener(() => BuyResource(selectedPayResource, payAmount, selectedEarnResource, earnAmount));

            }


            GameObject leftTextObject = bottomPanel.transform.GetChild(0).gameObject;
            GameObject sliderObject = bottomPanel.transform.GetChild(1).gameObject;
            GameObject rightTextObject = bottomPanel.transform.GetChild(2).gameObject;

            textLeftResource = leftTextObject.GetComponent<Text>();
            textRightResource = rightTextObject.GetComponent<Text>();
            
            tradeSlider = sliderObject.GetComponent<Slider>();
            tradeSlider.enabled = false;
            tradeSlider.maxValue = player.Wallet.GetResource(0) / ratio[1];

            tradeSlider.onValueChanged.AddListener(adjustTradeAmount);
            textLeftResource.text = "";
            textRightResource.text = "";
            //textLeftResource.text = ratio[0] * ratio[1] + "";
            //textRightResource.text = 1 + "";

            gm.marketplacePanel.SetActive(true);
        }

        private void CreateDwellingView()
        {
            currentUnitBuilding = (UnitBuilding)building;
            unitAmount = -1;
            GameObject dwellingContentPanel = gm.dwellingPanel.transform.GetChild(0).gameObject;

            // Set Buildingname Text
            GameObject buildingNameObject = gm.dwellingPanel.transform.GetChild(1).gameObject;
            buildingNameObject.GetComponent<Text>().text = Building.Name;

            // Set exit button
            GameObject exitButtonObject = gm.dwellingPanel.transform.GetChild(2).gameObject;
            exitButtonObject.GetComponent<Button>().onClick.AddListener(DestroyObjects);

            gm.SetUnitCard(dwellingContentPanel, currentUnitBuilding.Unit);
            
            // Sets the initial text for TotalUnits and ToBuyUnits
            GameObject purchasePanel = dwellingContentPanel.transform.GetChild(0).gameObject;

            // Set slider object and max value (max value is set to how many units the current player can afford);
            GameObject sliderObject = purchasePanel.transform.GetChild(1).gameObject;
            unitSlider = sliderObject.GetComponent<Slider>();
            unitSlider.onValueChanged.AddListener(UpdateUnitText);
            unitSlider.maxValue = player.Wallet.CanAffordCount(currentUnitBuilding.Unit, currentUnitBuilding.UnitsPresent);

            textLeftUnit = purchasePanel.transform.GetChild(0).gameObject.GetComponent<Text>();
            textRightUnit = purchasePanel.transform.GetChild(2).gameObject.GetComponent<Text>();
            textLeftUnit.text = currentUnitBuilding.UnitsPresent + "";
            textRightUnit.text = sliderObject.GetComponent<Slider>().value + "";


            //GameObject unitPanel = dwellingContentPanel.transform.GetChild(2).gameObject;

            // Set buy button onclick
            gm.purchaseButton.GetComponent<Button>().onClick.RemoveAllListeners();
            gm.purchaseButton.GetComponent<Button>().onClick.AddListener(() => BuyUnit(currentUnitBuilding.Unit, (int)unitSlider.value));

            gm.dwellingPanel.SetActive(true);
        }

        /// <summary>
        /// Function for unit slider to update the numbers on the UI
        /// </summary>
        /// <param name="value">Value of slider</param>
        private void UpdateUnitText(float value)
        {
            unitAmount = (int) value;
            textLeftUnit.text = (currentUnitBuilding.UnitsPresent - unitAmount) + "";
            textRightUnit.text = unitAmount + "";
            


            // Set panel info
            SetCostPanelText(currentUnitBuilding.Unit.Price.GetCostScaled((unitAmount)));
        }

        /*
        /// <summary>
        /// Adjusts the texts for total price to pay
        /// </summary>
        private void AdjustTotalCost()
        {
            for (int i = 0; i < totalCostTexts.Length; i++)
            {
                if (currentUnitBuilding.Unit.Price.GetResourceTab()[i] > 0)
                    totalCostTexts[i].text = (currentUnitBuilding.Unit.Price.GetResourceTab()[i] * tradeSlider.value) + "";
            }
        }
        */

        /// <summary>
        /// Sets which building to buy
        /// </summary>
        /// <param name="obj"></param>
        private void SetBuilding(Building toBuyBuilding, Vector2 position)
        {
            gm.purchaseButton.GetComponent<Button>().onClick.RemoveAllListeners();
            SetCostPanelText(toBuyBuilding.Cost);
            gm.purchaseButton.GetComponent<Button>().onClick.AddListener(() => BuyBuilding(toBuyBuilding));
        }

        private void SetHero(Hero toBuyHero, Vector2 position)
        {
            gm.purchaseButton.GetComponent<Button>().onClick.RemoveAllListeners();
            SetCostPanelText(toBuyHero.Cost);
            gm.purchaseButton.GetComponent<Button>().onClick.AddListener(() => BuyHero(toBuyHero));
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
                    if (!tradeSlider.isActiveAndEnabled)
                        tradeSlider.enabled = true;
                    tradeSlider.maxValue = (player.Wallet.GetResource(selectedPayResource) * ratio[selectedPayResource]) / ratio[selectedEarnResource];
                    Debug.Log(player.Wallet.GetResource(selectedPayResource));
                    adjustTradeAmount(1);
                    tradeSlider.value = 1;
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


            Cost cost = new Cost(0, 0, 0, 0, 0);
            cost.adjustResource(selectedPayResource, payAmount);
            cost.adjustResource(selectedEarnResource, -earnAmount);

            // Set panel info
            SetCostPanelText(cost);
        }



        /// <summary>
        /// Check all resources in the resourcearray in cost, set the panel's textboxes to the values
        /// </summary>
        /// <param name="cost">The costobject with the int array to find the pay or earn values</param>
        public void SetCostPanelText(Cost cost)
        {
            for (int i = 0; i < cost.GetResourceTab().Length; i++)
            {
                Text costText = gm.adjustResourcePanel.transform.GetChild(0).gameObject.transform.GetChild(i).GetComponent<Text>();
                // Set - and value if you can're supposed to pay the amount
                if (cost.GetResourceTab()[i] > 0)
                    costText.text = "-" + cost.GetResourceTab()[i];
                // + If you gain the value
                else if (cost.GetResourceTab()[i] < 0)
                {
                    costText.text = "+" + Math.Abs(cost.GetResourceTab()[i]);
                }
                // Else empty text
                else
                    costText.text = "";
            }
            // Show the panel and buybutton
            gm.adjustResourcePanel.SetActive(true);
        }

        public void BuyResource(int payPos, int pay, int earnPos, int earn)
        {
            if (payPos >= 0 && earnPos >= 0)
            {
                if (Player.Wallet.CanPay(payPos, payAmount))
                {
                    //Player.Wallet.adjustResource(payPos, -pay);
                    //Player.Wallet.adjustResource(earnPos, earn);

                    Cost resourceCost = new Cost();
                    resourceCost.adjustResource(payPos, pay);
                    resourceCost.adjustResource(earnPos, -earn);
                    Player.Wallet.Pay(resourceCost);
                    gm.updateResourceText();

                    for (int i = 0; i < 5; i++)
                    {
                        textResource[i].text = player.Wallet.GetResource(i) + "";
                        tradeSlider.maxValue = player.Wallet.GetResource(payPos) / ratio[earnPos];
                        //textLeftResource.text = "";
                        //textRightResource.text = "";
                        //slider.value = 1;
                    }
                }
            }
        }

        public void BuyHero(Hero hero)
        {
            //if (toBuyObject.GetType().BaseType.Name.Equals("Hero"))
            //{
                // checks if the player can afford the hero and if the hero is alive
                if (Player.Wallet.CanPay(hero.Cost) && (town.StationedHero == null || town.VisitingHero == null))
                {
                    // If theres stationed and visitingunits there, check if you can merge with the existing troops
                    if ((town.StationedUnits == null || hero.Units.CanMerge(town.StationedUnits)) || (town.VisitingUnits == null || hero.Units.CanMerge(town.VisitingUnits)))
                    {

                        if (town.VisitingHero == null)
                        {
                            town.VisitingHero = hero;
                            town.VisitingHero.Units.Merge(town.VisitingUnits);
                            town.VisitingUnits = town.VisitingHero.Units;
                        }
                        else if (town.StationedHero == null)
                        {
                            town.StationedHero = hero;
                            town.StationedHero.Units.Merge(town.StationedUnits);
                            town.StationedUnits = town.StationedHero.Units;
                        }

                        Player.Wallet.Pay(hero.Cost);
                        Debug.Log("Bought the hero" + hero.Name);

                        // TODO PLACE HERO LOGICALLY AND VISUALLY
                        gm.PlaceHero(Player, hero, town.Position);
                        DestroyObjects();
                        gm.ReDrawArmyInTown(town);
                        gm.updateResourceText();
                        // Update herolist and townlist UI
                        gm.updateOverworldUI(player);
                        // Deactivate resource panel
                        gm.adjustResourcePanel.SetActive(false);
                    }
                    else
                        Debug.Log("Could not merge with existing troops");
                }
                else
                    Debug.Log("Not enough gold");
            //}
        }

        public void BuyBuilding(Building building)
        {
            // Build building if town has not already built that day, player can pay, and building is not built already
            if (!Town.HasBuiltThisRound && Player.Wallet.CanPay(building.Cost) && !building.Built &&
                building.MeetsRequirements(town))
            {
                // Player pays
                Player.Wallet.Pay(building.Cost);
                town.HasBuiltThisRound = true;
                gm.updateResourceText();

                // Find the building in the town's list, build it and draw it in the view
                for (int i = 0; i < town.Buildings.Length; i++)
                {
                    if (town.Buildings[i].Equals(building))
                    {
                        Debug.Log("YOU BOUGHT: " + building.Name); // TODO remove
                        town.Buildings[i].Build();
                        gm.DrawBuilding(town, building, i);
                        DestroyObjects();
                    }
                }
                // Deactivate resource panel
                gm.adjustResourcePanel.SetActive(false);
            }
            else
            {
                Debug.Log(building.Name + "{HasBuilt=" + Town.HasBuiltThisRound + " ; CanPay=" + Player.Wallet.CanPay(building.Cost) + " ; isBuilt=" + building.Built + " ; MeetsRequirement=" + building.MeetsRequirements(town) + "}");
                // TODO: what's the graphic feedback for trying to purchase something unpurchasable?
            }
        }

        public void BuyUnit(Unit unit, int amount)
        {
            if (Player.Wallet.CanPayForMultiple(unit.Price, unitAmount) && town.StationedUnits.addUnit(unit, unitAmount) && currentUnitBuilding.AdjustPresentUnits((int)-unitSlider.value))
            {
                // Add to hero list if theres a stationed hero
                if (town.StationedHero != null)
                    town.StationedHero.Units = town.StationedUnits;

                // TODO cost instead?
                for (int i = 0; i < (int)unitSlider.value; i++)
                {
                    Player.Wallet.Pay(unit.Price);
                }

                textLeftUnit.text = currentUnitBuilding.UnitsPresent + "";
                textRightUnit.text = unitSlider.value + "";
                unitSlider.maxValue = player.Wallet.CanAffordCount(currentUnitBuilding.Unit, currentUnitBuilding.UnitsPresent);
                gm.ReDrawArmyInTown(town);
                gm.updateResourceText();
                // Deactivate resource panel
                gm.adjustResourcePanel.SetActive(false);

                // Set panel info
                if (unitSlider.maxValue > 0)
                    SetCostPanelText(currentUnitBuilding.Unit.Price.GetCostScaled((unitAmount)));

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

            gm.DeactivateTownPanels();
        }
    }
}