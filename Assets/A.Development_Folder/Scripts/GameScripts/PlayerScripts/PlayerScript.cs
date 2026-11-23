using UnityEngine;

public class PlayerScript : MonoBehaviour
{
    public Vector2 updatedPos;
    public ItemManager itemManager;

    void Awake()
    {
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        itemManager = GameManager.instance.ItemManager;
        updatedPos = new Vector2(0,0.5f);//Signify spawn
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.P)) RespawnPlayer();//Button to respawn the player
        if(Input.GetKeyDown(KeyCode.M)) CheatCode();//Gives player an assortment of cheats
        if(Input.GetKeyDown(KeyCode.L)) Debug.Log("Mouse position " + Camera.main.ScreenToWorldPoint(Input.mousePosition));
        if(Input.GetKeyDown(KeyCode.T)) TeleportPlayer();//Button to respawn the player
    }

<<<<<<< HEAD
    public void RespawnPlayer() {
        transform.position = updatedPos;    //Respawn to last position saved
        }

    public void TeleportPlayer() {
        transform.position = Camera.main.ScreenToWorldPoint(Input.mousePosition);   //teleport player to mouse cursor
        }

=======
    public void RespawnPlayer(){ transform.position = updatedPos; transform.rotation = new Quaternion(0,0,0,0); }//Respawn to last position saved
>>>>>>> origin/MapLevel2

    public void CheatCode()//Gives the player all weapons at once
    {
        itemManager.UnlockWeapon(1);
        itemManager.UnlockWeapon(2);
        itemManager.UnlockWeapon(3);
        itemManager.UnlockWeapon(4);
    }
}
