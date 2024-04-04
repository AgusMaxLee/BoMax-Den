using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;


public class PlayerStats : MonoBehaviour
{
    [SerializeField] private Animator animator;
    [SerializeField] public float currentHealth;
    [SerializeField] public float maxHealth = 100;
    [SerializeField] public float currentMana;
    [SerializeField] public float maxMana = 100;
    [SerializeField] public float baseMoveSpeed = 300f;
    [SerializeField] public float moveSpeed = 300f;
    [SerializeField] public int manaRegenRate = 10;
    [SerializeField] public int healthRegenRate = 5;
    [SerializeField] PlayerStatsUI playerStatsUI;

    private int baseMaxHealth = 100;
    private int baseMaxMana = 100;

    public float MoveSpeed
    {
        get { return moveSpeed; }
        set { moveSpeed = value; }
    }
    public UnityEvent<int> onTakeDamage;
    public UnityEvent onDeath;

    private bool isTakingDamage = false;
    private bool isDead = false;

    void Start()
    {
        currentHealth = maxHealth;
        currentMana = maxMana;
        moveSpeed = baseMoveSpeed;
    }

    void Update()
    {
        if (!InputManager.isSkillInput)
        {
            RegenerateMana();
        }
        //RegenerateHealth();

        if (isDead)
        {
            // Handle death logic
        }

        if (isTakingDamage)
        {
            // Handle damage animation or effects
        }
    }
    /*
    private void RegenerateHealth()
    {
        currentHealth += healthRegenRate * Time.deltaTime;
        if (currentHealth > maxHealth)
        {
            currentHealth = maxHealth;
        }
        if (currentHealth < 0)
        {
            currentHealth = 0;
        }
    }
    */
    private void RegenerateMana()
    {
        currentMana += manaRegenRate * Time.deltaTime;
        if (currentMana > maxMana)
        {
            currentMana = maxMana;
        }
        if (currentMana < 0)
        {
            currentMana = 0;
        }
    }

    public void TakeDamage(int amount)
    {
        if (!isDead && !isTakingDamage)
        {
            currentHealth -= amount;
            currentMana -= amount;
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
        animator.SetTrigger("Die");
        isDead = true;
        onDeath?.Invoke();
    }

    public void SetStateValues(float hpMultiplier, float manaMultiplier, float speedMultiplier)
    {
        maxHealth = Mathf.RoundToInt(baseMaxHealth * hpMultiplier);
        maxMana = Mathf.RoundToInt(baseMaxMana * manaMultiplier);
        currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth);
        currentMana = Mathf.Clamp(currentMana, 0, maxMana);
        moveSpeed = baseMoveSpeed * speedMultiplier;

        playerStatsUI.UpdateMaxValues(maxHealth, maxMana);
    }
}