using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    public PlayerUI playerUI;

    [Header("Health")]
    public int maxHealth = 100;
    private int currentHealth;
    private void Awake()
    {
        currentHealth = maxHealth;
    }

    /// <summary>
    /// Call this when the player takes damage
    /// </summary>
    /// <param name="amount">Amount of damage</param>
    public void TakeDamage(int amount)
    {
        currentHealth -= amount;
        currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth);

        if (currentHealth < 0) currentHealth = 0;
        
        playerUI?.SetHP(currentHealth);
        playerUI?.FlashDamage();
        
        if (currentHealth == 0)
        {
            Die();
        }
    }

    /// <summary>
    /// Handle player death
    /// </summary>
    private void Die()
    {
        Debug.Log("Player Died!");
        // Disable player input or play death animation
        // Example: disable the game object
        gameObject.SetActive(false);
    }

    /// <summary>
    /// Heal the player
    /// </summary>
    public void Heal(int amount)
    {
        currentHealth += amount;
        currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth);
        playerUI?.SetHP(currentHealth);
    }

    /// <summary>
    /// Optional: expose current health
    /// </summary>
    public int GetCurrentHealth()
    {
        return currentHealth;
    }
}
