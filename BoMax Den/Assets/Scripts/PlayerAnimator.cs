using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimator : MonoBehaviour
{
    [SerializeField] private Animator animator;

    //private void OnEnable()
    //{
    //    InputManager.onTakeDamage.AddListener(PlayTakeDamageAnimation);
    //    InputManager.onPlayerDeath.AddListener(PlayDeathAnimation);
    //}

    //private void OnDisable()
    //{
    //    InputManager.onTakeDamage.RemoveListener(PlayTakeDamageAnimation);
    //    InputManager.onPlayerDeath.RemoveListener(PlayDeathAnimation);
    //}

    private void Update()
    {
        UpdateMovementAnimation();
        UpdateAimingAnimation();
        UpdateJumpAnimation();
        UpdateSprintAnimation();
        UpdateShootAnimation();
    }

    private void UpdateMovementAnimation()
    {
        Vector2 movementInput = InputManager.movementInput;
        float movementMagnitude = movementInput.magnitude;

        if (movementMagnitude > 0)
        {
            animator.SetInteger("State", 1); // Walking animation
        }
        else
        {
            animator.SetInteger("State", 0); // Idle animation
        }
    }

    private void UpdateAimingAnimation()
    {
        bool isAiming = InputManager.isAimingInput;
        animator.SetBool("IsAiming", isAiming);
    }

    private void UpdateJumpAnimation()
    {
        bool isJumping = InputManager.isJumpInput;
        animator.SetBool("IsJumping", isJumping);
    }

    private void UpdateSprintAnimation()
    {
        bool isSprinting = InputManager.isSprintingInput;
        animator.SetBool("Running", isSprinting);
    }

    private void UpdateShootAnimation()
    {
        bool isShooting = InputManager.isShootingInput;
        animator.SetBool("Shooting", isShooting);
    }

    private void PlayTakeDamageAnimation(int amount)
    {
        animator.SetTrigger("TakeDamage");
    }

    private void PlayDeathAnimation()
    {
        animator.SetTrigger("Die");
    }
}