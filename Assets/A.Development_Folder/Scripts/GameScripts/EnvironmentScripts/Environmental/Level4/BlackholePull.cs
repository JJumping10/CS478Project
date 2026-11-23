using UnityEngine;

public class BlackholePull : MonoBehaviour
{
    public GameObject blackhole;
    public float pullSpeed;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()//Black hole pull script
    {
        blackhole = transform.parent.gameObject;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    
    void OnTriggerStay2D(Collider2D collision)
    {
        if(collision.gameObject.tag == "Player")
        {
            print("Jum");
            var player = collision.gameObject;
            player.transform.position = Vector2.MoveTowards(player.transform.position, blackhole.transform.position, Time.deltaTime * pullSpeed);
        }
    }
}
