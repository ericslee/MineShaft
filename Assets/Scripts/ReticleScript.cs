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
		minMaxRange = new Vector3 (0.01f, 5.0f);
		lightRangeSign = 1;

		Light[] lights = gameObject.GetComponentsInChildren<Light> ();
		childLight = lights[0];
		range = childLight.range;

		gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
    }
    
    // Update is called once per frame
    void Update()
    {
		//transform.localScale;// += new Vector3(0.0f, 0.1f, 0.1f);
		Light[] lights = gameObject.GetComponentsInChildren<Light> ();
		float percentage = ((float)gameManager.playerHealth) / 100.0f;
		float max = percentage*minMaxRange.y;
		childLight.range += lightRangeSign*0.1f;
		if (childLight.range < minMaxRange.x) lightRangeSign = 1;
		else if (childLight.range > max) lightRangeSign = -1;
		range = childLight.range;
        HandleInput();
    }

    void HandleInput()
    {
        if (Input.GetKey(KeyCode.RightArrow) || Input.GetKey(KeyCode.D))
        {
            transform.Translate(Vector2.right * 4f * Time.deltaTime);
        }
        if (Input.GetKey(KeyCode.LeftArrow) || Input.GetKey(KeyCode.A))
        {
            transform.Translate(-Vector2.right * 4f * Time.deltaTime);
        }
        if (Input.GetKey(KeyCode.UpArrow) || Input.GetKey(KeyCode.W))
        {
            transform.Translate(new Vector3(0, 0, -1) * 4f * Time.deltaTime);
        }
        if (Input.GetKey(KeyCode.DownArrow) || Input.GetKey(KeyCode.S))
        {
            transform.Translate(new Vector3(0, 0, 1) * 4f * Time.deltaTime);
        }
    }
}
