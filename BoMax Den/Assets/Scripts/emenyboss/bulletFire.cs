using UnityEngine;
using System.Collections;

public class BulletFire : MonoBehaviour
{
    public int damagePerSecond = 10; // ÿ����ɵ��˺�
    public float damageDuration = 3f; // ����˺��ĳ���ʱ��

    private bool hasHit = false; // ����ӵ��Ƿ��Ѿ��������
    private float hitTime; // ��¼�ӵ�����ʱ��

    void OnCollisionEnter(Collision collision)
    {
        // ����Ƿ���ײ�����
        if (collision.gameObject.CompareTag("Player") && !hasHit)
        {
            // ����ӵ�������
            hasHit = true;
            hitTime = Time.time;

            // ������������˺���Э��
            StartCoroutine(DamagePlayerCoroutine(collision.gameObject));
        }
    }

    IEnumerator DamagePlayerCoroutine(GameObject player)
    {
        while (Time.time - hitTime <= damageDuration)
        {
            // ÿ���������һ�������˺�
            PlayerHealth playerHealth = player.GetComponent<PlayerHealth>();
            if (playerHealth != null)
            {
                playerHealth.TakeDamage(damagePerSecond);
            }

            yield return new WaitForSeconds(1f); // �ȴ�1����
        }

        // �����˺�����ʱ��������ӵ�
        Destroy(gameObject);
    }
}

