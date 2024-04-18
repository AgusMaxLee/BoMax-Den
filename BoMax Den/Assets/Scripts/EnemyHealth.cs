using UnityEngine;
using UnityEngine.Events;

public class EnemyHealth : MonoBehaviour
{
    [SerializeField] private int currentHealth, maxHealth = 100;
    public UnityEvent<int> onTakeDamage;

    [SerializeField] private FloatingHealthBar healthBar;
    private Animator animator;
    private bool isHurtCooldown = false;
    public float hurtCooldownTime = 1f; // Cooldown time for the "hurt" animation in seconds

    private void Awake()
    {
        healthBar = GetComponentInChildren<FloatingHealthBar>();
        animator = GetComponent<Animator>();
    }

    public void Start()
    {
        currentHealth = maxHealth;
        healthBar.UpdateHealthBar(currentHealth, maxHealth);
    }

    public void TakeDamage(int amount)
    {
        if (currentHealth > 0 && !isHurtCooldown) // Check if not in cooldown
        {
            currentHealth -= amount;
            onTakeDamage?.Invoke(amount);
            healthBar.UpdateHealthBar(currentHealth, maxHealth);
            Debug.Log("Enemy took damage. Current Health: " + currentHealth);

            if (currentHealth <= 0)
            {
                Die();
            }
            else
            {
                // Set the "IsHurt" parameter to true for the duration of the "hurt" animation
                animator.SetBool("isHurt", true);
                isHurtCooldown = true; // Start the cooldown
                Invoke(nameof(ResetHurtCooldown), hurtCooldownTime); // Reset the cooldown after the specified time
            }
        }
    }

    private void ResetHurtCooldown()
    {
        animator.SetBool("isHurt", false);
        isHurtCooldown = false; // Reset the cooldown flag
    }

    private void Die()
    {
        // Additional actions when the enemy dies
        Destroy(gameObject);
    }
}
