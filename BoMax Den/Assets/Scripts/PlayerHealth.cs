using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;


public class PlayerHealth : MonoBehaviour
{
    [SerializeField] private Animator animator;
    [SerializeField] public int currentHealth;
    [SerializeField] public int maxHealth = 100;
    [SerializeField] public int currentMana;
    [SerializeField] public int maxMana = 100;
    public UnityEvent<int> onTakeDamage;
    public UnityEvent onDeath;

    private bool isTakingDamage = false;
    private bool isDead = false;

    private void Start()
    {
        currentHealth = maxHealth;
        currentMana = maxMana;
    }

    private void Update()
    {
        if (isDead)
        {
            // Handle death logic
        }

        if (isTakingDamage)
        {
            // Handle damage animation or effects
        }
    }

    public void TakeDamage(int amount)
    {
        if (!isDead && !isTakingDamage)
        {
            currentHealth -= amount;
            currentMana -= amount; // Deduct mana when taking damage for now
            onTakeDamage?.Invoke(amount);
            Debug.Log("Player took damage. Current Health: " + currentHealth);
            Debug.Log("Player mana reduced. Current Mana: " + currentMana);

            isTakingDamage = true;

            if (currentHealth <= 0)
            {
                Die();
            }
        }
    }

    private void Die()
    {
        // Handle death logic here
        animator.SetTrigger("Die"); // Example: Trigger death animation
        isDead = true;
        onDeath?.Invoke();
    }
}