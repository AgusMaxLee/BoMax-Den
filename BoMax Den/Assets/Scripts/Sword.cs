using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sword : MonoBehaviour
{
    [SerializeField] private int damage = 5;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Enemy"))
        {
            Debug.Log("Sword triggered enemy!");
            other.GetComponent<EnemyHealth>().TakeDamage(damage);
        }
    }
}
