using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public class DoorBehaviour : MonoBehaviour
{
    [SerializeField]
    private List<Sprite> frames = new List<Sprite>();
    [SerializeField]
    private float frameDelay = 0.2f;

    private int   currentFrame = 0;
    private float timeSinceLastFrame = 0.0f;
    
    private Boolean IsOpen = false;
    private SpriteRenderer spriteRenderer;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        CollectableBehaviour.collected += OnCollected;
        spriteRenderer = GetComponent<SpriteRenderer>();
        timeSinceLastFrame = 0.0f;
        currentFrame = 0;
    }

    // Update is called once per frame
    void Update()
    {
        if(IsOpen)
        {
            timeSinceLastFrame += Time.deltaTime;
            if (timeSinceLastFrame >= frameDelay)
            {
                currentFrame = (currentFrame + 1) % frames.Count;
                spriteRenderer.sprite = frames[currentFrame];
                timeSinceLastFrame = 0.0f;
            }
        }
    }

    private void OnCollected()
    {
        var collectablesGO = GameObject.Find("Collectables");
        if (collectablesGO != null)
        {
            int remaining = collectablesGO.transform.childCount;
            if(remaining == 1)
            {
                IsOpen = true;
            }
        }
    }
}
