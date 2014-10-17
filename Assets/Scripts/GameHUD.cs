﻿using UnityEngine;
using System.Collections;

public class GameHUD : MonoBehaviour
{
    GameManager gameManager;

    // win/loss conditions
    bool hasPlayerWon = false;
    bool hasPlayerLost = false;

    // Use this for initialization
    void Start()
    {
        // cache references
        gameManager = GetComponent<GameManager>();
    }
    
    // Update is called once per frame
    void Update()
    {
		if (gameManager.GetActivePlayer().getHealth () < 1) {
			hasPlayerLost = true;
		}
    }

    void OnGUI()
    {
        GUI.Label(new Rect(10, 5, Screen.width / 5, Screen.height / 25), "Current Level: " + gameManager.GetCurrentLevel());

        // Instructions
        GUI.Label(new Rect(10, 40, Screen.width / 5, Screen.height / 2 + 10), "Controls - \nA+D or Left+right arrows: movement" +
            "\n\nSpace: jump" +
            "\n\nLeft-shift: Switch gun" +
            "\n\nMouse: aim" +
            "\n\nClick: fire gun" +
            "\n\nU: fly in DEBUG" + 
		    "\n\nHealth: " + gameManager.GetActivePlayer().getHealth() +
		    "\n\nGun: " + gameManager.GetActivePlayer().GetGunType());

        // Win/loss
        if (hasPlayerWon)
		{
			Application.LoadLevel("Win"); 
            //GUI.Label(new Rect(Screen.width / 2 - Screen.width / 8, Screen.height / 2, Screen.width / 4, Screen.height / 3), "You escaped the mine!");
        } else if (hasPlayerLost)
		{
			Application.LoadLevel("Lose"); 
			//GUI.Label(new Rect(Screen.width / 2 - Screen.width / 16, Screen.height / 2, Screen.width / 4, Screen.height / 3), "You lose...");
        }
    }

    public void Win()
    {
        hasPlayerWon = true;
    }

    public void Lose()
    {
        hasPlayerLost = true;
    }
}
