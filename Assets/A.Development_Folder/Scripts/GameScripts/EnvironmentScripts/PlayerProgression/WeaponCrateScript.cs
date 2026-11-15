using Unity.VisualScripting.Antlr3.Runtime.Tree;
using UnityEngine;

public class WeaponCrateScript : MonoBehaviour
{
    public ItemManager itemManager;  //The actual ItemManager script

    void Awake()
    {
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        itemManager = GameManager.instance.ItemManager;
    }

    // Update is called once per frame
    void Update()
    {

    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.tag == "Player")
        {
            int newWeapon;
            switch (gameObject.name)
            {
                case "ShotgunWeaponCrate": newWeapon = 1; break;
                case "RifleWeaponCrate": newWeapon = 2; break;
                case "SniperWeaponCrate": newWeapon = 3; break;
                case "GrenadeWeaponCrate": newWeapon = 4; break;
                default: newWeapon = 0; break;
            }
            itemManager.UnlockWeapon(newWeapon);
            Destroy(gameObject);  //destroy crate from game
        } 
    }
}
