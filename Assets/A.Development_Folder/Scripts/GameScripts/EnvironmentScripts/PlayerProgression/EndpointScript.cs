using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class EndpointScript : MonoBehaviour
{
    public GameObject player, playerBackground;
    public PlayerScript playerScript;
    public BackgroundImageScript backgroundImageScript;
    public GameManager gameManager;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        gameManager = GameManager.instance;
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
            Pause();
            StartCoroutine(LoadNextLevel());
        }
    }

    void Pause()
    {
        gameManager.isPaused = true;
        gameManager.GameTimer.Stop();
        Time.timeScale = 0f;
        gameManager.inputActions.FindActionMap("Player").Disable();
    }

    IEnumerator LoadNextLevel()
    {
        var sceneName = SceneManager.GetActiveScene().name;//Get the scene name to compare to other scenes
        string nextScene;
        int background;
        yield return StartCoroutine(gameManager.GameTimer.TimeToPoints());
        yield return gameManager.AttemptsToPoints();
        yield return new WaitForSecondsRealtime(2f);

        switch (sceneName)
        {
            case "PlatformerLevel1Scene": nextScene = "PlatformerLevel2Scene"; background = 0; break;
            case "PlatformerLevel2Scene": nextScene = "PlatformerLevel3Scene"; background = 1; break;
            //case "PlatformerLevel3Scene": nextScene = "PlatformerLevel4Scene"; background = 1; break;
            //case "PlatformerLevel4Scene": nextScene = "PlatformerLevel5Scene"; background = 1; break;
            //case "PlatformerLevel5Scene": nextScene = "PlatformerLevel1Scene"; background = 0; break;
            default: nextScene = "TitleScreen"; background = 0; break;
        }

        //playerScript.updatedPos = new Vector2(0,0.5f);                                                          //Signify spawn to beginning of a new level
        //player.transform.position = playerScript.updatedPos;                                                    //Move the character to the new spawn
        //playerBackground.GetComponent<Image>().sprite = backgroundImageScript.ChangeBackground(background);     //Change background to next level
        //DontDestroyOnLoad(GameObject.Find("Player"));                                                           //Move the player game object to new scene
        SceneManager.LoadScene(nextScene);                                                                      //Load scene
    }
}
