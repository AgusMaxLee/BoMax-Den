using UnityEngine;

public class fireai : MonoBehaviour
{
    public Transform player;
    public GameObject bulletPrefab;
    public float bulletSpeed = 10f;
    public int damageAmount = 10;
    public float continuousDamageDuration = 5f;
    public float shootInterval = 1.5f; // 子弹发射间隔时间
    public Transform bulletSpawnPoint; // 子弹发射位置

    private bool playerInRange = false;
    private bool isDealingContinuousDamage = false;
    private float lastDamageTime;
    private float detectionRange = 20;
    private float lastShootTime; // 上次发射子弹时间

    void Update()
    {
        if (Vector3.Distance(transform.position, player.position) <= detectionRange)
        {
            playerInRange = true;
            // 检查敌人是否面向玩家
            Vector3 directionToPlayer = (player.position - transform.position).normalized;
            float dotProduct = Vector3.Dot(transform.forward, directionToPlayer);
            if (dotProduct > 0.5f) // 假设0.5为一个合适的阈值
            {
                // 如果敌人面向玩家，且距离玩家足够近，则发射子弹
                if (Time.time - lastShootTime >= shootInterval)
                {
                    ShootAtPlayer();
                    lastShootTime = Time.time;
                }
            }
        }
        else
        {
            playerInRange = false;
        }
    }

    void ShootAtPlayer()
    {
        if (!playerInRange)
        {
            return;
        }

        Vector3 spawnPosition = bulletSpawnPoint.position; // 使用指定的子弹发射位置
        GameObject bullet = Instantiate(bulletPrefab, spawnPosition, Quaternion.identity);
        Rigidbody bulletRb = bullet.GetComponent<Rigidbody>();
        Vector3 direction = (player.position - spawnPosition).normalized; // 方向基于玩家位置和发射位置
        bulletRb.velocity = direction * bulletSpeed;
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if (!isDealingContinuousDamage)
            {
                isDealingContinuousDamage = true;
                lastDamageTime = Time.time;
                InvokeRepeating("DealContinuousDamage", 0f, 1f);
            }
            else
            {
                lastDamageTime = Time.time;
            }
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            CancelInvoke("DealContinuousDamage");
            isDealingContinuousDamage = false;
        }
    }

    void DealContinuousDamage()
    {
        float timeSinceLastDamage = Time.time - lastDamageTime;

        if (timeSinceLastDamage >= continuousDamageDuration)
        {
            CancelInvoke("DealContinuousDamage");
            isDealingContinuousDamage = false;
        }
        else
        {
            PlayerHealth playerHealth = player.GetComponent<PlayerHealth>();
            if (playerHealth != null)
            {
                playerHealth.TakeDamage(damageAmount);
            }
        }
    }
}

