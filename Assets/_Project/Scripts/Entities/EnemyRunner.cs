using UnityEngine;
using ContraGoesRogue.Core.Interfaces;

namespace ContraGoesRogue.Entities
{
    [RequireComponent(typeof(HealthSystem), typeof(Rigidbody))]
    public class EnemyRunner : MonoBehaviour, IDamageable
    {
        private enum AIState { Idle, Chase }
        private AIState currentState = AIState.Idle;

        [Header("Combat Settings")]
        [SerializeField] private int damageToPlayer = 1;

        [Header("Movement & AI")]
        [SerializeField] private float moveSpeed = 4f;
        [SerializeField] private float chaseRange = 10f;

        private Rigidbody rb;
        private HealthSystem healthSystem;
        private Transform playerTransform;
        private bool isFacingRight = true;

        private void Awake()
        {
            rb = GetComponent<Rigidbody>();
            healthSystem = GetComponent<HealthSystem>();

            // Szukamy gracza na scenie przy starcie gry
            PlayerMovement player = FindFirstObjectByType<PlayerMovement>();
            if (player != null)
            {
                playerTransform = player.transform;
            }
            else
            {
                Debug.LogWarning("[EnemyRunner] Nie znaleziono obiektu PlayerMovement na scenie!");
            }
        }

        private void Start()
        {
            if (healthSystem != null)
            {
                healthSystem.OnDied.AddListener(Die);
            }
        }

        private void Update()
        {
            if (playerTransform == null) return;

            // ZARZĄDZANIE STANAMI
            float distanceToPlayer = Vector3.Distance(transform.position, playerTransform.position);
            currentState = distanceToPlayer <= chaseRange ? AIState.Chase : AIState.Idle;
        }

        private void FixedUpdate()
        {
            // WYKONYWANIE STANÓW (Fizyka)
            if (currentState == AIState.Chase && playerTransform != null)
            {
                ChasePlayer();
            }
            else
            {
                // Idle: Zatrzymaj ruch w osi X, ale pozwól grawitacji działać w Y
                rb.linearVelocity = new Vector3(0f, rb.linearVelocity.y, 0f);
            }
        }

        private void ChasePlayer()
        {
            // Obliczamy wektor kierunku do gracza
            Vector3 direction = (playerTransform.position - transform.position).normalized;

            // Biegniemy po osi X
            rb.linearVelocity = new Vector3(Mathf.Sign(direction.x) * moveSpeed, rb.linearVelocity.y, 0f);

            // Odwracanie modelu
            if (direction.x > 0 && !isFacingRight) Flip();
            else if (direction.x < 0 && isFacingRight) Flip();
        }

        private void Flip()
        {
            isFacingRight = !isFacingRight;
            rb.rotation = Quaternion.Euler(0f, isFacingRight ? 0f : 180f, 0f);
        }

        // ZADAWANIE OBRAŻEŃ GRACZOWI
        private void OnCollisionEnter(Collision collision)
        {
            if (collision.gameObject.CompareTag("Player"))
            {
                IDamageable damageable = collision.gameObject.GetComponent<IDamageable>();
                if (damageable != null)
                {
                    damageable.TakeDamage(damageToPlayer);
                }
            }
        }

        // Delegowanie do HealthSystem
        public void TakeDamage(int amount)
        {
            healthSystem?.TakeDamage(amount);
        }

        public int GetCurrentHealth()
        {
            return healthSystem != null ? healthSystem.GetCurrentHealth() : 0;
        }

        private void Die()
        {

            Destroy(gameObject);
        }

        // --- DEV TOOLS ---
        private void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.yellow;
            Gizmos.DrawWireSphere(transform.position, chaseRange);
        }
    }
}