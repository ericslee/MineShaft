using UnityEngine;
using System.Collections;

public class WinScript : MonoBehaviour {
	
	private GUIStyle buttonStyle; 
	// Use this for initialization 
	void Start () {
		
	} 
	
	// Update is called once per frame 
	void Update () { 
		
	} 
	
	void OnGUI (){ 
		GUILayout.BeginArea(new Rect(425, 150, 
		                             175, 425)); 
		GUILayout.TextField("You Win!");
		// Load the main scene 
		// The scene needs to be added into build setting to be loaded! 
		if (GUILayout.Button("New Game")) 
		{ 
			Application.LoadLevel("Level1"); 
		}
		
		if (GUILayout.Button("Exit")) 
		{ 
			Application.Quit(); 
			Debug.Log ("Application.Quit() only works in build, not in editor"); 
		} 
		
		GUILayout.EndArea(); 
	} 
}
