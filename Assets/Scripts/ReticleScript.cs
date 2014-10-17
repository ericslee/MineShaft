using UnityEngine;
using System.Collections;

public class ReticleScript : MonoBehaviour
{

	private Vector2 minMaxRange;//(f, 5.0f);
	private int lightRangeSign;
	private Light childLight;

	GameManager gameManager;

	public float range;

    // Use this for initialization
    void Start()
    {
		minMaxRange = new Vector3 (1.0f, 5.0f);
		lightRangeSign = 1;

		Light[] lights = gameObject.GetComponentsInChildren<Light> ();
		childLight = lights[0];
		range = childLight.range;

        // cache references
		gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
    }
    
    // Update is called once per frame
    void Update()
    {
		//transform.localScale;// += new Vector3(0.0f, 0.1f, 0.1f);
		Light[] lights = gameObject.GetComponentsInChildren<Light> ();
		if (gameManager.GetActivePlayer().GetGunType().Equals(GunType.GravityGun)) 
        {
            range = 0;
            childLight.range = 2.5f;
        }
        else 
        {
			float percentage = ((float)gameManager.GetActivePlayer().getHealth()) / 100.0f;
    		float max = percentage*minMaxRange.y;
    		childLight.range += lightRangeSign*0.1f;
    		if (childLight.range < minMaxRange.x) lightRangeSign = 1;
    		else if (childLight.range > max) lightRangeSign = -1;
    		range = childLight.range;
        }
    }

    void OnTriggerEnter(Collider other)
    {
		if (gameManager.GetActivePlayer().GetGunType().Equals(GunType.GravityGun)) 
        {
			gameManager.GetActivePlayer().AddToGravityList(other.gameObject);
        }
    }
}
