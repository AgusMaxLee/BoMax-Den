using UnityEngine;
using UnityEngine.AI;

public class ShootingAI : MonoBehaviour
{
    public NavMeshAgent agent;
    public Transform player;
    public LayerMask whatIsGround, whatIsPlayer;

    // Ѳ��״̬
    public Transform[] patrolPoints; // ָ����Ѳ�ߵ�
    private int currentPatrolIndex;
    public float patrolSpeed = 2f;

    // ����״̬
    public float timeBetweenAttacks;
    private bool alreadyAttacked;
    public GameObject projectilePrefab; // ȷ��Ԥ�Ƽ��� Rigidbody ���

    // ״̬
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
        // �������Ƿ�����Ұ��Χ�򹥻���Χ��
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
        agent.speed = patrolSpeed * 2; // ׷���ٶ�
    }

    private void AttackPlayer()
    {
        agent.SetDestination(transform.position); // ֹͣ�ƶ�
        transform.LookAt(player);

        if (!alreadyAttacked)
        {
            // �������
            ShootPlayer();
            alreadyAttacked = true;
            Invoke(nameof(ResetAttack), timeBetweenAttacks);
        }
    }

    private void ShootPlayer()
    {
        // ȷ����Ԥ�Ƽ����������� Rigidbody
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
