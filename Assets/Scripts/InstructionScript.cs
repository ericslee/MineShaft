using UnityEngine;
using System.Collections;

public class InstructionScript : MonoBehaviour {
	
	private GUIStyle buttonStyle; 
	
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		if (Input.GetKey (KeyCode.Escape)) {
			Application.Quit();
			Debug.Log ("Application.Quit() only works in build, not in editor"); 
		}
	}
	
	void OnGUI() {
		GUILayout.BeginArea(new Rect(100, 350, 
		                             800, 475)); 
		GUILayout.TextField("The player controls one miner that is stuck in a " +
		                    "mine shaft. The player's goal is to get them out of\n" +
		                    "the mine by solving different puzzles in order to get " +
		                    "the miner through each level, the last of\n" + 
		                    "which will be the exit from the mine. There are five levels, " +
		                    "and all must be completed for the player to win. The \n" +
		                    "player has to get the miner to the top of each level " +
		                    "screen in order to win the level, and will do this by\n" +
		                    "having the miner jump on different platforms that will " +
		                    "allow them to get higher up on the screen. However, the\n" +
		                    "platforms in each level are not necessarily reachable by " +
		                    "the miner or will be in inconvenient locations that do\n" +
		                    "not enable the miner to reach the top of the screen. Thus, the " +
		                    "player must use the miner's two guns - a gravity gun and a \n" +
		                    "platform creating gun, in order to create platforms or drag " +
		                    "existing platforms towards them. \n" +
		                    //"Each gun is stuck to one \n" +
		                    //"of the two miners, and the player can switch between which " +
		                    //"miner they are currently conrolling in order to use both.\n" +
		                    //"When not being directly controlled by the player, the  " +
		                    //"miner not in use will follow the user controlled miner. \n" +
		                    "To move the miner, the user will use the arrow keys " +
		                    "and WASD keys, and the space bar to make the miner jump.\n" +
		                    "make the miner jump." +
		                    //", and right shift to switch between " +
		                    //"miners. 
		                    "To go into shooting mode, click left shift, \n" +
		                    "then use the mouse to aim the targeting reticle, and left mouse" +
		                    "click to fire. Clicking p will bring the player to the\n" +
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
			Application.LoadLevel("MainMenu"); 
		}
		GUILayout.EndArea(); 
	}
}

