using UnityEngine;
using ContraGoesRogue.Core.Interfaces;

namespace ContraGoesRogue.Entities
{
    [RequireComponent(typeof(HealthSystem), typeof(Rigidbody))]
    public class EnemyDrone : MonoBehaviour, IDamageable
    {
        private enum AIState { Patrol, Attack, Escape }
        private AIState currentState = AIState.Patrol;

        [Header("Combat Settings")]
        [SerializeField] private GameObject bombPrefab;
        [SerializeField] private Transform firePoint;
        [SerializeField] private float patrolRange = 16f; // Range at which the drone will switch from patrolling to attacking
        [SerializeField] private float attackRange = 0.2f; // Range at which the drone will drop a bomb on the player
        [SerializeField] private bool isFacingRight = true;
        [SerializeField] private float moveSpeed = 3f;
        [SerializeField] private float patrolDistance = 5f;
        [SerializeField] private float attackAcceleration = 1.5f;
        [SerializeField] private float escapeSpeed = 4f;
        [SerializeField] private float escapeDisappearDistance = 15f;
        [Header("Flight Settings")]
        [SerializeField] private float hoverHeight = 6f; // How high the drone hovers above the player
        [SerializeField] private float obstacleCheckDistance = 1.5f; // How far ahead the drone checks for obstacles
        [SerializeField] private LayerMask obstacleMask;
        private Vector3 patrolStartPoint;
        private Rigidbody rb;
        private Vector3 targetVelocity;

        private Transform playerTransform;
        private HealthSystem healthSystem;

        private void Awake()
        {
            healthSystem = GetComponent<HealthSystem>();
            rb = GetComponent<Rigidbody>();
            PlayerMovement player = FindFirstObjectByType<PlayerMovement>();
            if (player != null)
            {
                playerTransform = player.transform;
            }
        }

        private void Start()
        {
            healthSystem.OnDied.AddListener(Die);
            patrolStartPoint = transform.position;
        }

        private void Update()
        {
            if (playerTransform == null) return;
            switch (currentState)
            {
                case AIState.Patrol:
                    HandlePatrol();
                    break;
                case AIState.Attack:
                    HandleAttack();
                    break;
                case AIState.Escape:
                    HandleEscape();
                    break;
            }
        }

        private void FixedUpdate()
        {
            rb.linearVelocity = targetVelocity;
        }

        private void HandlePatrol()
        {
            // Counting the vertical distance to the player to maintain hover height
            float velocityY = GetPatrolVelocityY();
            float distanceToPlayer = Vector3.Distance(transform.position, playerTransform.position);

            // Movement and wall avoidance
            if (IsWallAhead() || Mathf.Abs(transform.position.x - patrolStartPoint.x) >= patrolDistance)
            {
                Flip();
            }

            // Apply velocity
            targetVelocity = new Vector3((isFacingRight ? 1f : -1f) * moveSpeed, velocityY, 0f);

            // State change condition
            if (distanceToPlayer <= patrolRange)
            {
                currentState = AIState.Attack;
            }
        }

        private void HandleAttack()
        {
            // 1. Utrzymujemy wysokość
            float velocityY = GetHoverVelocityY();

            // 2. Obliczamy, w którą stronę musimy lecieć, aby dopaść gracza
            float distanceX = playerTransform.position.x - transform.position.x;
            float directionX = Mathf.Sign(distanceX); // Zwróci 1 (prawo) lub -1 (lewo)

            // 3. Upewniamy się, że dron patrzy w stronę gracza
            if (directionX > 0 && !isFacingRight)
            {
                Flip();
            }
            else if (directionX < 0 && isFacingRight)
            {
                Flip();
            }

            // 4. Ustalamy prędkość poziomą
            float velocityX = directionX * moveSpeed * attackAcceleration;

            // 5. Zapobiegamy wbijaniu się w ściany, jeśli gracz np. schowa się pod nawisem
            if (IsWallAhead())
            {
                velocityX = 0f; // Dron zatrzymuje się w poziomie, ale wciąż utrzymuje wysokość w osi Y
            }

            // 6. Przekazujemy wyliczoną prędkość (targetVelocity) dla metody FixedUpdate
            targetVelocity = new Vector3(velocityX, velocityY, 0f);

            // 7. Warunek zrzutu bomby - sprawdzamy, czy znaleźliśmy się bezpośrednio nad graczem
            if (Mathf.Abs(distanceX) <= attackRange)
            {
                DropBomb();
                currentState = AIState.Escape;
                GetComponent<Collider>().enabled = false;
            }
        }

        private void HandleEscape()
        {
            targetVelocity = new Vector3((isFacingRight ? 1f : -1f) * escapeSpeed, 1f * escapeSpeed, 0f);
            if (transform.position.y >= playerTransform.position.y + escapeDisappearDistance)
            {
                Destroy(gameObject);
            }
        }

        private float GetPatrolVelocityY()
        {
            // Dron dąży wyłącznie do swojej oryginalnej wysokości startowej, ignorując gracza
            float heightDifference = patrolStartPoint.y - transform.position.y;
            return heightDifference * 2f;
        }

        // Count the horizontal distance to the player
        private float GetHoverVelocityY()
        {
            float targetY = playerTransform.position.y + hoverHeight;
            float heightDifference = targetY - transform.position.y;
            return heightDifference * 2f;
        }

        // Check for obstacles in the direction of movement
        private bool IsWallAhead()
        {
            Vector3 castDirection = isFacingRight ? Vector3.right : Vector3.left;
            return Physics.Raycast(transform.position, castDirection, obstacleCheckDistance, obstacleMask);
        }

        private void Flip()
        {
            isFacingRight = !isFacingRight;
        }

        private void Die()
        {
            Debug.Log($"{gameObject.name} został zniszczony!");
            Destroy(gameObject);
        }

        private void DropBomb()
        {
            Instantiate(bombPrefab, firePoint.position, Quaternion.identity);
        }

        public void TakeDamage(int amount)
        {
            healthSystem.TakeDamage(amount);
        }

        public int GetCurrentHealth()
        {
            return healthSystem.GetCurrentHealth();
        }
    }
}
