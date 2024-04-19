using UnityEngine;
using UnityEngine.AI;

public class EnemyAI : MonoBehaviour
{
    public Transform[] waypoints;
    public float patrolSpeed = 2f;
    public float chaseSpeed = 5f;
    public float chaseDistance = 10f;
    public float explosionDistance = 0.5f;
    public int damage = 20;
    public float loseInterestDistance = 15f;
    public GameObject deathParticles;

    [SerializeField] private AudioClip deathSound;
    [SerializeField] private Transform playerTransform;

    private NavMeshAgent agent;
    private int waypointIndex = 0;
    private bool isChasing = false;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        agent.autoBraking = false;
        GoToNextWaypoint();
    }

    void Update()
    {
        if (playerTransform == null)
        {
            Debug.LogWarning("Player transform not assigned in EnemyAI script.");
            return;
        }

        float distanceToPlayer = Vector3.Distance(transform.position, playerTransform.position);

        if (isChasing)
        {
            if (distanceToPlayer > loseInterestDistance)
            {
                isChasing = false;
                GoToNextWaypoint();
            }
            else if (distanceToPlayer <= explosionDistance)
            {
                Explode();
            }
            else
            {
                agent.SetDestination(playerTransform.position);
                agent.speed = chaseSpeed;
            }
        }
        else
        {
            if (!agent.pathPending && agent.remainingDistance < 0.5f)
            {
                GoToNextWaypoint();
            }

            if (distanceToPlayer <= chaseDistance && IsPlayerInFront())
            {
                isChasing = true;
            }
        }
    }

    void GoToNextWaypoint()
    {
        if (waypoints.Length == 0)
            return;

        agent.destination = waypoints[waypointIndex].position;
        agent.speed = patrolSpeed;
        waypointIndex = (waypointIndex + 1) % waypoints.Length;
    }

    bool IsPlayerInFront()
    {
        if (playerTransform == null)
            return false;

        Vector3 directionToPlayer = (playerTransform.position - transform.position).normalized;
        return Vector3.Dot(directionToPlayer, transform.forward) > 0;
    }

    void Explode()
    {
        PlayerStats playerStats = playerTransform.GetComponent<PlayerStats>();

        if (playerStats != null)
        {
            playerStats.TakeDamage(damage);
        }

        if (deathParticles != null)
        {
            GameObject particles = Instantiate(deathParticles, transform.position, Quaternion.identity);
            Destroy(particles, 2f);
        }
        AudioManager.Instance.PlaySound(deathSound);
        Destroy(gameObject);
    }
}
