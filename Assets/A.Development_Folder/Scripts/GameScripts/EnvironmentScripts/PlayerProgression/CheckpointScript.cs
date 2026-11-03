using UnityEngine;

public class CheckpointScript : MonoBehaviour
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
        if(collision.gameObject.tag == "Player") playerScript.updatedPos = transform.position;//Set player spawn to checkpoint
    }
}
