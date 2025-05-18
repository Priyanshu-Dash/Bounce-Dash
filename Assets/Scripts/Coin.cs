using UnityEngine;

public class Coin : MonoBehaviour
{
    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            // Check if GameManager exists
            if (GameManager.Instance == null)
            {
                Debug.LogError("GameManager.Instance is null! Make sure GameManager exists in the scene.");
                return;
            }

            // Add coin and update UI
            GameManager.Instance.AddCoin();
            Destroy(gameObject);
            // TODO: Add coin pop effect
        }
    }
} 