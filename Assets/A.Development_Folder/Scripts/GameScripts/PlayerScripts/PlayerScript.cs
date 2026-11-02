using UnityEngine;

public class PlayerScript : MonoBehaviour
{
    public Vector2 updatedPos;
    public GameObject gameObj;
    public ItemManager itemManager;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        updatedPos = new Vector2(0,0.5f);//Signify spawn
        gameObj = GameObject.Find("ItemManager");
        itemManager = gameObj.GetComponent<ItemManager>();
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.P)) RespawnPlayer();//Button to respawn the player
        if(Input.GetKeyDown(KeyCode.M)) CheatCode();//Gives player an assortment of cheats
    }

    public void RespawnPlayer(){ transform.position = updatedPos; }//Respawn to last position saved

    public void CheatCode()//Gives the player all weapons at once
    {
        itemManager.UnlockWeapon(1, 1, true);
        itemManager.UnlockWeapon(2, 2, true);
        itemManager.UnlockWeapon(3, 3, true);
        itemManager.UnlockWeapon(4, 4, true);
    }
}
