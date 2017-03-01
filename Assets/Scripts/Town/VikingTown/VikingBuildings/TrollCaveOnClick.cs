using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TownView;

public class TrollCaveOnClick : MonoBehaviour
{
    int LOCAL_SPRITE = 0;
    Building building = new TrollCave();
    GameObject window;

    // TODO: Kostbart, gjør noe annet
    IngameObjectLibrary libs = new IngameObjectLibrary();
    

    void OnMouseDown()
    {
        Debug.Log("Du har klikket på " + building.Name);

        OpenWindow(building);
    }

    void OpenWindow(Building b)
    {
        // Opens up the window card
        SpriteRenderer sr = window.AddComponent<SpriteRenderer>();
        sr.sortingLayerName = "TownInteractive";
        sr.sprite = libs.GetUI(LOCAL_SPRITE);
    }
}
