using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyAI : MonoBehaviour
{
    public Transform[] patrolPoints;
    private int currentPatrolIndex = 0;

    public float patrolSpeed = 3.5f;
    public float chaseSpeed = 6.0f;
    public float playerDetectionDistance = 10f;
    public float returnToPatrolDistance = 15f;
    public float shootingDistance = 5f;
    public float detectionRayWidth = 1f;

    public GameObject bulletPrefab;
    public Transform bulletSpawnPoint;
    public float bulletSpeed = 10f;
    public float bulletLifetime = 2f;

    private Transform playerTransform;
    private NavMeshAgent agent;
    private Vector3 originalPosition;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        originalPosition = transform.position;
        SetDestination(patrolPoints[currentPatrolIndex].position);
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

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, returnToPatrolDistance);

        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position, playerDetectionDistance);

        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, shootingDistance);

        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, agent.stoppingDistance);

        Gizmos.color = Color.white;
        Gizmos.DrawRay(transform.position, transform.forward * playerDetectionDistance);
    }

    void DetectPlayer()
    {
        RaycastHit hit;

        if (Physics.SphereCast(transform.position, detectionRayWidth, transform.forward, out hit, playerDetectionDistance) && hit.collider.CompareTag("Player"))
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

    void ShootBullet()
    {
        GameObject bullet = Instantiate(bulletPrefab, bulletSpawnPoint.position, bulletSpawnPoint.rotation);

        Rigidbody bulletRb = bullet.GetComponent<Rigidbody>();
        bulletRb.velocity = bulletSpawnPoint.forward * bulletSpeed;

        StartCoroutine(DeactivateBulletAfterTime(bullet, bulletLifetime));
    }

    IEnumerator DeactivateBulletAfterTime(GameObject bullet, float time)
    {
        yield return new WaitForSeconds(time);
        Destroy(bullet);
    }
}

