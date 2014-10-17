using UnityEngine;
using System.Collections;

public class ReticleScript : MonoBehaviour
{

	private Vector2 minMaxRange;//(f, 5.0f);
	private int lightRangeSign;
	private Light childLight;

	GameManager gameManager;
    PlayerController playerController;

	public float range;

    // Use this for initialization
    void Start()
    {
		minMaxRange = new Vector3 (0.01f, 5.0f);
		lightRangeSign = 1;

		Light[] lights = gameObject.GetComponentsInChildren<Light> ();
		childLight = lights[0];
		range = childLight.range;

        // cache references
		gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
        playerController = GameObject.Find("PlatformMiner(Clone)").GetComponent<PlayerController>();
    }
    
    // Update is called once per frame
    void Update()
    {
		//transform.localScale;// += new Vector3(0.0f, 0.1f, 0.1f);
		Light[] lights = gameObject.GetComponentsInChildren<Light> ();
        if (playerController.GetGunType().Equals(GunType.GravityGun)) 
        {
            range = 0;
            childLight.range = 2.5f;
        }
        else 
        {
    		float percentage = ((float)gameManager.playerHealth) / 100.0f;
    		float max = percentage*minMaxRange.y;
    		childLight.range += lightRangeSign*0.1f;
    		if (childLight.range < minMaxRange.x) lightRangeSign = 1;
    		else if (childLight.range > max) lightRangeSign = -1;
    		range = childLight.range;
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (playerController.GetGunType().Equals(GunType.GravityGun)) 
        {
            playerController.AddToGravityList(other.gameObject);
        }
    }
}
