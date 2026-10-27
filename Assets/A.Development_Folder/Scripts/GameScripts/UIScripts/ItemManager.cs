using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.InputSystem;
using System;
using System.Collections;
using UnityEngine.Tilemaps;

public class ItemManager : MonoBehaviour
{

    public const int NUM_WEAPONS = 5;
    public static int SelectedWeapon = 0;
    public static int PreviousWeapon = 0;

    [SerializeField] private InputActionReference selectPistol;
    [SerializeField] private InputActionReference selectShotgun;
    [SerializeField] private InputActionReference selectRifle;
    [SerializeField] private InputActionReference selectSniper;
    [SerializeField] private InputActionReference selectGrenade;
    [SerializeField] private WeaponTile pistolTile;
    [SerializeField] private WeaponTile shotgunTile;
    [SerializeField] private WeaponTile rifleTile;
    [SerializeField] private WeaponTile sniperTile;
    [SerializeField] private WeaponTile grenadeTile;

    [System.Serializable]
    public class Weapon
    {
        public string name;
        public bool isActive;
        public bool playerHas;
        public float recoil;
        public float cooldown;
        public float magSize;
        public float reloadTime;
        public float ammo;
        public InputActionReference select;
        public float spread;
        public float recoilSpread;
        public float shotRange;
        public int bullets;
        public float nextShot;
        public float reloadEnd;
        public WeaponTile tile;

        public Weapon(string name, float recoil, float cooldown, float magSize, float ammo, float reloadTime, InputActionReference select,
                      float spread, float recoilSpread, float shotRange, int bullets, WeaponTile tile)
        {
            this.name = name;
            this.isActive = false;
            this.playerHas = false;
            this.recoil = recoil;
            this.cooldown = cooldown;
            this.magSize = magSize;
            this.ammo = ammo;
            this.reloadTime = reloadTime;
            this.select = select;
            this.spread = spread;
            this.recoilSpread = recoilSpread;
            this.shotRange = shotRange;
            this.bullets = bullets;
            this.nextShot = 0f;
            this.reloadEnd = 0f;
            this.tile = tile;
        }

        public void DisplayReload()
        {
            float currentTime = Time.time;
            float progress = (reloadEnd-currentTime)/reloadTime * 100f;
            if (currentTime <= reloadEnd)
            {
                tile.Reload(Mathf.Clamp(progress,0f,100f));
            }
//            yield return new WaitForSeconds(reloadTime);
            UpdateAmmoDisplay();
        }

        public void UpdateAmmoDisplay()
        {
            string ammoText = "∞";
            if (ammo != float.PositiveInfinity)
            {
                ammoText = ammo + " / " + magSize;
            }
            tile.UpdateAmmo(ammoText);
        }

        public void WeaponKeyActive()
        {
            tile.WeaponKeyActive();
        }
        public void WeaponKeyInactive()
        {
            tile.WeaponKeyInactive();
        }


    }

    // create weapons
    public static Weapon[] weapons = new Weapon[NUM_WEAPONS];

    // weapon actions
    [SerializeField] private InputActionReference reload;
    [SerializeField] private InputActionReference fire;

    private InputActionReference[] selectWeapons = new InputActionReference[NUM_WEAPONS];

    void Awake()
    {
        // create weapons
        weapons[0] = new Weapon("Pistol", 4.5f, 0.22f, float.PositiveInfinity, float.PositiveInfinity, 0f, selectPistol, 0f, 0f, 30f, 1, pistolTile);
        weapons[1] = new Weapon("Shotgun", 12f, 0.9f, 5f, 5f, 1.7f, selectShotgun, 12f, 6f, 30f, 7, shotgunTile);
        weapons[2] = new Weapon("Rifle", 2.2f, 0.13f, 18f, 18f, 1.9f, selectRifle, 1.5f, 1f, 30f, 1, rifleTile);
        weapons[3] = new Weapon("Sniper", 15f, 1.5f, 3f, 3f, 2.5f, selectSniper, 0f, 0f, 100f, 1, sniperTile);
        weapons[4] = new Weapon("Grenade", 4f, 0.7f, 4f, 4f, 2.2f, selectGrenade, 0f, 0f, 30f, 1, grenadeTile);

        //starting weapons
        weapons[0].playerHas = true;
        weapons[1].playerHas = true;
        weapons[2].playerHas = true;
        weapons[0].isActive = true;

        for (int i = 0; i < NUM_WEAPONS; i++)
        {
            weapons[i].tile.keybind.text = weapons[i].select.action.GetBindingDisplayString();
            weapons[i].tile.reloading.fillAmount = 0f;
            if (weapons[i].playerHas)
            {
                weapons[i].tile.MakeAvailable();
                weapons[i].select.action.Enable();
                weapons[i].UpdateAmmoDisplay();
            }
            else
            {
                weapons[i].tile.MakeUnavailable();
            }
            if (weapons[i].isActive)
            {
                weapons[i].tile.SelectWeapon();
            }
            else
            {
                weapons[i].tile.UnselectWeapon();
            }
        }


    }


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public static Weapon GetCurrentWeapon(int i)
    {
        if (weapons[i].playerHas)
        {
            weapons[SelectedWeapon].tile.UnselectWeapon();
            weapons[i].tile.SelectWeapon();
            SelectedWeapon = i;
            return weapons[i];
        }
        else
        {
            return weapons[SelectedWeapon];
        }
    }

}
