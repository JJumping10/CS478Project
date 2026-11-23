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
        //Added a reference to find game manager, but don't know how to reference correctly, it is up to (Paul) to set up the connection
        //P.S. The pause is not working either 
        gameManager = GameObject.Find("Game Manager").GetComponent<GameManager>();
    }

    // Update is called once per frame
    void Update()
    {
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.tag == "Player")
        { 
            gameManager.Pause();
            StartCoroutine(gameManager.LoadNextLevel());
        }
    }

}
