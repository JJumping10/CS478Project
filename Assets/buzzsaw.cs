using UnityEngine;

public class BuzzSaw : MonoBehaviour
{
    public float spinSpeed = 720f;
    public float damage = 20f;
    public float knockbackSpeed = 12f;
    public float upwardBoost = 0.4f;
    public float hitCooldown = 0.75f;

    private float nextHitTime = 0f;

    void Update()
    {
        transform.Rotate(0f, 0f, -spinSpeed * Time.deltaTime);
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        TryHit(other);
    }

    void OnTriggerStay2D(Collider2D other)
    {
        TryHit(other);
    }

    void TryHit(Collider2D other)
    {
        Debug.Log("Saw touched: " + other.name);

        if (Time.time < nextHitTime)
            return;

        Health_Script health = other.GetComponentInParent<Health_Script>();
        if (health == null)
        {
            Debug.Log("No Health_Script found on " + other.name + " or its parents");
            return;
        }

        nextHitTime = Time.time + hitCooldown;
        health.TakeDamage(damage);
        Debug.Log("Saw dealt " + damage + " damage");

        Rigidbody2D playerRb = other.attachedRigidbody;
        if (playerRb != null)
        {
            Vector2 away = (playerRb.position - (Vector2)transform.position).normalized;
            Vector2 knockback = (away + Vector2.up * upwardBoost).normalized;
            playerRb.linearVelocity = knockback * knockbackSpeed;
        }
    }
}