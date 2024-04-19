using UnityEngine;

public class ColdBossAI : MonoBehaviour
{
    public Transform player;  // Player's Transform
    public GameObject bulletPrefab;  // Bullet prefab
    public GameObject specialBulletPrefab;  // Special bullet prefab
    public Transform[] shootPoints;  // Array of shooting points    
    public float normalShootingSpeed = 2.0f;  // Initial shooting speed for normal bullets
    public float specialShootingSpeed = 3.0f;  // Initial shooting speed for special bullets
    private float nextNormalShootTime;
    private float nextSpecialShootTime;
    public float maxRateIncreaseDistance = 5.0f;  // Max distance threshold to increase shooting rate
    public float minShootingInterval = 0.67f;  // Minimum shooting interval
    public float shootingRange = 10.0f;  // Distance within which the boss will shoot

    private Animator animator;
    private bool isShootingNormal = false;  // Bool to control normal bullet shoot animation
    private bool isShootingSpecial = false;  // Bool to control special bullet shoot animation

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
                normalShootingSpeed = Mathf.Max(minShootingInterval, 2.0f * (distance / maxRateIncreaseDistance));
            }

            // If it's time to shoot normal bullets, fire a normal bullet
            if (Time.time >= nextNormalShootTime)
            {
                isShootingNormal = true;  // Set the normal shooting flag
                Shoot(bulletPrefab, normalShootingSpeed);  // Call Shoot method for normal bullets
                nextNormalShootTime = Time.time + normalShootingSpeed;
            }

            // If it's time to shoot special bullets, fire a special bullet
            if (Time.time >= nextSpecialShootTime)
            {
                isShootingSpecial = true;  // Set the special shooting flag
                Shoot(specialBulletPrefab, specialShootingSpeed);  // Call Shoot method for special bullets
                nextSpecialShootTime = Time.time + specialShootingSpeed;
            }
        }
        else
        {
            // If player is out of range, reset shooting speed and delay the next shot
            normalShootingSpeed = 2.0f;
            specialShootingSpeed = 3.0f;
            nextNormalShootTime = Time.time + normalShootingSpeed; // Delays the next potential normal shot
        }

        // Set the animator parameters based on the shooting flags
        animator.SetBool("isShooting", isShootingNormal);
        isShootingNormal = false;  // Reset the normal shooting flag after setting the animation parameter
    }

    void Shoot(GameObject bulletType, float shootingSpeed)
    {
        if (shootPoints.Length > 0 && player != null)
        {
            Transform shootPoint = shootPoints[Random.Range(0, shootPoints.Length)];
            GameObject bullet = Instantiate(bulletType, shootPoint.position, shootPoint.rotation);

            // Calculate direction towards the player
            Vector3 directionToPlayer = (player.position - shootPoint.position).normalized;

            // Add initial force towards the player
            Rigidbody bulletRb = bullet.GetComponent<Rigidbody>();
            if (bulletRb != null)
            {
                bulletRb.AddForce(directionToPlayer * 100, ForceMode.Impulse);
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
