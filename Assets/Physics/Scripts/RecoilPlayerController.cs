


using UnityEngine;
using UnityEngine.InputSystem;

public class RecoilPlayerController : MonoBehaviour
{
    // Player and aiming
    private Rigidbody2D rb;
    private Camera mainCamera;

    public int SelectedWeapon = 0;
    public Vector2 AimDirection = Vector2.right;

    private Vector2 recoilForce;

    // Pistol
    public float pistolRecoil = 4.5f;
    public float pistolCooldown = 0.22f;

    // Shotgun
    public float shotgunRecoil = 12f;
    public float shotgunCooldown = 0.9f;
    public int shotgunMagSize = 5;
    public float shotgunReloadTime = 1.7f;

    // Rifle
    public float rifleRecoil = 2.2f;
    public float rifleCooldown = 0.13f;
    public int rifleMagSize = 18;
    public float rifleReloadTime = 1.9f;

    // Sniper
    public float sniperRecoil = 15f;
    public float sniperCooldown = 1.5f;
    public int sniperMagSize = 3;
    public float sniperReloadTime = 2.5f;

    // Grenade launcher
    public float grenadeRecoil = 4f;
    public float grenadeCooldown = 0.7f;
    public int grenadeMagSize = 4;
    public float grenadeReloadTime = 2.2f;
    public GameObject grenadePrefab;

    // Ammunition
    private int shotgunAmmo;
    private int rifleAmmo;
    private int sniperAmmo;
    private int grenadeAmmo;

    // Cooldown and reload timers for all five weapons
    private float[] nextShot = new float[5];
    private float[] reloadEnd = new float[5];

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        mainCamera = Camera.main;

        shotgunAmmo = shotgunMagSize;
        rifleAmmo = rifleMagSize;
        sniperAmmo = sniperMagSize;
        grenadeAmmo = grenadeMagSize;
    }

    void Update()
    {
        if (Mouse.current == null || Keyboard.current == null)
            return;

        if (mainCamera == null)
            return;

        // Switch weapons
        if (Keyboard.current.digit1Key.wasPressedThisFrame)
            SelectedWeapon = 0;

        if (Keyboard.current.digit2Key.wasPressedThisFrame)
            SelectedWeapon = 1;

        if (Keyboard.current.digit3Key.wasPressedThisFrame)
            SelectedWeapon = 2;

        if (Keyboard.current.digit4Key.wasPressedThisFrame)
            SelectedWeapon = 3;

        if (Keyboard.current.digit5Key.wasPressedThisFrame)
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
        if (Keyboard.current.rKey.wasPressedThisFrame)
            Reload();

        CheckReloads();

        // Rifle is automatic, other weapons are single click
        if (SelectedWeapon == 2)
        {
            if (Mouse.current.leftButton.isPressed)
                Shoot();
        }
        else
        {
            if (Mouse.current.leftButton.wasPressedThisFrame)
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
        if (Time.time < nextShot[SelectedWeapon])
            return;

        if (reloadEnd[SelectedWeapon] > 0)
            return;

        float power = 0f;
        float cooldown = 0f;
        float spread = 0f;
        float recoilSpread = 0f;
        float shotRange = 30f;
        int bullets = 1;

        // Pistol
        if (SelectedWeapon == 0)
        {
            power = pistolRecoil;
            cooldown = pistolCooldown;
        }

        // Shotgun
        if (SelectedWeapon == 1)
        {
            if (shotgunAmmo <= 0)
            {
                Reload();
                return;
            }

            shotgunAmmo--;

            power = shotgunRecoil;
            cooldown = shotgunCooldown;
            bullets = 7;
            spread = 12f;
            recoilSpread = 6f;
        }

        // Rifle
        if (SelectedWeapon == 2)
        {
            if (rifleAmmo <= 0)
            {
                Reload();
                return;
            }

            rifleAmmo--;

            power = rifleRecoil;
            cooldown = rifleCooldown;
            spread = 1.5f;
            recoilSpread = 1f;
        }

        // Sniper
        if (SelectedWeapon == 3)
        {
            if (sniperAmmo <= 0)
            {
                Reload();
                return;
            }

            sniperAmmo--;

            power = sniperRecoil;
            cooldown = sniperCooldown;
            shotRange = 100f;
            spread = 0f;
            recoilSpread = 0f;
        }

        // Grenade
        if (SelectedWeapon == 4)
        {
            if (grenadeAmmo <= 0)
            {
                Reload();
                return;
            }

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

            grenadeAmmo--;

            power = grenadeRecoil;
            cooldown = grenadeCooldown;
        }

        // Set the cooldown
        nextShot[SelectedWeapon] = Time.time + cooldown;

        // Recoil pushes the player away from the shot
        float recoilAngle = Random.Range(-recoilSpread, recoilSpread);

        Vector2 pushDirection =
            Quaternion.Euler(0, 0, recoilAngle) * AimDirection;

        recoilForce -= pushDirection * power;

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

        for (int i = 0; i < bullets; i++)
        {
            float angle = Random.Range(-spread, spread);

            Vector2 bulletDirection =
                Quaternion.Euler(0, 0, angle) * AimDirection;

            RaycastHit2D hit = Physics2D.Raycast(
                muzzle,
                bulletDirection,
                shotRange
            );

            Vector2 endPoint =
                muzzle + bulletDirection * shotRange;

            if (hit.collider != null)
                endPoint = hit.point;

            Debug.DrawLine(muzzle, endPoint, Color.yellow, 0.15f);

            // Damage can be added later
        }
    }

    void Reload()
    {
        // Shotgun
        if (SelectedWeapon == 1)
        {
            if (shotgunAmmo < shotgunMagSize && reloadEnd[1] == 0)
                reloadEnd[1] = Time.time + shotgunReloadTime;
        }

        // Rifle
        if (SelectedWeapon == 2)
        {
            if (rifleAmmo < rifleMagSize && reloadEnd[2] == 0)
                reloadEnd[2] = Time.time + rifleReloadTime;
        }

        // Sniper
        if (SelectedWeapon == 3)
        {
            if (sniperAmmo < sniperMagSize && reloadEnd[3] == 0)
                reloadEnd[3] = Time.time + sniperReloadTime;
        }

        // Grenade
        if (SelectedWeapon == 4)
        {
            if (grenadeAmmo < grenadeMagSize && reloadEnd[4] == 0)
                reloadEnd[4] = Time.time + grenadeReloadTime;
        }
    }

    void CheckReloads()
    {
        if (reloadEnd[1] > 0 && Time.time >= reloadEnd[1])
        {
            shotgunAmmo = shotgunMagSize;
            reloadEnd[1] = 0;
        }

        if (reloadEnd[2] > 0 && Time.time >= reloadEnd[2])
        {
            rifleAmmo = rifleMagSize;
            reloadEnd[2] = 0;
        }

        if (reloadEnd[3] > 0 && Time.time >= reloadEnd[3])
        {
            sniperAmmo = sniperMagSize;
            reloadEnd[3] = 0;
        }

        if (reloadEnd[4] > 0 && Time.time >= reloadEnd[4])
        {
            grenadeAmmo = grenadeMagSize;
            reloadEnd[4] = 0;
        }
    }

    void OnGUI()
    {
        string weaponName = "Pistol";
        string ammoText = "Unlimited";

        if (SelectedWeapon == 1)
        {
            weaponName = "Shotgun";
            ammoText = shotgunAmmo + " / " + shotgunMagSize;
        }

        if (SelectedWeapon == 2)
        {
            weaponName = "Rifle";
            ammoText = rifleAmmo + " / " + rifleMagSize;
        }

        if (SelectedWeapon == 3)
        {
            weaponName = "Sniper";
            ammoText = sniperAmmo + " / " + sniperMagSize;
        }

        if (SelectedWeapon == 4)
        {
            weaponName = "Grenade";
            ammoText = grenadeAmmo + " / " + grenadeMagSize;
        }

        if (reloadEnd[SelectedWeapon] > 0)
            ammoText = "Reloading...";

        float cooldownLeft = nextShot[SelectedWeapon] - Time.time;

        if (cooldownLeft < 0)
            cooldownLeft = 0;

        GUI.Box(
            new Rect(10, 10, 300, 100),
            weaponName +
            "\nAmmo: " + ammoText +
            "\nCooldown: " + cooldownLeft.ToString("F1") + "s" +
            "\n1-5: Weapons | R: Reload"
        );
    }
}
