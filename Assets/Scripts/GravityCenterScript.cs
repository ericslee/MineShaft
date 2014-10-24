using UnityEngine;
using System.Collections;

public class GravityCenterScript : MonoBehaviour {

	public Transform gravityPlane;
	float gravityPlaneRotateX;
	Vector2 gravityPlaneMinMaxScale;
	float gravityAbsMaxScale;
	float gravityPlaneScale;

	// Use this for initialization
	void Start () {
		gravityPlaneRotateX = 0;
		gravityPlaneMinMaxScale = new Vector2(0,1.0f);
		gravityAbsMaxScale = 1;
		gravityPlaneScale = 0;
	}
	
	// Update is called once per frame
	void Update () {
		gravityPlaneRotateX += 0.025f;
		gravityPlane.Rotate(new Vector3(0, gravityPlaneRotateX, 0));

		if (gravityPlaneScale > gravityPlaneMinMaxScale.y){
			gravityPlaneScale = gravityPlaneMinMaxScale.x;
			gravityPlaneRotateX = 0;
		}
		gravityPlaneScale += 0.025f;

		gravityPlane.localScale = new Vector3(gravityPlaneScale,gravityPlaneScale,gravityPlaneScale);
	}

	public void SetGravityPlaneMax(float newMax){
		gravityPlaneMinMaxScale.y = newMax;
	}

	public float GetAbsMaxScale(){
		return gravityAbsMaxScale;
	}
}
