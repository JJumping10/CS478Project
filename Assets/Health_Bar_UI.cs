using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Health_Bar_UI : MonoBehaviour
{
    public float Health, MaxHealth, Width, Height;
    [SerializeField]
    private RectTransform healthBar;

    public void Set_Max_Health(float maxHealth)
    {
        MaxHealth = maxHealth;
    }

    public void Set_Health(float health)
    {
        Health = health;
        float newWidth = (Health / MaxHealth) * Width;
        healthBar.sizeDelta = new Vector2(newWidth, Height);
    }
}
