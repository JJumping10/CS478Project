using UnityEngine;

public class DeathBarrierScript : MonoBehaviour
{
    public GameObject player;
    public PlayerScript playerScript;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        player = GameObject.Find("Player");
        playerScript = player.GetComponent<PlayerScript>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    
    void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.tag == "Player") playerScript.RespawnPlayer();//If player touches death barrier, respawn the player
    }
}
