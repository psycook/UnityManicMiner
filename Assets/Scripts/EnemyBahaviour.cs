using System.Collections.Generic;
using UnityEngine;

enum EnemyMovementAxis
{
    Horizontal,
    Vertical
}

enum EnemyMovementDirection
{
    Right,
    Left,
    Up,
    Down
}

public class EnemyBahaviour : MonoBehaviour
{
    [SerializeField] private EnemyMovementAxis axis = EnemyMovementAxis.Horizontal;
    [SerializeField] private float speed = 100.0f;
    [SerializeField] private float min = 0.0f;
    [SerializeField] private float max = 0.0f;
    [SerializeField] private EnemyMovementDirection direction = EnemyMovementDirection.Right;
    [SerializeField] List<Sprite> frames = new List<Sprite>();
    [SerializeField] private int currentFrame = 0;
    [SerializeField] private float frameDelay = 0.1f;

    private float timeSinceLastFrame = 0.0f;
    private SpriteRenderer spriteRenderer;
    private Rigidbody2D rb;


    public delegate void OnDeath();
    public static event OnDeath death;

    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        rb = GetComponent<Rigidbody2D>();
        currentFrame = 0;

        //set min start positions
        if (axis == EnemyMovementAxis.Horizontal)
        {
            transform.position = new Vector2(min, transform.position.y);
        }
        else if (axis == EnemyMovementAxis.Vertical)
        {
            transform.position = new Vector2(transform.position.x, min);
        }
    }

    void Update()
    {
        //update the frames
        timeSinceLastFrame += Time.deltaTime;
        if (timeSinceLastFrame >= frameDelay)
        {
            currentFrame = (currentFrame + 1) % frames.Count;
            spriteRenderer.sprite = frames[currentFrame];
            timeSinceLastFrame = 0.0f;
        }

        //update the movement taking into account delta time
        float movement = 0.0f;
        switch (axis)
        {
            case EnemyMovementAxis.Horizontal:
                movement = direction == EnemyMovementDirection.Right ? 1.0f : -1.0f;
                rb.linearVelocity = new Vector2((movement * speed), rb.linearVelocity.y);
                break;
            case EnemyMovementAxis.Vertical:
                movement = direction == EnemyMovementDirection.Up ? 1.0f : -1.0f;
                rb.linearVelocity = new Vector2(rb.linearVelocity.x, (movement * speed));
                break;
        }


        // check for min and max and reverse direction
        if (axis == EnemyMovementAxis.Horizontal)
        {
            if (transform.position.x >= max)
            {
                direction = EnemyMovementDirection.Left;
            }
            else if (transform.position.x <= min)
            {
                direction = EnemyMovementDirection.Right;
            }
        }
        else if (axis == EnemyMovementAxis.Vertical)
        {
            if (transform.position.y >= max)
            {
                direction = EnemyMovementDirection.Down;
            }
            else if (transform.position.y <= min)
            {
                direction = EnemyMovementDirection.Up;
            }
        }

        // flip sprite if moving left
        if (direction == EnemyMovementDirection.Left)
        {
            spriteRenderer.flipX = true;
        }
        else
        {
            spriteRenderer.flipX = false;
        }

        //flip sprite if moving up
        if (direction == EnemyMovementDirection.Up)
        {
            spriteRenderer.flipY = true;
        }
        else
        {
            spriteRenderer.flipY = false;
        }
    }

    //check for on trigger with the Player and destroy the player
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            Destroy(other.gameObject);
            death?.Invoke();
        }
    }
}
