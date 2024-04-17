using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Explosive : MonoBehaviour
{
    [SerializeField] private float triggerForce = 0.5f; 
    [SerializeField] private float explosionRadius = 5;
    [SerializeField] private float explosionForce = 500;
    [SerializeField] private GameObject particlesPrefab;
    [SerializeField] private int damage = 10;
    private Rigidbody rb;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Enemy") || collision.gameObject.CompareTag("Floor"))
        {
            Explode();
        }
    }

    private void Explode()
    {
        var surroundingObjects = Physics.OverlapSphere(transform.position, explosionRadius);

        foreach (var obj in surroundingObjects)
        {
            var rb = obj.GetComponent<Rigidbody>();
            if (rb == null) continue;

            rb.AddExplosionForce(explosionForce, transform.position, explosionRadius, 1);

            var enemyHealth = obj.GetComponent<EnemyHealth>();
            if (enemyHealth != null)
            {
                enemyHealth.TakeDamage(damage);
            }
        }
        GameObject particle = Instantiate(particlesPrefab, transform.position, Quaternion.identity);
        Destroy(gameObject);
        Destroy(particle, 2f);
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, explosionRadius);
    }
}
