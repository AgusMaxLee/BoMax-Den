using UnityEngine;

public class BossController : MonoBehaviour
{
    public GameObject minionPrefab; // С�ֵ�Ԥ����
    public GameObject bulletPrefab; // �ӵ���Ԥ����
    public Transform player;
    public Transform[] spawnPoints; // С�����ɵ�
    public float shootingInterval = 2f; // ������
    public float spawnCheckInterval = 5f; // ���С�����ɵļ��
    public float detectionRange = 50f; // ��Ҽ�ⷶΧ

    private float lastShootTime;
    private float lastSpawnCheckTime;
    private int minionsAlive = 0;

    void Update()
    {
        float distance = Vector3.Distance(transform.position, player.position);

        if (distance <= detectionRange)
        {
            HandleShooting();
            HandleMinions();
        }
        else
        {
            DestroyAllMinions();
        }
    }

    private void HandleShooting()
    {
        if (Time.time > lastShootTime + shootingInterval)
        {
            Shoot();
            lastShootTime = Time.time;
        }
    }

    private void HandleMinions()
    {
        if (Time.time > lastSpawnCheckTime + spawnCheckInterval)
        {
            if (minionsAlive < 3)
            {
                SpawnMinion();
            }
            lastSpawnCheckTime = Time.time;
        }
    }

    private void Shoot()
    {
        Instantiate(bulletPrefab, transform.position, Quaternion.LookRotation(player.position - transform.position));
    }

    private void SpawnMinion()
    {
        if (spawnPoints.Length > 0)
        {
            Transform spawnPoint = spawnPoints[Random.Range(0, spawnPoints.Length)];
            GameObject minion = Instantiate(minionPrefab, spawnPoint.position, spawnPoint.rotation);
            minion.GetComponent<MinionController>().SetTarget(player); // �趨Ŀ��Ϊ���
            minionsAlive++;
        }
    }

    private void DestroyAllMinions()
    {
        foreach (Transform spawnPoint in spawnPoints)
        {
            foreach (Transform child in spawnPoint)
            {
                Destroy(child.gameObject);
            }
        }
        minionsAlive = 0;
    }

    public void MinionDied()
    {
        minionsAlive--;
    }
}
