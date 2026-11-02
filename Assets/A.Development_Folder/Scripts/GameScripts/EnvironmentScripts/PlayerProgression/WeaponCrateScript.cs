using UnityEngine;

public class WeaponCrateScript : MonoBehaviour
{
    public GameObject gameObj;//Use to get game object of the ItemManager
    public ItemManager itemManager;//The actual ItemManager script
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        gameObj = GameObject.Find("ItemManager");
        itemManager = gameObj.GetComponent<ItemManager>();
    }

    // Update is called once per frame
    void Update()
    {

    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.tag == "Player")
        {
            if(gameObject.name == "ShotgunWeaponCrate") itemManager.UnlockWeapon(1, 1, true);//Unlocked from shotgun create
            if(gameObject.name == "RifleWeaponCrate") itemManager.UnlockWeapon(2, 2, true);//Unlocked from rifle create
            if(gameObject.name == "SniperWeaponCrate") itemManager.UnlockWeapon(3, 3, true);//Unlocked from sniper create
            if(gameObject.name == "GrenadeWeaponCrate") itemManager.UnlockWeapon(4, 4, true);//Unlocked from grenade create
            Destroy(gameObject);//destroy crate from game
        } 
    }
}
