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

    GameObject cardWindow;
    GameObject exitButton;
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

    // TODO move all castle listeners to this update
    // TODO townHallOnClick's town.building = built
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
        cardWindow.transform.parent = GameObject.Find("Town").transform;
        cardWindow.name = "BuildingWindow";
        cardWindow.tag = "toDestroy";
        cardWindow.transform.position = cardWindow.transform.parent.position;
        SpriteRenderer sr = cardWindow.AddComponent<SpriteRenderer>();
        sr.sprite = libs.GetUI(card.GetSpriteID());
        sr.sortingLayerName = "TownInteractive";

        // disables colliders in other layers when window is open
        for (int i = 0; i < BuildingObjects.Length; i++)
        {
            // TODO: make into list so we dont have to check for null?
            if (BuildingObjects[i] != null)
                BuildingObjects[i].GetComponent<PolygonCollider2D>().enabled = false;
        }

        Vector2 exitBtnPosition = cardWindow.transform.parent.position;


        if (windowType == WindowTypes.TOWN_HALL_CARD)
        {
            exitBtnPosition = getUpRightCorner(sr);
            // TOOD: get parents order and i++
            int layer = 1;
            float offsetX = sr.transform.position.x - (sr.bounds.size.x/3);
            float offsetY = sr.transform.position.y + (sr.bounds.size.y/4);
            Vector2 previousPosition = new Vector2(offsetX, offsetY);
            float startX = previousPosition.x;
            for (int i=0; i<town.Buildings.Length; i++)
            {
                GameObject buildingObject = new GameObject();
                buildingObject.transform.parent = GameObject.Find("Canvas").transform;
                buildingObject.name = town.Buildings[i].Name;
                buildingObject.tag = "toDestroy";

                buildingObject.AddComponent<TownHallOnClick>();
                buildingObject.GetComponent<TownHallOnClick>().Building = town.Buildings[i];
                buildingObject.GetComponent<TownHallOnClick>().Town = Town;
                buildingObject.GetComponent<TownHallOnClick>().Player = Player;

                /*
                Text text = buildingObject.AddComponent<Text>();
                text.text = town.Buildings[i].Name;
                text.font = UnityEngine.Resources.Load<Font>("Font/ARIAL");
                */

                /*
                GameObject textObject = new GameObject();
                Text text = textObject.AddComponent<Text>();
                text.text = town.Buildings[i].Name;
                text.font = UnityEngine.Resources.Load<Font>("Font/ARIAL");
                textObject.transform.parent = buildingObject.transform;
                textObject.transform.position = previousPosition;
                textObject.tag = "toDestroy";
                */

                SpriteRenderer spr = buildingObject.AddComponent<SpriteRenderer>();
                spr.sprite = libs.GetTown(town.Buildings[i].GetSpriteBlueprintID());
                spr.sortingOrder = layer++;
                spr.sortingLayerName = "TownInteractive";
                
                BoxCollider2D collider = buildingObject.AddComponent<BoxCollider2D>();
                collider.size = spr.bounds.size;



                float newX = previousPosition.x;
                Debug.Log(newX);
                float newY = previousPosition.y;
                buildingObject.transform.position = previousPosition;
                if (previousPosition.x > sr.transform.position.x)
                {
                    newX = startX;
                    newY -= (sr.bounds.size.y / 2.5f);
                }
                else
                    newX += (sr.bounds.size.x / 3);
                previousPosition = new Vector2(newX, newY);
            }
        }
        CreateExitButton(exitBtnPosition);
    }

    private Vector2 getUpRightCorner(SpriteRenderer sr)
    {
        return new Vector2(sr.transform.position.x + (sr.bounds.size.x / 2.2f), sr.transform.position.y + (sr.bounds.size.y / 2.2f));
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
        sr.sprite = libs.GetDebugSprite(3);
        sr.sortingLayerName = "TownInteractive";
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
}
