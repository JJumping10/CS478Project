using UnityEngine;

public class MoonPlayer : MonoBehaviour
{
    public GameObject player;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        player = GameObject.Find("Player");
        player.GetComponent<Rigidbody2D>().gravityScale = 1.75f;
        player.GetComponent<Rigidbody2D>().constraints = RigidbodyConstraints2D.None;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
