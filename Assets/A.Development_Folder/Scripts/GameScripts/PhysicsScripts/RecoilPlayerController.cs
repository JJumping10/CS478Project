using UnityEngine;
using UnityEngine.InputSystem;

public class RecoilPlayerController : MonoBehaviour
{
    // Player and aiming
    private Rigidbody2D rb;
    private Camera mainCamera;
    public ItemManager itemManager;

    public int SelectedWeapon = 0;
    public const int NUM_WEAPONS = 5;
    public Vector2 AimDirection = Vector2.right;

    private Vector2 recoilForce;

    [SerializeField] private InputActionReference selectPistol;
    [SerializeField] private InputActionReference selectShotgun;
    [SerializeField] private InputActionReference selectRifle;
    [SerializeField] private InputActionReference selectSniper;
    [SerializeField] private InputActionReference selectGrenade;

    [System.Serializable]
    public class Weapon
    {
        public string name;
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

        public Weapon(string name, float recoil, float cooldown, float magSize, float ammo, float reloadTime, InputActionReference select,
                      float spread, float recoilSpread, float shotRange, int bullets)
        {
            this.name = name;
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
        }
    }

    // create weapons
    public Weapon[] weapons = new Weapon[NUM_WEAPONS];
    public GameObject grenadePrefab;

    // weapon actions
    [SerializeField] private InputActionReference reload;
    [SerializeField] private InputActionReference fire;


    void Start()
    {
        weapons[0] = new Weapon("Pistol", 4.5f, 0.22f, float.PositiveInfinity, float.PositiveInfinity, 0f, selectPistol, 0f, 0f, 30f, 1);
        weapons[1] = new Weapon("Shotgun", 12f, 0.9f, 5f, 5f, 1.7f, selectShotgun, 12f, 6f, 30f, 7);
        weapons[2] = new Weapon("Rifle", 2.2f, 0.13f, 18f, 18f, 1.9f, selectRifle, 1.5f, 1f, 30f, 1);
        weapons[3] = new Weapon("Sniper", 15f, 1.5f, 3f, 3f, 2.5f, selectSniper, 0f, 0f, 100f, 1);
        weapons[4] = new Weapon("Grenade", 4f, 0.7f, 4f, 4f, 2.2f, selectGrenade, 0f, 0f, 30f, 1);

        rb = GetComponent<Rigidbody2D>();
        mainCamera = Camera.main;

        for(int i = 0; i < NUM_WEAPONS; i++)
        {
            weapons[i].select.action.Enable();
        }
        reload.action.Enable();
        fire.action.Enable();
    }

    void Update()
    {
        if (Mouse.current == null || Keyboard.current == null)
            return;

        if (mainCamera == null)
            return;

        // Switch weapons
        if (selectPistol.action.WasPressedThisFrame())
            SelectedWeapon = 0;

        if (selectShotgun.action.WasPressedThisFrame())
            SelectedWeapon = 1;

        if (selectRifle.action.WasPressedThisFrame())
            SelectedWeapon = 2;

        if (selectSniper.action.WasPressedThisFrame())
            SelectedWeapon = 3;

        if (selectGrenade.action.WasPressedThisFrame())
            SelectedWeapon = 4;

        // Aim toward mouse
        Vector3 mousePos = Mouse.current.position.ReadValue();

        mousePos.z = Mathf.Abs(
            mainCamera.transform.position.z - transform.position.z
        );

        Vector3 mouseWorld = mainCamera.ScreenToWorldPoint(mousePos);
        Vector2 direction = mouseWorld - transform.position;

        if (direction.magnitude > 0.01f)
            AimDirection = direction.normalized;

        // Reload
        if (reload.action.WasPressedThisFrame())
            Reload();

        CheckReloads();

        // Rifle is automatic, other weapons are single click
        if (SelectedWeapon == 2)
        {
            if (fire.action.IsPressed())
                Shoot();
        }
        else
        {
            if (fire.action.WasPressedThisFrame())
                Shoot();
        }

        Debug.DrawRay(transform.position, AimDirection * 2f, Color.cyan);
    }

    void FixedUpdate()
    {
        // Apply recoil to the player
        if (recoilForce != Vector2.zero)
        {
            rb.AddForce(recoilForce, ForceMode2D.Impulse);
            recoilForce = Vector2.zero;
        }
    }

    void Shoot()
    {
        // Check cooldown and reload
        if (Time.time < weapons[SelectedWeapon].nextShot)
            return;

        if (weapons[SelectedWeapon].reloadEnd > 0)
            return;

        if (weapons[SelectedWeapon].ammo <= 0)
        {
            Reload();
            return;
        }
        weapons[SelectedWeapon].ammo--;


        // Grenade
        if (SelectedWeapon == 4)
        {
            if (grenadePrefab == null)
            {
                Debug.LogWarning("Grenade Prefab is missing!");
                return;
            }

            if (grenadePrefab.GetComponent<GrenadeProjectile>() == null)
            {
                Debug.LogWarning("Add GrenadeProjectile to the prefab!");
                return;
            }
        }

        // Set the cooldown
        weapons[SelectedWeapon].nextShot = Time.time + weapons[SelectedWeapon].cooldown;

        // Recoil pushes the player away from the shot
        float recoilAngle = Random.Range(-weapons[SelectedWeapon].recoilSpread, weapons[SelectedWeapon].recoilSpread);

        Vector2 pushDirection =
            Quaternion.Euler(0, 0, recoilAngle) * AimDirection;

        recoilForce -= pushDirection * weapons[SelectedWeapon].recoil;

        // Grenades are physical projectiles
        if (SelectedWeapon == 4)
        {
            Vector2 spawnPosition = rb.position + AimDirection * 0.9f;

            GameObject grenade = Instantiate(
                grenadePrefab,
                spawnPosition,
                Quaternion.identity
            );

            GrenadeProjectile grenadeScript =
                grenade.GetComponent<GrenadeProjectile>();

            grenadeScript.Launch(
                AimDirection,
                GetComponent<Collider2D>()
            );

            return;
        }

        // Other weapons shoot instantly using raycasts
        Vector2 muzzle = rb.position + AimDirection * 0.7f;

        for (int i = 0; i < weapons[SelectedWeapon].bullets; i++)
        {
            float angle = Random.Range(-weapons[SelectedWeapon].spread, weapons[SelectedWeapon].spread);

            Vector2 bulletDirection =
                Quaternion.Euler(0, 0, angle) * AimDirection;

            RaycastHit2D hit = Physics2D.Raycast(
                muzzle,
                bulletDirection,
                weapons[SelectedWeapon].shotRange
            );

            Vector2 endPoint =
                muzzle + bulletDirection * weapons[SelectedWeapon].shotRange;

            if (hit.collider != null)
                endPoint = hit.point;

            Debug.DrawLine(muzzle, endPoint, Color.yellow, 0.15f);

            // Damage can be added later
        }
    }

    void Reload()
    {
        if(weapons[SelectedWeapon].ammo < weapons[SelectedWeapon].magSize && weapons[SelectedWeapon].reloadEnd == 0)
        {
            weapons[SelectedWeapon].reloadEnd = Time.time + weapons[SelectedWeapon].reloadTime;
        }
    }

    void CheckReloads()
    {
        if (weapons[SelectedWeapon].reloadEnd > 0 && Time.time >= weapons[SelectedWeapon].reloadEnd)
        {
            weapons[SelectedWeapon].ammo = weapons[SelectedWeapon].magSize;
            weapons[SelectedWeapon].reloadEnd = 0;
        }
    }

    void OnGUI()
    {
        string ammoText = "∞";

        if (SelectedWeapon > 0)
        {
            ammoText = weapons[SelectedWeapon].ammo + " / " + weapons[SelectedWeapon].magSize;
        }

        if (weapons[SelectedWeapon].reloadEnd > 0)
            ammoText = "Reloading...";

        float cooldownLeft = weapons[SelectedWeapon].nextShot - Time.time;

        if (cooldownLeft < 0)
            cooldownLeft = 0;

        GUI.Box(
            new Rect(10, 110, 300, 100),
            weapons[SelectedWeapon].name +
            "\nAmmo: " + ammoText +
            "\nCooldown: " + cooldownLeft.ToString("F1") + "s" +
            "\n1-5: Weapons | R: Reload"
        );
    }
}
