using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance { get; private set; }
    public TextMeshProUGUI scoreText;
    public TextMeshProUGUI coinText;
    public GameObject startScreen;
    public GameObject gameOverScreen;
    public TextMeshProUGUI finalScoreText;
    public TextMeshProUGUI finalCoinText;
    public TextMeshProUGUI highScoreText;
    public TextMeshProUGUI totalCoinsText;
    public Button restartButton;
    public TextMeshProUGUI tapToStartText;
    public GameObject shopPanel;
    public Button openShopButton;
    public Button closeShopButton;

    void Awake()
    {
        // Singleton pattern
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void Start()
    {
        // Initialize UI
        if (scoreText != null) 
        {
            scoreText.text = "0";
            scoreText.gameObject.SetActive(false); // Hide score text initially
        }
        if (coinText != null) 
        {
            coinText.text = "0";
            coinText.gameObject.SetActive(false); // Hide coin text initially
        }
        
        // Setup restart button
        if (restartButton != null)
        {
            restartButton.onClick.RemoveAllListeners(); // Clear any existing listeners
            restartButton.onClick.AddListener(() => {
                Debug.Log("Restart button clicked"); // Debug log
                RestartGame();
            });
        }
        else
        {
            Debug.LogError("Restart button is not assigned in UIManager!");
        }

        // Setup shop buttons
        if (openShopButton != null)
            openShopButton.onClick.AddListener(OpenShop);
        if (closeShopButton != null)
            closeShopButton.onClick.AddListener(CloseShop);

        ShowStart();
    }

    public void UpdateScore(int score)
    {
        if (scoreText != null)
        {
            scoreText.text =  score.ToString();
            scoreText.gameObject.SetActive(true); // Ensure score text is visible
        }
    }

    public void UpdateCoins(int coins)
    {
        if (coinText != null)
        {
            coinText.text =  coins.ToString();
            coinText.gameObject.SetActive(true); // Ensure coin text is visible
        }
    }

    public void ShowStart()
    {
        if (startScreen != null) startScreen.SetActive(true);
        if (gameOverScreen != null) gameOverScreen.SetActive(false);
        if (tapToStartText != null) tapToStartText.gameObject.SetActive(true);
        
        // Hide score and coin text during start screen
        if (scoreText != null) scoreText.gameObject.SetActive(false);
        if (coinText != null) coinText.gameObject.SetActive(false);
    }

    public void HideStartScreen()
    {
        if (startScreen != null) startScreen.SetActive(false);
        if (tapToStartText != null) tapToStartText.gameObject.SetActive(false);
        
        // Show score and coin text when game starts
        if (scoreText != null) scoreText.gameObject.SetActive(true);
        if (coinText != null) coinText.gameObject.SetActive(true);
    }

    public void ShowGameOver(int score, int coins, int highScore, int totalCoins)
    {
        if (startScreen != null) startScreen.SetActive(false);
        if (gameOverScreen != null) gameOverScreen.SetActive(true);
        if (finalScoreText != null) finalScoreText.text =  score.ToString();
        if (finalCoinText != null) finalCoinText.text =  coins.ToString();
        if (highScoreText != null) highScoreText.text =  highScore.ToString();
        if (totalCoinsText != null) totalCoinsText.text =  totalCoins.ToString();
        
        // Keep score and coin text visible
        if (scoreText != null) scoreText.gameObject.SetActive(true);
        if (coinText != null) coinText.gameObject.SetActive(true);
    }

    public void RestartGame()
    {
        Debug.Log("RestartGame called in UIManager"); // Debug log
        if (GameManager.Instance != null)
        {
            GameManager.Instance.RestartGame();
        }
        else
        {
            Debug.LogError("GameManager.Instance is null!");
        }
    }

    public void UpdateHighScoreAndTotalCoins(int highScore, int totalCoins)
    {
        if (highScoreText != null) highScoreText.text =  highScore.ToString();
        if (totalCoinsText != null) totalCoinsText.text =  totalCoins.ToString();
    }

    public void OpenShop()
    {
        if (shopPanel != null) shopPanel.SetActive(true);
    }

    public void CloseShop()
    {
        if (shopPanel != null) shopPanel.SetActive(false);
    }
} 