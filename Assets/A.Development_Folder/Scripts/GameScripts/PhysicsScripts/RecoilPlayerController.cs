using UnityEngine;
using UnityEngine.InputSystem;

public class RecoilPlayerController : MonoBehaviour
{
    // Player and aiming
    private Rigidbody2D rb;
    private Camera mainCamera;
    public ItemManager itemManager;
    public GameManager gameManager;

    public ItemManager.Weapon currentWeapon;
    public Vector2 AimDirection = Vector2.right;

    private Vector2 recoilForce;

    // create weapons
    public GameObject grenadePrefab;

    // weapon actions
    [SerializeField] private InputActionReference selectPistol;
    [SerializeField] private InputActionReference selectShotgun;
    [SerializeField] private InputActionReference selectRifle;
    [SerializeField] private InputActionReference selectSniper;
    [SerializeField] private InputActionReference selectGrenade;
    [SerializeField] private InputActionReference reload;
    [SerializeField] private InputActionReference fire;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        mainCamera = Camera.main;
        currentWeapon = ItemManager.GetCurrentWeapon(0);

        reload.action.Enable();
        fire.action.Enable();
    }

    void Update()
    {
        if (gameManager.isPaused == false)
        {
            if (Mouse.current == null || Keyboard.current == null)
                return;

            if (mainCamera == null)
                return;

            // Switch weapons
            if (selectPistol.action.WasPressedThisFrame())
            {
                currentWeapon = ItemManager.GetCurrentWeapon(0);
            }
            if (selectShotgun.action.WasPressedThisFrame())
            {
                currentWeapon = ItemManager.GetCurrentWeapon(1);
            }

            if (selectRifle.action.WasPressedThisFrame())
            {
                currentWeapon = ItemManager.GetCurrentWeapon(2);
            }

            if (selectSniper.action.WasPressedThisFrame())
            {
                currentWeapon = ItemManager.GetCurrentWeapon(3);
            }

            if (selectGrenade.action.WasPressedThisFrame())
            {
                currentWeapon = ItemManager.GetCurrentWeapon(4);
            }

            // Aim toward mouse
            Vector3 mousePos = Mouse.current.position.ReadValue();

            mousePos.z = Mathf.Abs(
                mainCamera.transform.position.z - transform.position.z
            );

            Vector3 mouseWorld = mainCamera.ScreenToWorldPoint(mousePos);
            Vector2 direction = mouseWorld - transform.position;

            if (direction.magnitude > 0.01f)
                AimDirection = direction.normalized;

            // cooldown
            currentWeapon.DisplayCooldown();

            // Reload
            if (reload.action.WasPressedThisFrame())
                Reload();

            CheckReloads();

            // Rifle is automatic, other weapons are single click
            if (currentWeapon.name == "Rifle")
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
        if (Time.time < currentWeapon.nextShot)
            return;

        if (currentWeapon.reloadEnd > 0)
            return;

        if (currentWeapon.ammo <= 0)
        {
            Reload();
            return;
        }
        currentWeapon.ammo--;
        currentWeapon.UpdateAmmoDisplay();


        // Grenade
        if (currentWeapon.name == "Grenade")
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
        currentWeapon.nextShot = Time.time + currentWeapon.cooldown;

        // Recoil pushes the player away from the shot
        float recoilAngle = Random.Range(-currentWeapon.recoilSpread, currentWeapon.recoilSpread);

        Vector2 pushDirection =
            Quaternion.Euler(0, 0, recoilAngle) * AimDirection;

        recoilForce -= pushDirection * currentWeapon.recoil;

        // Grenades are physical projectiles
        if (currentWeapon.name == "Grenade")
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

        for (int i = 0; i < currentWeapon.bullets; i++)
        {
            float angle = Random.Range(-currentWeapon.spread, currentWeapon.spread);

            Vector2 bulletDirection =
                Quaternion.Euler(0, 0, angle) * AimDirection;

            RaycastHit2D hit = Physics2D.Raycast(
                muzzle,
                bulletDirection,
                currentWeapon.shotRange
            );

            Vector2 endPoint =
                muzzle + bulletDirection * currentWeapon.shotRange;

            if (hit.collider != null)
                endPoint = hit.point;

            Debug.DrawLine(muzzle, endPoint, Color.yellow, 0.15f);

            // Damage can be added later
        }
    }

    void Reload()
    {
        if(currentWeapon.ammo < currentWeapon.magSize && currentWeapon.reloadEnd == 0)
        {
            currentWeapon.reloadEnd = Time.time + currentWeapon.reloadTime;
            currentWeapon.WeaponKeyInactive();
            currentWeapon.DisplayReload();
        }
    }

    void CheckReloads()
    {
        if (currentWeapon.reloadEnd > 0)
        {
            currentWeapon.DisplayReload();
            if (Time.time >= currentWeapon.reloadEnd)
            {
                currentWeapon.ammo = currentWeapon.magSize;
                currentWeapon.reloadEnd = 0;
                currentWeapon.WeaponKeyActive();
                currentWeapon.UpdateAmmoDisplay();
            }
        }
    }

    public InputActionReference GetInput(int i)
    {
        return currentWeapon.select;
    }

    public WeaponTile GetTile(int i)
    {
        return currentWeapon.tile;
    }


    void OnGUI()
    {
        string ammoText = "∞";

        if (currentWeapon.ammo != float.PositiveInfinity)
        {
            ammoText = currentWeapon.ammo + " / " + currentWeapon.magSize;
        }

        if (currentWeapon.reloadEnd > 0)
            ammoText = "Reloading...";

        float cooldownLeft = currentWeapon.nextShot - Time.time;

        if (cooldownLeft < 0)
            cooldownLeft = 0;

        GUI.Box(
            new Rect(10, 110, 300, 100),
            currentWeapon.name +
            "\nAmmo: " + ammoText +
            "\nCooldown: " + cooldownLeft.ToString("F1") + "s" +
            "\n1-5: Weapons | R: Reload"
        );
    }
}
