using UnityEngine;
using ContraGoesRogue.Core.Input;

namespace ContraGoesRogue.Entities
{
    public class PlayerMovement : MonoBehaviour
    {
        // Start is called once before the first execution of Update after the MonoBehaviour is created
        [Header("Movement")]
        [SerializeField] private float moveSpeed = 5f;
        [SerializeField] private float jumpForce = 7f;

        [Header("Ground Detection")]
        [SerializeField] private Transform groundCheck;
        [SerializeField] private float groundCheckRadius = 0.2f;
        [SerializeField] private LayerMask groundMask;
        private bool isGrounded;


        private Rigidbody rb;
        private InputReader inputReader;
        public bool IsFacingRight { get; private set; } = true;



        void Awake()
        {
            rb = GetComponent<Rigidbody>();
            inputReader = GetComponent<InputReader>();
        }

        void FixedUpdate()
        {
            isGrounded = Physics.CheckSphere(groundCheck.position, groundCheckRadius, groundMask);

            rb.linearVelocity = new Vector3(inputReader.MoveInput.x * moveSpeed, rb.linearVelocity.y, 0f);
                        
            float horizontalInput = inputReader.MoveInput.x;

            if (horizontalInput > 0 && !IsFacingRight)
            {
                Flip();
            }
            else if (horizontalInput < 0 && IsFacingRight)
            {
                Flip();
            }

            if ((inputReader.JumpTriggered) && isGrounded)
            {
                rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
                inputReader.ConsumeJump();
            }    
        }

        public void Flip()
        {
            IsFacingRight = !IsFacingRight;
            transform.Rotate(0f, 180f, 0f);
        }
    }
}
