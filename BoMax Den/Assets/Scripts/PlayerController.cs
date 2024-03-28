using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using UnityEngine.InputSystem.LowLevel;
using System.Text;

public class PlayerController : MonoBehaviour
{
    [Header("Movement Settings")]
    [SerializeField] private float moveSpeed = 400;
    [SerializeField] private float jumpForce = 1;
    [SerializeField] private GameObject crosshair;
    [SerializeField] private Animator animator;

    [Header("Projectile Settings")]
    [SerializeField] private GameObject projectileDirectionObject;
    [SerializeField] private float bulletSpeed = 5f;

    [SerializeField] private GameObject fireBullet;
    [SerializeField] private GameObject waterBullet;
    [SerializeField] private GameObject earthBullet;

    [SerializeField] private ParticleSystem flamethrowerParticles;
    [SerializeField] private GameObject fireCollider;
    [SerializeField] private GameObject swordCollider;
    [SerializeField] private GameObject waterWave;
    [SerializeField] private GameObject earthBall;

    [SerializeField] private Transform flamethrowerSpawnPoint;
    [SerializeField] private Transform waterSwordSpawnPoint1;
    [SerializeField] private Transform waterSwordSpawnPoint2;
    [SerializeField] private Transform waterSwordSpawnPoint3;
    [SerializeField] private Transform RockSpikeSpawnPoint;

    [Header("State Settings")]
    [SerializeField] private PlayerHealth playerHealth;
    [SerializeField] private Camera aimingCamera;
    [SerializeField] private PlayerAnimator playerAnimator;

    [Header("Player Models")]
    [SerializeField] private SkinnedMeshRenderer meshRendererToUse;
    [SerializeField] private Material[] materialToUse;
    [SerializeField] private GameObject normalStaff;
    [SerializeField] private GameObject fireStaff;
    [SerializeField] private GameObject waterStaff;
    [SerializeField] private GameObject earthStaff;

    private Rigidbody rb;
    private Renderer playerRenderer;
    private bool canJump = true;
    private Vector2 movementInput;
    private bool isMoving = false;
    public PlayerState currentState;
    private float timeSinceLastShot = 0f;
    private int currentSpawnIndex = 0;
    private bool inSkillMode = false;

    private const int NormalHP = 100;
    private const int FireHP = 80;
    private const int WaterHP = 100;
    private const int EarthHP = 150;

    private const int NormalMana = 100;
    private const int FireMana = 100;
    private const int WaterMana = 150;
    private const int EarthMana = 100;

    private const float NormalSpeed = 400f;
    private const float FireSpeed = 400f;
    private const float WaterSpeed = 500f;
    private const float EarthSpeed = 350f;
    public enum PlayerState
    {
        Normal,
        Fire,
        Water,
        Earth
    }

    void Start()
    {
        currentState = PlayerState.Normal;
        rb = GetComponent<Rigidbody>();
        playerRenderer = GetComponent<Renderer>();
        playerHealth = GetComponent<PlayerHealth>();

    }

    void FixedUpdate()
    {
        HandleMovement();
        HandleAiming();
        HandleJump();
        UpdateCrosshairVisibility();
        SwitchPlayerState(currentState);
        playerAnimator.UpdatePlayerState(currentState);
    }
    //Basic Player Functions
    private void HandleMovement()
    {
        float horizontalInput = InputManager.movementInput.x;
        float verticalInput = InputManager.movementInput.y;
        bool isSprinting = InputManager.isSprintingInput;

        Vector3 forwardDirection = Camera.main.transform.forward;
        forwardDirection.y = 0;
        Vector3 rightDirection = Camera.main.transform.right;

        Vector3 movementDirection = forwardDirection * verticalInput + rightDirection * horizontalInput;
        movementDirection.Normalize();

        float currentMoveSpeed = isSprinting ? moveSpeed * 2 : moveSpeed;

        Vector3 movement = movementDirection * (currentMoveSpeed * Time.deltaTime);
        rb.velocity = new Vector3(movement.x, rb.velocity.y, movement.z);

        if (movementDirection != Vector3.zero && !InputManager.isAimingInput)
        {
            TurnTowardsMovementDirection(movementDirection);
        }
    }
    private void HandleJump()
    {
        if (InputManager.isJumpInput && canJump)
        {
            rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
            canJump = false;
        }
    }

    private void HandleAiming()
    {
        if (InputManager.isAimingInput)
        {
            Vector3 lookDirection = aimingCamera.transform.forward;
            this.transform.rotation = Quaternion.LookRotation(lookDirection);
        }
    }
    private void TurnTowardsMovementDirection(Vector3 moveDirection)
    {
        Quaternion targetRotation = Quaternion.LookRotation(moveDirection);
        rb.MoveRotation(Quaternion.Slerp(rb.rotation, targetRotation, Time.deltaTime * 5f));
    }
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Floor"))
        {
            canJump = true;
        }
    }
    private void UpdateCrosshairVisibility()
    {
        crosshair.SetActive(InputManager.isAimingInput);
    }

    //Normal State Functions
    private void HandleNormalAttack()
    {
        if (InputManager.isSwingingInput)
        {
            swordCollider.SetActive(true);
        }
        else
        {
            swordCollider.SetActive(false);
        }
    }
    // Fire State Functions
    private void HandleShootingFire()
    {
        timeSinceLastShot += Time.deltaTime;

        if (!inSkillMode && InputManager.isShootingFireInput && timeSinceLastShot >= 0.5)
        {
            Vector3 direction = (projectileDirectionObject.transform.position - RockSpikeSpawnPoint.position).normalized;

            GameObject bullet = Instantiate(fireBullet, RockSpikeSpawnPoint.position, Quaternion.LookRotation(direction));
            bullet.GetComponent<Rigidbody>().AddForce(direction * bulletSpeed);
            Destroy(bullet, 2f);
            timeSinceLastShot = 0f;
        }
    }

    private void HandleSkillFire()
    {
        if (InputManager.isSkillInput && InputManager.isAimingInput && InputManager.isShootingFireInput)
        {
            inSkillMode = true;
            flamethrowerParticles.Play();
            fireCollider.SetActive(true);
        }
        else
        {
            inSkillMode = false;
            flamethrowerParticles.Stop();
            fireCollider.SetActive(false);
        }
    }
    private void HandleUltimateFire()
    {
        
    }

    //Water State Functions
    private void HandleShootingWater()
    {
        timeSinceLastShot += Time.deltaTime;
        if (!inSkillMode && InputManager.isShootingWaterInput && timeSinceLastShot >= 0.3)
        {
            Transform selectedSpawnPoint = null;

            // Cycle through spawn points in order
            switch (currentSpawnIndex)
            {
                case 0:
                    selectedSpawnPoint = waterSwordSpawnPoint1;
                    break;
                case 1:
                    selectedSpawnPoint = waterSwordSpawnPoint3;
                    break;
                case 2:
                    selectedSpawnPoint = waterSwordSpawnPoint2;
                    break;
            }

            if (selectedSpawnPoint != null)
            {
                Vector3 direction = (projectileDirectionObject.transform.position - selectedSpawnPoint.position).normalized;
                Quaternion rotation = Quaternion.LookRotation(direction);

                GameObject bullet = Instantiate(waterBullet, selectedSpawnPoint.position, rotation);
                bullet.GetComponent<Rigidbody>().AddForce(direction * bulletSpeed);
                Destroy(bullet, 2f);
            }

            currentSpawnIndex = (currentSpawnIndex + 1) % 3;

            timeSinceLastShot = 0f;
        }
    }
    private void HandleSkillWater()
    {
        if (InputManager.isSkillInput)
        {
            inSkillMode = true;
            
        }
        else
        {
            inSkillMode = false;
            
        }
    }
    private void HandleUltimateWater()
    {

    }
    //Earth State Functions
    private void HandleShootingEarth()
    {
        timeSinceLastShot += Time.deltaTime;

        if (InputManager.isShootingEarthInput && timeSinceLastShot >= 1.4)
        {
            Vector3 direction = (projectileDirectionObject.transform.position - RockSpikeSpawnPoint.position).normalized;

            GameObject bullet = Instantiate(earthBullet, RockSpikeSpawnPoint.position, Quaternion.LookRotation(direction));
            bullet.GetComponent<Rigidbody>().AddForce(direction * bulletSpeed);
            Destroy(bullet, 2f);

            timeSinceLastShot = 0f;
        }
    }
    private void HandleSkillEarth()
    {
        timeSinceLastShot += Time.deltaTime;
        if (InputManager.isSkillInput && timeSinceLastShot >= 1)
        {
            Vector3 direction = (projectileDirectionObject.transform.position - waterSwordSpawnPoint2.position).normalized;
            Quaternion rotation = Quaternion.LookRotation(direction);

            GameObject fireball = Instantiate(earthBall, waterSwordSpawnPoint2.position, rotation);
            fireball.GetComponent<Rigidbody>().AddForce(direction * 3000);
            Destroy(fireball, 3f);

            timeSinceLastShot = 0f;
        }
    }
    private void HandleUltimateEarth()
    {

    }

    public void SwitchPlayerState(PlayerState newState)
    {
        currentState = newState;

        switch (currentState)
        {
            case PlayerState.Normal:
                NormalState();
                break;
            case PlayerState.Fire:
                FireState();
                break;
            case PlayerState.Water:
                WaterState();
                break;
            case PlayerState.Earth:
                EarthState();
                break;
        }
    }

    private void NormalState()
    {
        meshRendererToUse.sharedMaterial = materialToUse[0];

        normalStaff.SetActive(true);
        fireStaff.SetActive(false);
        waterStaff.SetActive(false);
        earthStaff.SetActive(false);

        playerHealth.currentHealth = NormalHP;
        playerHealth.currentMana = NormalMana;
        moveSpeed = NormalSpeed;

        HandleNormalAttack();

    }

    private void FireState()
    {
        meshRendererToUse.sharedMaterial = materialToUse[1];
        normalStaff.SetActive(false);
        fireStaff.SetActive(false);
        waterStaff.SetActive(false);
        earthStaff.SetActive(true);

        playerHealth.currentHealth = FireHP;
        playerHealth.currentMana = FireMana;
        moveSpeed = FireSpeed;

        HandleShootingFire();
        HandleSkillFire();
        HandleUltimateFire();
    }

    private void WaterState()
    { 
        meshRendererToUse.sharedMaterial = materialToUse[2];
        normalStaff.SetActive(false);
        fireStaff.SetActive(false);
        waterStaff.SetActive(true);
        earthStaff.SetActive(false);

        playerHealth.currentHealth = WaterHP;
        playerHealth.currentMana = WaterMana;
        moveSpeed = WaterSpeed;

        HandleShootingWater();
        HandleSkillWater();
        HandleUltimateWater();
    }

    private void EarthState()
    {
        meshRendererToUse.sharedMaterial = materialToUse[3];
        normalStaff.SetActive(false);
        fireStaff.SetActive(true);
        waterStaff.SetActive(false);
        earthStaff.SetActive(false);

        playerHealth.currentHealth = EarthHP;
        playerHealth.currentMana = EarthMana;
        moveSpeed = EarthSpeed;

        HandleShootingEarth();
        HandleSkillEarth();
        HandleUltimateEarth();
    }
}
