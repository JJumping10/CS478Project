using UnityEngine;

public class PlayerScript : MonoBehaviour
{
    public Vector2 updatedPos;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        updatedPos = new Vector2(0,0.5f);//Signify spawn
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.P)) RespawnPlayer();//Button to respawn the player
    }

    public void RespawnPlayer()
    {
        transform.position = updatedPos;//Respawn to last position saved
    }
}
