using UnityEngine;
using System.Collections;

public class LoseScript : MonoBehaviour {
	public AudioClip loseSound;
	private GUIStyle buttonStyle; 
	// Use this for initialization 
	void Start () {
		AudioSource.PlayClipAtPoint (loseSound, transform.position);	
	} 
	
	// Update is called once per frame 
	void Update () { 
		
	} 
	
	void OnGUI (){ 
		GUILayout.BeginArea(new Rect(425, 150, 
		                             175, 425)); 
		GUILayout.TextField("You Lose!");
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

