using UnityEngine;
using System.Collections;

public class PlayerController : MonoBehaviour {

	// Controls
	float distToGround;

	// Use this for initialization
	void Start () {
		// get distance to ground
		distToGround = collider.bounds.extents.y;
	}
	
	// Update is called once per frame
	void FixedUpdate () {
		HandleMovement();
	}

	void HandleMovement() {
		if (Input.GetKey (KeyCode.RightArrow) || Input.GetKey(KeyCode.D)) {
			transform.Translate(Vector2.right * 4f * Time.deltaTime);
		}
		if (Input.GetKey (KeyCode.LeftArrow) || Input.GetKey(KeyCode.A)) {
			transform.Translate(-Vector2.right * 4f * Time.deltaTime);
		}

		// Jump
		if (Input.GetKeyDown (KeyCode.Space) && IsGrounded()) {
			rigidbody.velocity = new Vector3(0, 8, 0);
		}
	}

	bool IsGrounded() {
		return Physics.Raycast(transform.position, -Vector3.up, distToGround + 0.1f);
	}
}
