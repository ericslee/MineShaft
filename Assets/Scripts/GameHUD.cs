using UnityEngine;
using System.Collections;

public class GameHUD : MonoBehaviour
{
    GameManager gameManager;

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
    }
}
