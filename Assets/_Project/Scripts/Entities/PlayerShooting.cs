using ContraGoesRogue.Core.Input;
using UnityEngine;

namespace ContraGoesRogue.Entities
{

    public class PlayerShooting : MonoBehaviour
    {
        // field properties - to allow setting them from other scripts while keeping them visible in the inspector
        [field: SerializeField] public GameObject BulletPrefab { get; set; }
        [field: SerializeField] public Transform FirePoint { get; set; }

        private InputReader inputReader;
        private PlayerMovement playerMovement;

        private void Awake()
        {
            inputReader = GetComponent<InputReader>();
            playerMovement = GetComponent<PlayerMovement>();
            if (inputReader == null)
            {
                Debug.LogWarning($"[PlayerShooting] Brak komponentu InputReader na {gameObject.name}.");
            }
        }


        // Update is called once per frame
        void Update()
        {
            if (inputReader.ShootTriggered)
            {
                Shoot();
            }
        }

        public void Shoot()
        {
            Vector2 aimDirection;
            if (inputReader.MoveInput == Vector2.zero)
            {
                aimDirection = playerMovement.IsFacingRight ? Vector2.right : Vector2.left;
            }
            else
            {
                aimDirection = inputReader.MoveInput;
            }
            if (aimDirection.y < 0 && aimDirection.x == 0 && playerMovement.IsGrounded)
            {
                aimDirection = playerMovement.IsFacingRight ? Vector2.right : Vector2.left;
            }
            float angle = Mathf.Atan2(aimDirection.y, aimDirection.x) * Mathf.Rad2Deg;
            Quaternion bulletRotation = Quaternion.Euler(0f, 0f, angle);
            Instantiate(BulletPrefab, FirePoint.position, bulletRotation);
            inputReader.ConsumeShoot();
                     
        }
    }
}