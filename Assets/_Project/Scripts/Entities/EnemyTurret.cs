using UnityEngine;
using ContraGoesRogue.Core.Interfaces;

namespace ContraGoesRogue.Entities
{
    [RequireComponent(typeof(HealthSystem))]
    public class EnemyTurret : MonoBehaviour, IDamageable
    {
        [Header("Combat Settings")]
        [SerializeField] private GameObject bulletPrefab;
        [SerializeField] private Transform firePoint;
        [SerializeField] private float fireRate = 1.5f;
        [SerializeField] private float attackRange = 12f;
        [SerializeField] private bool isFacingRight = true;

        private float fireTimer;
        private Transform playerTransform;
        private HealthSystem healthSystem;

        private void Awake()
        {
            healthSystem = GetComponent<HealthSystem>();
            PlayerMovement player = FindFirstObjectByType<PlayerMovement>();
            if (player != null)
            {
                playerTransform = player.transform;
            }
        }

        private void Start()
        {
            healthSystem.OnDied.AddListener(Die);
        }

        private void Update()
        {
            if (playerTransform == null) return;

            fireTimer -= Time.deltaTime;
            float distanceToPlayer = Vector3.Distance(transform.position, playerTransform.position);

            Vector2 directionToPlayer = (playerTransform.position - transform.position);

            bool isPlayerInFront = isFacingRight ? directionToPlayer.x > 0 : directionToPlayer.x < 0;

            if (distanceToPlayer <= attackRange && isPlayerInFront && fireTimer <= 0f)
            {
                Shoot(directionToPlayer);
                fireTimer = fireRate;
            }
        }

        private void Shoot (Vector2 direction)
        {
            float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
            Quaternion bulletRotation = Quaternion.Euler(0f, 0f, angle);
            Instantiate(bulletPrefab, firePoint.position, bulletRotation);
        }

        public void TakeDamage(int amount)
        {
            healthSystem.TakeDamage(amount);
        }

        public int GetCurrentHealth()
        {
            return healthSystem.GetCurrentHealth();
        }

        private void Die()
        {
            Debug.Log($"{gameObject.name} został zniszczony!");
            Destroy(gameObject);
        }

        private void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.yellow;
            Gizmos.DrawWireSphere(transform.position, attackRange);

            Gizmos.color = Color.red;
            Vector3 faceDirection = isFacingRight ? Vector3.right : Vector3.left;
            Gizmos.DrawRay(transform.position, faceDirection * attackRange);
        }
    }
}