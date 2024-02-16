using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] float moveSpeed = 1;
    [SerializeField] float jumpForce = 1;
    [SerializeField] GameObject crosshair;
    [SerializeField] private Transform projectileSpawnPoint;
    [SerializeField] private Animator animator;
    public GameObject bulletPrefab;
    public float bulletSpeed = 10f;
    Rigidbody rb;
    private Renderer playerRenderer;
    private bool isMoving = false;
    private bool canJump = true;

    // Reference to the aiming camera
    public Camera aimingCamera;

    // Start is called before the first frame update
    void Start()
    {
        //Caching
        rb = GetComponent<Rigidbody>();
        playerRenderer = GetComponent<Renderer>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        MoveInDirectionOfCamera();

        if (InputManager.isAimingInput && aimingCamera != null)
        {
            RotateTowardsAimingCamera();
        }

        if (InputManager.isJumpInput && canJump)
        {
            Jump();
        }

        if (InputManager.isShootingInput)
        {
            Shoot();
        }

        UpdateCrosshairVisibility();
    }

    private void MoveInDirectionOfCamera()
    {
        float horizontalInput = InputManager.movementInput.x;
        float verticalInput = InputManager.movementInput.y;
        bool isSprinting = InputManager.isSprintingInput;

        // Calculate the FORWARD direction relative to the camera
        Vector3 forwardDirection = Camera.main.transform.forward;
        forwardDirection.y = 0; // Remove vertical movement

        // Calculate the RIGHT direction relative to the camera
        Vector3 rightDirection = Camera.main.transform.right;

        // Combine the forward and right direction into a single vector
        Vector3 movementDirection = forwardDirection * verticalInput + rightDirection * horizontalInput;
        movementDirection.Normalize(); // Ensure the direction vector has a length of 1

        // Calculate the speed based on whether sprinting or not
        float currentMoveSpeed = isSprinting ? moveSpeed * 2 : moveSpeed;
 
        // Apply the movement to the Rigidbody
        Vector3 movement = movementDirection * (currentMoveSpeed * Time.deltaTime);
        rb.velocity = new Vector3(movement.x, rb.velocity.y, movement.z);

        // Rotate the player towards the movement direction
        if (movementDirection != Vector3.zero)
        {
            TurnTowardsMovementDirection(movementDirection);
        }
    }

    private void Jump()
    {
        rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
        canJump = false; // Disable jumping until the player hits the floor again
    }

    private void Shoot()
    {
        GameObject bullet = Instantiate(bulletPrefab, projectileSpawnPoint.position, Quaternion.identity) as GameObject;
        bullet.GetComponent<Rigidbody>().AddForce(transform.forward * bulletSpeed);
        Destroy(bullet, 0.3f);
    }    
    private void TurnTowardsMovementDirection(Vector3 moveDirection)
    {
        //This will create a rotation that looks in the direction of the moveDirection parameter
        Quaternion targetRotation = Quaternion.LookRotation(moveDirection);
        rb.rotation = targetRotation;
    }
    // Rotate player towards aiming camera direction
    private void RotateTowardsAimingCamera()
    {
        Vector3 lookDirection = Camera.main.transform.forward;// - transform.position;
        //lookDirection.y = 0; // Ensure the player doesn't tilt upwards or downwards

        // Rotate the player towards the aiming camera's direction
        if (lookDirection != Vector3.zero)
        {
            //Quaternion targetRotation = Camera.main.transform.rotation;
            //rb.rotation = targetRotation;
            this.transform.rotation = Quaternion.LookRotation(lookDirection);
        }
    }

    // Detect when the player hits the floor
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Floor"))
        {
            canJump = true; // Reset the ability to jump when hitting the floor
        }
    }
    private void UpdateCrosshairVisibility()
    {
        if (crosshair != null)
        {
            if (InputManager.isAimingInput)
            {
                crosshair.SetActive(true);
            }
            else
            {
                crosshair.SetActive(false);
            }
        }
    }
    public void ChangePlayerColor(Color newColor)
    {
        playerRenderer.material.color = newColor;
    }
}
