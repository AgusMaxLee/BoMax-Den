using UnityEngine;

public class EnemyBullet : MonoBehaviour
{
    [SerializeField] private float speed = 20f;    // 子弹的速度
    [SerializeField] private int damage = 10;      // 子弹对玩家造成的伤害
    [SerializeField] private string targetTag = "Player"; // 假设玩家的标签是"Player"

    private Rigidbody rb;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.velocity = transform.forward * speed; // 子弹在其前方方向上以指定速度移动
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag(targetTag))
        {
            // 尝试获取玩家的健康组件
            PlayerHealth playerHealth = collision.gameObject.GetComponent<PlayerHealth>();
            if (playerHealth != null)
            {
                playerHealth.TakeDamage(damage); // 对玩家应用伤害
            }

            Destroy(gameObject); // 击中目标后销毁子弹
        }
    }
}
