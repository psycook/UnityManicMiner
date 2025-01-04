using UnityEngine;
using UnityEngine.InputSystem;
using System.Collections.Generic;
using Unity.VisualScripting;

enum ConveyorState { None, Left, Right }

public class MinerWillyBehaviour : MonoBehaviour
{
    [SerializeField]
    private float moveSpeed = 0.5f;
    [SerializeField]
    private float jumpForce = 1f;
    [SerializeField]
    private float fallForce = 1f;
    [SerializeField]
    private Collider2D groundCollider;
    [SerializeField]
    private Collider2D groundCheckCollider;

    // debug, remove serialized fields
    [SerializeField]
    private bool isGrounded = false;
    [SerializeField]
    private bool isJumping = false;
    [SerializeField]
    private bool isFalling = false;
    [SerializeField]
    private ConveyorState conveyorState = ConveyorState.None;
    [SerializeField]
    private float move = 0.0f;
    [SerializeField]
    private List<Collider2D> insideTrigger = new List<Collider2D>();

    private Rigidbody2D rb;
    private SpriteRenderer spriteRenderer;
    private Animator animator;
    private PlayerControls controls;
    private Vector2 moveInput;

    void Awake()
    {
        // Get the components
        spriteRenderer = GetComponent<SpriteRenderer>();
        animator = GetComponent<Animator>();
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
        if (isFalling)
        {
            groundCollider.enabled = true;
            groundCheckCollider.enabled = true;
            rb.linearVelocity = new Vector2(0.0f, -fallForce);
            return;
        }

        if (isJumping)
        {
            // disable the ground collider if moving upwards
            if (rb.linearVelocity.y > 0 && groundCollider != null)
            {
                groundCollider.enabled = false;
                groundCheckCollider.enabled = false;
            }
            else
            {
                groundCollider.enabled = true;
                groundCheckCollider.enabled = true;
            }
            rb.linearVelocity = new Vector2(move * moveSpeed, rb.linearVelocity.y);
            return;
        }

        if (isGrounded)
        {
            move = moveInput.x;

            if (conveyorState == ConveyorState.Left)
            {
                move = move <= 0.0f ? -1.0f : 0.0f;
                rb.linearVelocity = new Vector2(move * moveSpeed, rb.linearVelocity.y);
            }
            else if (conveyorState == ConveyorState.Right)
            {
                move = move <= 0.0f ? 0.0f : 1.0f;
                rb.linearVelocity = new Vector2(move * moveSpeed, rb.linearVelocity.y);
            }
            else if (conveyorState == ConveyorState.None)
            {
                rb.linearVelocity = new Vector2(move * moveSpeed, rb.linearVelocity.y);
            }            
        }

        // set direction
        if (move < 0)
        {
            spriteRenderer.flipX = true;
        }
        else if (move > 0)
        {
            spriteRenderer.flipX = false;
        }

        // set animator
        animator.SetFloat("xVelocity", Mathf.Abs(rb.linearVelocity.x));
    }

    void Jump()
    {
        if (isGrounded)
        {
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpForce);
            setIsJumping(true);
            setIsFalling(false);
            setIsGrounded(false);
            conveyorState = ConveyorState.None;
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        // don't include collisions with collectables or deadly objects 
        if (other.CompareTag("Collectable") || other.CompareTag("Deadly")) return;

        // add the collider to the list of colliders inside the trigger
        insideTrigger.Add(other);

        // reset the conveyor state
        conveyorState = ConveyorState.None;

        // check for ground or wall colliders
        foreach (Collider2D collider in insideTrigger)
        {
            if (collider.CompareTag("Ground") || collider.CompareTag("Wall") || collider.CompareTag("Quicksand"))
            {
                setIsGrounded(true);
                setIsFalling(false);
                setIsJumping(false);
            } 
            else if(collider.CompareTag("ConveyorLeft"))
            {
                conveyorState = ConveyorState.Left;
                setIsGrounded(true);
                setIsFalling(false);
                setIsJumping(false);
            } 
            else if(collider.CompareTag("ConveyorRight"))
            {
                conveyorState = ConveyorState.Right;
                setIsGrounded(true);
                setIsFalling(false);
                setIsJumping(false);
            }
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        // don't include collisions with collectables or deadly objects 
        if (other.CompareTag("Collectable") || other.CompareTag("Deadly")) return;

        // remove the collider to the list of colliders inside the trigger
        insideTrigger.Remove(other);

        // if not jumping and nothing is inside the trigger, set the player to falling
        if (!isJumping && insideTrigger.Count == 0)
        {
            setIsFalling(true);
            setIsGrounded(false);
            setIsJumping(false);
            conveyorState = ConveyorState.None;
        }
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        Debug.Log($"Collision with {collision.gameObject.tag}, jumping state is {isJumping}");

        if (isJumping)
        {
            Debug.Log("Setting to falling");
            setIsFalling(true);
            setIsJumping(false);
        }
    }

    private void setIsGrounded(bool grounded)
    {
        isGrounded = grounded;
    }

    private void setIsJumping(bool jumping)
    {
        Debug.Log($"Setting isJumping to {jumping}");
        isJumping = jumping;
        animator.SetBool("isJumping", isJumping);
    }

    private void setIsFalling(bool falling)
    {
        isFalling = falling;
        animator.SetBool("isFalling", falling);
    }
}