using UnityEngine;

public class EnemyBullet : MonoBehaviour
{
    [SerializeField] private float speed = 20f;
    [SerializeField] private int damage = 10;
    [SerializeField] private string targetTag = "Player";

    private Rigidbody rb;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();

        // 尝试获取玩家位置
        GameObject player = GameObject.FindGameObjectWithTag(targetTag);
        if (player != null)
        {
            // 计算指向玩家的方向向量
            Vector3 targetDirection = (player.transform.position - transform.position).normalized;
            // 设置子弹的速度为该方向乘以速度值
            rb.velocity = targetDirection * speed;
        }
        else
        {
            // 如果没有找到玩家，子弹默认直线飞行
            rb.velocity = transform.forward * speed;
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag(targetTag))
        {
            PlayerStats playerStats = collision.gameObject.GetComponent<PlayerStats>();
            if (playerStats != null)
            {
                playerStats.TakeDamage(damage);
            }

            Destroy(gameObject);
        }
    }
}

