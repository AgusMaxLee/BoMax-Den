using UnityEngine;
using UnityEngine.AI; // �������ڷ��ʵ���ϵͳ�������ռ�

public class EnemyAI : MonoBehaviour
{
    public Transform[] waypoints;
    public float patrolSpeed = 2f;
    public float chaseSpeed = 5f;
    public float chaseDistance = 10f;
    public float explosionDistance = 0.5f;
    public float damage = 20f;
    public float loseInterestDistance = 15f;

    private Transform player;
    private NavMeshAgent agent;
    private int waypointIndex = 0;
    private bool isChasing = false;

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
        agent = GetComponent<NavMeshAgent>(); // ��ȡNavMeshAgent���
        agent.autoBraking = false; // ��ֹ�ӽ�Ŀ��ʱ����
        GoToNextWaypoint();
    }

    void Update()
    {
        float distanceToPlayer = Vector3.Distance(transform.position, player.position);

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
                agent.SetDestination(player.position);
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
        Vector3 directionToPlayer = (player.position - transform.position).normalized;
        return Vector3.Dot(directionToPlayer, transform.forward) > 0;
    }

    void Explode()
    {
        Debug.Log($"BOOM! Caused {damage} damage to the player.");
        Destroy(gameObject);
    }
}
