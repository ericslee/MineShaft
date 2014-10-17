using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public enum GunType {PlatformGun, GravityGun};

public class PlayerController : MonoBehaviour
{
	//Sound
	AudioClip jumpSound;//=  Resources.Load("MineShaft/Assets/Sounds/hop.wav") as AudioClip;
    AudioSource jumpSource;

    // Controls
    float distToGround;
    bool collidingWall; // used for disabling left, right controls when colliding with a wall

    // Gun controls
    GameObject targetingReticle;
    Object targetingReticlePrefab;

    // Gun state
    GunType currentGun;

    // Platform gun
    Object platformPrefab;
    GameObject currentActivePlatform;

    // Gravity gun
    Object gravityCenterPrefab;
    GameObject currentActiveGravityCenter;
    HashSet<GameObject> gravityTargets = new HashSet<GameObject>(); // set of objects to be affected by gravity

	int health;

	Quaternion frontRotation;
	Quaternion leftRotation;
	Quaternion rightRotation;

    // Use this for initialization
    void Start()
    {
        // cache references
        platformPrefab = Resources.Load("Prefabs/Platform");
        gravityCenterPrefab = Resources.Load("Prefabs/GravityCenter");
        targetingReticlePrefab = Resources.Load("Prefabs/Reticle");

        // set up sounds
        jumpSource = GetComponents<AudioSource>()[0];

        // get distance to ground
        distToGround = collider.bounds.extents.y;

		health = 100;

		frontRotation = Quaternion.Euler(0,180,0);
		leftRotation = Quaternion.Euler(0,-90,0);
		rightRotation = Quaternion.Euler(0,90,0);

        currentGun = GunType.PlatformGun;

        // do not allow collisions between reticle and certain objects that should be not be selectable for gravity center effects
        Physics.IgnoreLayerCollision(8, 9, true);

        // instantiate recticle
        Vector3 reticlePosition = new Vector3(transform.position.x + 4f, transform.position.y + 3, transform.position.z);   
        targetingReticle = (GameObject)Instantiate(targetingReticlePrefab, reticlePosition, Quaternion.Euler(90, 0, 0));
    }

    void Update()
    {
        HandleInput();
    }

    void HandleInput()
    {
        HandleMovement();
        HandleGunControls(); 
    }

    void HandleMovement()
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
            if (Input.GetKeyDown(KeyCode.Space) /*|| Input.GetKeyDown(KeyCode.UpArrow)*/)
            {
                animation.Play("jump");
                rigidbody.velocity = new Vector3(0, 9, 0);
                jumpSource.Play();
            }
            transform.rotation = rotation;
        }

        // FOR DEBUGGING, multi jump
        if (Input.GetKey(KeyCode.U))
        {
            rigidbody.velocity = new Vector3(0, 8, 0);
        }
    }

    void HandleGunControls()
    {
        // Switch gun type
        if (Input.GetKeyDown(KeyCode.LeftShift)) 
        {
            currentGun = currentGun.Equals(GunType.PlatformGun) ? GunType.GravityGun : GunType.PlatformGun;

            // disable gravity center if switching to platform gun
            if (currentActiveGravityCenter)
            {
                Destroy(currentActiveGravityCenter);
                gravityTargets.Clear();
            }
        }

        if (targetingReticle) 
        {
            // Move reticle with mouse
            Vector3 pos = Input.mousePosition;
            pos.z = transform.position.z - Camera.main.transform.position.z;
            Vector3 newReticlePos = Camera.main.ScreenToWorldPoint(pos);

            // clip x and y positions if necessary
            if (newReticlePos.x < -10.2f) 
                newReticlePos.x = -10.2f; 
            else if (newReticlePos.x > 10.2f)
                newReticlePos.x = 10.2f;
            if (newReticlePos.y < -2f)
                newReticlePos.y = -2f;

            targetingReticle.transform.position = newReticlePos;

            // Click to fire gun
            if (Input.GetMouseButtonDown(0))
            {
                if (currentGun.Equals(GunType.PlatformGun))
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
                    
                }
                else if (currentGun.Equals(GunType.GravityGun))
                {
                    // update gravity center location
                    if (currentActiveGravityCenter) 
                    {
                        Destroy(currentActiveGravityCenter);
                    }
                    currentActiveGravityCenter = (GameObject)Instantiate(gravityCenterPrefab, targetingReticle.transform.position, Quaternion.Euler(0, 90, 0));
                }  
            }
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
        else if (collision.gameObject.tag.Equals("Enemy"))
        {
            Debug.Log("hit player");
        }
    }

    void OnCollisionExit()
    {
        collidingWall = false;
    }

    public void AddToGravityList(GameObject obj)
    {
        gravityTargets.Add(obj);

        // move object over to gravity center
        if (currentActiveGravityCenter)
        {
            float timeToGravityCenter = Vector3.Distance(currentActiveGravityCenter.transform.position, obj.transform.position) / 5.0f;
            iTween.MoveTo(obj, currentActiveGravityCenter.transform.position, timeToGravityCenter);
        }
    }

	public int getHealth(){
		return health;
	}

	public void setHealth(int newHealth){
		health = newHealth;
	}

    public GunType GetGunType()
    {
        return currentGun;
    }
}
