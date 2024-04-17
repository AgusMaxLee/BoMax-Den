using UnityEngine;

public class fireai : MonoBehaviour
{
    public Transform player;
    public GameObject bulletPrefab;
    public float bulletSpeed = 10f;
    public int damageAmount = 10;
    public float continuousDamageDuration = 5f;
    public float shootInterval = 1.5f; // �ӵ�������ʱ��
    public Transform bulletSpawnPoint; // �ӵ�����λ��

    private bool playerInRange = false;
    private bool isDealingContinuousDamage = false;
    private float lastDamageTime;
    private float detectionRange = 20;
    private float lastShootTime; // �ϴη����ӵ�ʱ��

    void Update()
    {
        if (Vector3.Distance(transform.position, player.position) <= detectionRange)
        {
            playerInRange = true;
            // �������Ƿ��������
            Vector3 directionToPlayer = (player.position - transform.position).normalized;
            float dotProduct = Vector3.Dot(transform.forward, directionToPlayer);
            if (dotProduct > 0.5f) // ����0.5Ϊһ�����ʵ���ֵ
            {
                // �������������ң��Ҿ�������㹻���������ӵ�
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

        Vector3 spawnPosition = bulletSpawnPoint.position; // ʹ��ָ�����ӵ�����λ��
        GameObject bullet = Instantiate(bulletPrefab, spawnPosition, Quaternion.identity);
        Rigidbody bulletRb = bullet.GetComponent<Rigidbody>();
        Vector3 direction = (player.position - spawnPosition).normalized; // ����������λ�úͷ���λ��
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

