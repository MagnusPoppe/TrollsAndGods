using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour {

    public int amountOfPlayers;
    Player[] players;
    int whoseTurn;
    Date date;
    int[,] canWalk;
    Reaction[,] reactions;

	// Use this for initialization
	void Start () {
        players = new Player[amountOfPlayers];
        whoseTurn = 0;
        date = new Date();
	}
	
	// Update is called once per frame
	void Update () {
		if (Input.GetMouseButtonDown(0))
        {
            //todo add what happens when you click on something
        }
        else if (Input.GetMouseButtonDown(1))
        {
            // add what happens when you right click things
        }
	}

    public Player getPlayer(int index)
    {
        return players[index];
    }
}
