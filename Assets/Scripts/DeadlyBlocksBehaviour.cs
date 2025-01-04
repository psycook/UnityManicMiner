using UnityEngine;

public class DeadlyBlockBehaviour : MonoBehaviour
{

    public delegate void OnDeath();
    public static event OnDeath death;

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
