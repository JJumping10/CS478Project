using UnityEngine;

public class AsteriodsScript : MonoBehaviour
{
    public PlayerScript playerScript;
    public GameObject player;
    public bool isFollowingAbove, isFollowingRight, isSpecificFollow, followX, followY, cancelAsteroid;
    public float modifiedX, modifiedY;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        player = GameObject.Find("Player");
        playerScript = player.GetComponent<PlayerScript>();
    }

    // Update is called once per frame
    void Update()//Asteriods spawning near player position
    {
        if(!isSpecificFollow)
        {
            if(isFollowingAbove) transform.position = new Vector3(player.transform.position.x, transform.position.y, 0);
            if(isFollowingRight) transform.position = new Vector3(player.transform.position.x + 30, transform.position.y, 0);
        } else { SpecificFollow(); }
        if(cancelAsteroid) CancelAsteroid();
    }

    void OnParticleTrigger()//Respawn the player back when hit by asteriod 
    {
        playerScript.RespawnPlayer();
    }

    void SpecificFollow()//Follow the player in a certain way throughout the level
    {
        if(followX) modifiedX = player.transform.position.x + 30;
        if(followY) modifiedY = player.transform.position.y;
        transform.position = new Vector3(modifiedX, modifiedY, 0);
    }

    void CancelAsteroid()//This is for the beginning part of level 5
    {
        if(player.transform.position.x > 120) gameObject.SetActive(false);
    }
}
