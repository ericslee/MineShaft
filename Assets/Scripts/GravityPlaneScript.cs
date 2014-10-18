using UnityEngine;
using System.Collections;

public class GravityPlaneScript : MonoBehaviour {

	GameManager gameManager;

	// Use this for initialization
	void Start () {
		gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void OnTriggerEnter(Collider other)
	{
		if (other.tag == "Platform"){
			gameManager.GetActivePlayer().AddToGravityList(other.gameObject);
		}

	}
}
