using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using UnityEngine.InputSystem.LowLevel;
using System.Text;

public class PlayerController : MonoBehaviour
{
    [Header("Movement Settings")]
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
    [SerializeField] public PlayerState currentState;
    [SerializeField] public PlayerStats playerStats;
    [SerializeField] private Camera aimingCamera;
    [SerializeField] private PlayerAnimator playerAnimator;

    [Header("Skill Settings")]
    [SerializeField] private float manaCostFireSkill = 5f;
    [SerializeField] private float manaCostWaterSkill = 4;
    [SerializeField] private float manaCostEarthSkill = 20;

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
    private float timeSinceLastShot = 0f;
    private int currentSpawnIndex = 0;
    private bool inSkillMode = false;
    private bool switchStateRequested = false;
    private bool isFrozen = false;
    public enum PlayerState
    {
        Normal,
        Fire,
        Water,
        Earth
    }

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        playerRenderer = GetComponent<Renderer>();
        playerStats = GetComponent<PlayerStats>();
        currentState = PlayerState.Normal;
    }

    void FixedUpdate()
    {
        HandleMovement();
        HandleAiming();
        HandleJump();
        UpdateCrosshairVisibility();
        if (switchStateRequested)
        {
            SwitchPlayerState(currentState);
            switchStateRequested = false;
        }
        HandleCurrentStateActions();
        playerAnimator.UpdatePlayerState(currentState);
    }
    // Basic Player Functions
    private void HandleMovement()
    {
        if (isFrozen) return;

        float horizontalInput = InputManager.movementInput.x;
        float verticalInput = InputManager.movementInput.y;
        bool isSprinting = InputManager.isSprintingInput;


        if (InputManager.isAimingInput)
        {
            isSprinting = false;
        }

        Vector3 forwardDirection = Camera.main.transform.forward;
        forwardDirection.y = 0;
        Vector3 rightDirection = Camera.main.transform.right;

        Vector3 movementDirection = forwardDirection * verticalInput + rightDirection * horizontalInput;
        movementDirection.Normalize();

        float currentMoveSpeed = isSprinting ? playerStats.MoveSpeed * 2 : playerStats.MoveSpeed;

        Vector3 movement = movementDirection * (currentMoveSpeed * Time.deltaTime);
        movement.y = rb.velocity.y;
        rb.velocity = new Vector3(movement.x, rb.velocity.y, movement.z);

        if (movementDirection != Vector3.zero && !InputManager.isAimingInput)
        {
            TurnTowardsMovementDirection(movementDirection);
        }
    }
    public void FreezePlayer(float duration)
    {
        StartCoroutine(FreezeDuration(duration));
    }
    private IEnumerator FreezeDuration(float duration)
    {
        isFrozen = true; 
        yield return new WaitForSeconds(duration);
        isFrozen = false;
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

    // Normal State Functions
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
            if (InputManager.isAimingInput)
            {
                Ray ray = new Ray(aimingCamera.transform.position, aimingCamera.transform.forward);
                RaycastHit hit;

                if (Physics.Raycast(ray, out hit))
                {
                    Vector3 direction = (hit.point - RockSpikeSpawnPoint.position).normalized;

                    GameObject bullet = Instantiate(fireBullet, RockSpikeSpawnPoint.position, Quaternion.LookRotation(direction));
                    bullet.GetComponent<Rigidbody>().AddForce(direction * bulletSpeed);
                    Destroy(bullet, 1f);
                }
            }
            else
            {
                Vector3 direction = (projectileDirectionObject.transform.position - RockSpikeSpawnPoint.position).normalized;

                GameObject bullet = Instantiate(fireBullet, RockSpikeSpawnPoint.position, Quaternion.LookRotation(direction));
                bullet.GetComponent<Rigidbody>().AddForce(direction * bulletSpeed);
                Destroy(bullet, 0.5f);
            }

            timeSinceLastShot = 0f;
        }
    }
    private void HandleSkillFire()
    {
        if (InputManager.isSkillInput)
        {
            if (playerStats.currentMana >= manaCostFireSkill)
            {
                if (InputManager.isAimingInput && InputManager.isShootingFireInput)
                {
                    inSkillMode = true;
                    flamethrowerParticles.Play();
                    fireCollider.SetActive(true);
                    playerStats.currentMana -= manaCostFireSkill;
                }
                else
                {
                    flamethrowerParticles.Stop();
                    fireCollider.SetActive(false);
                }
            }
            else
            {
                flamethrowerParticles.Stop();
                fireCollider.SetActive(false);

            }
        }
        else
        {
            inSkillMode = false;
            flamethrowerParticles.Stop();
            fireCollider.SetActive(false);
        }
    }

    // Water State Functions
    private void HandleShootingWater()
    {
        timeSinceLastShot += Time.deltaTime;

        if (!inSkillMode && InputManager.isShootingWaterInput && timeSinceLastShot >= 0.7)
        {
            if (InputManager.isAimingInput)
            {
                Ray ray = new Ray(aimingCamera.transform.position, aimingCamera.transform.forward);
                RaycastHit hit;

                if (Physics.Raycast(ray, out hit))
                {
                    Vector3 direction = (hit.point - RockSpikeSpawnPoint.position).normalized;

                    GameObject bullet = Instantiate(waterBullet, RockSpikeSpawnPoint.position, Quaternion.LookRotation(direction));
                    bullet.GetComponent<Rigidbody>().AddForce(direction * bulletSpeed);
                    Destroy(bullet, 2f);
                }
            }
            else
            {
                Vector3 direction = (projectileDirectionObject.transform.position - RockSpikeSpawnPoint.position).normalized;

                GameObject bullet = Instantiate(waterBullet, RockSpikeSpawnPoint.position, Quaternion.LookRotation(direction));
                bullet.GetComponent<Rigidbody>().AddForce(direction * bulletSpeed);
                Destroy(bullet, 0.7f);
            }

            timeSinceLastShot = 0f;
        }
    }
    private void HandleSkillWater()
    {
        if (InputManager.isSkillInput)
        {
            if (playerStats.currentMana >= manaCostWaterSkill)
            {
                timeSinceLastShot += Time.deltaTime;
                if (InputManager.isShootingWaterInput && InputManager.isAimingInput && timeSinceLastShot >= 0.3)
                {
                    inSkillMode = true;
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
                    Ray ray = new Ray(aimingCamera.transform.position, aimingCamera.transform.forward);
                    RaycastHit hit;

                    if (Physics.Raycast(ray, out hit))
                    {
                        Vector3 direction = (hit.point - selectedSpawnPoint.position).normalized;
                        Quaternion rotation = Quaternion.LookRotation(direction);

                        GameObject bullet = Instantiate(waterBullet, selectedSpawnPoint.position, rotation);
                        playerStats.currentMana -= manaCostWaterSkill;
                        bullet.GetComponent<Rigidbody>().AddForce(direction * bulletSpeed);
                        Destroy(bullet, 0.7f);
                    }

                    currentSpawnIndex = (currentSpawnIndex + 1) % 3;
                    timeSinceLastShot = 0f;
                }
            }
        }
        else
        {
            inSkillMode = false;
        }
    }
    // Earth State Functions
    private void HandleShootingEarth()
    {
        timeSinceLastShot += Time.deltaTime;

        if (InputManager.isShootingEarthInput && timeSinceLastShot >= 1.4)
        {
            if (InputManager.isAimingInput)
            {
                // Cast a ray from the aiming camera's position
                Ray ray = new Ray(aimingCamera.transform.position, aimingCamera.transform.forward);
                RaycastHit hit;

                if (Physics.Raycast(ray, out hit))
                {
                    Vector3 direction = (hit.point - RockSpikeSpawnPoint.position).normalized;

                    GameObject bullet = Instantiate(earthBullet, RockSpikeSpawnPoint.position, Quaternion.LookRotation(direction));
                    bullet.GetComponent<Rigidbody>().AddForce(direction * bulletSpeed);
                    Destroy(bullet, 1f);
                }
            }
            else
            {
                // Use non-raycast logic
                Vector3 direction = (projectileDirectionObject.transform.position - RockSpikeSpawnPoint.position).normalized;

                GameObject bullet = Instantiate(earthBullet, RockSpikeSpawnPoint.position, Quaternion.LookRotation(direction));
                bullet.GetComponent<Rigidbody>().AddForce(direction * bulletSpeed);
                Destroy(bullet, 2f);
            }

            timeSinceLastShot = 0f;
        }
    }
    private void HandleSkillEarth()
    {
        timeSinceLastShot += Time.deltaTime;

        if (InputManager.isSkillInput && timeSinceLastShot >= 1)
        {
            if (playerStats.currentMana >= manaCostEarthSkill)
            {
                if (InputManager.isAimingInput)
                {
                    Ray ray = new Ray(aimingCamera.transform.position, aimingCamera.transform.forward);
                    RaycastHit hit;

                    if (Physics.Raycast(ray, out hit))
                    {
                        Vector3 direction = (hit.point - waterSwordSpawnPoint2.position).normalized;
                        Quaternion rotation = Quaternion.LookRotation(direction);

                        GameObject fireball = Instantiate(earthBall, waterSwordSpawnPoint2.position, rotation);
                        playerStats.currentMana -= manaCostEarthSkill;
                        fireball.GetComponent<Rigidbody>().AddForce(direction * 3000);
                        Destroy(fireball, 3f);

                        timeSinceLastShot = 0f;
                    }
                }
                else
                {
                    Vector3 direction = (projectileDirectionObject.transform.position - waterSwordSpawnPoint2.position).normalized;
                    Quaternion rotation = Quaternion.LookRotation(direction);

                    GameObject fireball = Instantiate(earthBall, waterSwordSpawnPoint2.position, rotation);
                    playerStats.currentMana -= manaCostEarthSkill;
                    fireball.GetComponent<Rigidbody>().AddForce(direction * 3000);
                    Destroy(fireball, 3f);

                    timeSinceLastShot = 0f;
                }
            }
            else
            {
                inSkillMode = false;
            }
        }
    }
    // Switching State Functions
    public void SwitchPlayerState(PlayerState newState)
    {
        currentState = newState;

        switch (currentState)
        {
            case PlayerState.Normal:
                playerStats.SetStateValues(1f, 0f, 1f);
                NormalState();
                break;
            case PlayerState.Fire:
                playerStats.SetStateValues(0.8f, 1f, 1f);
                FireState();
                break;
            case PlayerState.Water:
                playerStats.SetStateValues(1f, 1.5f, 1.5f);
                WaterState();
                break;
            case PlayerState.Earth:
                playerStats.SetStateValues(1.5f, 1f, 0.875f);
                EarthState();
                break;
        }
    }
    public void RequestStateSwitch(PlayerState newState)
    {
        switchStateRequested = true;
        currentState = newState;
    }
    private void HandleCurrentStateActions()
    {
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


        HandleNormalAttack();

    }
    private void FireState()
    {
        meshRendererToUse.sharedMaterial = materialToUse[1];
        normalStaff.SetActive(false);
        fireStaff.SetActive(false);
        waterStaff.SetActive(false);
        earthStaff.SetActive(true);


        HandleShootingFire();
        HandleSkillFire();
    }
    private void WaterState()
    { 
        meshRendererToUse.sharedMaterial = materialToUse[2];
        normalStaff.SetActive(false);
        fireStaff.SetActive(false);
        waterStaff.SetActive(true);
        earthStaff.SetActive(false);


        HandleShootingWater();
        HandleSkillWater();
    }
    private void EarthState()
    {
        meshRendererToUse.sharedMaterial = materialToUse[3];
        normalStaff.SetActive(false);
        fireStaff.SetActive(true);
        waterStaff.SetActive(false);
        earthStaff.SetActive(false);

        HandleShootingEarth();
        HandleSkillEarth();
    }
}
