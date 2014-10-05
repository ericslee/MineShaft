using UnityEngine;
using System.Collections;

public class PlayerController : MonoBehaviour {

	// Controls
	float distToGround;
	bool collidingWall; // used for disabling left, right controls when colliding with a wall

	// Use this for initialization
	void Start() 
    {
		// get distance to ground
		distToGround = collider.bounds.extents.y;
	}

	// Update is called once per frame
	void Update() 
    {
		HandleMovement();
	}

	void HandleMovement() 
    {
		if (!collidingWall) {
			if (Input.GetKey (KeyCode.RightArrow) || Input.GetKey(KeyCode.D)) {
				transform.Translate(Vector2.right * 4f * Time.deltaTime);
			}
			if (Input.GetKey (KeyCode.LeftArrow) || Input.GetKey(KeyCode.A)) {
				transform.Translate(-Vector2.right * 4f * Time.deltaTime);
			}
		}

		// Jump
		if (IsGrounded()) {
			collidingWall = false;
			if (Input.GetKeyDown (KeyCode.Space) || Input.GetKeyDown(KeyCode.UpArrow)) {
				rigidbody.velocity = new Vector3(0, 8, 0);
			}
		}
	}

	bool IsGrounded() 
    {
		return Physics.Raycast(transform.position, -Vector3.up, distToGround + 0.1f);
	}

	void OnCollisionEnter(Collision collision) 
    { 
		// environment tag needed in case we want to be able to control 
		if (collision.gameObject.tag.Equals("Environment") && !IsGrounded()) {
			collidingWall = true;
		}
	}

	void OnCollisionExit() 
    {
		collidingWall = false;
	}
}
