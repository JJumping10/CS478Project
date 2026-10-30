using UnityEngine;

public class Health_Script : MonoBehaviour
{
    public float Health, MaxHealth;

    [SerializeField]
    private Health_Bar_UI healthBarUI;

    private Vector3 startPosition;
    private Quaternion startRotation;

    void Start()
    {
        startPosition = transform.position;
        startRotation = transform.rotation;

        Health = MaxHealth;
        healthBarUI.Set_Max_Health(MaxHealth);
        healthBarUI.Set_Health(Health);
    }

    void Update()
    {
        if (Input.GetKeyDown("i"))
        {
            Set_Health(-20f);
        }
        if (Input.GetKeyDown("u"))
        {
            Set_Health(20f);
        }
    }

    // The saw calls this
    public void TakeDamage(float amount)
    {
        Set_Health(-amount);
    }

    private void Set_Health(float Health_Change)
    {
        Health += Health_Change;
        Health = Mathf.Clamp(Health, 0, MaxHealth);
        healthBarUI.Set_Health(Health);

        if (Health <= 0)
        {
            Respawn();
        }
    }

    private void Respawn()
    {
        transform.position = startPosition;
        transform.rotation = startRotation;

        Rigidbody2D rb = GetComponent<Rigidbody2D>();
        if (rb != null)
        {
            rb.linearVelocity = Vector2.zero;
            rb.angularVelocity = 0f;
        }

        Health = MaxHealth;
        healthBarUI.Set_Health(Health);
    }
}