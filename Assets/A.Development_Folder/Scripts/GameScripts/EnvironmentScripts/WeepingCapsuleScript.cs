using UnityEngine;

public class WeepingCapsuleScript : MonoBehaviour
{
    public GameObject player;
    public PlayerScript playerScript;
    public GameManager gameManager;

    void Awake()
    {
        gameManager = GameManager.instance;
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        player = GameObject.Find("Player");
        playerScript = player.GetComponent<PlayerScript>();
    }

    // Update is called once per frame
    void Update()
    {
        FollowPlayer();
    }

    void FollowPlayer()//Follow player base on their location
    {
        transform.position = Vector2.MoveTowards(transform.position, player.transform.position, Vector2.Distance(transform.position, player.transform.position) * Time.deltaTime/1.5f);
    }
    void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.tag == "Player")
        {
            playerScript.RespawnPlayer();//If player touches death barrier, respawn the player
            gameManager.AddAttempt();
            transform.position = player.transform.position + new Vector3(-7,2.25f,0);//Move enemy back away from the player
        }
    }
}
