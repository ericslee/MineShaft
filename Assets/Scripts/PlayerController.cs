using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public enum GunType {PlatformGun, GravityGun};

public class PlayerController : MonoBehaviour
{
	//Pause Menu
	bool pause = false;

	public float jumpHeight;
	public int health;
    public int invincibilityFrames;

	//Sound
    AudioSource jumpSource;
    AudioSource takeDamageSource;

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
    Object playerPlatformPrefab;
    GameObject currentActivePlatform;

    // Gravity gun
    Object gravityCenterPrefab;
    GameObject currentActiveGravityCenter;
	GravityCenterScript gravityScript;
    HashSet<GameObject> gravityTargets = new HashSet<GameObject>(); // set of objects to be affected by gravity
	float gravityRange;

	Quaternion frontRotation;
	Quaternion leftRotation;
	Quaternion rightRotation;

	bool m_isActive;

	int numFrameFloating;
	bool platformChanged;

	public int numFrameFloatingExposed;
	public float minTargetingHeight, maxTargetingHeight, targetingWidth;

    // Use this for initialization
    void Start()
    {
        // cache references
        platformPrefab = Resources.Load("Prefabs/Platform");
        playerPlatformPrefab = Resources.Load("Prefabs/PlayerPlatform");
        gravityCenterPrefab = Resources.Load("Prefabs/GravityCenter");
        targetingReticlePrefab = Resources.Load("Prefabs/Reticle");

        // set up sounds
        jumpSource = GetComponents<AudioSource>()[0];
        takeDamageSource = GetComponents<AudioSource>()[1];

        // get distance to ground
        distToGround = collider.bounds.extents.y;

		frontRotation = Quaternion.Euler(0,180,0);
		leftRotation = Quaternion.Euler(0,-90,0);
		rightRotation = Quaternion.Euler(0,90,0);

        currentGun = GunType.PlatformGun;

        invincibilityFrames = 0;

		numFrameFloating = -1;
		platformChanged = false;

        // do not allow collisions between reticle and certain objects that should be not be selectable for gravity center effects
        Physics.IgnoreLayerCollision(8, 9, true);

        // instantiate recticle
        Vector3 reticlePosition = new Vector3(transform.position.x + 4f, transform.position.y + 3, transform.position.z);   
        targetingReticle = (GameObject)Instantiate(targetingReticlePrefab, reticlePosition, Quaternion.Euler(90, 0, 0));
    }

    void Update()
    {
        if (m_isActive) HandleInput();

		if (currentActiveGravityCenter){
		gravityScript.SetGravityPlaneMax(gravityScript.GetAbsMaxScale() * ((float)health+10.0f)/100.0f);
		}

        invincibilityFrames++;
		numFrameFloating -= 1;
    }

    void HandleInput()
    {
		HandleMisc ();
        HandleMovement();
        HandleGunControls(); 
    }

	void HandleMisc() {
		if (Input.GetKey (KeyCode.Escape)) {
			Application.Quit();
			Debug.Log ("Application.Quit() only works in build, not in editor"); 
		}
		if (Input.GetKey(KeyCode.P)){//Input.GetKey ("p") || Input.GetKey("P")) {
			pause = !pause;
			//Application.Quit();
		}
		if (pause) {
			Time.timeScale = 0;
			GUI.Box(new Rect(425, 150, 175, 425),"Paused");
			//GUILayout.BeginArea(new Rect(425, 150, 175, 425));
			
			if (GUILayout.Button("Resume Game")) 
			{ 
				pause = false;
			}
			
			if (GUILayout.Button("New Game")) 
			{ 
				Application.LoadLevel("Mine"); 
			}
					
			if (GUILayout.Button("Exit")) 
			{ 
				Application.Quit(); 
				Debug.Log ("Application.Quit() only works in build, not in editor"); 
			}
			//GUILayout.EndArea(); 
		}
		if(!pause) {
			Time.timeScale = 1;
		}
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
                //animation.Stop();
            }
        }
        
        // Jump
        if (IsGrounded() || numFrameFloating > 0)
        {
            Quaternion rotation = transform.rotation;
            transform.rotation = Quaternion.identity;
            collidingWall = false;
            if (Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown(KeyCode.W))
            {
                animation.Play("jump");
                rigidbody.velocity = new Vector3(0, jumpHeight, 0);
                jumpSource.Play();
				numFrameFloating = -1;
            }
            transform.rotation = rotation;
        }

		if (numFrameFloating > 0 && platformChanged){
			rigidbody.useGravity = false;
		}
		else{
			rigidbody.useGravity = true;
			platformChanged = false;
		}

        // FOR DEBUGGING, multi jump
        if (Input.GetKey(KeyCode.U))
        {
            rigidbody.velocity = new Vector3(0, 8, 0);
        }
    }


	void Follow(){
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

			float heightBase = transform.position.y+transform.localScale.y;
			float maxWidth = targetingWidth;
			float minHeight = minTargetingHeight, maxHeight = maxTargetingHeight;

			float minX = -1000, maxX = 1000;
			if (transform.rotation == rightRotation){
				minX = transform.position.x;
				maxX = transform.position.x + maxWidth;
			}
			else{
				minX = transform.position.x - maxWidth;
				maxX = transform.position.x;
			}

			if (newReticlePos.x < minX){
				newReticlePos.x = minX;
			}
			if (newReticlePos.x > maxX){
				newReticlePos.x = maxX;
			}

			float xDiff = Mathf.Abs(newReticlePos.x - transform.position.x);

			float heightAtX = minHeight*(1-xDiff) + maxHeight*xDiff;

			if(newReticlePos.y > heightBase + heightAtX){
				newReticlePos.y = heightBase + heightAtX;
			}
			else if(newReticlePos.y < heightBase - heightAtX){
				newReticlePos.y = heightBase - heightAtX;
			}

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
						platformChanged = true;
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
                        (GameObject)Instantiate(playerPlatformPrefab, platformPosition, Quaternion.identity);
                    
                }
                else if (currentGun.Equals(GunType.GravityGun))
                {
                    // update gravity center location
                    if (currentActiveGravityCenter) 
                    {
                        Destroy(currentActiveGravityCenter);
                    }
                    currentActiveGravityCenter = (GameObject)Instantiate(gravityCenterPrefab, targetingReticle.transform.position, Quaternion.Euler(0, 90, 0));
					gravityScript = currentActiveGravityCenter.GetComponent<GravityCenterScript>();
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
        else if (invincibilityFrames > 100 && collision.gameObject.tag.Equals("Enemy"))
        {
            // damage and knock back
            setHealth(getHealth() - 10);
            Vector3 knockbackForce;
            if (gameObject.GetComponent<Rigidbody>().velocity.x < 0)
                knockbackForce = new Vector3(-600, 600, 0);
            else
                knockbackForce = new Vector3(600, 600, 0);
            gameObject.GetComponent<Rigidbody>().AddForce(knockbackForce);

            invincibilityFrames = 0;

            takeDamageSource.Play();
        }
		else if (collision.gameObject.tag.Equals("PlayerPlatform") && IsGrounded()){
			numFrameFloating = numFrameFloatingExposed;
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


	public void setActive(bool isActive){
		m_isActive = isActive;
	}

	public bool isActive(){
		return m_isActive;
	}

    public GunType GetGunType()
    {
        return currentGun;
    }

	public float GetGravityRange(){
		return gravityRange;
	}
}
