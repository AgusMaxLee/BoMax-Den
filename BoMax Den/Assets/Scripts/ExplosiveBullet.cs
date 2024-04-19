using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExplosiveBullet : MonoBehaviour
{
    [SerializeField] private float triggerForce = 0.5f;
    [SerializeField] private float explosionRadius = 5;
    [SerializeField] private float explosionForce = 500;
    [SerializeField] private GameObject particlesPrefab;
    [SerializeField] private int damage = 10;
    private Rigidbody rb;

    [SerializeField] private AudioClip explosionSFX;
    private void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Enemy") || other.gameObject.CompareTag("Floor"))
        {
            Explode();
        }
    }

    private void Explode()
    {
        Collider[] surroundingColliders = Physics.OverlapSphere(transform.position, explosionRadius);

        foreach (Collider collider in surroundingColliders)
        {
            Rigidbody rb = collider.GetComponent<Rigidbody>();
            if (rb != null)
            {
                rb.AddExplosionForce(explosionForce, transform.position, explosionRadius, 1);
            }

            EnemyHealth enemyHealth = collider.GetComponent<EnemyHealth>();
            if (enemyHealth != null)
            {
                enemyHealth.TakeDamage(damage);
            }
            else
            {
                DummyHealth dummyHealth = collider.GetComponent<DummyHealth>();
                if (dummyHealth != null)
                {
                    dummyHealth.TakeDamage(damage);
                }
            }
        }

        AudioManager.Instance.PlaySound(explosionSFX);
        // Instantiate and destroy particle effects
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
