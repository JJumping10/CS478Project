
using UnityEngine;

public class GrenadeProjectile : MonoBehaviour
{
    // Grenade settings
    public float speed = 14f;
    public float explosionRadius = 3.5f;
    public float explosionForce = 32f;

    // Optional explosion effect
    public GameObject explosionEffect;

    private Rigidbody2D rb;
    private bool exploded = false;

    // Called when the player shoots a grenade
    public void Launch(Vector2 direction, Collider2D playerCollider)
    {
        rb = GetComponent<Rigidbody2D>();

        // Stop the grenade from hitting the player directly
        if (playerCollider != null)
        {
            Physics2D.IgnoreCollision(
                GetComponent<Collider2D>(),
                playerCollider
            );
        }

        // Send the grenade toward the mouse
        rb.linearVelocity = direction * speed;

        // Remove grenades that never hit anything
        Destroy(gameObject, 8f);
    }

    // Explode when the grenade hits something
    void OnCollisionEnter2D(Collision2D collision)
    {
        if (exploded == true)
            return;

        exploded = true;

        // Find everything near the explosion
        Collider2D[] nearbyObjects = Physics2D.OverlapCircleAll(
            transform.position,
            explosionRadius,
            ~0
        );

        // Push nearby physics objects away
        for (int i = 0; i < nearbyObjects.Length; i++)
        {
            Rigidbody2D target =
                nearbyObjects[i].attachedRigidbody;

            if (target != null && target != rb)
            {
                Vector2 pushDirection =
                    target.position - (Vector2)transform.position;

                float distance = pushDirection.magnitude;

                if (distance < 0.01f)
                    pushDirection = Vector2.up;
                else
                    pushDirection = pushDirection.normalized;

                // Closer objects get more knockback
                float power = explosionForce *
                    (1f - distance / explosionRadius);

                target.AddForce(
                    pushDirection * power,
                    ForceMode2D.Impulse
                );
            }
        }

        // Create explosion effect if one is assigned
        if (explosionEffect != null)
        {
            GameObject effect = Instantiate(
                explosionEffect,
                transform.position,
                Quaternion.identity
            );

            Destroy(effect, 0.2f);
        }

        // Remove the grenade
        Destroy(gameObject);
    }

    // Show the explosion radius in the Scene view
    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, explosionRadius);
    }
}
