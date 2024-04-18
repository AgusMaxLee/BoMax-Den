using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sword : MonoBehaviour
{
    [SerializeField] private int damage = 5;

    private void OnTriggerEnter(Collider other)
    {
        EnemyHealth enemyHealth = other.GetComponent<EnemyHealth>();
        if (enemyHealth != null)
        {
            enemyHealth.TakeDamage(damage);
        }
        else
        {
            DummyHealth dummyHealth = other.GetComponent<DummyHealth>();
            if (dummyHealth != null)
            {
                dummyHealth.TakeDamage(damage);
            }
        }
    }
}
