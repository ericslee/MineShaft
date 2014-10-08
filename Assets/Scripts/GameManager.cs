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
	public int playerHealth;

    // list of level triggers, (level - 1 is the index)
    List<GameObject> levelTriggers = new List<GameObject>();

    // Prefabs
    Object playerPrefab;
    Object platformPrefab;
    Object groundPrefab;
    Object wallPrefab;
    Object newLevelTriggerPrefab;
    Object fogPrefab;
    Object backdropPrefab;

    // references
    GameHUD hud;

    // Use this for initialization
    void Start()
    {
        // Cache references
        playerPrefab = Resources.Load("Prefabs/Player");
        platformPrefab = Resources.Load("Prefabs/Platform");
        groundPrefab = Resources.Load("Prefabs/Ground");
        wallPrefab = Resources.Load("Prefabs/Wall");
        newLevelTriggerPrefab = Resources.Load("Prefabs/NewLevelTrigger");
        fogPrefab = Resources.Load("Prefabs/FogParticleSystem");
        backdropPrefab = Resources.Load("Prefabs/Backdrop");
        hud = GetComponent<GameHUD>();

        // Instantiate player at start of level 1
        player = (GameObject)Instantiate(playerPrefab);
		playerHealth = 100;
        currentLevel = 1;

        SetUpLevel();

        CreateFog();
    }
    
    void SetUpLevel()
    {
        ///////////////////// First level /////////////////////
        Instantiate(groundPrefab, new Vector3(0f, -7f, 0f), Quaternion.identity);
        Instantiate(wallPrefab, new Vector3(-20f, 0f, 0f), Quaternion.identity);
        Instantiate(wallPrefab, new Vector3(20f, 0f, 0f), Quaternion.identity);

        Instantiate(platformPrefab, new Vector3(-2f, 0.2f, 0f), Quaternion.identity);
        Instantiate(platformPrefab, new Vector3(5f, 3.5f, 0f), Quaternion.identity);
        //Instantiate(platformPrefab, new Vector3(-2f, 6f, 0f), Quaternion.identity);
        Instantiate(platformPrefab, new Vector3(5f, 9f, 0f), Quaternion.identity);
        Instantiate(platformPrefab, new Vector3(-2f, 12f, 0f), Quaternion.identity);
        Instantiate(platformPrefab, new Vector3(5f, 15f, 0f), Quaternion.identity);
        //Instantiate(platformPrefab, new Vector3(-2f, 18f, 0f), Quaternion.identity);
        Instantiate(platformPrefab, new Vector3(5f, 21f, 0f), Quaternion.identity);
        Instantiate(platformPrefab, new Vector3(-2f, 24f, 0f), Quaternion.identity);
        Instantiate(platformPrefab, new Vector3(5f, 27f, 0f), Quaternion.identity);

        Instantiate(backdropPrefab, new Vector3(0, 0, 0.5f), Quaternion.Euler(270, 0, 0));
        Instantiate(backdropPrefab, new Vector3(0, 29, 0.5f), Quaternion.Euler(270, 0, 0));

        // level change triggers
        levelTriggers.Add((GameObject)Instantiate(newLevelTriggerPrefab, new Vector3(5f, 25f, 0f), Quaternion.identity));
        levelTriggers.Add((GameObject)Instantiate(newLevelTriggerPrefab, new Vector3(5f, 29f, 0f), Quaternion.identity));
        levelTriggers[0].GetComponent<LevelControl>().SetCorrespondingLevel(1);
        levelTriggers[0].GetComponent<LevelControl>().SetCameraPosition(new Vector3(0f, 12f, -30f));
        levelTriggers[1].GetComponent<LevelControl>().SetCorrespondingLevel(2);
        levelTriggers[1].GetComponent<LevelControl>().SetCameraPosition(new Vector3(0f, 40f, -30f));

        ///////////////////// Second level /////////////////////
        Instantiate(platformPrefab, new Vector3(-2f, 30f, 0f), Quaternion.identity);
        Instantiate(platformPrefab, new Vector3(5f, 33f, 0f), Quaternion.identity);
        //Instantiate(platformPrefab, new Vector3(-2f, 36f, 0f), Quaternion.identity);
        Instantiate(platformPrefab, new Vector3(5f, 39f, 0f), Quaternion.identity);
        Instantiate(platformPrefab, new Vector3(-2f, 42f, 0f), Quaternion.identity);
        //Instantiate(platformPrefab, new Vector3(5f, 45f, 0f), Quaternion.identity);

        ///////////////////// Winning trigger /////////////////////
        levelTriggers.Add((GameObject)Instantiate(newLevelTriggerPrefab, new Vector3(-41f, 49f, 0f), Quaternion.identity));
        levelTriggers.Add((GameObject)Instantiate(newLevelTriggerPrefab, new Vector3(41f, 49f, 0f), Quaternion.identity));
        levelTriggers[2].GetComponent<LevelControl>().SetIsWinningTrigger(true);
        levelTriggers[3].GetComponent<LevelControl>().SetIsWinningTrigger(true);
    }

    void CreateFog()
    {
        Instantiate(fogPrefab, new Vector3(-15f, 0f, 0f), Quaternion.identity);
    }
    
    // Update is called once per frame
    void Update()
    {
		if (Input.GetKey(KeyCode.N))
		{
			playerHealth -= 1;
		}
		else if (Input.GetKey(KeyCode.M))
		{
			playerHealth += 1;
		}
    }

    public void ChangeLevel(int level, Vector3 cameraPos) 
    {
        currentLevel = level;

        // move camera
        iTween.MoveTo(Camera.main.gameObject, cameraPos, 1.0f);
    }

    public void Win()
    {
        // winning sequence
        if (hud != null) 
        {
            hud.Win();
        }
    }

    public void Lose()
    {
        // losing sequence
        hud.Lose();
    }

    /*
     *  Getters and setters
     */ 

    public int GetCurrentLevel()
    {
        return currentLevel;
    }
}
