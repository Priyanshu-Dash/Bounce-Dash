using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [Header("Movement Settings")]
    public float moveSpeed = 5f;
    public float bounceForce = 10f;
    public float maxHorizontal = 2.5f;

    [Header("Touch Settings")]
    public float touchSensitivity = 2f;
    public float touchDeadzone = 0.1f;

    [Header("Platform Check")]
    public float maxTimeWithoutPlatform = 1f;
    private float timeWithoutPlatform = 0f;
    private bool isTouchingPlatform = false;

    private Rigidbody2D rb;
    private float horizontalInput = 0f;
    private Vector2 touchStartPos;
    private bool isTouching = false;
    private bool isMouseDragging = false;
    private bool isPaused = false;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        // Initial bounce
        Bounce();
    }

    void Update()
    {
        HandleInput();
        ClampPosition();
        CheckPlatformTime();
    }

    void FixedUpdate()
    {
        if (isPaused) {
            rb.linearVelocity = Vector2.zero;
            rb.isKinematic = true;
            return;
        } else {
            rb.isKinematic = false;
        }
        // Only move horizontally, vertical handled by physics
        rb.linearVelocity = new Vector2(horizontalInput * moveSpeed, rb.linearVelocity.y);
    }

    void HandleInput()
    {
        // Keyboard input
        horizontalInput = Input.GetAxisRaw("Horizontal");

        // Mouse drag input
        if (Input.GetMouseButtonDown(0))
        {
            touchStartPos = Input.mousePosition;
            isMouseDragging = true;
        }
        else if (Input.GetMouseButton(0) && isMouseDragging)
        {
            Vector2 mouseDelta = (Vector2)Input.mousePosition - touchStartPos;
            float dragMagnitude = mouseDelta.magnitude / Screen.width;

            if (dragMagnitude > touchDeadzone)
            {
                horizontalInput = Mathf.Clamp(mouseDelta.x * touchSensitivity / Screen.width, -1f, 1f);
            }
            else
            {
                horizontalInput = 0f;
            }
        }
        else if (Input.GetMouseButtonUp(0))
        {
            isMouseDragging = false;
            horizontalInput = 0f;
        }

        // Touch input
        if (Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);

            switch (touch.phase)
            {
                case TouchPhase.Began:
                    touchStartPos = touch.position;
                    isTouching = true;
                    break;

                case TouchPhase.Moved:
                    if (isTouching)
                    {
                        // Calculate swipe direction and magnitude
                        Vector2 swipeDelta = touch.position - touchStartPos;
                        float swipeMagnitude = swipeDelta.magnitude / Screen.width;

                        // Only process if swipe is significant enough
                        if (swipeMagnitude > touchDeadzone)
                        {
                            // Normalize the swipe direction and apply sensitivity
                            horizontalInput = Mathf.Clamp(swipeDelta.x * touchSensitivity / Screen.width, -1f, 1f);
                        }
                        else
                        {
                            horizontalInput = 0f;
                        }
                    }
                    break;

                case TouchPhase.Ended:
                case TouchPhase.Canceled:
                    isTouching = false;
                    horizontalInput = 0f;
                    break;
            }
        }
        else if (!isMouseDragging)
        {
            // Reset touch state if no touches are detected and not mouse dragging
            isTouching = false;
        }
    }

    void ClampPosition()
    {
        Vector3 pos = transform.position;
        pos.x = Mathf.Clamp(pos.x, -maxHorizontal, maxHorizontal);
        transform.position = pos;
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        // Check if collision is with a platform
        if (collision.gameObject.CompareTag("Platform"))
        {
            isTouchingPlatform = true;
            timeWithoutPlatform = 0f;
            
            // Bounce only when hitting ground or platforms
            if (collision.contacts[0].normal.y > 0.5f)
            {
                Bounce();
            }
        }
        // Check if collision is with an obstacle
        else if (collision.gameObject.CompareTag("Obstacle"))
        {
            if (GameManager.Instance != null)
            {
                GameManager.Instance.GameOver();
            }
        }
    }

    void OnCollisionExit2D(Collision2D collision)
    {
        // Check if leaving a platform
        if (collision.gameObject.CompareTag("Platform"))
        {
            isTouchingPlatform = false;
        }
    }

    void CheckPlatformTime()
    {
        if (!isTouchingPlatform)
        {
            timeWithoutPlatform += Time.deltaTime;
            if (timeWithoutPlatform >= maxTimeWithoutPlatform)
            {
                if (GameManager.Instance != null)
                {
                    GameManager.Instance.GameOver();
                }
            }
        }
    }

    void Bounce()
    {
        rb.linearVelocity = new Vector2(rb.linearVelocity.x, bounceForce);
    }

    public void PauseBall()
    {
        isPaused = true;
    }

    public void ResumeBall()
    {
        isPaused = false;
    }
} 