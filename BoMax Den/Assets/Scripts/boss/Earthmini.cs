using UnityEngine;

public class Earthmini : MonoBehaviour
{
    public GameObject bulletPrefab;  // 子弹的预制体
    public Transform shootPoint;     // 子弹发射点
    public float shootingInterval = 3f;  // 射击间隔
    private float lastShootTime;
    public Transform playerTarget;

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
        playerTarget = target;  // 设置目标
    }

    private void Shoot()
    {
        if (playerTarget != null)
        {
            var bullet = Instantiate(bulletPrefab, shootPoint.position, Quaternion.identity);
            bullet.transform.LookAt(playerTarget.position);
        }
    }

    private void OnDestroy()
    {
        // 当小怪被销毁时调用，确保没有遗留的操作
    }
}

