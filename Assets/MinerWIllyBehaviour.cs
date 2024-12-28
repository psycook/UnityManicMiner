using UnityEngine;
using UnityEngine.InputSystem;

public class MinerWillyBehaviour : MonoBehaviour
{
    public float moveSpeed = 0.5f;    // Movement speed
    public float jumpForce = 1f;  // Jump force
    private Rigidbody2D rb;
    private Vector2 moveInput;     // Store movement input
    private bool isGrounded = false;

    private PlayerControls controls;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();

        // Initialize input system
        controls = new PlayerControls();

        // Bind input actions
        controls.Player.Move.performed += ctx => moveInput = ctx.ReadValue<Vector2>();
        controls.Player.Move.canceled += ctx => moveInput = Vector2.zero;
        controls.Player.Jump.performed += ctx => Jump();
    }

    void OnEnable()
    {
        controls.Enable();
    }

    void OnDisable()
    {
        controls.Disable();
    }

    void FixedUpdate()
    {
        // Horizontal movement
        float move = moveInput.x;
        rb.linearVelocity = new Vector2(move * moveSpeed, rb.linearVelocity.y);
    }

    void Jump()
    {
        if (isGrounded)
        {
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpForce);
            isGrounded = false;
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        // Check if the player lands on a surface
        if (collision.contacts[0].normal.y > 0.5f)
        {
            isGrounded = true;
        }
    }
}
