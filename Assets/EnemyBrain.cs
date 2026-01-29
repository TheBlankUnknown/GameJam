using UnityEngine;

public class Enemy : MonoBehaviour
{
    [Header("Stats")]
    public float speed = 3f;
    public int maxHealth = 50;
    public int damage = 10;

    private int currentHealth;

    [Header("Target")]
    public Transform player;

    [Header("Damage Settings")]
    public float attackRange = 1.5f; // distance to deal damage
    public float attackCooldown = 1f;

    private float lastAttackTime;

    private void Start()
    {
        currentHealth = maxHealth;

        // Auto-find the player if not assigned
        if (player == null && GameObject.FindWithTag("Player") != null)
        {
            player = GameObject.FindWithTag("Player").transform;
        }
    }

    private void Update()
    {
        if (player == null) return;

        FollowPlayer();
        TryAttack();
    }

    private void FollowPlayer()
    {
        // Move towards the player
        Vector3 direction = (player.position - transform.position).normalized;
        direction.y = 0f; // keep on the same height
        transform.position += direction * speed * Time.deltaTime;

        // Optional: face the player
        if (direction != Vector3.zero)
        {
            transform.rotation = Quaternion.LookRotation(direction);
        }
    }

    private void TryAttack()
    {
        if (Time.time < lastAttackTime + attackCooldown) return;

        float distance = Vector3.Distance(transform.position, player.position);
        if (distance <= attackRange)
        {
            // Deal damage
            PlayerHealth playerHealth = player.GetComponent<PlayerHealth>();
            if (playerHealth != null)
            {
                playerHealth.TakeDamage(damage);
            }

            lastAttackTime = Time.time;
        }
    }

    public void TakeDamage(int amount)
    {
        currentHealth -= amount;
        if (currentHealth <= 0)
        {
            Die();
        }
    }

    private void Die()
    {
        Destroy(gameObject); // simple death
    }
}
