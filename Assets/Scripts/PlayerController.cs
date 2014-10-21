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

    // Use this for initialization
    void Start()
    {
        // cache references
        platformPrefab = Resources.Load("Prefabs/Platform");
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
			
			/*if (GUILayout.Button("Instructions")) 
			{ 
				//Application.LoadLevel("Instructions"); 
				GUI.Box(new Rect(100, 30, 800, 475),"Instructions");
				GUILayout.TextField("The player controls two miners that are stuck in a " +
				                    "mine shaft. The player's goal is to get them out of\n" +
				                    "the mine by solving different puzzles in order to get " +
				                    "the miners to the exit door of each level, the last of\n" + 
				                    "which will be the exit from the mine. There are five levels, " +
				                    "and all must be completed for the player to win. The \n" +
				                    "player has to get the miners to the top of each level " +
				                    "screen in order to win the level, and will do this by\n" +
				                    "having the miners jump on different platforms that will " +
				                    "allow them to get higher up on the screen. However, the\n" +
				                    "platforms in each level are not necessarily reachable by " +
				                    "the miners or will be in inconvenient locations that do\n" +
				                    "not enable the miners to reach the exit door. Thus, the " +
				                    "player must use the miners' two guns - a gravity gun and a \n" +
				                    "platform creating gun, in order to create platforms or drag " +
				                    "existing platforms towards them. Each gun is stuck to one \n" +
				                    "of the two miners, and the player can switch between which " +
				                    "miner they are currently conrolling in order to use both.\n" +
				                    "When not being directly controlled by the player, the  " +
				                    "miner not in use will follow the user controlled miner. \n" +
				                    "To move the miners, the user will use the arrow keys " +
				                    "and WASD keys, will use the space bar to\n" +
				                    "make the miners jump, and right shift to switch between " +
				                    "miners. To go into shooting mode, click left shift, \n" +
				                    "then use WASD/arrow keys to aim the targeting reticle," +
				                    "and space to fire. Clicking p will bring the player to the\n" +
				                    "pause menu." + 
				                    "In addition to the above stipulations, the player will need\n" +
				                    "to complete each level in a specific amount of time. This time " + 
				                    "is measured by a mist that will rise up from the bottom of the\n" +
				                    "screen. Extended exposure to the mist will cause miners to" +
				                    "become alien-like, which means that their alien weapons will\n" +
				                    "not malfunction, however they will die and thus lose the game" + 
				                    "if they are in the mist for too long.\n");
				if (GUILayout.Button("Back")) 
				{ 
					Time.timeScale = 1;
				} 

			}*/
			
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
        if (IsGrounded())
        {
            Quaternion rotation = transform.rotation;
            transform.rotation = Quaternion.identity;
            collidingWall = false;
            if (Input.GetKeyDown(KeyCode.Space) /*|| Input.GetKeyDown(KeyCode.UpArrow)*/)
            {
                animation.Play("jump");
                rigidbody.velocity = new Vector3(0, jumpHeight, 0);
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
