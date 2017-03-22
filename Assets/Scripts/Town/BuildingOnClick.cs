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
            
            // Default set resources to nothing
            selectedEarnResource = selectedPayResource = -1;

            // Initialize frame
            frame = new GameObject();
            frame.name = "frame";
            frameImage = frame.AddComponent<SpriteRenderer>();
            frameImage.sortingLayerName = "TownInteractive";

            // TODO: Make less specific
            BuildingCard card = new BuildingCard(windowType, IngameObjectLibrary.Category.UI);

            canvas = GameObject.Find("Canvas");

            // Creates a building card game ojbect with a spriterenderer, sets its position, layer, name and parent
            cardWindow = new GameObject();
            cardWindow.transform.parent = canvas.transform;
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

            // Creates a town hall view if the building type matches it
            if (windowType == WindowTypes.TOWN_HALL_CARD)
            {
                CreateBuyButton();
                CreateTownHallView();
            }
            // Creates a tavern view if the building type matches it
            else if (windowType == WindowTypes.TAVERN_CARD)
            {
                CreateBuyButton();
                CreateTavernView();
            }
            // Creates a marketplace view if the building type matches it
            else if (windowType == WindowTypes.MARKETPLACE_CARD)
            {
                CreateBuyButton();
                CreateMarketplaceView();
            }
            // Creates a dwelling view if the building type matches it
            else if (windowType == WindowTypes.DWELLING_CARD)
            {
                CreateBuyButton();
                CreateDwellingView();
            }
            // Creates a general building view if the building type matches it
            else if (windowType == WindowTypes.BUILDING_CARD)
            {
                CreateBuildingView();
            }


            CreateExitButton();
            frame.transform.parent = GameObject.Find("TownCardPanel").transform;
        }

        /// <summary>
        /// General view for a building with no specific function. Simply states its name and description
        /// </summary>
        private void CreateBuildingView()
        {
            // Add text below building
            GameObject textObject = new GameObject();
            textObject.transform.parent = GameObject.Find("TownCardPanel").transform;
            textObject.transform.localScale = canvas.transform.localScale;
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
            UnitBuilding unitBuilding = (UnitBuilding) building;

            toBuyObject = unitBuilding.Unit;
            unitAmount = -1;

            string unitName = unitBuilding.GetUnitName();
            string unitAttack = unitBuilding.GetAttack() + "";
            string unitDefense = unitBuilding.GetDefense() + "";
            string unitMagic = unitBuilding.GetMagic() + "";
            string unitSpeed = unitBuilding.GetSpeed() + "";

            // TODO: moves


            GameObject unitNameObject = new GameObject();
            unitNameObject.transform.parent = cardWindow.transform;
            unitNameObject.transform.position = new Vector2(cardWindow.transform.position.x + 0.11f, cardWindow.transform.position.y + 2.34f);
            unitNameObject.transform.localScale = canvas.transform.localScale;
            unitNameObject.name = unitName + " name text";
            Text unitNameText = unitNameObject.AddComponent<Text>();
            unitNameText.text = unitName;
            unitNameText.font = UnityEngine.Resources.Load<Font>("Fonts/ARIAL");
            unitNameText.fontSize = 18;
            unitNameText.color = Color.black;

            GameObject unitAttackObject = new GameObject();
            unitAttackObject.transform.parent = cardWindow.transform;
            unitAttackObject.transform.position = new Vector2(cardWindow.transform.position.x + 2.14f, cardWindow.transform.position.y - 1.25f);
            unitAttackObject.transform.localScale = canvas.transform.localScale;
            unitAttackObject.name = unitName + " attack text";
            Text unitAttackText = unitAttackObject.AddComponent<Text>();
            unitAttackText.text = unitAttack;
            unitAttackText.font = UnityEngine.Resources.Load<Font>("Fonts/ARIAL");
            unitAttackText.fontSize = 18;
            unitAttackText.color = Color.black;

            GameObject unitDefenseObject = new GameObject();
            unitDefenseObject.transform.parent = cardWindow.transform;
            unitDefenseObject.transform.position = new Vector2(cardWindow.transform.position.x + 2.14f, cardWindow.transform.position.y - 1.9f);
            unitDefenseObject.transform.localScale = canvas.transform.localScale;
            unitDefenseObject.name = unitName + " defense text"; ;
            Text unitDefenseText = unitDefenseObject.AddComponent<Text>();
            unitDefenseText.text = unitDefense;
            unitDefenseText.font = UnityEngine.Resources.Load<Font>("Fonts/ARIAL");
            unitDefenseText.fontSize = 18;
            unitDefenseText.color = Color.black;

            GameObject unitMagicObject = new GameObject();
            unitMagicObject.transform.parent = cardWindow.transform;
            unitMagicObject.transform.position = new Vector2(cardWindow.transform.position.x + 2.14f, cardWindow.transform.position.y - 2.5f);
            unitMagicObject.transform.localScale = canvas.transform.localScale;
            unitMagicObject.name = unitName + " magic text";
            Text unitMagicText = unitMagicObject.AddComponent<Text>();
            unitMagicText.text = unitMagic;
            unitMagicText.font = UnityEngine.Resources.Load<Font>("Fonts/ARIAL");
            unitMagicText.fontSize = 18;
            unitMagicText.color = Color.black;

            GameObject unitSpeedObject = new GameObject();
            unitSpeedObject.transform.parent = cardWindow.transform;
            unitSpeedObject.transform.position = new Vector2(cardWindow.transform.position.x + 2.14f, cardWindow.transform.position.y - 3f);
            unitSpeedObject.transform.localScale = canvas.transform.localScale;
            unitSpeedObject.name = unitName + " speed text";
            Text unitSpeedText = unitSpeedObject.AddComponent<Text>();
            unitSpeedText.text = unitSpeed;
            unitSpeedText.font = UnityEngine.Resources.Load<Font>("Fonts/ARIAL");
            unitSpeedText.fontSize = 18;
            unitSpeedText.color = Color.black;


            string prefabPath = "Prefabs/Slider";
            GameObject sliderObject = Instantiate(UnityEngine.Resources.Load<GameObject>(prefabPath));
            sliderObject.transform.parent = cardSpriteRenderer.transform;
            sliderObject.transform.localScale = canvas.transform.localScale;
            float bottomY = cardSpriteRenderer.bounds.size.y / 4;
            sliderObject.transform.position = new Vector2(cardSpriteRenderer.transform.position.x, bottomY);
            slider = sliderObject.GetComponent<Slider>();
            slider.maxValue = 100; // TODO: max is how many units available in stack
            slider.onValueChanged.AddListener(adjustUnits);
        }

        private void adjustUnits(float value)
        {
            unitAmount = (int) value;
        }

        /// <summary>
        /// Creates the marketplace view for trading resources
        /// </summary>
        private void CreateMarketplaceView()
        {
            Vector2 nextPositionPay = new Vector2(cardSpriteRenderer.transform.position.x, cardSpriteRenderer.transform.position.y + (cardSpriteRenderer.bounds.size.y / 4));
            Vector2 nextPositionEarn = new Vector2(nextPositionPay.x, (nextPositionPay.y - cardSpriteRenderer.bounds.size.y/5f));
            float startX = nextPositionPay.x;

            float leftX = startX;
            float rightX = startX;
            
            resourceFrame = new GameObject();
            resourceFrameImage = resourceFrame.AddComponent<SpriteRenderer>();
            resourceFrameImage.sortingLayerName = "TownInteractive";
            resourceFrameImage.sprite = UnityEngine.Resources.Load<Sprite>("Sprites/UI/resource_frame");
            resourceFrame.name = "frame";
            resourceFrame.tag = "toDestroy";

            // Prepare global text for your resources, if you trade they will be updated
            textResource = new Text[5];

            for (int i = 0; i < System.Enum.GetNames(typeof(Resources.type)).Length; i++)
            {

                // Top resource imagebutton with listener
                GameObject resourceObjectPay = Instantiate(UnityEngine.Resources.Load<GameObject>("Prefabs/Button"));
                resourceObjectPay.transform.parent = GameObject.Find("TownCardPanel").transform;
                resourceObjectPay.transform.position = nextPositionPay;
                resourceObjectPay.name = player.Wallet.GetResourceName(i);
                resourceObjectPay.tag = "toDestroy";
                int selectedResource = i;
                //resourceObjectPay.GetComponent<Image>().sprite = libs.GetPortrait(selectedHero.GetPortraitID());
                Button buttonPay = resourceObjectPay.GetComponent<Button>();
                buttonPay.onClick.AddListener(() => setTrade(true, selectedResource, resourceObjectPay.transform.position));
                
                // Quick and dirty switch
                switch (i)
                {
                    case 0: resourceObjectPay.GetComponent<Image>().sprite = UnityEngine.Resources.Load<Sprite>("Sprites/UI/gold"); break;
                    case 1: resourceObjectPay.GetComponent<Image>().sprite = UnityEngine.Resources.Load<Sprite>("Sprites/UI/wood"); break;
                    case 2: resourceObjectPay.GetComponent<Image>().sprite = UnityEngine.Resources.Load<Sprite>("Sprites/UI/ore"); break;
                    case 3: resourceObjectPay.GetComponent<Image>().sprite = UnityEngine.Resources.Load<Sprite>("Sprites/UI/crystal"); break;
                    case 4: resourceObjectPay.GetComponent<Image>().sprite = UnityEngine.Resources.Load<Sprite>("Sprites/UI/gem"); break;
                }
                RectTransform rectPay = resourceObjectPay.GetComponent<RectTransform>();
                rectPay.sizeDelta = new Vector2(resourceObjectPay.GetComponent<Image>().sprite.bounds.size.x, resourceObjectPay.GetComponent<Image>().sprite.bounds.size.y) * 2;

                GameObject textNameObject = new GameObject();
                textNameObject.transform.parent = resourceObjectPay.transform;
                textNameObject.transform.localScale = canvas.transform.localScale;
                textNameObject.name = player.Wallet.GetResource(i) + ", " + player.Wallet.GetResourceName(i);
                textResource[i] = textNameObject.AddComponent<Text>();
                textResource[i].font = UnityEngine.Resources.Load<Font>("Fonts/ARIAL");
                textResource[i].fontSize = 16;
                textResource[i].text = player.Wallet.GetResource(i) + "";
                textResource[i].color = Color.black;
                textResource[i].alignment = TextAnchor.LowerCenter;
                textNameObject.transform.position = nextPositionPay;


                // Bottom resource with listener
                GameObject resourceObjectEarn = Instantiate(UnityEngine.Resources.Load<GameObject>("Prefabs/Button"));
                resourceObjectEarn.transform.parent = GameObject.Find("TownCardPanel").transform;
                resourceObjectEarn.transform.position = nextPositionEarn;
                resourceObjectEarn.name = resourceObjectPay.name;
                resourceObjectEarn.tag = "toDestroy";
                //resourceObjectPay.GetComponent<Image>().sprite = libs.GetPortrait(selectedHero.GetPortraitID());
                RectTransform rectEarn = resourceObjectEarn.GetComponent<RectTransform>();
                rectEarn.sizeDelta = rectPay.sizeDelta;
                Button buttonEarn = resourceObjectEarn.GetComponent<Button>();
                buttonEarn.onClick.AddListener(() => setTrade(false, selectedResource, resourceObjectEarn.transform.position));
                resourceObjectEarn.GetComponent<Image>().sprite = resourceObjectPay.GetComponent<Image>().sprite;
                
                // Calculate the position for the next gameobject
                float newX = nextPositionPay.x;
                float newY = nextPositionPay.y;

                if (i % 2 == 0)
                {
                    newX = leftX -= (resourceObjectEarn.GetComponent<Image>().sprite.bounds.size.x * 4f);
                }
                else
                {
                    newX = rightX += (resourceObjectEarn.GetComponent<Image>().sprite.bounds.size.x * 4f);
                }
                nextPositionPay = new Vector2(newX, newY);
                nextPositionEarn = new Vector2(newX, nextPositionEarn.y);
                
            }

            string prefabPath = "Prefabs/Slider";
            GameObject sliderObject = Instantiate(UnityEngine.Resources.Load<GameObject>(prefabPath));
            sliderObject.transform.parent = cardSpriteRenderer.transform;
            sliderObject.transform.localScale = canvas.transform.localScale;
            
            float bottomY = cardSpriteRenderer.transform.position.y * 0.5f;

            sliderObject.transform.position = new Vector2(cardSpriteRenderer.transform.position.x, bottomY);
            

            GameObject textLeftObject = new GameObject();
            textLeftObject.transform.parent = cardSpriteRenderer.transform;
            textLeftObject.transform.localScale = canvas.transform.localScale;
            textLeftResource = textLeftObject.AddComponent<Text>();
            textLeftResource.font = UnityEngine.Resources.Load<Font>("Fonts/ARIAL");
            textLeftResource.fontSize = 16;
            //textLeftResource.text = ratio[0] * ratio[1] + "";
            textLeftResource.color = Color.black;
            textLeftResource.alignment = TextAnchor.MiddleRight;
            textLeftObject.transform.position = new Vector2((cardSpriteRenderer.transform.position.x * 0.92f), bottomY);
            

            GameObject textRightObject = new GameObject();
            textRightObject.transform.parent = cardSpriteRenderer.transform;
            textRightObject.transform.localScale = canvas.transform.localScale;
            textRightResource = textRightObject.AddComponent<Text>();
            textRightResource.font = UnityEngine.Resources.Load<Font>("Fonts/ARIAL");
            textRightResource.fontSize = 16;
            //textRightResource.text = 1 + "";
            textRightResource.color = Color.black;
            textRightResource.alignment = TextAnchor.MiddleLeft;
            textRightResource.transform.position = new Vector2((cardSpriteRenderer.transform.position.x * 1.08f), bottomY);
            
            slider = sliderObject.GetComponent<Slider>();
            slider.enabled = false;
            slider.maxValue = player.Wallet.GetResource(0) / ratio[1];

            slider.onValueChanged.AddListener(adjustTradeAmount);
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
                // If it's already purchased or you can't build it, use another sprite, set by an offset
                int offset = 0;
                if (town.Buildings[i].Built)
                    offset = town.Buildings.Length;
                else if (!Player.Wallet.CanPay(town.Buildings[i].Cost) || town.HasBuiltThisRound || !town.Buildings[i].MeetsRequirements(town))
                    offset = town.Buildings.Length * 2;

                // Building imagebutton with listener
                GameObject buildingObject = Instantiate(UnityEngine.Resources.Load<GameObject>("Prefabs/Button"));
                buildingObject.transform.parent = GameObject.Find("TownCardPanel").transform;
                buildingObject.transform.position = nextPosition;
                buildingObject.name = town.Buildings[i].Name;
                buildingObject.tag = "toDestroy";
                Building selectedBuilding = town.Buildings[i];
                buildingObject.GetComponent<Image>().sprite = libs.GetTown(selectedBuilding.GetSpriteBlueprintID() + offset);
                RectTransform rect = buildingObject.GetComponent<RectTransform>();
                rect.sizeDelta = new Vector2(buildingObject.GetComponent<Image>().sprite.bounds.size.x, buildingObject.GetComponent<Image>().sprite.bounds.size.y);
                Button button = buildingObject.GetComponent<Button>();
                button.onClick.AddListener(() => setBuy(selectedBuilding, buildingObject.transform.position));

                // Add text below building
                GameObject textObject = new GameObject();
                textObject.transform.parent = GameObject.Find(town.Buildings[i].Name).transform;
                textObject.transform.localScale = canvas.transform.localScale;
                textObject.name = town.Buildings[i].Name + " text";
                textObject.tag = "toDestroy";
                Text text = textObject.AddComponent<Text>();
                text.font = UnityEngine.Resources.Load<Font>("Fonts/ARIAL");
                text.fontSize = 18;
                text.text = town.Buildings[i].Name;
                text.alignment = TextAnchor.UpperCenter;
                text.color = Color.black;

                // Position icon and text
                buildingObject.transform.position = nextPosition;
                textObject.transform.position = new Vector2(buildingObject.transform.position.x, buildingObject.transform.position.y - buildingObject.GetComponent<Image>().sprite.bounds.size.y);


                // Create text and image for every resource next to the icon
                Vector2 position = new Vector2(buildingObject.transform.position.x + (buildingObject.GetComponent<Image>().sprite.bounds.size.x / 1.6f), nextPosition.y + buildingObject.GetComponent<Image>().sprite.bounds.size.y / 2);
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
                        textCostObject.transform.localScale = canvas.transform.localScale;
                        textCostObject.name = town.Buildings[i].Cost.ToString(j);
                        Text textCost = textCostObject.AddComponent<Text>();
                        textCost.font = UnityEngine.Resources.Load<Font>("Fonts/ARIAL");
                        textCost.text = town.Buildings[i].Cost.CostToString(j);
                        textCost.color = Color.black;
                        textCost.alignment = TextAnchor.MiddleLeft;

                        imageObject.transform.position = position;
                        textCostObject.transform.position = new Vector2(position.x + (sprResource.bounds.size.x * 2.3f), position.y);
                        position = new Vector2(buildingObject.transform.position.x + (buildingObject.GetComponent<Image>().sprite.bounds.size.x / 1.6f), position.y - sprResource.bounds.size.y);

                    }
                }
                // Calculate the position for the next gameobject
                float newX = nextPosition.x;
                float newY = nextPosition.y;

                if (nextPosition.x > cardSpriteRenderer.transform.position.x + buildingObject.GetComponent<Image>().sprite.bounds.size.x * 1.3f)
                {
                    newX = startX;
                    newY -= (buildingObject.GetComponent<Image>().sprite.bounds.size.y * 1.5f);
                }
                else
                    newX += (buildingObject.GetComponent<Image>().sprite.bounds.size.x * 1.45f);
                nextPosition = new Vector2(newX, newY);
            }
        }

        /// <summary>
        /// Creates the tavern view for buying heroes
        /// </summary>
        public void CreateTavernView()
        {
            Vector2 nextPosition = new Vector2(cardSpriteRenderer.transform.position.x, cardSpriteRenderer.transform.position.y + (cardSpriteRenderer.bounds.size.y / 4));
            float startX = nextPosition.x;

            float leftX = startX;
            float rightX = startX;
            // Add each hero that you can buy in Tavern
            int count = 0;
            for (int i = 0; i < gm.heroes.Length; i++)
            {
                if (gm.heroes[i] != null && !gm.heroes[i].Alive)
                {
                    // Hero imagebutton with listener
                    GameObject heroObject = Instantiate(UnityEngine.Resources.Load<GameObject>("Prefabs/Button"));
                    heroObject.transform.parent = canvas.transform;
                    heroObject.transform.position = nextPosition;
                    Hero selectedHero = gm.heroes[i];
                    heroObject.GetComponent<Image>().sprite = libs.GetPortrait(selectedHero.GetPortraitID());
                    RectTransform rect = heroObject.GetComponent<RectTransform>();
                    rect.sizeDelta = new Vector2(heroObject.GetComponent<Image>().sprite.bounds.size.x, heroObject.GetComponent<Image>().sprite.bounds.size.y);
                    Button button = heroObject.GetComponent<Button>();
                    button.onClick.AddListener(() => setBuy(selectedHero, heroObject.transform.position));
                    heroObject.tag = "toDestroy";

                    // Hero name Text
                    GameObject textNameObject = new GameObject();
                    textNameObject.transform.parent = heroObject.transform;
                    textNameObject.transform.localScale = canvas.transform.localScale;
                    textNameObject.name = gm.heroes[i].Name;
                    textNameObject.tag = "toDestroy";
                    Text textName = textNameObject.AddComponent<Text>();
                    textName.font = UnityEngine.Resources.Load<Font>("Fonts/ARIAL");
                    textName.fontSize = 18;
                    textName.text = gm.heroes[i].Name;
                    textName.color = Color.black;
                    textNameObject.transform.position = new Vector2(nextPosition.x, (nextPosition.y - heroObject.GetComponent<Image>().sprite.bounds.size.y));

                    // Calculate the position for the next gameobject
                    float newX = nextPosition.x;
                    float newY = nextPosition.y;

                    if (count % 2 == 0)
                        newX = leftX -= (heroObject.GetComponent<Image>().sprite.bounds.size.x * 1.2f);
                    else
                        newX = rightX += (heroObject.GetComponent<Image>().sprite.bounds.size.x * 1.2f);
                    nextPosition = new Vector2(newX, newY);
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
        void CreateExitButton()
        {

            exitButtonObject = Instantiate(UnityEngine.Resources.Load<GameObject>("Prefabs/Button"));
            exitButtonObject.transform.parent = cardWindow.transform;
            exitButtonObject.GetComponent<Image>().sprite = libs.GetUI(new ExitButton().GetSpriteID());
            exitButtonObject.name = "ExitButton";
            exitButtonObject.tag = "toDestroy";
            RectTransform rect = exitButtonObject.GetComponent<RectTransform>();
            rect.sizeDelta = new Vector2(exitButtonObject.GetComponent<Image>().sprite.bounds.size.x, exitButtonObject.GetComponent<Image>().sprite.bounds.size.y);
            Button button = exitButtonObject.GetComponent<Button>();
            button.onClick.AddListener(DestroyObjects);
            exitButtonObject.transform.position = getUpRightCorner(cardSpriteRenderer);
        }

        /// <summary>
        /// Creates a gameobject exitbutton to exit the building screen
        /// </summary>
        void CreateBuyButton()
        {
            // gets the positions for exit button and buy button if the window type requires it
            buyButtonObject = Instantiate(UnityEngine.Resources.Load<GameObject>("Prefabs/Button"));
            buyButtonObject.transform.parent = cardWindow.transform;
            buyButtonObject.GetComponent<Image>().sprite = libs.GetUI(new ExitButton().GetSpriteID() + 1);
            buyButtonObject.name = "BuyButton";
            buyButtonObject.tag = "toDestroy";
            RectTransform rect = buyButtonObject.GetComponent<RectTransform>();
            rect.sizeDelta = new Vector2(buyButtonObject.GetComponent<Image>().sprite.bounds.size.x, buyButtonObject.GetComponent<Image>().sprite.bounds.size.y);
            Button button = buyButtonObject.GetComponent<Button>();
            button.onClick.AddListener(purchase);
            buyButtonObject.transform.position = getDownRightCorner(cardSpriteRenderer);
        }

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
            if((top && type != selectedEarnResource) || (!top && type != selectedPayResource))
            {
                frameImage.sprite = UnityEngine.Resources.Load<Sprite>("Sprites/UI/resource_frame");
                if (top)
                {
                    selectedPayResource = type;
                    frame.transform.position = position;
                }
                else
                {
                    selectedEarnResource = type;
                    resourceFrame.transform.position = position;
                }
                if(selectedEarnResource >= 0 && selectedPayResource >= 0)
                {
                    if(!slider.isActiveAndEnabled)
                        slider.enabled = true;
                    slider.maxValue = (player.Wallet.GetResource(selectedPayResource) * ratio[selectedPayResource]) / ratio[selectedEarnResource];
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
            payAmount = (int)(value * ratio[selectedEarnResource]) / ratio[selectedPayResource];
            earnAmount = (int)value;
            textLeftResource.text = payAmount + "";
            textRightResource.text = earnAmount + "";
        }


        /// <summary>
        /// When clicked on buybutton, try to purchase active object
        /// </summary>
        private void purchase()
        {
            if(toBuyObject != null)
            {
                if (toBuyObject.GetType().BaseType.Name.Equals("Hero"))
                {
                    Hero buyHero = (Hero)toBuyObject;
                    // checks if the player can afford the hero and if the hero is alive
                    if (Player.Wallet.CanPay(buyHero.Cost) && Player.addHero(buyHero))
                    {
                        Player.Wallet.Pay(buyHero.Cost);
                        Debug.Log("Bought the hero" + buyHero.Name); // TODO remove
                        DestroyObjects();
                    }
                    else
                        Debug.Log("Not enough gold");
                    return;
                }
                else if (toBuyObject.GetType().BaseType.Name.Equals("Building"))
                {
                    Building buyBuilding = (Building)toBuyObject;

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
            }
            else if (toBuyObject.GetType().BaseType.BaseType.Name.Equals("Unit"))
            {
                Unit unit = (Unit) toBuyObject;

                if (Player.Wallet.CanPay(unit.Price) && town.StationedUnits.addUnit(unit, unitAmount))
                {
                    Player.Wallet.Pay(unit.Price);
                }
            }
            else if(selectedPayResource >= 0 && selectedEarnResource >= 0)
            {
                if(Player.Wallet.CanPay(selectedPayResource, payAmount))
                {
                    Player.Wallet.adjustResource(selectedPayResource, -payAmount);
                    Player.Wallet.adjustResource(selectedEarnResource, earnAmount);
                    gm.updateResourceText();

                    for(int i=0; i<5; i++)
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

            foreach (GameObject go in GameObject.FindGameObjectsWithTag("toDestroy"))
                Destroy(go);
        }
    }

}