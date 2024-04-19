using UnityEngine;

public class EarthBossAI : MonoBehaviour
{
    public Transform player;  // Player's Transform
    public GameObject bulletPrefab;  // Bullet prefab
    public GameObject minionPrefab; // Minion prefab
    public Transform[] shootPoints;  // Array of shooting points    
    public Transform[] spawnPoints; // Array of minion spawn points
    public float shootingSpeed = 2.0f;  // Initial shooting speed
    public float shootingInterval = 2f; // Normal shooting interval
    public float spawnCheckInterval = 5f; // Minion spawn check interval
    public float detectionRange = 50f; // Player detection range

    private float nextShootTime;
    private float nextSpawnCheckTime;
    private int minionsAlive = 0;
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
        if (distance <= detectionRange)
        {
            FacePlayer();
        }

        // If the player is within the detection range
        if (distance <= detectionRange)
        {
            // If it's time to shoot, fire a bullet
            if (Time.time >= nextShootTime)
            {
                isShooting = true;  // Set the shooting flag
                ShootBullet();  // Call ShootBullet method
                nextShootTime = Time.time + shootingSpeed;
            }

            // Handle minion spawning
            if (Time.time >= nextSpawnCheckTime && minionsAlive < 3)
            {
                Transform spawnPoint = FindAvailableSpawnPoint();
                if (spawnPoint != null)
                {
                    SpawnMinion(spawnPoint);
                }
                nextSpawnCheckTime = Time.time + spawnCheckInterval;
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
                bulletRb.AddForce(directionToPlayer * 90, ForceMode.Impulse);
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

    private Transform FindAvailableSpawnPoint()
    {
        foreach (Transform point in spawnPoints)
        {
            if (!IsPointOccupied(point.position))
            {
                return point;
            }
        }
        return null;
    }

    private bool IsPointOccupied(Vector3 point)
    {
        Collider[] colliders = Physics.OverlapSphere(point, 1.0f);
        foreach (Collider collider in colliders)
        {
            if (collider.CompareTag("DynamicMinion") || collider.CompareTag("Player"))
            {
                return true;
            }
        }
        return false;
    }

    private void SpawnMinion(Transform spawnPoint)
    {
        GameObject minion = Instantiate(minionPrefab, spawnPoint.position, spawnPoint.rotation);
        minion.tag = "DynamicMinion"; // Set the generated minion's tag to "DynamicMinion"
        minion.GetComponent<EarthMiniAI>().SetTarget(player);
        minionsAlive++;
    }
}