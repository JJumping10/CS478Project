using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.InputSystem;

public class ItemManager : MonoBehaviour
{

    public Key pistolKey = Key.Digit1;
    public Key shotgunKey = Key.Digit2;
    public Key rifleKey = Key.Digit3;
    public Key sniperKey = Key.Digit4;
    public Key grenadeKey = Key.Digit5;
    public Key reloadKey = Key.R;

    [System.Serializable]
    public class ItemUI
    {
        public GameObject reloading;
        public GameObject unselected;
        public GameObject unavailable;
        public TMP_Text keybind;
        public TMP_Text ammoQuantity;

    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
