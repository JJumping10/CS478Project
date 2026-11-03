using UnityEngine;

public class DestructibleScript : MonoBehaviour
{
    public GameObject destructor;//Specific game object used to destroy the destructible
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.name == destructor.gameObject.name) Destroy(gameObject);
        if(collision.gameObject.name == destructor.gameObject.name + "(Clone)") Destroy(gameObject);
    }
}
