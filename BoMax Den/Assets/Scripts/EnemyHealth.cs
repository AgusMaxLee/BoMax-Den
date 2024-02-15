using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class EnemyHealth : MonoBehaviour
{
    [SerializeField] private Animator animator;
    [SerializeField] private int currentHealth, maxHealth = 100;
    public UnityEvent<int> onTakeDamage;
    

    // Time duration for the "Hit" animation state
    [SerializeField] private float hitAnimationDuration = 1f;
    private bool isTakingDamage = false;

    // Time to wait before resetting animation and health after death
    [SerializeField] private float resetDelay = 15f;
    private float deathTimer = 0f;
    private bool isDead = false;

    [SerializeField] FloatingHealthBar healthBar;

    private void Awake()
    {
        healthBar = GetComponentInChildren<FloatingHealthBar>();
    }
    public void Start()
    {
        currentHealth = maxHealth;
        healthBar.UpdateHealthBar(currentHealth, maxHealth);
    }
    void Update()
    {
        // Check if the enemy is dead
        if (isDead)
        {
            deathTimer += Time.deltaTime;
            if (deathTimer >= resetDelay)
            {
                // Reset animation and health
                animator.SetInteger("State", 0);
                currentHealth = maxHealth;
                isDead = false;
                deathTimer = 0f;
            }
        }

        // Check if the enemy is currently taking damage and the hit animation duration has passed
        if (isTakingDamage)
        {
            hitAnimationDuration -= Time.deltaTime;
            if (hitAnimationDuration <= 0f)
            {
                // Return to the default state
                animator.SetInteger("State", 0);
                isTakingDamage = false;
                hitAnimationDuration = 1f; // Reset hit animation duration for the next hit
            }
        }
    }

    public void TakeDamage(int amount)
    {
        if (!isTakingDamage && !isDead)
        {
            currentHealth -= amount;
            onTakeDamage?.Invoke(amount);
            healthBar.UpdateHealthBar(currentHealth, maxHealth);
            Debug.Log("Enemy took damage. Current Health: " + currentHealth);
            
            animator.SetInteger("State", 1);

            if (currentHealth <= 0)
            {
                animator.SetInteger("State", 2);
                isDead = true;
            }

            isTakingDamage = true;
        }
    }
}
