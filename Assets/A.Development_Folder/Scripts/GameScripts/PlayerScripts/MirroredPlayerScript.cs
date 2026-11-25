using UnityEngine;

public class MirroredPlayerScript : MonoBehaviour
{
    public GameObject player;
    public float startingXPos, offset;//Variables required to properly mirror
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        player = GameObject.Find("Player");
    }

    // Update is called once per frame
    void Update()
    {
        float mirrorX = -player.transform.position.x + offset + startingXPos;//Formula needed to mirror player position
        transform.position = new Vector3(mirrorX,player.transform.position.y,0);//Mirrors the player position
    }
}
