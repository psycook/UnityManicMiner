using UnityEngine;
using System.Collections.Generic;
using System;

public class QuicksandBehaviour : MonoBehaviour
{
    [SerializeField]
    private List<Sprite> frames = new List<Sprite>();
    [SerializeField] private float sinkDelay = 0.2f;

    [SerializeField]
    private Boolean isSinking = false;
    [SerializeField]
    private int currentFrame = 0;

    private SpriteRenderer spriteRenderer;

    float timeSinceLastFrame = 0.0f;

    public void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        currentFrame = 0;
        timeSinceLastFrame = 0.0f;
        isSinking = false;
    }

    void Update()
    {
        if (isSinking)
        {
            timeSinceLastFrame += Time.deltaTime;
            if (timeSinceLastFrame >= sinkDelay)
            {
                timeSinceLastFrame = 0.0f;
                if (currentFrame < frames.Count - 1)
                {
                    currentFrame++;
                    spriteRenderer.sprite = frames[currentFrame];
                }
                else
                {
                    Destroy(gameObject);
                }
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {                
        if (!other.CompareTag("Player")) return;
        {
            isSinking = true;
            timeSinceLastFrame = 0.0f;
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (!other.CompareTag("Player")) return;
        {
            isSinking = false;
        }
    }
}

