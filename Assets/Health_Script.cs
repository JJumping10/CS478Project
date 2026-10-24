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
        // Remember where the player began the level
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

        Rigidbody rb = GetComponent<Rigidbody>();

        if (rb != null)
        {
            rb.linearVelocity = Vector3.zero;
        }

        Health = MaxHealth;
        healthBarUI.Set_Health(Health);
    }
}