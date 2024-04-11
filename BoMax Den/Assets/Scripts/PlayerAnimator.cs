using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static PlayerController;

public class PlayerAnimator : MonoBehaviour
{
    [SerializeField] private Animator animator;
    [SerializeField] private PlayerState currentPlayerState;
    private Vector2 movementInput;
    private float movementMagnitude;
    void Start()
    {
        // Initialize movementInput once in the Start() method
        movementInput = Vector2.zero;
        movementMagnitude = 0f;
    }
    private void Update()
    {
        UpdateMovementAnimation();
        UpdateAimingAnimation();
        UpdateJumpAnimation();
        UpdateSprintAnimation();
        UpdateInteractAnimation();
        UpdateHealAnimation();
        UpdateShootAnimation();
        UpdateSkillAnimation();
    }
    private void UpdateMovementAnimation()
    {
        movementInput = InputManager.movementInput;
        movementMagnitude = movementInput.magnitude; 

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

        switch (currentPlayerState)
        {
            case PlayerState.Normal:
                animator.SetBool("Aiming_Normal", isAiming);
                break;
            case PlayerState.Fire:
                animator.SetBool("Aiming_Fire", isAiming);
                break;
            case PlayerState.Water:
                animator.SetBool("Aiming_Water", isAiming);
                break;
            case PlayerState.Earth:
                animator.SetBool("Aiming_Earth", isAiming);
                break;
        }
    }

    private void UpdateJumpAnimation()
    {
        bool isJumping = InputManager.isJumpInput;
        animator.SetBool("IsJumping", isJumping);
    }

    private void UpdateSprintAnimation()
    {
        bool isSprinting = InputManager.isSprintingInput;
        animator.SetBool("Running", isSprinting && movementMagnitude > 0);
    }

    private void UpdateInteractAnimation()
    {
        bool isInteracting = InputManager.isInteractInput;
        animator.SetBool("Interact", isInteracting);
    }
    private void UpdateHealAnimation()
    {
        bool isHealing = InputManager.isHealInput;
        animator.SetBool("Heal", isHealing);
    }

    private void UpdateShootAnimation()
    {
        bool isShooting = InputManager.isSwingingInput;

        switch (currentPlayerState)
        {
            case PlayerState.Normal:
                animator.SetBool("Shooting_Normal", isShooting);
                break;
            case PlayerState.Fire:
                animator.SetBool("Shooting_Fire", isShooting);
                break;
            case PlayerState.Water:
                animator.SetBool("Shooting_Water", isShooting);
                break;
            case PlayerState.Earth:
                animator.SetBool("Shooting_Earth", isShooting);
                break;

        }
    }
    private void UpdateSkillAnimation()
    {
        bool isUsingSkill = InputManager.isSkillInput;

        switch (currentPlayerState)
        {
            case PlayerState.Normal:
                animator.SetBool("Skill_Normal", isUsingSkill);
                break;
            case PlayerState.Fire:
                animator.SetBool("Skill_Fire", isUsingSkill);
                break;
            case PlayerState.Water:
                animator.SetBool("Skill_Water", isUsingSkill);
                break;
            case PlayerState.Earth:
                animator.SetBool("Skill_Earth", isUsingSkill);
                break;
        }
    }
    public void UpdatePlayerState(PlayerState newState)
    {
        currentPlayerState = newState;
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