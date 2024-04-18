using UnityEngine;
using UnityEngine.AI;

public class ShootingAI : MonoBehaviour
{
    public NavMeshAgent agent;
    public Transform player;

    public Transform[] patrolPoints;
    private int currentPatrolIndex;
    public float patrolSpeed = 2f;

    public float timeBetweenAttacks;
    private bool alreadyAttacked;
    public GameObject projectilePrefab;
    public Transform bulletSpawnPoint;

    public float sightRange, attackRange;
    private bool playerInSightRange, playerInAttackRange;

    public float projectileSpeed = 32f;

    private void Awake()
    {
        player = GameObject.FindWithTag("Player")?.transform;
        agent = GetComponent<NavMeshAgent>();
    }

    private void Start()
    {
        agent.autoBraking = false;
        currentPatrolIndex = 0;
        SetDestinationToPatrolPoint();
    }

    private void Update()
    {
        playerInSightRange = CheckSphere(transform.position, sightRange, "Player");
        playerInAttackRange = CheckSphere(transform.position, attackRange, "Player");

        if (!playerInSightRange && !playerInAttackRange)
        {
            Patroling();
        }
        else
        {
            if (playerInSightRange && !playerInAttackRange)
            {
                ChasePlayer();
            }
            else if (playerInSightRange && playerInAttackRange)
            {
                AttackPlayer();
            }
        }
    }

    private bool CheckSphere(Vector3 position, float radius, string tag)
    {
        Collider[] colliders = Physics.OverlapSphere(position, radius);
        foreach (Collider collider in colliders)
        {
            if (collider.CompareTag(tag))
            {
                return true;
            }
        }
        return false;
    }

    private void Patroling()
    {
        if (agent.remainingDistance <= agent.stoppingDistance)
        {
            currentPatrolIndex = (currentPatrolIndex + 1) % patrolPoints.Length;
            SetDestinationToPatrolPoint();
        }
    }

    private void SetDestinationToPatrolPoint()
    {
        if (patrolPoints.Length > 0)
        {
            agent.SetDestination(patrolPoints[currentPatrolIndex].position);
            agent.speed = patrolSpeed;
        }
    }

    private void ChasePlayer()
    {
        agent.SetDestination(player.position);
        agent.speed = patrolSpeed * 2;
    }

    private void AttackPlayer()
    {
        agent.SetDestination(transform.position);
        transform.LookAt(player);

        if (!alreadyAttacked)
        {
            ShootPlayer();
            alreadyAttacked = true;
            Invoke(nameof(ResetAttack), timeBetweenAttacks);
        }
    }

    private void ShootPlayer()
    {
        if (bulletSpawnPoint == null) return;

        GameObject projectile = Instantiate(projectilePrefab, bulletSpawnPoint.position, Quaternion.identity);
        Rigidbody rb = projectile.GetComponent<Rigidbody>();
        if (rb != null)
        {
            rb.AddForce(bulletSpawnPoint.forward * projectileSpeed, ForceMode.Impulse);
        }
        else
        {
            Debug.LogWarning("Projectile prefab does not have a Rigidbody component attached.");
        }
    }

    private void ResetAttack()
    {
        alreadyAttacked = false;
    }
}
