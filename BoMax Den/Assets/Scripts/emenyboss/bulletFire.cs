using UnityEngine;
using System.Collections;

public class BulletFire : MonoBehaviour
{
    public int damagePerSecond = 10; // 每秒造成的伤害
    public float damageDuration = 3f; // 造成伤害的持续时间

    private bool hasHit = false; // 标记子弹是否已经命中玩家
    private float hitTime; // 记录子弹命中时间

    void OnCollisionEnter(Collision collision)
    {
        // 检测是否碰撞到玩家
        if (collision.gameObject.CompareTag("Player") && !hasHit)
        {
            // 标记子弹已命中
            hasHit = true;
            hitTime = Time.time;

            // 启动持续造成伤害的协程
            StartCoroutine(DamagePlayerCoroutine(collision.gameObject));
        }
    }

    IEnumerator DamagePlayerCoroutine(GameObject player)
    {
        while (Time.time - hitTime <= damageDuration)
        {
            // 每秒对玩家造成一定量的伤害
            PlayerHealth playerHealth = player.GetComponent<PlayerHealth>();
            if (playerHealth != null)
            {
                playerHealth.TakeDamage(damagePerSecond);
            }

            yield return new WaitForSeconds(1f); // 等待1秒钟
        }

        // 结束伤害持续时间后销毁子弹
        Destroy(gameObject);
    }
}

