using ContraGoesRogue.Core.Interfaces;
using UnityEngine;

namespace ContraGoesRogue.Weapons
{
    public class Projectile : MonoBehaviour
    {
        [SerializeField] private float speed = 10f;
        [SerializeField] private int damage = 1;
        [SerializeField] private float lifetime = 3f;
        private Rigidbody rb;


        private void Awake()
        {
            rb = GetComponent<Rigidbody>();
            if (rb == null)
            {
                Debug.LogWarning($"[Projectile] Brak komponentu Rigidbody na {gameObject.name}.");
            }
        }
        void Start()
        {
            rb.linearVelocity = transform.right * speed;
            Destroy(gameObject, lifetime);
        }

        
        private void OnTriggerEnter(Collider other)
        {
            if (other.TryGetComponent(out IDamageable damageable))
            {
                damageable.TakeDamage(damage);
            }
            Destroy(gameObject);
        }
    }
}