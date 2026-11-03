using UnityEngine;
using UnityEngine.SceneManagement;

public class EndpointScript : MonoBehaviour
{
    public PlayerScript playerScript;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        playerScript = GameObject.Find("Player").GetComponent<PlayerScript>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.tag == "Player"){ 
            var sceneName = SceneManager.GetActiveScene().name;
            if (sceneName == "PlatformerLevel1Scene") SceneManager.LoadScene("PlatformerLevel2Scene");
            if (sceneName == "PlatformerLevel2Scene") SceneManager.LoadScene("PlatformerLevel3Scene");
            playerScript.updatedPos = new Vector2(0,0.5f);//Signify spawn to beginning of a new level
            if (sceneName == "PlatformerLevel3Scene") SceneManager.LoadScene("TitleScreen");
        }
    }
}
