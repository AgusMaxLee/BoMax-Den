using UnityEngine;

public class venomai: MonoBehaviour
{
    public Transform player;  // Player's Transform
    public GameObject bulletPrefab;  // Bullet prefab
    public Transform[] shootPoints;  // Array of shooting points
    public float shootingSpeed = 2.0f;  // Initial shooting speed
    private float nextShootTime;

    public float maxRateIncreaseDistance = 5.0f;  // Max distance threshold to increase shooting rate
    public float minShootingInterval = 0.67f;  // Minimum shooting interval
    public float shootingRange = 10.0f;  // Distance within which the boss will shoot

    private void Update()
    {
        float distance = Vector3.Distance(transform.position, player.position);

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
                ShootBullet();
                nextShootTime = Time.time + shootingSpeed;
            }
        }
        else
        {
            // If player is out of range, reset shooting speed and delay the next shot
            shootingSpeed = 2.0f;
            nextShootTime = Time.time + shootingSpeed; // Delays the next potential shot
        }
    }

    void ShootBullet()
    {
        // Instantiate a bullet from a randomly selected shooting point
        if (shootPoints.Length > 0)
        {
            Transform shootPoint = shootPoints[Random.Range(0, shootPoints.Length)];
            Instantiate(bulletPrefab, shootPoint.position, shootPoint.rotation);
        }
    }
}

