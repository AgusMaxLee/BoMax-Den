using UnityEngine;
using UnityEngine.Events;

public class EnemyHealth : MonoBehaviour
{
    [SerializeField] private int currentHealth, maxHealth = 100;
    public UnityEvent<int> onTakeDamage;

    [SerializeField] private FloatingHealthBar healthBar;
    private EnemyAnimation enemyAnimation;

    [SerializeField] private AudioClip deathSound;


    private void Awake()
    {
        healthBar = GetComponentInChildren<FloatingHealthBar>();
        enemyAnimation = GetComponent<EnemyAnimation>();
    }

    public void Start()
    {
        currentHealth = maxHealth;
        healthBar.UpdateHealthBar(currentHealth, maxHealth);
    }

    public void TakeDamage(int amount)
    {
        if (currentHealth > 0) // Remove the condition for hurt cooldown    
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
                enemyAnimation.StartHurtAnimation();  // Trigger the hurt animation
            }
        }
    }

    private void Die()
    {
        AudioManager.Instance.PlaySound(deathSound);
        Destroy(gameObject);
    }
}
