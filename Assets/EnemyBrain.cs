using UnityEngine;
using System.Collections.Generic;

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
    public float attackRange = 1.5f;
    public float attackCooldown = 1f;

    private float lastAttackTime;

    // Track bullets that already damaged this enemy
    private HashSet<Projectile> bulletsThatHitMe = new HashSet<Projectile>();

    private void Start()
    {
        currentHealth = maxHealth;

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
        Vector3 direction = (player.position - transform.position).normalized;
        direction.y = 0f;

        transform.position += direction * speed * Time.deltaTime;

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
            PlayerHealth playerHealth = player.GetComponent<PlayerHealth>();
            if (playerHealth != null)
            {
                playerHealth.TakeDamage(damage);
            }

            lastAttackTime = Time.time;
        }
    }

    // Called by the bullet
    public void TakeDamage(int amount)
    {
        currentHealth -= amount;
        Debug.Log($"{name} took {amount} damage. HP: {currentHealth}");

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    private void Die()
    {
        Debug.Log($"{name} died!");
        Destroy(gameObject);
    }
}
