using UnityEngine;

public class FireBossAI : MonoBehaviour
{
    public Transform player;  // Player's Transform
    public GameObject bulletPrefab;  // Bullet prefab
    public Transform[] shootPoints;  // Array of shooting points    
    public float shootingSpeed = 2.0f;  // Initial shooting speed
    private float nextShootTime;

    public float maxRateIncreaseDistance = 5.0f;  // Max distance threshold to increase shooting rate
    public float minShootingInterval = 0.67f;  // Minimum shooting interval
    public float shootingRange = 10.0f;  // Distance within which the boss will shoot

    private Animator animator;
    private bool isShooting = false;  // Bool to control shoot animation

    private void Start()
    {
        animator = GetComponent<Animator>();
    }

    private void Update()
    {
        float distance = Vector3.Distance(transform.position, player.position);

        // Face the player if in range
        if (distance <= shootingRange)
        {
            FacePlayer();
        }

        // If the player is within the shooting range
        if (distance <= shootingRange)
        {
            // If player is close enough, modify shooting interval based on distance, but not lower than the minimum interval
            if (distance < maxRateIncreaseDistance)
            {
                shootingSpeed = Mathf.Max(minShootingInterval, 2.0f * (distance / maxRateIncreaseDistance));
            }

            // If it's time to shoot, fire a bullet
            if (Time.time >= nextShootTime)
            {
                isShooting = true;  // Set the shooting flag
                ShootBullet();  // Call ShootBullet method
                nextShootTime = Time.time + shootingSpeed;
            }
        }
        else
        {
            // If player is out of range, reset shooting speed and delay the next shot
            shootingSpeed = 2.0f;
            nextShootTime = Time.time + shootingSpeed; // Delays the next potential shot
        }

        // Set the animator parameter based on the shooting flag
        animator.SetBool("isShooting", isShooting);
        isShooting = false;  // Reset the shooting flag after setting the animation parameter
    }

    void ShootBullet()
    {
        // Instantiate a bullet from a randomly selected shooting point
        if (shootPoints.Length > 0 && player != null)
        {
            Transform shootPoint = shootPoints[Random.Range(0, shootPoints.Length)];
            GameObject bullet = Instantiate(bulletPrefab, shootPoint.position, shootPoint.rotation);

            // Calculate direction towards the player
            Vector3 directionToPlayer = (player.position - shootPoint.position).normalized;

            // Add initial force towards the player
            Rigidbody bulletRb = bullet.GetComponent<Rigidbody>();
            if (bulletRb != null)
            {
                bulletRb.AddForce(directionToPlayer * 60, ForceMode.Impulse);
            }
            else
            {
                Debug.LogWarning("Bullet prefab does not have a Rigidbody component.");
            }
        }
    }

    void FacePlayer()
    {
        Vector3 directionToPlayer = (player.position - transform.position).normalized;
        Quaternion lookRotation = Quaternion.LookRotation(new Vector3(directionToPlayer.x, 0, directionToPlayer.z));
        transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * 5);
    }
}
