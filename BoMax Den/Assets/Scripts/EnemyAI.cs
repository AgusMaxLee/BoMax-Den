using UnityEngine;
using UnityEngine.AI;
using System.Collections.Generic;

public class EnemyAI : MonoBehaviour
{
    public Transform[] patrolPoints;
    private int currentPatrolIndex = 0;

    public float patrolSpeed = 3.5f;  // Ñ²ÂßËÙ¶È
    public float chaseSpeed = 6.0f;   // ×·ÖðËÙ¶È
    public float playerDetectionDistance = 10f;  // Íæ¼Ò¼ì²â¾àÀë
    public float returnToPatrolDistance = 15f;  // Íæ¼ÒÔ¶Àëºó·µ»ØÑ²ÂßµÄ¾àÀë

    public GameObject bulletPrefab;
    public Transform bulletSpawnPoint;
    public float bulletSpeed = 10f;
    public float bulletLifetime = 2f;
    public float shootingDistance = 5f;

    private Transform playerTransform;
    private NavMeshAgent agent;
    private Vector3 originalPosition;

    private List<GameObject> bulletPool = new List<GameObject>();

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        originalPosition = transform.position;
        SetDestination(patrolPoints[currentPatrolIndex].position);

        InitializeBulletPool();
    }

    void Update()
    {
        DetectPlayer();

        if (playerTransform != null)
        {
            ChasePlayer();

            if (Vector3.Distance(transform.position, playerTransform.position) <= shootingDistance)
            {
                ShootBullet();
            }
        }
        else
        {
            Patrol();
        }
    }

    void DetectPlayer()
    {
        RaycastHit hit;

        if (Physics.Raycast(transform.position, transform.forward, out hit, playerDetectionDistance) && hit.collider.CompareTag("Player"))
        {
            playerTransform = hit.collider.transform;
        }
        else
        {
            playerTransform = null;
        }
    }

    void ChasePlayer()
    {
        agent.destination = playerTransform.position;
        agent.speed = chaseSpeed;

        if (Vector3.Distance(transform.position, playerTransform.position) > returnToPatrolDistance)
        {
            playerTransform = null;
            agent.speed = patrolSpeed;
            ReturnToPatrol();
        }
    }

    void Patrol()
    {
        if (agent.remainingDistance < 0.5f)
        {
            SetNextPatrolPoint();
        }
    }

    void ReturnToPatrol()
    {
        SetDestination(originalPosition);
    }

    void SetNextPatrolPoint()
    {
        currentPatrolIndex = (currentPatrolIndex + 1) % patrolPoints.Length;
        SetDestination(patrolPoints[currentPatrolIndex].position);
    }

    void SetDestination(Vector3 targetPosition)
    {
        agent.SetDestination(targetPosition);
    }

    void InitializeBulletPool()
    {
        for (int i = 0; i < 10; i++)
        {
            GameObject bullet = Instantiate(bulletPrefab, Vector3.zero, Quaternion.identity);
            bullet.SetActive(false);
            bulletPool.Add(bullet);
        }
    }

    void ShootBullet()
    {
        GameObject bullet = GetInactiveBullet();

        if (bullet != null)
        {
            bullet.transform.position = bulletSpawnPoint.position;
            bullet.transform.rotation = bulletSpawnPoint.rotation;
            bullet.SetActive(true);

            Rigidbody bulletRb = bullet.GetComponent<Rigidbody>();
            bulletRb.velocity = bulletSpawnPoint.forward * bulletSpeed;

            StartCoroutine(DeactivateBulletAfterTime(bullet, bulletLifetime));
        }
    }

    GameObject GetInactiveBullet()
    {
        for (int i = 0; i < bulletPool.Count; i++)
        {
            if (!bulletPool[i].activeInHierarchy)
            {
                return bulletPool[i];
            }
        }

        return null;
    }

    System.Collections.IEnumerator DeactivateBulletAfterTime(GameObject bullet, float time)
    {
        yield return new WaitForSeconds(time);
        bullet.SetActive(false);
    }
}
