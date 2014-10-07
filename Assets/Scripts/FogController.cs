using UnityEngine;
using System.Collections;

public class FogController : MonoBehaviour
{
    GameManager gameManager;

    // Use this for initialization
    void Start()
    {
        // cache references
        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
    }
    
    // Update is called once per frame
    void Update()
    {
        transform.localScale += new Vector3(0f, 1f * Time.deltaTime, 0f);
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.tag.Equals("Player"))
        {
            gameManager.Lose();
        }
    }
}
