using UnityEngine;
using UnityEngine.UI;

public class AirMeterBehaviour : MonoBehaviour
{
    [SerializeField]
    Image airBar;

    [SerializeField]
    public float barLength = 800.0f;

    [SerializeField]
    float airRate = 2.0f;

    float maxAir = 100.0f;
    private float currentAir;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        currentAir = maxAir;
    }

    // Update is called once per frame
    void Update()
    {
                // Decrement air over time
        currentAir -= Time.deltaTime * airRate; // Adjust the rate as needed
        currentAir = Mathf.Clamp(currentAir, 0, maxAir);

        //set the wide of the air bar
        airBar.rectTransform.sizeDelta = new Vector2(barLength * (currentAir / maxAir), airBar.rectTransform.sizeDelta.y);

        // Optional: Handle what happens when air runs out
        if (currentAir <= 0)
        {
            //.Log("Out of air!");
            // Trigger level failure or other logic here
        }
    }
}