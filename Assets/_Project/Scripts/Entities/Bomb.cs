using UnityEngine;
using ContraGoesRogue.Core.Interfaces;

namespace ContraGoesRogue.Entities
{
    public class Bomb : MonoBehaviour
    {
        [Header("Explosion Settings")]
        [SerializeField] private float explosionRadius = 5f;
        [SerializeField] private int damage = 2;
        [SerializeField] private LayerMask damageableLayers;
        private Rigidbody rb;

        private void Awake()
        {
            rb = GetComponent<Rigidbody>();
            if (rb == null)
            {
                Debug.LogWarning($"[Bomb] Brak komponentu Rigidbody na {gameObject.name}.");
            }
        }
        private void Explode()
        {
            Collider[] hitColliders = Physics.OverlapSphere(transform.position, explosionRadius, damageableLayers);

            foreach (Collider hitCollider in hitColliders)
            {
                if (hitCollider.TryGetComponent(out IDamageable damageable))
                {
                    damageable.TakeDamage(damage);
                }
            }

            // Efekty wizualne (Opcjonalnie, na przyszłość)
            // Instantiate(explosionParticlePrefab, transform.position, Quaternion.identity);

            Destroy(gameObject);
        }

        private void OnTriggerEnter(Collider other)
        {
            Explode();
        }

        private void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(transform.position, explosionRadius);
        }
    }
}
