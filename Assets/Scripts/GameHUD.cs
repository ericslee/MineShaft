using UnityEngine;
using System.Collections;

public class GameHUD : MonoBehaviour
{
	//Health Bar
	//float barDisplay = 0;
	//Vector2 pos = new Vector2 (20, 40);
	//Vector2 size;
	//public Texture2D progressBarEmpty;
	//public Texture2D progressBarFull;
	GUIStyle healthGood = new GUIStyle ();
	GUIStyle healthMedium = new GUIStyle();
	GUIStyle healthBad = new GUIStyle ();

    GameManager gameManager;

    // win/loss conditions
    bool hasPlayerWon = false;
    bool hasPlayerLost = false;

    // Use this for initialization
    void Start()
    {
        // cache references
        gameManager = GetComponent<GameManager>();
		//size = new Vector2(Screen.width / 5, Screen.height / 25 + 10);
		Color[] c = new Color[1];
		c [0] = Color.yellow;
		//healthGood.normal.background.SetPixels = Color.green;

		//healthBad.normal.background.SetPixels = Color.red;
    }
    
	Texture2D createTex( int w, int h, float health)
	{
		Color c;
		if (gameManager.GetActivePlayer().getHealth() > 50) {
			c = new Color(0f, 1f, 0f, 0.5f);
		} else if (gameManager.GetActivePlayer().getHealth() <= 50 && gameManager.GetActivePlayer().getHealth() > 10) {
			c = new Color( 1f, 1f, 0f, 0.5f );
		} else {
			c = new Color( 1f, 0f, 0f, 0.5f );
		}
		Color[] ps= new Color[w * h];
		for( int i = 0; i < h; ++i )
		{
			for (int j = 0; j < w; j++) {
				if (j < w * (gameManager.GetActivePlayer().getHealth()/100f)) {
					ps[ i*w + j ] = c;
				} else {
					ps[i*w + j] = new Color(0f,0f,0f,0.5f);
				}
			}
		}
		Texture2D tex = new Texture2D(w, h);
		tex.SetPixels( ps );
		tex.Apply();
		return tex;
	}

    // Update is called once per frame
    void Update()
    {
		if (gameManager.GetActivePlayer().getHealth () < 1) {
			hasPlayerLost = true;
		}
    }

    void OnGUI()
    {

        GUI.Label(new Rect(10, 5, Screen.width / 5, Screen.height / 25), "Current Level: " + gameManager.GetCurrentLevel());

        // Instructions
        GUI.Label(new Rect(10, 40, Screen.width / 5, Screen.height / 2 + 10), 
		    /*"Controls - \nA+D or Left+right arrows: movement" +
            "\n\nSpace: jump" +
            "\n\nLeft-shift: Switch gun" +
            "\n\nMouse: aim" +
            "\n\nClick: fire gun" +
            "\n\nU: fly in DEBUG" +*/ 
		    "\n\nHealth: " + gameManager.GetActivePlayer().getHealth() +
		    "\n\nGun: " + gameManager.GetActivePlayer().GetGunType());
		//		GUI.skin.horizontalSlider = red;
		Texture2D tex = createTex( Screen.width / 5, Screen.height / 2 + 10, gameManager.GetActivePlayer().getHealth());
		Debug.Log ("Made it here");
		GUI.skin.horizontalSlider.normal.background = tex;
		GUI.HorizontalSlider (new Rect(10, 50, Screen.width / 5, Screen.height / 2 + 10),gameManager.GetActivePlayer().getHealth(), 0, 100);
        // Win/loss
        if (hasPlayerWon)
		{
			Application.LoadLevel("Win"); 
            //GUI.Label(new Rect(Screen.width / 2 - Screen.width / 8, Screen.height / 2, Screen.width / 4, Screen.height / 3), "You escaped the mine!");
        } else if (hasPlayerLost)
		{
			Application.LoadLevel("Lose"); 
			//GUI.Label(new Rect(Screen.width / 2 - Screen.width / 16, Screen.height / 2, Screen.width / 4, Screen.height / 3), "You lose...");
        }
    }

    public void Win()
    {
        hasPlayerWon = true;
    }

    public void Lose()
    {
        hasPlayerLost = true;
    }
}
