using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class EndpointScript : MonoBehaviour
{
    public GameObject player, playerBackground;
    public PlayerScript playerScript;
    public BackgroundImageScript backgroundImageScript;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        player = GameObject.Find("Player");//Player game object
        playerBackground = GameObject.Find("BackgroundImage");//Player background image game object
        playerScript = player.GetComponent<PlayerScript>();//Get the player script to move the spawn
        backgroundImageScript = GameObject.Find("BackgroundImage").GetComponent<BackgroundImageScript>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.tag == "Player"){ 
            var sceneName = SceneManager.GetActiveScene().name;//Get the scene name to compare to other scenes
            if (sceneName == "PlatformerLevel1Scene")
            {
                playerScript.updatedPos = new Vector2(0,0.5f);//Signify spawn to beginning of a new level
                player.transform.position = playerScript.updatedPos;//Move the character to the new spawn
                playerBackground.GetComponent<Image>().sprite = backgroundImageScript.ChangeBackground(0);//Change background to next level
                DontDestroyOnLoad(GameObject.Find("Player"));//Move the player game object to new scene
                SceneManager.LoadScene("PlatformerLevel2Scene");//Load scene
            }
            if (sceneName == "PlatformerLevel2Scene")
            {
                playerScript.updatedPos = new Vector2(0,0.5f);//Signify spawn to beginning of a new level
                player.transform.position = playerScript.updatedPos;//Move the character to the new spawn
                playerBackground.GetComponent<Image>().sprite = backgroundImageScript.ChangeBackground(1);//Change background to next level
                DontDestroyOnLoad(GameObject.Find("Player"));//Move the player game object to new scene
                SceneManager.LoadScene("PlatformerLevel3Scene");//Load Scene
            }
            if (sceneName == "PlatformerLevel3Scene")
            {
                Destroy(player);
                SceneManager.LoadScene("TitleScreen");//Load to title screen because no more levels... for now
            } 
        }
    }
}
