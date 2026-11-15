using UnityEngine;

public class MovingPlatform : MonoBehaviour
{
    public float speed;//*Required* speed of the platform
    public float minX, maxX, minY, maxY;//*Required* Range of either
    public float tX, tY;//Time to move the platform
    public bool isMovingDown, isMovingRight;//Either move it one way or the other
    public Vector3 startingPos;//The position at the start of the platform

    public bool isOnPlatform;//If player on platform or not
    public Rigidbody2D rb;//Rb2d of the platform to manipulate
    public GameObject player;//Get the player when needed
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        startingPos = transform.position;
        GetComponent<Rigidbody2D>().constraints = RigidbodyConstraints2D.FreezeAll;
    }
    // Update is called once per frame
    void Update()
    {
        float speedRate = Time.deltaTime * speed;//Speed at which the platform will move at
        var targetPosition = startingPos + new Vector3(XRangeMovement(speedRate),YRangeMovement(speedRate));//The place the platform will move to 
        transform.position = Vector3.MoveTowards(transform.position, targetPosition, speedRate);//Moves to specific direction
        Carry();//Method of the platform that will carry the character
    }
    float XRangeMovement(float speedRate)
    {
        if(maxX == minX) return 0;
        if(isMovingRight) tX -= speedRate; else tX += speedRate;
        if(tX>maxX){ isMovingRight = true; } else if(tX<minX){ isMovingRight = false; }
        return tX;
    }    
    float YRangeMovement(float speedRate)
    {
        if(maxY == minY) return 0;
        if(isMovingDown) tY -= speedRate; else tY += speedRate;
        if(tY>maxY){ isMovingDown = true; } else if(tY<minY){ isMovingDown = false; }
        return tY;
    }

    void Carry()
    {
        //"hitColliders" is just a collection of objects that overlap a specific area (i.e. the player on the platform position)
        Collider2D[] hitColliders = Physics2D.OverlapBoxAll(transform.position + new Vector3(0,0.25f,0), new Vector2(3,1), 0f);
        foreach(var h in hitColliders)//Checks for any and all collisions
        {
            //This finds specifically the player and makes true if they are stepping on it
            if(h.gameObject.tag == "Player") {
                isOnPlatform = true;
                player = h.gameObject;
                break;
            } else {
                isOnPlatform = false;
                continue;
            }
        }
        
        if(isOnPlatform){//If player on the platform
            player.transform.SetParent(transform);
        } else {//What to do with the platform when not stepped on anymore by the player
            if(player != null) player.transform.SetParent(null);
        } 
    }
}
