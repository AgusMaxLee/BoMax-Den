using UnityEngine;

public class MinionController : MonoBehaviour
{
    public GameObject bulletPrefab;
    public float shootingInterval = 3f;
    private float lastShootTime;
    private Transform playerTarget;

    void Update()
    {
        if (playerTarget != null && Time.time > lastShootTime + shootingInterval)
        {
            Shoot();
            lastShootTime = Time.time;
        }
    }

    public void SetTarget(Transform target)
    {
        playerTarget = target;
    }

    private void Shoot()
    {
        Instantiate(bulletPrefab, transform.position, Quaternion.LookRotation(playerTarget.position - transform.position));
    }

    void OnDestroy()
    {
        FindObjectOfType<BossController>().MinionDied(); // Notify the boss that a minion has died
    }
}

