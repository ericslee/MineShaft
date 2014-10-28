using UnityEngine;
using System.Collections;

public class GameHUD : MonoBehaviour
{

    GameManager gameManager;

    // win/loss conditions
    bool hasPlayerWon = false;
    bool hasPlayerLost = false;

    Texture2D tex;
    int currHealthMeterValue;

    // Use this for initialization
    void Start()
    {
        // cache references
        gameManager = GetComponent<GameManager>();

        currHealthMeterValue = gameManager.GetActivePlayer().getHealth();
        tex = createTex(Screen.width / 6, Screen.height / 2 + 10, gameManager.GetActivePlayer().getHealth());
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
		Texture2D texture = new Texture2D(w, h);
		texture.SetPixels( ps );
		texture.Apply();

        Resources.UnloadUnusedAssets();

        return texture;
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

        GUI.Label(new Rect(50, 15, Screen.width / 5, Screen.height / 25), "Current Level: " + gameManager.GetCurrentLevel());

		
		GUI.Label(new Rect(50, 50, Screen.width / 5, Screen.height / 25), "Gun: " + gameManager.GetActivePlayer().GetGunType());
        GUI.Label (new Rect (50, 85, Screen.width / 5, Screen.height / 2 + 10), 
		          "Health: " + gameManager.GetActivePlayer ().getHealth ());

        // Health bar
        if (currHealthMeterValue != gameManager.GetActivePlayer().getHealth() && gameManager.GetActivePlayer().getHealth() > 0)
        {
		    tex = createTex(Screen.width / 6, Screen.height / 2 + 10, gameManager.GetActivePlayer().getHealth());
            currHealthMeterValue = gameManager.GetActivePlayer().getHealth();
        }
        GUI.skin.horizontalSlider.normal.background = tex;
		GUI.HorizontalSlider (new Rect(50, 105, Screen.width / 6, Screen.height / 2 + 10), gameManager.GetActivePlayer().getHealth(), 0, 100);

        // Win/loss
        if (hasPlayerWon)
		{
			Application.LoadLevel("Win"); 
        } else if (hasPlayerLost)
		{
			Application.LoadLevel("Lose"); 
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
