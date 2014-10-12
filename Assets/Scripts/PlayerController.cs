using UnityEngine;
using System.Collections;

public class PlayerController : MonoBehaviour
{
    // Controls
    float distToGround;
    bool collidingWall; // used for disabling left, right controls when colliding with a wall

    // Gun controls
    bool shootingMode = false;
    GameObject targetingReticle;
    Object targetingReticlePrefab;

    // Platform gun
    Object platformPrefab;
    GameObject currentActivePlatform;

	int health;

	Quaternion frontRotation;
	Quaternion leftRotation;
	Quaternion rightRotation;

    // Use this for initialization
    void Start()
    {
        // cache references
        platformPrefab = Resources.Load("Prefabs/Platform");
        targetingReticlePrefab = Resources.Load("Prefabs/Reticle");

        // get distance to ground
        distToGround = collider.bounds.extents.y;

		health = 100;

		frontRotation = Quaternion.Euler(0,180,0);
		leftRotation = Quaternion.Euler(0,-90,0);
		rightRotation = Quaternion.Euler(0,90,0);
    }

    // Update is called once per frame
    void Update()
    {
        HandleInput();
    }

    void HandleInput()
    {

        // Switch into shooting mode
        if (Input.GetKeyDown(KeyCode.LeftShift)) 
        {
            shootingMode = !shootingMode;
            HandleReticle();
        }

        if (!shootingMode)
        {
            if (!collidingWall)
            {
                if (Input.GetKey(KeyCode.RightArrow) || Input.GetKey(KeyCode.D))
                {
					transform.rotation = Quaternion.identity;
                    transform.Translate(Vector2.right * 4f * Time.deltaTime);
					if (!animation.IsPlaying("walking") && !animation.IsPlaying("jump") && IsGrounded()){
						animation.Play("walking");
					}
					transform.rotation = rightRotation;
					/*Component[] comps = gameObject.GetComponentsInChildren<Transform>();
					Component mesh = null;
					for (int i=0; i<comps.Length; i+=1){
						if (comps[i].name == "AlphaHighResMeshes"){
							mesh = comps[i];
							break;
						}
					}
					if (mesh){
						Transform[] allChildren = mesh.GetComponentsInChildren<Transform>();
						foreach (Transform child in allChildren) {
							// do whatever with child transform here
							child.transform.Translate(100,0,0);
						}
					}*/
                }
                else if (Input.GetKey(KeyCode.LeftArrow) || Input.GetKey(KeyCode.A))
                {
					transform.rotation = Quaternion.identity;
                    transform.Translate(-Vector2.right * 4f * Time.deltaTime);
					if (!animation.IsPlaying("walking") && !animation.IsPlaying("jump") && IsGrounded()){
						animation.Play("walking");
					}
					transform.rotation = leftRotation;
                }
				else if (animation.IsPlaying("walking")){
					animation.Stop();
				}
            }

            // Jump
            if (IsGrounded())
            {
				Quaternion rotation = transform.rotation;
				transform.rotation = Quaternion.identity;
                collidingWall = false;
                if (Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown(KeyCode.UpArrow))
                {
					animation.Play("jump");
                    rigidbody.velocity = new Vector3(0, 9, 0);
                }
				transform.rotation = rotation;
            }
        }

        if (shootingMode && targetingReticle) 
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                // create platform at the location of the targeting reticle
                if (currentActivePlatform)
                {
                    Destroy(currentActivePlatform);
                }

				Vector3 platformPosition = targetingReticle.transform.position;
				float randomOffsetX = Random.value;
				float randomOffsetY = Random.value;
				Light[] lights = targetingReticle.GetComponentsInChildren<Light> ();
				float range = 0;
				if (lights.Length != 1){
					Debug.Log("Error, targeting reticle should have exactly 1 light child");
				}
				else{
					range = lights[0].range;
				}
				platformPosition.x += randomOffsetX*range;
				platformPosition.y += randomOffsetY*range;

                currentActivePlatform = 
					(GameObject)Instantiate(platformPrefab, platformPosition, Quaternion.identity);


                // exit out of shooting mode
                shootingMode = false;
                HandleReticle();
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

    void HandleReticle() 
    {
        if (shootingMode) 
        {
            // create reticle
            //TODO: instantiate reticle position relative to which way the character is facing
            Vector3 reticlePosition = new Vector3(transform.position.x + 4f, transform.position.y + 3, transform.position.z);	
			targetingReticle = (GameObject)Instantiate(targetingReticlePrefab, reticlePosition, Quaternion.Euler(90, 0, 0));
        } else 
        {
            // destroy reticle
            if (targetingReticle)
            {
                Destroy(targetingReticle);
            }
        }
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

	public int getHealth(){
		return health;
	}

	public void setHealth(int newHealth){
		health = newHealth;
	}
}
