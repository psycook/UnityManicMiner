using System.Collections.Generic;
using UnityEngine;

public class LifeBehaviour : MonoBehaviour
{
    [SerializeField]
    private List<Sprite> frames = new List<Sprite>();
    private float frameDelay = 0.1f;

    private int currentFrame = 0;
    private float timeSinceLastFrame = 0.0f;

    void Update()
    {
        //animate the frames
        timeSinceLastFrame += Time.deltaTime;
        if (timeSinceLastFrame >= frameDelay)
        {
            currentFrame = (currentFrame + 1) % frames.Count;
            GetComponent<SpriteRenderer>().sprite = frames[currentFrame];
            timeSinceLastFrame = 0.0f;
        }
    }
}
