using UnityEngine;

public class Earthmini : MonoBehaviour
{
    public GameObject bulletPrefab;  // �ӵ���Ԥ����
    public Transform shootPoint;     // �ӵ������
    public float shootingInterval = 3f;  // ������
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
        playerTarget = target;  // ����Ŀ��
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
        // ��С�ֱ�����ʱ���ã�ȷ��û�������Ĳ���
    }
}

