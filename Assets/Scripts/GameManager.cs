using UnityEngine;
using UnityEngine.EventSystems;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }
    public int score = 0;
    public int coins = 0;
    public int highScore = 0;
    public int totalCoins = 0;
    public bool isGameOver = false;
    public bool isGameStarted = false;
    public Vector3 playerStartPosition = new Vector3(0f, 0f, 0f);
    public GameObject[] characterPrefabs; // Assign Player, Player 1, Player 2 in Inspector
    public bool gameOverAnimation = false;
    public Animator gameOverAnimator; // Assign this in the Inspector to your Game Over UI Animator

    private float timeSurvived = 0f;
    private GameObject player;

    // PlayerPrefs keys
    private const string HIGH_SCORE_KEY = "HighScore";
    private const string TOTAL_COINS_KEY = "TotalCoins";

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

        // Load saved data
        LoadPlayerData();
        
        // Update high score and total coins UI at game start
        if (UIManager.Instance != null)
        {
            UIManager.Instance.UpdateHighScoreAndTotalCoins(highScore, totalCoins);
        }
    }

    void Start()
    {
        // Load selected character index (default 0)
        int selectedCharacterIndex = PlayerPrefs.GetInt("SelectedCharacter", 0);

        // Instantiate the selected character prefab
        if (characterPrefabs != null && characterPrefabs.Length > selectedCharacterIndex)
        {
            player = Instantiate(characterPrefabs[selectedCharacterIndex], playerStartPosition, Quaternion.identity);
            // Set camera follow target
            CameraFollow camFollow = Camera.main.GetComponent<CameraFollow>();
            if (camFollow != null)
            {
                camFollow.SetTarget(player.transform);
            }
            // Update spawner targets
            CoinSpawner coinSpawner = FindFirstObjectByType<CoinSpawner>();
            if (coinSpawner != null)
            {
                coinSpawner.SetTarget(player.transform);
                coinSpawner.ResetSpawner();
            }
        }
        else
        {
            Debug.LogError("Selected character index out of range or characterPrefabs not assigned!");
        }

        // Initialize game state
        ResetGame();
        // Remove initial pause to allow ball to bounce
        Time.timeScale = 1f;
    }

    void Update()
    {
        if (!isGameStarted && !isGameOver)
        {
            // Mouse
            if (Input.GetMouseButtonDown(0))
            {
                bool pointerOverUI = EventSystem.current != null && EventSystem.current.IsPointerOverGameObject();
                if (!pointerOverUI)
                {
                    StartGame();
                    return;
                }
            }

            // Touch
            if (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Began)
            {
                int touchId = Input.GetTouch(0).fingerId;
                bool pointerOverUI = EventSystem.current != null && EventSystem.current.IsPointerOverGameObject(touchId);
                if (!pointerOverUI)
                {
                    StartGame();
                    return;
                }
            }

            // Keyboard input
            float horizontalInput = Input.GetAxisRaw("Horizontal");
            if (horizontalInput != 0)
            {
                StartGame();
                return;
            }
        }

        if (isGameStarted && !isGameOver)
        {
            timeSurvived += Time.deltaTime;
            // New scoring system: time survived + (coins / 2)
            score = (int)timeSurvived + (coins / 2);
            
            // Update UI if available
            if (UIManager.Instance != null)
            {
                UIManager.Instance.UpdateScore(score);
            }
        }
    }

    void StartGame()
    {
        isGameStarted = true;
        if (UIManager.Instance != null)
        {
            UIManager.Instance.HideStartScreen();
        }
    }

    public void AddCoin()
    {
        coins++;
        totalCoins++;
        // Save total coins
        SavePlayerData();
        
        // Update UI if available
        if (UIManager.Instance != null)
        {
            UIManager.Instance.UpdateCoins(coins);
        }
    }

    public void GameOver()
    {
        isGameOver = true;
        gameOverAnimation = true;
        // Time.timeScale = 0f; // Do not pause the whole scene

        // Pause the ball only
        if (player != null)
        {
            PlayerController pc = player.GetComponent<PlayerController>();
            if (pc != null)
            {
                pc.PauseBall();
            }
        }

        // Trigger the Game Over animation if animator is assigned
        if (gameOverAnimator != null)
        {
            gameOverAnimator.SetTrigger("GameOver");
        }

        // Check for new high score
        if (score > highScore)
        {
            highScore = score;
            SavePlayerData();
        }
        
        // Show Game Over UI if available
        if (UIManager.Instance != null)
        {
            UIManager.Instance.ShowGameOver(score, coins, highScore, totalCoins);
        }
    }

    public void RestartGame()
    {
        Debug.Log("RestartGame called in GameManager"); // Debug log

        gameOverAnimation = false;

        // Resume the ball if it exists
        if (player != null)
        {
            PlayerController pc = player.GetComponent<PlayerController>();
            if (pc != null)
            {
                pc.ResumeBall();
            }
        }

        // Destroy current player if it exists
        if (player != null)
        {
            Destroy(player);
        }

        // Instantiate the selected character prefab
        int selectedCharacterIndex = PlayerPrefs.GetInt("SelectedCharacter", 0);
        if (characterPrefabs != null && characterPrefabs.Length > selectedCharacterIndex)
        {
            player = Instantiate(characterPrefabs[selectedCharacterIndex], playerStartPosition, Quaternion.identity);
            // Set camera follow target
            CameraFollow camFollow = Camera.main.GetComponent<CameraFollow>();
            if (camFollow != null)
            {
                camFollow.SetTarget(player.transform);
            }
            // Update spawner targets
            CoinSpawner coinSpawner = FindFirstObjectByType<CoinSpawner>();
            if (coinSpawner != null)
            {
                coinSpawner.SetTarget(player.transform);
                coinSpawner.ResetSpawner();
            }
        }
        else
        {
            Debug.LogError("Selected character index out of range or characterPrefabs not assigned!");
        }

        // Destroy all obstacles and coins
        GameObject[] obstacles = GameObject.FindGameObjectsWithTag("Obstacle");
        foreach (GameObject obstacle in obstacles)
        {
            if (obstacle != null)
            {
                Destroy(obstacle);
            }
        }

        GameObject[] coins = GameObject.FindGameObjectsWithTag("Coin");
        foreach (GameObject coin in coins)
        {
            if (coin != null)
            {
                Destroy(coin);
            }
        }

        // Reset game state
        ResetGame();

        // Resume game speed
        // Time.timeScale = 1f;
    }

    public void SwitchPlayer(int characterIndex)
    {
        // Destroy current player
        if (player != null)
            Destroy(player);

        // Instantiate new player
        if (characterPrefabs != null && characterPrefabs.Length > characterIndex)
        {
            player = Instantiate(characterPrefabs[characterIndex], playerStartPosition, Quaternion.identity);

            // Set camera follow target
            CameraFollow camFollow = Camera.main.GetComponent<CameraFollow>();
            if (camFollow != null)
                camFollow.SetTarget(player.transform);

            // Update spawner targets
            CoinSpawner coinSpawner = FindFirstObjectByType<CoinSpawner>();
            if (coinSpawner != null)
            {
                coinSpawner.SetTarget(player.transform);
                coinSpawner.ResetSpawner();
            }
        }
    }

    private void ResetGame()
    {
        score = 0;
        coins = 0;
        isGameOver = false;
        isGameStarted = false;
        timeSurvived = 0f;
        gameOverAnimation = false;
        
        // Reset UI
        if (UIManager.Instance != null)
        {
            UIManager.Instance.UpdateScore(0);
            UIManager.Instance.UpdateCoins(0);
            UIManager.Instance.ShowStart();
        }
    }

    private void SavePlayerData()
    {
        PlayerPrefs.SetInt(HIGH_SCORE_KEY, highScore);
        PlayerPrefs.SetInt(TOTAL_COINS_KEY, totalCoins);
        PlayerPrefs.Save();
    }

    private void LoadPlayerData()
    {
        highScore = PlayerPrefs.GetInt(HIGH_SCORE_KEY, 0);
        totalCoins = PlayerPrefs.GetInt(TOTAL_COINS_KEY, 0);
    }
} 
