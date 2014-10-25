using UnityEngine;
using System.Collections;

public class EnemyController : MonoBehaviour {

	Vector3 startPosition;
	int direction;

	public float enemySpeed;

	// Use this for initialization
	void Start () {
		startPosition = transform.position;
		direction = 1;
	}
	
	// Update is called once per frame
	void Update () {
		if (transform.position.x > startPosition.x + 1.5){
			direction = -1;
		}
		else if (transform.position.x < startPosition.x - 1.5){
			direction = 1;
		}

		//transform.position.x += direction*0.1f;
		transform.Translate(new Vector3(direction*enemySpeed, 0, 0));
	}
}
