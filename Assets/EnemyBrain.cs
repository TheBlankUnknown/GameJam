using UnityEngine;
using System.Collections.Generic;

public class Enemy : MonoBehaviour
{
    [Header("Stats")]
    public float speed = 3f;
    public int maxHealth = 50;
    public int damage = 10;

    private int currentHealth;

    private Transform player;

    [Header("Damage Settings")]
    public float attackRange = 1.5f;
    public float attackCooldown = 1f;

    private float lastAttackTime;

    // Track bullets that already damaged this enemy
    private HashSet<Projectile> bulletsThatHitMe = new HashSet<Projectile>();

    // Money reward
    public int moneyReward = 50;

    private PlayerUI playerUI;

    private void Start()
    {
        currentHealth = maxHealth;

        player = GameObject.FindWithTag("Player").transform;
        playerUI = FindObjectOfType<PlayerUI>();
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

        // Give money to player
        if (playerUI != null)
        {
            Debug.Log($"Gave Money!");
                    
            playerUI.AddMoney(moneyReward);
        }

        Destroy(gameObject);
    }
}
