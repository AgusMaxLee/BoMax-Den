using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class EnemyHealth : MonoBehaviour
{
    [SerializeField] private Animator animator;
    [SerializeField] private int currentHealth, maxHealth = 100;
    public UnityEvent<int> onTakeDamage;

    [SerializeField] private float hitAnimationDuration = 1f;
    private bool isTakingDamage = false;
    private bool isDead = false;
    private bool isRecovering = false;

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
        if (isRecovering)
        {
            hitAnimationDuration -= Time.deltaTime;
            if (hitAnimationDuration <= 0f)
            {
                currentHealth = maxHealth;
                healthBar.UpdateHealthBar(currentHealth, maxHealth);
                isRecovering = false;
                animator.SetBool("Recover", true); // Transition to recover animation
                animator.SetBool("Dead", false); // Reset dead animation state
                isTakingDamage = false; // Reset taking damage flag
                isDead = false; // Reset dead flag
            }
        }
    }

    public bool IsDead
    {
        get { return currentHealth <= 0; }
    }

    public void TakeDamage(int amount)
    {
        if (!isDead && !isRecovering)
        {
            currentHealth -= amount;
            onTakeDamage?.Invoke(amount);
            healthBar.UpdateHealthBar(currentHealth, maxHealth);
            Debug.Log("Enemy took damage. Current Health: " + currentHealth);

            animator.SetBool("Hit", true);

            // Invoke a method to reset "Hit" after 2 seconds
            Invoke("ResetHitParameter", 0.5f);
            
            if (currentHealth <= 0)
            {
                isDead = true;
                animator.SetBool("Dead", true);
                StartRecovery();
            }
            else
            {
                isTakingDamage = true; // Set taking damage flag
            }
        }
    }

    private void ResetHitParameter()
    {
        animator.SetBool("Hit", false);
    }

    private void StartRecovery()
    {
        isRecovering = true;
        hitAnimationDuration = 2f; // Set recovery duration
        animator.SetBool("Recover", false); // Ensure Recover parameter is set to false initially
    }

    // Called by animation event when the recovery animation finishes
    public void OnRecoveryComplete()
    {
        animator.SetBool("Recover", true); // Transition back to idle animation
    }
}

