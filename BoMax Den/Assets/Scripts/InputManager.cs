using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class InputManager : MonoBehaviour
{
    PlayerControls controls;

    public static Vector2 movementInput;
    public static Vector2 turnInput;
    public static bool isAimingInput = false;
    public static bool isJumpInput = false;
    public static bool isSprintingInput = false;
    public static bool isSwingingInput = false;
    public static bool isShootingFireInput = false;
    public static bool isShootingWaterInput = false;
    public static bool isShootingEarthInput = false;
    public static bool isSkillInput = false;
    public static bool isInteractInput = false;
    public static bool isHealInput = false;


    private void Awake()
    {
        controls = new PlayerControls();
    }

    void OnEnable()
    {
        controls.Player.Move.performed += Move;
        controls.Player.Move.canceled += Move;

        controls.Player.Turn.performed += Turn;
        controls.Player.Turn.canceled += Turn;

        controls.Player.Aiming.performed += ctx => isAimingInput = true;
        controls.Player.Aiming.canceled += ctx => isAimingInput = false;

        controls.Player.Jump.performed += ctx => isJumpInput = true;
        controls.Player.Jump.canceled += ctx => isJumpInput = false;

        controls.Player.Sprint.performed += ctx => isSprintingInput = true;
        controls.Player.Sprint.canceled += ctx => isSprintingInput = false;

        controls.Player.SwingSword.performed += ctx => isSwingingInput = true;
        controls.Player.SwingSword.canceled += ctx => isSwingingInput = false;

        controls.Player.ShootFire.performed += ctx => isShootingFireInput = true;
        controls.Player.ShootFire.canceled += ctx => isShootingFireInput = false;

        controls.Player.ShootWater.performed += ctx => isShootingWaterInput = true;
        controls.Player.ShootWater.canceled += ctx => isShootingWaterInput = false;

        controls.Player.ShootEarth.performed += ctx => isShootingEarthInput = true;
        controls.Player.ShootEarth.canceled += ctx => isShootingEarthInput = false;

        controls.Player.Skill.performed += ctx => isSkillInput = true;
        controls.Player.Skill.canceled += ctx => isSkillInput = false;

        controls.Player.Interact.performed += ctx => isInteractInput = true;
        controls.Player.Interact.canceled += ctx => isInteractInput = false;

        controls.Player.Heal.performed += ctx => isHealInput = true;
        controls.Player.Heal.canceled += ctx => isHealInput = false;



        controls.Player.Enable();
    }

    void OnDisable()
    {
        controls.Player.Move.performed -= Move;
        controls.Player.Move.canceled -= Move;

        controls.Player.Turn.performed -= Turn;
        controls.Player.Turn.canceled -= Turn;

        controls.Player.Jump.performed -= ctx => isJumpInput = true;
        controls.Player.Jump.canceled -= ctx => isJumpInput = false;

        controls.Player.Sprint.performed -= ctx => isSprintingInput = true;
        controls.Player.Sprint.canceled -= ctx => isSprintingInput = false;

        controls.Player.SwingSword.performed -= ctx => isSwingingInput = true;
        controls.Player.SwingSword.canceled -= ctx => isSwingingInput = false;

        controls.Player.ShootFire.performed -= ctx => isShootingFireInput = true;
        controls.Player.ShootFire.canceled -= ctx => isShootingFireInput = false;

        controls.Player.ShootWater.performed -= ctx => isShootingWaterInput = true;
        controls.Player.ShootWater.canceled -= ctx => isShootingWaterInput = false;

        controls.Player.ShootEarth.performed -= ctx => isShootingEarthInput = true;
        controls.Player.ShootEarth.canceled -= ctx => isShootingEarthInput = false;

        controls.Player.Skill.performed -= ctx => isSkillInput = true;
        controls.Player.Skill.canceled -= ctx => isSkillInput = false;

        controls.Player.Interact.performed -= ctx => isInteractInput = true;
        controls.Player.Interact.canceled -= ctx => isInteractInput = false;

        controls.Player.Heal.performed -= ctx => isHealInput = true;
        controls.Player.Heal.canceled -= ctx => isHealInput = false;
    }

    private void Move(InputAction.CallbackContext ctx)
    {
        movementInput = ctx.ReadValue<Vector2>();
    }

    private void Turn(InputAction.CallbackContext ctx)
    {
        turnInput = ctx.ReadValue<Vector2>();
    }
}
