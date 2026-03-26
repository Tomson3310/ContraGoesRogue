using UnityEngine;
using ContraGoesRogue.Core.Input;

namespace ContraGoesRogue.Entities
{
    public class PlayerMovement : MonoBehaviour
    {
        [Header("Movement")]
        [SerializeField] private float moveSpeed = 5f;
        [SerializeField] private float jumpForce = 12f;

        [Header("Ground Detection")]
        [SerializeField] private Transform groundCheck;
        [SerializeField] private float groundCheckRadius = 0.1f;
        [SerializeField] private LayerMask groundMask;
        private bool isGrounded;

        [Header("Jump Polish")]
        [SerializeField] private float jumpBufferTime = 0.2f;
        private float jumpBufferCounter;

        private Rigidbody rb;
        private InputReader inputReader;
        public bool IsFacingRight { get; private set; } = true;

        void Awake()
        {
            rb = GetComponent<Rigidbody>();
            inputReader = GetComponent<InputReader>();
        }

        void Update()
        {
            // ZARZĄDZANIE BUFOREM SKOKU
            if (inputReader.JumpTriggered)
            {
                jumpBufferCounter = jumpBufferTime;
                inputReader.ConsumeJump();
            }
            else
            {
                jumpBufferCounter -= Time.deltaTime;
            }
        }

        void FixedUpdate()
        {
            // WYKRYWANIE PODŁOŻA
            isGrounded = Physics.CheckSphere(groundCheck.position, groundCheckRadius, groundMask);

            // RUCH POZIOMY
            rb.linearVelocity = new Vector3(inputReader.MoveInput.x * moveSpeed, rb.linearVelocity.y, 0f);

            // OBRACANIE (zsynchronizowane z fizyką)
            // 0.01f - deadzone, żeby uniknąć drgań przy bardzo małych wartościach
            float horizontalInput = inputReader.MoveInput.x;
            if (horizontalInput > 0.01f && !IsFacingRight)
            {
                Flip();
            }
            else if (horizontalInput < -0.01f && IsFacingRight)
            {
                Flip();
            }

            // WYKONANIE SKOKU Z BUFORA
            if (jumpBufferCounter > 0f && isGrounded)
            {
                rb.linearVelocity = new Vector3(rb.linearVelocity.x, 0f, rb.linearVelocity.z);
                rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
                jumpBufferCounter = 0f;
            }
        }

        public void Flip()
        {
            IsFacingRight = !IsFacingRight;

            // Obliczamy nowy kąt
            Quaternion newRotation = Quaternion.Euler(0f, IsFacingRight ? 0f : 180f, 0f);

            // Przypisujemy do fizyki
            rb.rotation = newRotation;

            // Przypisujemy do transform (na potrzeby testów automatycznych)
            transform.rotation = newRotation;
        }

        private void OnDrawGizmosSelected()
        {
            if (groundCheck != null)
            {
                Gizmos.color = Color.red;
                Gizmos.DrawWireSphere(groundCheck.position, groundCheckRadius);
            }
        }
    }
}