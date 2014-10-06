using UnityEngine;
using System.Collections;
using System.Collections.Generic;

// This script will be used to manage the global game state

public class GameManager : MonoBehaviour
{
    // Game state
    int currentLevel;

    // Player
    GameObject player;

    // Camera positions for each level (level - 1 is the index)
    List<Vector3> cameraPositions = new List<Vector3>();
    // list of level triggers
    List<GameObject> levelTriggers = new List<GameObject>();

    // Prefabs
    Object playerPrefab;
    Object platformPrefab;
    Object groundPrefab;
    Object wallPrefab;
    Object newLevelTriggerPrefab;

    // Use this for initialization
    void Start()
    {
        // Cache references
        playerPrefab = Resources.Load("Prefabs/Player");
        platformPrefab = Resources.Load("Prefabs/Platform");
        groundPrefab = Resources.Load("Prefabs/Ground");
        wallPrefab = Resources.Load("Prefabs/Wall");
        newLevelTriggerPrefab = Resources.Load("Prefabs/NewLevelTrigger");

        // Instantiate player at start of level 1
        player = (GameObject)Instantiate(playerPrefab);
        currentLevel = 1;

        SetUpLevel();
        AddCameraPositions();
    }
    
    void SetUpLevel()
    {
        ///////////////////// First level /////////////////////
        Instantiate(groundPrefab, new Vector3(0f, -7f, 0f), Quaternion.identity);
        Instantiate(wallPrefab, new Vector3(-20f, 0f, 0f), Quaternion.identity);
        Instantiate(wallPrefab, new Vector3(20f, 0f, 0f), Quaternion.identity);

        Instantiate(platformPrefab, new Vector3(-2f, 0.2f, 0f), Quaternion.identity);
        Instantiate(platformPrefab, new Vector3(5f, 3.5f, 0f), Quaternion.identity);
        Instantiate(platformPrefab, new Vector3(-2f, 6f, 0f), Quaternion.identity);
        Instantiate(platformPrefab, new Vector3(5f, 9f, 0f), Quaternion.identity);
        Instantiate(platformPrefab, new Vector3(-2f, 12f, 0f), Quaternion.identity);
        Instantiate(platformPrefab, new Vector3(5f, 15f, 0f), Quaternion.identity);
        Instantiate(platformPrefab, new Vector3(-2f, 18f, 0f), Quaternion.identity);
        Instantiate(platformPrefab, new Vector3(5f, 21f, 0f), Quaternion.identity);
        Instantiate(platformPrefab, new Vector3(-2f, 24f, 0f), Quaternion.identity);
        Instantiate(platformPrefab, new Vector3(5f, 27f, 0f), Quaternion.identity);

        // level change triggers
        levelTriggers.Add((GameObject)Instantiate(newLevelTriggerPrefab, new Vector3(5f, 25f, 0f), Quaternion.identity));
        levelTriggers.Add((GameObject)Instantiate(newLevelTriggerPrefab, new Vector3(5f, 29f, 0f), Quaternion.identity));
        levelTriggers[0].GetComponent<LevelControl>().SetCorrespondingLevel(1);
        levelTriggers[1].GetComponent<LevelControl>().SetCorrespondingLevel(2);

        ///////////////////// Second level /////////////////////
        Instantiate(platformPrefab, new Vector3(-2f, 30f, 0f), Quaternion.identity);
        Instantiate(platformPrefab, new Vector3(5f, 33f, 0f), Quaternion.identity);
        Instantiate(platformPrefab, new Vector3(-2f, 36f, 0f), Quaternion.identity);
        Instantiate(platformPrefab, new Vector3(5f, 39f, 0f), Quaternion.identity);
    }

    void AddCameraPositions() 
    {
        // for each level
        cameraPositions.Add(new Vector3(0f, 12f, -30f));
        cameraPositions.Add(new Vector3(0f, 40f, -30f));
    }
    
    // Update is called once per frame
    void Update()
    {
    
    }

    public void ChangeLevel(int level) 
    {
        currentLevel = level;

        // move camera
        iTween.MoveTo(Camera.main.gameObject, cameraPositions[currentLevel - 1], 1.0f);
    }

    /*
     *  Getters and setters
     */ 

    public int GetCurrentLevel()
    {
        return currentLevel;
    }
}
