using UnityEngine;

public class SandstormScript : MonoBehaviour
{
    public GameObject player;
    public Rigidbody2D rb;
    public RecoilPlayerController rpc;//Get the recoil script to modify the weapon recoil to terrain
    public float leftwardForce, downwardForce, timer;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        player = GameObject.Find("Player");
        rb = player.GetComponent<Rigidbody2D>();
        rpc = player.GetComponent<RecoilPlayerController>();
        leftwardForce = -1.75f;
        downwardForce = -3;
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = player.transform.position + new Vector3(32,1.5f);//The particle emitter follows the player, but also keeps its distance
        if (Input.GetMouseButtonDown(0)) timer = .5f;//Gives a partial window to time before sandstorm blowback
        if (timer <= 0) rb.linearVelocity = new Vector2(leftwardForce,downwardForce);//This line will pull the player to the left, this is because of the sandstorm
        timer -= Time.deltaTime;
    }
}
