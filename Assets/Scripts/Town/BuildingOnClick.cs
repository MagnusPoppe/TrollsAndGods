using UnityEngine;
using TownView;
using UI;

public class BuildingOnClick : MonoBehaviour {

    public Building building;
    SpriteRenderer spriteRenderer;
    float add = 1f;
    
    IngameObjectLibrary libs;
    SpriteSystem spriteSystem;

    GameObject cardWindow;
    GameObject exitButton;
    GameObject[] buildingObjects;

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

    void Start()
    {
        spriteRenderer = gameObject.GetComponent<SpriteRenderer>();
        libs = GameManager.libs;

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

    /// <summary>
    /// Creates the game objects to open a building window
    /// </summary>
    /// <param name="b">The building that was clicked on in the town window</param>
    void OpenWindow(Building b)
    {

        // Gets the type of window associated with the given building
        int windowType = b.UIType();

        // TODO: Make less specific
        DwellingCard card = new DwellingCard(windowType, IngameObjectLibrary.Category.UI);

        // Creates an object with a spriterenderer, sets its position, layer, name and parent
        cardWindow = new GameObject();
        cardWindow.transform.parent = GameObject.Find("Town").transform;
        cardWindow.name = "BuildingWindow";
        cardWindow.transform.position = cardWindow.transform.parent.position;
        SpriteRenderer sr = cardWindow.AddComponent<SpriteRenderer>();
        sr.sprite = libs.GetUI(card.GetSpriteID());
        sr.sortingLayerName = "TownInteractive";

        // disables colliders in other layers when window is open
        for (int i = 0; i < BuildingObjects.Length; i++)
        {
            BuildingObjects[i].GetComponent<PolygonCollider2D>().enabled = false;
        }

        CreateExitButton();
    }
    
    /// <summary>
    /// Creates a gameobject exitbutton to exit the building screen
    /// </summary>
    void CreateExitButton()
    {
        // creates object and sets its name and position
        exitButton = new GameObject();
        exitButton.name = "ExitButton";
        exitButton.transform.position = cardWindow.transform.parent.position;

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
