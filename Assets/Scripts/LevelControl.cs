using UnityEngine;
using System.Collections;

/*
 *  Script used to transition to a new level and move the camera accordingly when the object this script is attached to is crossed  
 */
public class LevelControl : MonoBehaviour
{

    // level control
    int correspondingLevel; // level that the game should switch to when this threshold is crossed

    GameManager gameManager;

    // Use this for initialization
    void Start()
    {
        // cache references
        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
    }
    
    // Update is called once per frame
    void Update()
    {

    }

    void OnTriggerEnter(Collider other)
    {
        if (correspondingLevel != gameManager.GetCurrentLevel() && other.tag.Equals("Player"))
        {
            // increment the level if entering from below
            gameManager.ChangeLevel(correspondingLevel);
        }
    }

    /*
     *  Getters and setters
     */ 

    public void SetCorrespondingLevel(int level)
    {
        correspondingLevel = level;
    }
}
