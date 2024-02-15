using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static EnemyHealth;

public class Bullet : MonoBehaviour
{
    [SerializeField] public float speed = 1f;
    [SerializeField] public int damage = 5;
    public GameObject bulletPrefab;
    public Rigidbody rb;
    // Update is called once per frame
    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }
    public void OnTriggerEnter(Collider collision)
    {
        if (collision.CompareTag("Enemy"))
        {
            collision.GetComponent<EnemyHealth>().TakeDamage(damage);
        }
    }   
}
