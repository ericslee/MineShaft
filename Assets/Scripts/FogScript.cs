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
			PlayerController pc = other.GetComponent<PlayerController>();
			if (pc){
				pc.setHealth(pc.getHealth()-1);
			}
			//gameManager.playerHealth -= 1;
		}
	}
}
