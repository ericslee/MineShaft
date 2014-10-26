using UnityEngine;
using System.Collections;

public class TitleScript : MonoBehaviour {
	
	private GUIStyle buttonStyle; 
	private Texture newGameButton; 
	// Use this for initialization 
	void Start () {
		newGameButton = (Texture)Resources.Load("Resources/Materials/Textures/ngb");
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
		//GUILayout.TextField("MineShaft");
		// Load the main scene 
		// The scene needs to be added into build setting to be loaded! 
		if (GUI.Button(new Rect(440, 450, 125, 20), newGameButton)) {
			Application.LoadLevel("Mine"); 
		}

		if (GUILayout.Button("New Game")) 
		{ 
			Application.LoadLevel("Mine"); 
		}
		
		if (GUILayout.Button ("Instructions")) {
			Application.LoadLevel("Instructions");
		}
		
		if (GUILayout.Button("Exit")) 
		{ 
			Application.Quit(); 
			Debug.Log ("Application.Quit() only works in build, not in editor"); 
		} 
		
		GUILayout.EndArea(); 
	} 
}
