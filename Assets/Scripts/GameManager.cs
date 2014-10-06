using UnityEngine;
using System.Collections;

// This script will be used to manage the global game state

public class GameManager : MonoBehaviour {

	// Player
	GameObject player;

	// Prefabs
	Object playerPrefab;
	Object platformPrefab;
	Object groundPrefab;
    Object wallPrefab;

	// Use this for initialization
	void Start () 
    {
		// Cache references
		playerPrefab = Resources.Load("Prefabs/Player");
		platformPrefab = Resources.Load("Prefabs/Platform");
		groundPrefab = Resources.Load("Prefabs/Ground");
        wallPrefab = Resources.Load("Prefabs/Wall");

		// Instantiate player at start of level 1
		player = (GameObject)Instantiate(playerPrefab);

		SetUpLevel();
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
	}
	
	// Update is called once per frame
	void Update () 
    {
	
	}
}
