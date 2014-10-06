using UnityEngine;
using System.Collections;

public class PlayerController : MonoBehaviour
{

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
        if (!collidingWall)
        {
            if (Input.GetKey(KeyCode.RightArrow) || Input.GetKey(KeyCode.D))
            {
                transform.Translate(Vector2.right * 4f * Time.deltaTime);
            }
            if (Input.GetKey(KeyCode.LeftArrow) || Input.GetKey(KeyCode.A))
            {
                transform.Translate(-Vector2.right * 4f * Time.deltaTime);
            }
        }

        // Jump
        if (IsGrounded())
        {
            collidingWall = false;
            if (Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown(KeyCode.UpArrow))
            {
                rigidbody.velocity = new Vector3(0, 8, 0);
            }
        }

        // FOR DEBUGGING, multi jump
        if (Input.GetKey(KeyCode.U))
        {
            rigidbody.velocity = new Vector3(0, 8, 0);
        }
    }

    // checks if player is grounded with three tiny raycasts from the left bound, center, and right bound of the collider
    bool IsGrounded()
    {
        Vector3 leftBound = new Vector3(transform.position.x - (GetComponent<BoxCollider>().size.x / 2), transform.position.y, transform.position.z);
        Vector3 rightBound = new Vector3(transform.position.x + (GetComponent<BoxCollider>().size.x / 2), transform.position.y, transform.position.z);

        return (Physics.Raycast(transform.position, -Vector3.up, distToGround + 0.1f)
            || (Physics.Raycast(leftBound, -Vector3.up, distToGround + 0.1f))
            || (Physics.Raycast(rightBound, -Vector3.up, distToGround + 0.1f)));
    }

    void OnCollisionEnter(Collision collision)
    { 
        // environment tag needed in case we want to be able to control 
        if (collision.gameObject.tag.Equals("Environment") && !IsGrounded())
        {
            collidingWall = true;
        }
    }

    void OnCollisionExit()
    {
        collidingWall = false;
    }
}
