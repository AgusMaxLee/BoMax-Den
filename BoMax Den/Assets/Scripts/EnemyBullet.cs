using UnityEngine;

public class EnemyBullet : MonoBehaviour
{
    [SerializeField] private float speed = 20f;
    [SerializeField] private int damage = 10;
    [SerializeField] private string targetTag = "Player";

    private Rigidbody rb;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();

        // ���Ի�ȡ���λ��
        GameObject player = GameObject.FindGameObjectWithTag(targetTag);
        if (player != null)
        {
            // ����ָ����ҵķ�������
            Vector3 targetDirection = (player.transform.position - transform.position).normalized;
            // �����ӵ����ٶ�Ϊ�÷�������ٶ�ֵ
            rb.velocity = targetDirection * speed;
        }
        else
        {
            // ���û���ҵ���ң��ӵ�Ĭ��ֱ�߷���
            rb.velocity = transform.forward * speed;
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag(targetTag))
        {
            PlayerStats playerStats = collision.gameObject.GetComponent<PlayerStats>();
            if (playerStats != null)
            {
                playerStats.TakeDamage(damage);
            }

            Destroy(gameObject);
        }
    }
}

