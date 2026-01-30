using UnityEngine;

[RequireComponent(typeof(Rigidbody), typeof(Collider))]
public class Projectile : MonoBehaviour
{
    public float speed = 20f;
    public int damage = 25;
    public float lifeTime = 5f;

    private Rigidbody rb;
    private bool hasDealtDamage = false;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        rb.useGravity = true; // turn ON if you want bullet drop
        rb.isKinematic = false;
        rb.collisionDetectionMode = CollisionDetectionMode.Continuous;
    }

    private void Start()
    {
        Destroy(gameObject, lifeTime);
    }

    public void Launch(Vector3 direction, Vector3 shooterVelocity)
    {
        rb.linearVelocity = direction.normalized * speed + shooterVelocity;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (hasDealtDamage) return;

        Enemy enemy = collision.collider.GetComponent<Enemy>();
        if (enemy != null)
        {
            enemy.TakeDamage(damage);
            hasDealtDamage = true;
        }

        // DO NOT destroy the bullet
        // Physics continues normally
    }
}
