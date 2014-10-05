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

	// Use this for initialization
	void Start () 
    {
		// Cache references
		playerPrefab = Resources.Load("Prefabs/Player");
		platformPrefab = Resources.Load("Prefabs/Platform");
		groundPrefab = Resources.Load("Prefabs/Ground");

		// Instantiate player at start of level 1
		player = (GameObject)Instantiate(playerPrefab);

		SetUpLevel();
	}
	
	void SetUpLevel() 
    {
		Instantiate(groundPrefab, new Vector3(0f, -7f, 0f), Quaternion.identity);
   
		Instantiate(platformPrefab, new Vector3(-2f, 0.2f, 0f), Quaternion.identity);
	}
	
	// Update is called once per frame
	void Update () 
    {
	
	}
}
