using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEngine.UI;
using UI;
using UnityEngine.EventSystems;
using OverworldObjects;
using TownView;
using ResourceBuilding = OverworldObjects.ResourceBuilding;

/// <summary>
/// Opens bar to show the player how much he earns
/// </summary>
class ResourceIncomePanel : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    private Player player;
    private GameObject resourcePanel;
    private GameManager gm;
    
    /// <summary>
    /// Sets gamemanager, the panel and hides the panel
    /// </summary>
    void Awake()
    {
        gm = GameObject.Find("GameManager").GetComponent<GameManager>();
        resourcePanel = GameObject.Find("OverworldIncomePanel");
        resourcePanel.SetActive(false);
    }

    /// <summary>
    /// Sets resource bar text and activates it
    /// </summary>
    /// <param name="eventData">event</param>
    public void OnPointerEnter(PointerEventData eventData)
    {
        player = gm.Players[gm.WhoseTurn];
        resourcePanel.SetActive(true);
        Resources resource = player.GetIncome();
        for (int i = 0; i <Resources.TYPES; i++)
        {
            // sets text of the gameobjects
            GameObject resourceObject = resourcePanel.transform.GetChild(0).transform.GetChild(i).gameObject;
            resourceObject.GetComponent<Text>().text = "+" + resource.GetResource(i) + " / day";
        }
    }

    /// <summary>
    /// Hides resourcebar
    /// </summary>
    /// <param name="eventData">event</param>
    public void OnPointerExit(PointerEventData eventData)
    {
        resourcePanel.SetActive(false);
    }
}
