using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAnimation : MonoBehaviour
{
    [SerializeField] private Animator animator;
    public float hurtCooldownTime = 1f; // Cooldown time for the "hurt" animation in seconds

    private bool isHurtCooldown = false; // Flag to track if the "hurt" animation is on cooldown

    private void Awake()
    {
        animator = GetComponent<Animator>();
    }

    public void StartHurtAnimation()
    {
        if (!isHurtCooldown)
        {
            animator.SetBool("isHurt", true);
            isHurtCooldown = true;
            Invoke(nameof(ResetHurtCooldown), hurtCooldownTime); // Reset the cooldown after the specified time
        }
    }

    private void ResetHurtCooldown()
    {
        animator.SetBool("isHurt", false);
        isHurtCooldown = false; // Reset the cooldown flag
    }
}
