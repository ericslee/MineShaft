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

        Instantiate(platformPrefab, new Vector3(-6f, 0.2f, 0f), Quaternion.identity);
        Instantiate(platformPrefab, new Vector3(5f, 3.5f, 0f), Quaternion.identity);
        Instantiate(platformPrefab, new Vector3(9f, 9f, 0f), Quaternion.identity);
        Instantiate(platformPrefab, new Vector3(-2f, 12f, 0f), Quaternion.identity);
        Instantiate(platformPrefab, new Vector3(4f, 15f, 0f), Quaternion.identity);
        Instantiate(platformPrefab, new Vector3(-6f, 21f, 0f), Quaternion.identity);
        Instantiate(platformPrefab, new Vector3(-2f, 24f, 0f), Quaternion.identity);
        Instantiate(platformPrefab, new Vector3(5f, 27f, 0f), Quaternion.identity);

        Instantiate(backdropPrefab, new Vector3(0, 0, 0.5f), Quaternion.Euler(270, 0, 0));
		Instantiate(backdropPrefab, new Vector3(0, 29, 0.5f), Quaternion.Euler(270, 0, 0));

        // level change triggers
		//Level 1 to Level 2
        levelTriggers.Add((GameObject)Instantiate(newLevelTriggerPrefab, new Vector3(5f, 25f, 0f), Quaternion.identity));
		levelTriggers.Add((GameObject)Instantiate(newLevelTriggerPrefab, new Vector3(5f, 29f, 0f), Quaternion.identity));
		levelTriggers[0].GetComponent<LevelControl>().SetCorrespondingLevel(1);
		levelTriggers[0].GetComponent<LevelControl>().SetCameraPosition(new Vector3(0f, 12f, -30f));
        levelTriggers[1].GetComponent<LevelControl>().SetCorrespondingLevel(2);
        levelTriggers[1].GetComponent<LevelControl>().SetCameraPosition(new Vector3(0f, 42f, -30f));

		//Level 2 to Level 3
		levelTriggers.Add((GameObject)Instantiate(newLevelTriggerPrefab, new Vector3(5f, 55f, 0f), Quaternion.identity));
		levelTriggers.Add((GameObject)Instantiate(newLevelTriggerPrefab, new Vector3(5f, 59f, 0f), Quaternion.identity));
        levelTriggers[2].GetComponent<LevelControl>().SetCorrespondingLevel(2);
        levelTriggers[2].GetComponent<LevelControl>().SetCameraPosition(new Vector3(0f, 42f, -30f));
        levelTriggers[3].GetComponent<LevelControl>().SetCorrespondingLevel(3);
        levelTriggers[3].GetComponent<LevelControl>().SetCameraPosition(new Vector3(0f, 76f, -30f));

		//Level 3 to Level 4
        levelTriggers.Add((GameObject)Instantiate(newLevelTriggerPrefab, new Vector3(5f, 90f, 0f), Quaternion.identity));
        levelTriggers.Add((GameObject)Instantiate(newLevelTriggerPrefab, new Vector3(5f, 94f, 0f), Quaternion.identity));
        levelTriggers[4].GetComponent<LevelControl>().SetCorrespondingLevel(3);
        levelTriggers[4].GetComponent<LevelControl>().SetCameraPosition(new Vector3(0f, 76f, -30f));
        levelTriggers[5].GetComponent<LevelControl>().SetCorrespondingLevel(4);
        levelTriggers[5].GetComponent<LevelControl>().SetCameraPosition(new Vector3(0f, 104f, -30f));

        //Level 4 to Level 5
        levelTriggers.Add((GameObject)Instantiate(newLevelTriggerPrefab, new Vector3(5f, 118f, 0f), Quaternion.identity));
        levelTriggers.Add((GameObject)Instantiate(newLevelTriggerPrefab, new Vector3(5f, 122f, 0f), Quaternion.identity));
        levelTriggers[6].GetComponent<LevelControl>().SetCorrespondingLevel(4);
        levelTriggers[6].GetComponent<LevelControl>().SetCameraPosition(new Vector3(0f, 104f, -30f));
        levelTriggers[7].GetComponent<LevelControl>().SetCorrespondingLevel(5);
        levelTriggers[7].GetComponent<LevelControl>().SetCameraPosition(new Vector3(0f, 134f, -30f));

        ///////////////////// Second level /////////////////////
		Instantiate(wallPrefab, new Vector3(-20f, 40f, 0f), Quaternion.identity);
		Instantiate(wallPrefab, new Vector3(20f, 40f, 0f), Quaternion.identity);
		Instantiate(backdropPrefab, new Vector3(0, 29, 0.5f), Quaternion.Euler(270, 0, 0));
		Instantiate(backdropPrefab, new Vector3(0, 58, 0.5f), Quaternion.Euler(270, 0, 0));
		///////////////////// Row 1 //////////////////////////// 
        //Instantiate(platformPrefab, new Vector3(-9f, 34f, 0f), Quaternion.identity);
        Instantiate(platformPrefab, new Vector3(-5.75f, 34f, 0f), Quaternion.identity);
		Instantiate(platformPrefab, new Vector3(-2.5f, 34f, 0f), Quaternion.identity);
		Instantiate(platformPrefab, new Vector3(1.3f, 34f, 0f), Quaternion.identity);
		Instantiate(platformPrefab, new Vector3(5.25f, 34f, 0f), Quaternion.identity);
		Instantiate(platformPrefab, new Vector3(8.5f, 34f, 0f), Quaternion.identity);
		///////////////////// Row 2 ////////////////////////////
		Instantiate(platformPrefab, new Vector3(-2f, 42f, 0f), Quaternion.identity);
		///////////////////// Row 3 ////////////////////////////
		//Instantiate(platformPrefab, new Vector3(-9f, 46f, 0f), Quaternion.identity);
		Instantiate(platformPrefab, new Vector3(-5.25f, 46f, 0f), Quaternion.identity);
		Instantiate(platformPrefab, new Vector3(-2f, 46f, 0f), Quaternion.identity);
		Instantiate(platformPrefab, new Vector3(2f, 46f, 0f), Quaternion.identity);
		Instantiate(platformPrefab, new Vector3(4.9f, 46f, 0f), Quaternion.identity);
		Instantiate(platformPrefab, new Vector3(9f, 46f, 0f), Quaternion.identity);
		//////////////////// Rows 4 & 5 ////////////////////////
		Instantiate(platformPrefab, new Vector3(-6f, 49f, 0f), Quaternion.identity);
		Instantiate(platformPrefab, new Vector3(3f, 52f, 0f), Quaternion.identity);
		//////////////////// Row 6 //////////////////////////// 
		//Instantiate(platformPrefab, new Vector3(-9f, 56f, 0f), Quaternion.identity);
		Instantiate(platformPrefab, new Vector3(-5.75f, 55f, 0f), Quaternion.identity);
		Instantiate(platformPrefab, new Vector3(-2.5f, 55f, 0f), Quaternion.identity);
		Instantiate(platformPrefab, new Vector3(1.3f, 55f, 0f), Quaternion.identity);
		Instantiate(platformPrefab, new Vector3(5.25f, 55f, 0f), Quaternion.identity);
		Instantiate(platformPrefab, new Vector3(8.5f, 55f, 0f), Quaternion.identity);
		//////////////////// Row 7 //////////////////////////// 
		Instantiate(platformPrefab, new Vector3(-2f, 58f, 0f), Quaternion.identity);

		///////////////////// Third level /////////////////////
		Instantiate(wallPrefab, new Vector3(-20f, 80f, 0f), Quaternion.identity);
		Instantiate(wallPrefab, new Vector3(20f, 80f, 0f), Quaternion.identity);
		Instantiate(backdropPrefab, new Vector3(0, 58, 0.5f), Quaternion.Euler(270, 0, 0));
		Instantiate(backdropPrefab, new Vector3(0, 87, 0.5f), Quaternion.Euler(270, 0, 0));
		///////////////////// Cage ///////////////////////////
		Instantiate(platformPrefab, new Vector3(-9.5f, 63f, 0f), Quaternion.identity);
		Instantiate(platformPrefab, new Vector3(-7.5f, 60f, 0f), Quaternion.Euler(0, 0, 90));
		Instantiate(platformPrefab, new Vector3(-4.25f, 63f, 0f), Quaternion.identity);
		Instantiate(platformPrefab, new Vector3(-2.25f, 60f, 0f), Quaternion.Euler(0, 0, 90));

        ///////////////////// Winning trigger /////////////////////
        levelTriggers.Add((GameObject)Instantiate(newLevelTriggerPrefab, new Vector3(-41f, 128f, 0f), Quaternion.identity));
        levelTriggers.Add((GameObject)Instantiate(newLevelTriggerPrefab, new Vector3(41f, 128f, 0f), Quaternion.identity));
        levelTriggers[8].GetComponent<LevelControl>().SetIsWinningTrigger(true);
        levelTriggers[9].GetComponent<LevelControl>().SetIsWinningTrigger(true);
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
