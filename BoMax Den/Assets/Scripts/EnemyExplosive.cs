using UnityEngine;

public class EnemyExplosive : MonoBehaviour
{
    [SerializeField] private float explosionRadius = 5;
    [SerializeField] private float explosionForce = 500;
    [SerializeField] private GameObject particlesPrefab;
    [SerializeField] private int damage = 10;

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player") || other.gameObject.CompareTag("Floor"))
        {
            Explode();
        }
    }

    private void Explode()
    {
        Collider[] surroundingColliders = Physics.OverlapSphere(transform.position, explosionRadius);

        foreach (Collider collider in surroundingColliders)
        {
            if (collider.CompareTag("Player"))
            {
                PlayerStats playerStats = collider.GetComponent<PlayerStats>();
                if (playerStats != null)
                {
                    playerStats.TakeDamage(damage);
                }
            }
        }

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