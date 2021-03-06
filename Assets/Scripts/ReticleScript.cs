﻿using UnityEngine;
using System.Collections;

public class ReticleScript : MonoBehaviour
{

	private Vector2 minMaxRange;//(f, 5.0f);
	private int lightRangeSign;
	private Light childLight;

	GameManager gameManager;

	public float range;

    // Appearance
    private Color platformGunColor = new Color(0.278f, 1, 0.969f);
    private Color gravityGunColor = new Color(0.447f, 0, 1);
    private Material platformReticleMaterial;
    private Material gravityReticleMaterial;

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
        platformReticleMaterial = (Material)Resources.Load("Materials/Materials/Reticle");
        gravityReticleMaterial = (Material)Resources.Load("Materials/Materials/ReticleGravity");
    }

    // Update is called once per frame
    void Update()
    {
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
			//check if other is within the gravity's pull
			//gameManager.GetActivePlayer().AddToGravityList(other.gameObject);
        }
    }

    public void ChangeGun()
    {
        Light[] lights = gameObject.GetComponentsInChildren<Light> ();
        childLight = lights[0];
        if (gameManager.GetActivePlayer().GetGunType().Equals(GunType.GravityGun))
        {
            childLight.color = gravityGunColor;
            gameObject.renderer.material = gravityReticleMaterial;
        } 
        else
        {
            childLight.color = platformGunColor;
            gameObject.renderer.material = platformReticleMaterial;
        }
    }
}
