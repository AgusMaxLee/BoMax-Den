using UnityEngine;

public class coldai : MonoBehaviour
{
    public Transform player;  // 玩家的Transform
    public GameObject bulletPrefab;  // 普通子弹的预制体
    public GameObject specialBulletPrefab;  // 特殊子弹的预制体
    public Transform[] shootPoints;  // 发射点数组
    public float shootInterval = 0.7f;  // 普通射击间隔
    public float specialShootInterval = 3.0f;  // 特殊子弹射击间隔
    public float shootRange = 20.0f;  // 射击距离

    private float nextShootTime;
    private float nextSpecialShootTime;

    void Update()
    {
        float distance = Vector3.Distance(transform.position, player.position);

        // 当玩家在射击范围内
        if (distance <= shootRange)
        {
            // 普通子弹射击逻辑
            if (Time.time >= nextShootTime)
            {
                Shoot(bulletPrefab);
                nextShootTime = Time.time + shootInterval;
            }

            // 特殊子弹射击逻辑
            if (Time.time >= nextSpecialShootTime)
            {
                Shoot(specialBulletPrefab);
                nextSpecialShootTime = Time.time + specialShootInterval;
            }
        }
        else
        {
            // 重置计时器
            nextShootTime = Time.time + shootInterval;
            nextSpecialShootTime = Time.time + specialShootInterval;
        }
    }

    void Shoot(GameObject bulletType)
    {
        if (shootPoints.Length > 0)
        {
            Transform shootPoint = shootPoints[Random.Range(0, shootPoints.Length)];
            GameObject bullet = Instantiate(bulletType, shootPoint.position, Quaternion.identity);
            bullet.transform.LookAt(player.position);
        }
    }
}
