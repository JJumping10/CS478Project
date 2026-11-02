using UnityEngine;
using TMPro;
using UnityEngine.Tilemaps;
using UnityEngine.UI;

public class WeaponTile : MonoBehaviour
{

    public Image reloading;
    public GameObject unselected;
    public GameObject unavailable;
    public TMP_Text keybind;
    public TMP_Text ammoQuantity;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void MakeAvailable()
    {
        unavailable.SetActive(false);
    }
    public void MakeUnavailable()
    {
        unavailable.SetActive(true);
    }

    public void SelectWeapon()
    {
        unselected.SetActive(false);
    }

    public void UnselectWeapon()
    {
        unselected.SetActive(true);
    }

    public void Reload(float percentRemaining)
    {
        reloading.fillAmount = percentRemaining / 100f;
    }

    public void WeaponKeyInactive()
    {
        keybind.alpha = 0.3f;
    }

    public void WeaponKeyActive()
    {
        keybind.alpha = 1f;
    }


    public void UpdateAmmo(string ammoText)
    {
        ammoQuantity.text = ammoText;
    }

}
