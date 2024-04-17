using UnityEngine;
using UnityEngine.AI;

public class ShootingAI : MonoBehaviour
{
    public NavMeshAgent agent;
    public Transform player;
    public LayerMask whatIsGround, whatIsPlayer;

    // 巡逻状态
    public Transform[] patrolPoints; // 指定的巡逻点
    private int currentPatrolIndex;
    public float patrolSpeed = 2f;

    // 攻击状态
    public float timeBetweenAttacks;
    private bool alreadyAttacked;
    public GameObject projectilePrefab; // 确保预制件有 Rigidbody 组件

    // 状态
    public float sightRange, attackRange;
    private bool playerInSightRange, playerInAttackRange;

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
        // 检测玩家是否在视野范围或攻击范围内
        playerInSightRange = Physics.CheckSphere(transform.position, sightRange, whatIsPlayer);
        playerInAttackRange = Physics.CheckSphere(transform.position, attackRange, whatIsPlayer);

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
        agent.speed = patrolSpeed * 2; // 追击速度
    }

    private void AttackPlayer()
    {
        agent.SetDestination(transform.position); // 停止移动
        transform.LookAt(player);

        if (!alreadyAttacked)
        {
            // 攻击玩家
            ShootPlayer();
            alreadyAttacked = true;
            Invoke(nameof(ResetAttack), timeBetweenAttacks);
        }
    }

    private void ShootPlayer()
    {
        // 确保有预制件和它上面有 Rigidbody
        GameObject projectile = Instantiate(projectilePrefab, transform.position + transform.forward * 2, Quaternion.identity);
        Rigidbody rb = projectile.GetComponent<Rigidbody>();
        if (rb != null)
        {
            rb.AddForce(transform.forward * 32f, ForceMode.Impulse);
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
