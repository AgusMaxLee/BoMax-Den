using UnityEngine;

public class EnemyBullet : MonoBehaviour
{
    [SerializeField] private float speed = 20f;    // �ӵ����ٶ�
    [SerializeField] private int damage = 10;      // �ӵ��������ɵ��˺�
    [SerializeField] private string targetTag = "Player"; // ������ҵı�ǩ��"Player"

    private Rigidbody rb;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.velocity = transform.forward * speed; // �ӵ�����ǰ����������ָ���ٶ��ƶ�
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag(targetTag))
        {
            // ���Ի�ȡ��ҵĽ������
            PlayerHealth playerHealth = collision.gameObject.GetComponent<PlayerHealth>();
            if (playerHealth != null)
            {
                playerHealth.TakeDamage(damage); // �����Ӧ���˺�
            }

            Destroy(gameObject); // ����Ŀ��������ӵ�
        }
    }
}
