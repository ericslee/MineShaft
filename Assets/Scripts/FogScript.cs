using UnityEngine;
using System.Collections;

public class FogScript : MonoBehaviour {

	GameManager gameManager;

	// Use this for initialization
	void Start () {
		gameManager = GetComponent<GameManager>();
	}
	
	// Update is called once per frame
	void Update () {
	}

	void OnParticleCollision(GameObject other) {
		if (other.tag == "Player"){
			PlayerController pc = gameManager.GetActivePlayer();
			//pc.setHealth(pc.getHealth()-1);
		}
	}
}
