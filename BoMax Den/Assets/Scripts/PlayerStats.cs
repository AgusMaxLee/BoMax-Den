using System.Collections;
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
    [SerializeField] public int healthRegenRate = 10;
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

    private bool isDead = false;
    private bool isTakingDamage = false; // Added variable declaration

    private void Start()
    {
        currentHealth = maxHealth;
        currentMana = maxMana;
        moveSpeed = baseMoveSpeed;
    }

    private void Update()
    {
        if (!InputManager.isSkillInput)
        {
            RegenerateMana();
        }

        if (isDead)
        {
            // Handle death logic
        }
    }

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
        if (!isDead)
        {
            currentHealth -= amount;
            onTakeDamage?.Invoke(amount);
            animator.SetTrigger("TakeDamage");
            Debug.Log("Player took damage. Current Health: " + currentHealth);

            if (currentHealth <= 0)
            {
                Die();
            }
            else
            {
                StartCoroutine(TakeDamageCoroutine());
            }
        }
    }

    private IEnumerator TakeDamageCoroutine()
    {
        isTakingDamage = true;
        yield return new WaitForSeconds(0.5f); // Adjust the delay as needed
        isTakingDamage = false;
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
