using UnityEngine;
using System.Collections.Generic;

public class CollectableBehaviour : MonoBehaviour
{
    [SerializeField]
    private List<Sprite> frames = new List<Sprite>();

    [SerializeField]
    private float animationSpeed = 0.2f;
    private int currentFrame = 0;
    private float lastUpdate;

    public delegate void OnCollected();
    public static event OnCollected collected;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        currentFrame = 0;
    }

    // Update is called once per frame
    void Update()
    {
        //cycle through the frames taking the animation speed into account
        if (Time.time - lastUpdate > animationSpeed)
        {
            lastUpdate = Time.time;
            currentFrame = (currentFrame + 1) % frames.Count;
            GetComponent<SpriteRenderer>().sprite = frames[currentFrame];
        }
        
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.CompareTag("Player")) return;
        {
            Destroy(gameObject);
            collected?.Invoke();
        }
    }   
}
