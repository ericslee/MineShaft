using UnityEngine;
using System.Collections;

public class WinScript : MonoBehaviour {
	public AudioClip winSound;
	private GUIStyle buttonStyle; 
	// Use this for initialization 
	void Start () {
		AudioSource.PlayClipAtPoint (winSound, transform.position);
	} 
	
	// Update is called once per frame 
	void Update () { 
		if (Input.GetKey (KeyCode.Escape)) {
			Application.Quit();
			Debug.Log ("Application.Quit() only works in build, not in editor"); 
		}
	} 
	
	void OnGUI (){ 
		GUILayout.BeginArea(new Rect(425, 350, 
		                             175, 425)); 
		// Load the main scene 
		// The scene needs to be added into build setting to be loaded! 
		if (GUILayout.Button("New Game")) 
		{ 
			Application.LoadLevel("Mine"); 
		}
		
		if (GUILayout.Button("Exit")) 
		{ 
			Application.Quit(); 
			Debug.Log ("Application.Quit() only works in build, not in editor"); 
		} 
		
		GUILayout.EndArea(); 
	} 
}
