using UnityEngine;
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
    
    }

    void OnGUI()
    {
        GUI.Label(new Rect(10, 5, Screen.width / 5, Screen.height / 25), "Current Level: " + gameManager.GetCurrentLevel());

        // Instructions
        GUI.Label(new Rect(10, 40, Screen.width / 5, Screen.height / 2), "Controls - \nLeft+right arrows: movement" +
            "\n\nSpace: jump" +
            "\n\nLeft-shift: toggle shooting mode" +
            "\n\nIn shooting mode -" +
            "\nArrow keys: aim" +
            "\n\nSpace: fire platform");

        // Win/loss
        if (hasPlayerWon)
        {
            GUI.Label(new Rect(Screen.width / 2 - Screen.width / 8, Screen.height / 2, Screen.width / 4, Screen.height / 3), "You escaped the mine!");
        } else if (hasPlayerLost)
        {
            GUI.Label(new Rect(Screen.width / 2 - Screen.width / 16, Screen.height / 2, Screen.width / 4, Screen.height / 3), "You lose...");
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
