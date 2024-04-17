using UnityEngine;

public class coldai : MonoBehaviour
{
    public Transform player;  // ��ҵ�Transform
    public GameObject bulletPrefab;  // ��ͨ�ӵ���Ԥ����
    public GameObject specialBulletPrefab;  // �����ӵ���Ԥ����
    public Transform[] shootPoints;  // ���������
    public float shootInterval = 0.7f;  // ��ͨ������
    public float specialShootInterval = 3.0f;  // �����ӵ�������
    public float shootRange = 20.0f;  // �������

    private float nextShootTime;
    private float nextSpecialShootTime;

    void Update()
    {
        float distance = Vector3.Distance(transform.position, player.position);

        // ������������Χ��
        if (distance <= shootRange)
        {
            // ��ͨ�ӵ�����߼�
            if (Time.time >= nextShootTime)
            {
                Shoot(bulletPrefab);
                nextShootTime = Time.time + shootInterval;
            }

            // �����ӵ�����߼�
            if (Time.time >= nextSpecialShootTime)
            {
                Shoot(specialBulletPrefab);
                nextSpecialShootTime = Time.time + specialShootInterval;
            }
        }
        else
        {
            // ���ü�ʱ��
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
