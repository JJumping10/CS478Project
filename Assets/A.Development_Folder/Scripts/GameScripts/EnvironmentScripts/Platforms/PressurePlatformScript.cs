using UnityEngine;

public class PressurePlatformScript : MonoBehaviour
{
    public bool isOnPlatform;//If player on platform or not
    public float fallSpeed;//Alter to create different speeds of falling of the platform
    public Vector3 startingPos;//Starting position of platform
    Rigidbody2D rb;//Rb2d of the platform to manipulate
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        startingPos = transform.position;
        rb = GetComponent<Rigidbody2D>();//To get into specifically the platform rb2d constraints
    }

    // Update is called once per frame
    void Update()
    {
        //"hitColliders" is just a collection of objects that overlap a specific area (i.e. the player on the platform position)
        Collider2D[] hitColliders = Physics2D.OverlapBoxAll(transform.position + new Vector3(0,0.1f,0), new Vector2(3,1), 0f);
        foreach(var h in hitColliders)//Checks for any and all collisions
        {
            //This finds specifically the player and makes true if they are stepping on it
            if(h.gameObject.tag == "Player") { isOnPlatform = true; break; } else { isOnPlatform = false; continue; }
        }
        
        if(isOnPlatform){//If player on the platform
            rb.constraints = RigidbodyConstraints2D.FreezePositionX | RigidbodyConstraints2D.FreezeRotation;//This lowers the platform when stepped on by the player
            GetComponent<Rigidbody2D>().linearDamping = fallSpeed;//Drops slowly
        } else {//What to do with the platform when not stepped on anymore by the player
            if(transform.position.y >= startingPos.y){ rb.constraints = RigidbodyConstraints2D.FreezeAll; }//Freezes the plaform x position, y position, and rotation so it won't move farther than it needs to
            if (transform.position.y <= startingPos.y){//if the platform is lower than its starting position
                GetComponent<Rigidbody2D>().gravityScale = -.1f;//To lift the platform back up
                rb.constraints = RigidbodyConstraints2D.None;//Need this so it can remove the y position freeze  
                rb.constraints = RigidbodyConstraints2D.FreezePositionX | RigidbodyConstraints2D.FreezeRotation;//This only allows vertical movement
                GetComponent<Rigidbody2D>().linearDamping = 0;//Rises at a moderate rate
            }
        }        
    }
}
