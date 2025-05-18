using UnityEngine;

public class Obstacle : MonoBehaviour
{
    void OnCollisionEnter2D(Collision2D collision)
    {
        Debug.Log("Collision detected with: " + collision.gameObject.name);
        
        if (collision.gameObject.CompareTag("Player"))
        {
            Debug.Log("Player hit obstacle!");
            if (GameManager.Instance != null)
            {
                GameManager.Instance.GameOver();
            }
            else
            {
                Debug.LogError("GameManager.Instance is null! Make sure GameManager exists in the scene.");
            }
        }
    }
} 