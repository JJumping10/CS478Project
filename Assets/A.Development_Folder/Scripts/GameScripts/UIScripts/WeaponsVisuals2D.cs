

using UnityEngine;

public class WeaponVisuals2D : MonoBehaviour
{
    // Weapon objects
    public RecoilPlayerController controller;
    public Transform weaponPivot;
    public GameObject[] weaponBlocks;

    // Aiming line
    public LineRenderer aimingLine;
    public float aimLineLength = 6f;

    void Start()
    {
        // Get the player controller
        if (controller == null)
            controller = GetComponent<RecoilPlayerController>();

        // Set up the aiming line
        aimingLine.positionCount = 2;
        aimingLine.useWorldSpace = true;
    }

    void LateUpdate()
    {
        // Get the selected weapon
        int selectedWeapon = ItemManager.SelectedWeapon;

        // Get the direction the player is aiming
        Vector2 aimDirection = controller.AimDirection;

        // Rotate the weapon toward the mouse
        float angle = Mathf.Atan2(aimDirection.y, aimDirection.x);
        angle = angle * Mathf.Rad2Deg;

        weaponPivot.rotation = Quaternion.Euler(0, 0, angle);

        // Show only the selected weapon
        for (int i = 0; i < weaponBlocks.Length; i++)
        {
            if (i == selectedWeapon)
            {
                weaponBlocks[i].SetActive(true);
            }
            else
            {
                weaponBlocks[i].SetActive(false);
            }
        }

        // Get the current weapon
        GameObject currentWeapon = weaponBlocks[selectedWeapon];

        // Find the end of the weapon
        Vector3 startPoint = currentWeapon.transform.TransformPoint(
            new Vector3(0.5f, 0, 0)
        );

        // Find where the aiming line ends
        Vector3 endPoint = startPoint + (Vector3)aimDirection * aimLineLength;

        // Draw the aiming line
        aimingLine.SetPosition(0, startPoint);
        aimingLine.SetPosition(1, endPoint);
    }
}
