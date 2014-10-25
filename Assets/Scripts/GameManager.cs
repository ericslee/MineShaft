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
	PlayerController playerController;

	GameObject player2;
	PlayerController player2Controller;

	int activePlayer;

    // list of level triggers, (level - 1 is the index)
    List<GameObject> levelTriggers = new List<GameObject>();

    // Prefabs
    Object playerPrefab;
	Object player2Prefab;
    Object platformPrefab;
	Object debris1Prefab;
    Object groundPrefab;
    Object wallPrefab;
	Object wallRightPrefab;
    Object newLevelTriggerPrefab;
    Object fogPrefab;
    Object backdropPrefab;
    Object enemyPrefab;

	bool[] hasSpawnedFog;

    // references
    GameHUD hud;

    // Use this for initialization
    void Start()
    {
		activePlayer = 0;
        // Cache references
        playerPrefab = Resources.Load("Characters/PlatformMiner");
		//player2Prefab = Resources.Load("Characters/GravityMiner");
		platformPrefab = Resources.Load("Prefabs/Platform");
		debris1Prefab = Resources.Load("Prefabs/Debris");
        groundPrefab = Resources.Load("Prefabs/Ground");
        wallPrefab = Resources.Load("Prefabs/Wall2");
		wallRightPrefab = Resources.Load("Prefabs/Wall2Right");
        newLevelTriggerPrefab = Resources.Load("Prefabs/NewLevelTrigger");
        fogPrefab = Resources.Load("Prefabs/FogParticleSystem");
        backdropPrefab = Resources.Load("Prefabs/Backdrop");
        enemyPrefab = Resources.Load("Prefabs/Enemy");
        hud = GetComponent<GameHUD>();

        // Instantiate player at start of level 1
		player = (GameObject)Instantiate(playerPrefab, new Vector3(1f, 0f, 0f), Quaternion.Euler(0,0,0));
		playerController = player.GetComponent<PlayerController>();
		playerController.setActive(true);

		//player2 = (GameObject)Instantiate(player2Prefab, new Vector3(1f, 0f, 1.5f), Quaternion.Euler(0,0,0));
		//player2Controller = player2.GetComponent<PlayerController>();
		//player2Controller.setActive(false);

        currentLevel = 1;

        SetUpLevel();
        AddEnemies();
        CreateFog();

		hasSpawnedFog = new bool[5];
		hasSpawnedFog[0] = true;
		hasSpawnedFog[1] = false;
		hasSpawnedFog[2] = false;
		hasSpawnedFog[3] = false;
		hasSpawnedFog[4] = false;
    }
    
	void SpawnPlatform(Vector3 position){
		GameObject platform = (GameObject) Instantiate(platformPrefab, position, Quaternion.identity);
		Transform [] planes = platform.GetComponentsInChildren<Transform>();
		for (int i=0; i<planes.Length; i+=1){
			if (planes[i].name.Contains("Plane")){
				float randVal = Random.value;
				float tx = randVal*3.0f-1.5f; //[-1.5,1.5];
				randVal = Random.value;
				float sx = randVal*0.3f-0.15f; //[-.15,.15];

				planes[i].Translate(tx,0,0);
				Vector3 oldScale = planes[i].transform.localScale;
				planes[i].transform.localScale.Set(sx, oldScale.y, oldScale.z);
			}
		}
	}

	void SpawnMine(){
		Instantiate(groundPrefab, new Vector3(7f, -7f, 0f), Quaternion.identity);
		Instantiate(wallPrefab, new Vector3(-20.0f, 10f, 0f), Quaternion.Euler(270, 0, 0));
		Instantiate(wallPrefab, new Vector3(-20.0f, 35f, 0f), Quaternion.Euler(270, 0, 0));
		Instantiate(wallPrefab, new Vector3(-20.0f, 60f, 0f), Quaternion.Euler(270, 0, 0));
		Instantiate(wallPrefab, new Vector3(-20.0f, 85f, 0f), Quaternion.Euler(270, 0, 0));
		Instantiate(wallPrefab, new Vector3(-20.0f, 110f, 0f), Quaternion.Euler(270, 0, 0));
		Instantiate(wallPrefab, new Vector3(-20.0f, 135f, 0f), Quaternion.Euler(270, 0, 0));
		
		Instantiate(wallRightPrefab, new Vector3(20.0f, 10f, 0f), Quaternion.Euler(270, 0, 0));
		Instantiate(wallRightPrefab, new Vector3(20.0f, 35f, 0f), Quaternion.Euler(270, 0, 0));
		Instantiate(wallRightPrefab, new Vector3(20.0f, 60f, 0f), Quaternion.Euler(270, 0, 0));
		Instantiate(wallRightPrefab, new Vector3(20.0f, 85f, 0f), Quaternion.Euler(270, 0, 0));
		Instantiate(wallRightPrefab, new Vector3(20.0f, 110f, 0f), Quaternion.Euler(270, 0, 0));
		Instantiate(wallRightPrefab, new Vector3(20.0f, 135f, 0f), Quaternion.Euler(270, 0, 0));

		Instantiate(backdropPrefab, new Vector3(0, 0, 1.0f), Quaternion.Euler(270, 0, 0));
		Instantiate(backdropPrefab, new Vector3(0, 29, 1.0f), Quaternion.Euler(270, 0, 0));
		Instantiate(backdropPrefab, new Vector3(0, 29, 2.5f), Quaternion.Euler(270, 0, 0));
		Instantiate(backdropPrefab, new Vector3(0, 58, 2.5f), Quaternion.Euler(270, 0, 0));
		Instantiate(backdropPrefab, new Vector3(0, 59, 2.5f), Quaternion.Euler(270, 0, 0));
		Instantiate(backdropPrefab, new Vector3(0, 88, 2.5f), Quaternion.Euler(270, 0, 0));
		Instantiate(backdropPrefab, new Vector3(0, 83, 2.5f), Quaternion.Euler(270, 0, 0));
		Instantiate(backdropPrefab, new Vector3(0, 114, 2.5f), Quaternion.Euler(270, 0, 0));
		Instantiate(backdropPrefab, new Vector3(0, 114, 2.5f), Quaternion.Euler(270, 0, 0));
		Instantiate(backdropPrefab, new Vector3(0, 143, 2.5f), Quaternion.Euler(270, 0, 0));
	}

	void SpawnLevel1(){
		///////////////////// First level /////////////////////
		SpawnPlatform(new Vector3(-6f, 0.2f, 0f));
		SpawnPlatform(new Vector3(5f, 3.5f, 0f));
		SpawnPlatform(new Vector3(-2f, 12f, 0f));
		SpawnPlatform(new Vector3(9f, 9f, 0f));
		SpawnPlatform(new Vector3(4f, 15f, 0f));
		SpawnPlatform(new Vector3(-2f, 24f, 0f));
		SpawnPlatform(new Vector3(5f, 27f, 0f));
	}

	void SpawnLevel2(){
		///////////////////// Second level /////////////////////
		///////////////////// Row 1 //////////////////////////// 
		SpawnPlatform(new Vector3(-9f, 34f, 0f));
		SpawnPlatform(new Vector3(-5.75f, 34f, 0f));
		SpawnPlatform(new Vector3(-2.5f, 34f, 0f));
		SpawnPlatform(new Vector3(1.3f, 34f, 0f));
		SpawnPlatform(new Vector3(5.25f, 34f, 0f));
		SpawnPlatform(new Vector3(8.5f, 34f, 0f));
		///////////////////// Row 2 ////////////////////////////
		SpawnPlatform(new Vector3(-2f, 42f, 0f));
		///////////////////// Row 3 ////////////////////////////
		SpawnPlatform(new Vector3(-9f, 46f, 0f));
		SpawnPlatform(new Vector3(-5.25f, 46f, 0f));
		SpawnPlatform(new Vector3(-2f, 46f, 0f));
		SpawnPlatform(new Vector3(2f, 46f, 0f));
		SpawnPlatform(new Vector3(4.9f, 46f, 0f));
		SpawnPlatform(new Vector3(9f, 46f, 0f));
		//////////////////// Rows 4 & 5 ////////////////////////
		SpawnPlatform(new Vector3(-6f, 49f, 0f));
		SpawnPlatform(new Vector3(3f, 52f, 0f));
		//////////////////// Row 6 //////////////////////////// 
		//SpawnPlatform(new Vector3(-9f, 56f, 0f));
		SpawnPlatform(new Vector3(-5.75f, 55f, 0f));
		SpawnPlatform(new Vector3(-2.5f, 55f, 0f));
		SpawnPlatform(new Vector3(1.3f, 55f, 0f));
		SpawnPlatform(new Vector3(5.25f, 55f, 0f));
		SpawnPlatform(new Vector3(8.5f, 55f, 0f));
		//////////////////// Row 7 //////////////////////////// 
		SpawnPlatform(new Vector3(-2f, 57f, 0f));
		SpawnPlatform(new Vector3(1.75f, 59f, 0f));
	}

	void SpawnLevel3(){
		///////////////////// Third level /////////////////////
		///////////////////// Cage 1 ///////////////////////////
		Instantiate(platformPrefab, new Vector3(-7.5f, 60f, 0f), Quaternion.Euler(0, 0, 45));
		Instantiate(platformPrefab, new Vector3(-3f, 60f, 0f), Quaternion.Euler(0, 0, -70));
		Instantiate(platformPrefab, new Vector3(3.75f, 60f, 0f), Quaternion.Euler(0, 0, 30));
		SpawnPlatform(new Vector3(-9.5f, 63f, 0f));
		SpawnPlatform(new Vector3(-5f, 63f, 0f));
		SpawnPlatform(new Vector3(-1.75f, 63f, 0f));
		SpawnPlatform(new Vector3(1.5f, 63f, 0f));
		SpawnPlatform(new Vector3(5.75f, 63f, 0f));
		SpawnPlatform(new Vector3(9f, 63f, 0f));
		Instantiate(debris1Prefab, new Vector3(9.0f, 63.0f, 0f), Quaternion.identity);
		Instantiate(debris1Prefab, new Vector3(4.0f, 63.0f, 0f), Quaternion.identity);
		Instantiate(debris1Prefab, new Vector3(-2.0f, 63.0f, 0f), Quaternion.identity);
		Instantiate(debris1Prefab, new Vector3(-10.0f, 63.0f, 0f), Quaternion.identity);
		///////////////////// Cage 2 ///////////////////////////
		SpawnPlatform(new Vector3(4.75f, 70f, 0f));
		SpawnPlatform(new Vector3(1.5f, 70f, 0f));
		SpawnPlatform(new Vector3(-2.5f, 70f, 0f));
		SpawnPlatform(new Vector3(-5.75f, 70f, 0f));
		SpawnPlatform(new Vector3(-10.25f, 70f, 0f));
		Instantiate(debris1Prefab, new Vector3(-9.5f, 70.0f, 0f), Quaternion.identity);
		Instantiate(debris1Prefab, new Vector3(-0.5f, 70.0f, 0f), Quaternion.identity);
		Instantiate(debris1Prefab, new Vector3(-0.25f, 70.0f, 0f), Quaternion.identity);
		Instantiate(debris1Prefab, new Vector3(4.5f, 70.0f, 0f), Quaternion.identity);
		Instantiate(debris1Prefab, new Vector3(7.75f, 70.0f, 0f), Quaternion.identity);
		///////////////////// Cage 3 ///////////////////////////
		SpawnPlatform(new Vector3(-9.5f, 77f, 0f));
		SpawnPlatform(new Vector3(-6.25f, 77f, 0f));
		SpawnPlatform(new Vector3(-3f, 77f, 0f));
		SpawnPlatform(new Vector3(-2f, 77f, 0f));
		SpawnPlatform(new Vector3(2.5f, 77f, 0f));
		SpawnPlatform(new Vector3(5.75f, 77f, 0f));
		SpawnPlatform(new Vector3(9f, 77f, 0f));
		Instantiate(debris1Prefab, new Vector3(-7.5f, 77.0f, 0f), Quaternion.identity);
		Instantiate(debris1Prefab, new Vector3(-3.0f, 77.0f, 0f), Quaternion.identity);
		Instantiate(debris1Prefab, new Vector3(3.75f, 77.0f, 0f), Quaternion.identity);
		///////////////////// Cage 4 ///////////////////////////
		SpawnPlatform(new Vector3(-9.5f, 84f, 0f));
		SpawnPlatform(new Vector3(-5f, 84f, 0f));
		SpawnPlatform(new Vector3(-1.75f, 84f, 0f));
		SpawnPlatform(new Vector3(1.5f, 84f, 0f));
		SpawnPlatform(new Vector3(5.75f, 84f, 0f));
		SpawnPlatform(new Vector3(9f, 84f, 0f));
		///////////////////// Cage 5 ///////////////////////////
		SpawnPlatform(new Vector3(1.5f, 87f, 0f));
	}

	void SpawnLevel4(){
		///////////////////// Fourth level /////////////////////
		///////////////////// Row 1 ////////////////////////////
		SpawnPlatform(new Vector3(9f, 90f, 0f));
		SpawnPlatform(new Vector3(4.75f, 90f, 0f));
		SpawnPlatform(new Vector3(1.5f, 90f, 0f));
		SpawnPlatform(new Vector3(-2.5f, 90f, 0f));
		SpawnPlatform(new Vector3(-5.75f, 90f, 0f));
		SpawnPlatform(new Vector3(-10.25f, 90f, 0f));
		///////////////////// Row 2 ////////////////////////////
		SpawnPlatform(new Vector3(-9.5f, 92f, 0f));
		SpawnPlatform(new Vector3(-6.25f, 92f, 0f));
		SpawnPlatform(new Vector3(-3f, 92f, 0f));
		SpawnPlatform(new Vector3(-2f, 92f, 0f));
		SpawnPlatform(new Vector3(2.5f, 92f, 0f));
		SpawnPlatform(new Vector3(5.75f, 92f, 0f));
		SpawnPlatform(new Vector3(9f, 92f, 0f));
		///////////////////// Row 3 ////////////////////////////
		SpawnPlatform(new Vector3(-9.5f, 94f, 0f));
		SpawnPlatform(new Vector3(-5f, 94f, 0f));
		SpawnPlatform(new Vector3(-1.75f, 94f, 0f));
		SpawnPlatform(new Vector3(1.5f, 94f, 0f));
		SpawnPlatform(new Vector3(5.75f, 94f, 0f));
		SpawnPlatform(new Vector3(9f, 94f, 0f));
		///////////////////// Row 4 ////////////////////////////
		SpawnPlatform(new Vector3(9f, 120f, 0f));
	}

	void SpawnLevel5(){

		///////////////////// Fifth level /////////////////////
		///////////////////// Cage 1 ///////////////////////////
		Instantiate(platformPrefab, new Vector3(3.75f, 123.25f, 0f), Quaternion.Euler(0, 0, 90));
		Instantiate(platformPrefab, new Vector3(3.75f, 120f, 0f), Quaternion.Euler(0, 0, 90));
		SpawnPlatform(new Vector3(-9f, 120f, 0f));
		SpawnPlatform(new Vector3(-5.75f, 120f, 0f));
		SpawnPlatform(new Vector3(-2.5f, 120f, 0f));
		SpawnPlatform(new Vector3(0.75f, 120f, 0f));
		SpawnPlatform(new Vector3(-6.25f, 130f, 0f));
		SpawnPlatform(new Vector3(-3f, 130f, 0f));
		SpawnPlatform(new Vector3(0.25f, 130f, 0f));
		SpawnPlatform(new Vector3(3.5f, 130f, 0f));
		SpawnPlatform(new Vector3(6.75f, 130f, 0f));
		SpawnPlatform(new Vector3(10f, 130f, 0f));
		///////////////////// Cage 2 ///////////////////////////
		Instantiate(platformPrefab, new Vector3(9f, 128f, 0f), Quaternion.Euler(0, 0, 90));
		Instantiate(platformPrefab, new Vector3(7f, 128f, 0f), Quaternion.Euler(0, 0, 90));
		Instantiate(platformPrefab, new Vector3(4f, 128f, 0f), Quaternion.Euler(0, 0, 90));
		Instantiate(platformPrefab, new Vector3(2f, 128f, 0f), Quaternion.Euler(0, 0, 90));
		Instantiate(platformPrefab, new Vector3(0.5f, 128f, 0f), Quaternion.Euler(0, 0, 90));
		Instantiate(platformPrefab, new Vector3(-2f, 128f, 0f), Quaternion.Euler(0, 0, 90));
		Instantiate(platformPrefab, new Vector3(-4f, 128f, 0f), Quaternion.Euler(0, 0, 90));
		Instantiate(platformPrefab, new Vector3(-7f, 128f, 0f), Quaternion.Euler(0, 0, 90));
		Instantiate(platformPrefab, new Vector3(-10f, 128f, 0f), Quaternion.Euler(0, 0, 90));
		SpawnPlatform(new Vector3(-9.5f, 126f, 0f));
		SpawnPlatform(new Vector3(-6.25f, 126f, 0f));
		SpawnPlatform(new Vector3(-3f, 126f, 0f));
		SpawnPlatform(new Vector3(0.25f, 126f, 0f));
		SpawnPlatform(new Vector3(3.25f, 126f, 0f));
		SpawnPlatform(new Vector3(5.75f, 126f, 0f));
		SpawnPlatform(new Vector3(9f, 126f, 0f));
		///////////////////// Cage 3 ///////////////////////////
		SpawnPlatform(new Vector3(-9.5f, 140f, 0f));
		SpawnPlatform(new Vector3(-6.25f, 140f, 0f));
		SpawnPlatform(new Vector3(-3f, 140f, 0f));
		SpawnPlatform(new Vector3(-2f, 140f, 0f));
		SpawnPlatform(new Vector3(1f, 140f, 0f));
		SpawnPlatform(new Vector3(2.5f, 140f, 0f));
		SpawnPlatform(new Vector3(5.75f, 140f, 0f));
		SpawnPlatform(new Vector3(9f, 140f, 0f));
		///////////////////// Cage 4 ///////////////////////////
		Instantiate(platformPrefab, new Vector3(-10f, 142f, 0f), Quaternion.Euler(0, 0, 90));
		Instantiate(platformPrefab, new Vector3(-7.5f, 142f, 0f), Quaternion.Euler(0, 0, 90));
		Instantiate(platformPrefab, new Vector3(-5f, 142f, 0f), Quaternion.Euler(0, 0, 90));
		Instantiate(platformPrefab, new Vector3(-3.5f, 142f, 0f), Quaternion.Euler(0, 0, 90));
		Instantiate(platformPrefab, new Vector3(-1f, 142f, 0f), Quaternion.Euler(0, 0, 90));
		Instantiate(platformPrefab, new Vector3(-1.75f, 142f, 0f), Quaternion.Euler(0, 0, 90));
		Instantiate(platformPrefab, new Vector3(1f, 142f, 0f), Quaternion.Euler(0, 0, 90));
		Instantiate(platformPrefab, new Vector3(3f, 142f, 0f), Quaternion.Euler(0, 0, 90));
		Instantiate(platformPrefab, new Vector3(5.5f, 142f, 0f), Quaternion.Euler(0, 0, 90));
		Instantiate(platformPrefab, new Vector3(7f, 142f, 0f), Quaternion.Euler(0, 0, 90));
		SpawnPlatform(new Vector3(-9.5f, 144f, 0f));
		SpawnPlatform(new Vector3(-5f, 144f, 0f));
		SpawnPlatform(new Vector3(-1.75f, 144f, 0f));
		SpawnPlatform(new Vector3(1.5f, 144f, 0f));
		SpawnPlatform(new Vector3(5.75f, 144f, 0f));
		SpawnPlatform(new Vector3(9f, 144f, 0f));
		///////////////////// Cage 5 ///////////////////////////
	}
	
	void SpawnLevelTriggers(){
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
		levelTriggers[3].GetComponent<LevelControl>().SetCameraPosition(new Vector3(0f, 72f, -30f));
		
		//Level 3 to Level 4
		levelTriggers.Add((GameObject)Instantiate(newLevelTriggerPrefab, new Vector3(5f, 85f, 0f), Quaternion.identity));
		levelTriggers.Add((GameObject)Instantiate(newLevelTriggerPrefab, new Vector3(5f, 89f, 0f), Quaternion.identity));
		levelTriggers[4].GetComponent<LevelControl>().SetCorrespondingLevel(3);
		levelTriggers[4].GetComponent<LevelControl>().SetCameraPosition(new Vector3(0f, 72f, -30f));
		levelTriggers[5].GetComponent<LevelControl>().SetCorrespondingLevel(4);
		levelTriggers[5].GetComponent<LevelControl>().SetCameraPosition(new Vector3(0f, 104f, -30f));
		
		//Level 4 to Level 5
		levelTriggers.Add((GameObject)Instantiate(newLevelTriggerPrefab, new Vector3(5f, 118f, 0f), Quaternion.identity));
		levelTriggers.Add((GameObject)Instantiate(newLevelTriggerPrefab, new Vector3(5f, 122f, 0f), Quaternion.identity));
		levelTriggers[6].GetComponent<LevelControl>().SetCorrespondingLevel(4);
		levelTriggers[6].GetComponent<LevelControl>().SetCameraPosition(new Vector3(0f, 104f, -30f));
		levelTriggers[7].GetComponent<LevelControl>().SetCorrespondingLevel(5);
		levelTriggers[7].GetComponent<LevelControl>().SetCameraPosition(new Vector3(0f, 134f, -30f));

		///////////////////// Winning trigger /////////////////////
		levelTriggers.Add((GameObject)Instantiate(newLevelTriggerPrefab, new Vector3(-41f, 148f, 0f), Quaternion.identity));
		levelTriggers.Add((GameObject)Instantiate(newLevelTriggerPrefab, new Vector3(41f, 148f, 0f), Quaternion.identity));
		levelTriggers[8].GetComponent<LevelControl>().SetIsWinningTrigger(true);
		levelTriggers[9].GetComponent<LevelControl>().SetIsWinningTrigger(true);
	}
	
	void SetUpLevel()
	{
		SpawnMine();
		SpawnLevelTriggers();
		SpawnLevel1();
		SpawnLevel2();
		SpawnLevel3();
		SpawnLevel4();
		SpawnLevel5();
    }

    void CreateFog()
    {
        //Instantiate(fogPrefab, new Vector3(0f, -10f, 0f), Quaternion.identity);
    }

    void AddEnemies()
    {
        Instantiate(enemyPrefab, new Vector3(9f, 10f, 0f), Quaternion.identity);
    }
    
    // Update is called once per frame
    void Update()
    {
		if (Input.GetKey(KeyCode.N))
		{
			GetActivePlayer().setHealth(playerController.getHealth()-1);
		}
		else if (Input.GetKey(KeyCode.M))
		{
			GetActivePlayer().setHealth(playerController.getHealth()+1);
		}
    }

    public void ChangeLevel(int level, Vector3 cameraPos) 
    {
        currentLevel = level;

        // move camera
        iTween.MoveTo(Camera.main.gameObject, cameraPos, 1.0f);
		//spawn a new fog thing!
		if (level == 2 && !hasSpawnedFog[level-1]) Instantiate(fogPrefab, new Vector3(10.0f, 35.0f, 0f), Quaternion.identity);
		else if (level == 3 && !hasSpawnedFog[level-1]) Instantiate(fogPrefab, new Vector3(-10.0f, 35.0f, 0f), Quaternion.identity);
		else if (level == 4 && !hasSpawnedFog[level-1]) Instantiate(fogPrefab, new Vector3(10.0f, 90.0f, 0f), Quaternion.identity);
		else if (level == 5 && !hasSpawnedFog[level-1]) Instantiate(fogPrefab, new Vector3(0.0f, 120.0f, 0f), Quaternion.identity);
		hasSpawnedFog[level-1] = true;
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

	public PlayerController GetActivePlayer(){
		if (activePlayer == 0) return playerController;
		else return player2Controller;
	}
}
