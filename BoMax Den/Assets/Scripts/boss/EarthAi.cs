using UnityEngine;

public class EarthAi : MonoBehaviour
{
    public GameObject minionPrefab; // 小怪的预制体
    public GameObject bulletPrefab; // 子弹的预制体
    public Transform player;
    public Transform[] spawnPoints; // 小怪生成点
    public Transform[] shootingParts; // Boss的射击部位
    public float shootingInterval = 2f; // 射击间隔
    public float spawnCheckInterval = 5f; // 检查小怪生成的间隔
    public float detectionRange = 50f; // 玩家检测范围

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
        if (Time.time > lastSpawnCheckTime + spawnCheckInterval && minionsAlive < 3)
        {
            Transform spawnPoint = FindAvailableSpawnPoint();
            if (spawnPoint != null)
            {
                SpawnMinion(spawnPoint);
            }
            lastSpawnCheckTime = Time.time;
        }
    }

    private Transform FindAvailableSpawnPoint()
    {
        foreach (Transform point in spawnPoints)
        {
            if (!IsPointOccupied(point.position))
            {
                return point;
            }
        }
        return null;
    }

    private bool IsPointOccupied(Vector3 point)
    {
        Collider[] colliders = Physics.OverlapSphere(point, 1.0f);
        foreach (Collider collider in colliders)
        {
            if (collider.CompareTag("DynamicMinion") || collider.CompareTag("Player"))
            {
                return true;
            }
        }
        return false;
    }

    private void SpawnMinion(Transform spawnPoint)
    {
        GameObject minion = Instantiate(minionPrefab, spawnPoint.position, spawnPoint.rotation);
        minion.tag = "DynamicMinion"; // 将生成的小怪标签设置为“DynamicMinion”
        minion.GetComponent<Earthmini>().SetTarget(player);
        minionsAlive++;
    }

    private void Shoot()
    {
        if (shootingParts.Length > 0)
        {
            Transform shootPart = shootingParts[Random.Range(0, shootingParts.Length)];
            Instantiate(bulletPrefab, shootPart.position, Quaternion.LookRotation(player.position - shootPart.position));
        }
        else
        {
            Instantiate(bulletPrefab, transform.position, Quaternion.LookRotation(player.position - transform.position));
        }
    }

    private void DestroyAllMinions()
    {
        GameObject[] minions = GameObject.FindGameObjectsWithTag("DynamicMinion");
        foreach (GameObject minion in minions)
        {
            Destroy(minion);
        }
        minionsAlive = 0;
    }

    public void MinionDied()
    {
        minionsAlive--;
    }
}

