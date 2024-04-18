using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using UnityEngine;
using static DummyHealth;

public class PlayerBullet : MonoBehaviour
{
    [SerializeField] private int damage = 5;

    private Rigidbody rb;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    private void OnTriggerEnter(Collider other)
    {
        // Check if the collider's game object is tagged as "Enemy"
        if (other.gameObject.CompareTag("Enemy"))
        {
            EnemyHealth enemyHealth = other.gameObject.GetComponent<EnemyHealth>();
            if (enemyHealth != null)
            {
                enemyHealth.TakeDamage(damage);
            }
            else
            {
                DummyHealth dummyHealth = other.gameObject.GetComponent<DummyHealth>();
                if (dummyHealth != null)
                {
                    dummyHealth.TakeDamage(damage);
                }
            }

            Destroy(gameObject);
        }
    }
}