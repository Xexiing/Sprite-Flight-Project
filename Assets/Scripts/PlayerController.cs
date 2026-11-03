using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;

public class PlayerController : MonoBehaviour
{
    public InputAction moveForward;
    public InputAction lookPosition;

    public float thrustForce = 3f;
    public float maxSpeed = 8f;
    public GameObject boosterFlame;
    public float minScale = 0.7f;
    public float maxScale = 1.5f;
    public float interval = 0.1f;
    private float timer;
    private Vector3 baseScale;

    private float elapsedTime = 0f;
    private float score = 0f;
    private float highScore=0f;
    public float scoreMultiplier = 10f;

    public UIDocument uiDocument;
    private Label scoreText;
    private Label highScoreText;
    private Button restartButton;

    public GameObject explosionEffect;
    public GameObject borderParent;
    Rigidbody2D rb;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();

        moveForward.Enable();
        lookPosition.Enable();

        baseScale = boosterFlame.transform.localScale;

        scoreText = uiDocument.rootVisualElement.Q<Label>("ScoreLabel");
        highScoreText = uiDocument.rootVisualElement.Q<Label>("HighScoreLabel");
        highScore = PlayerPrefs.GetFloat("HighScore", 0f);
        highScoreText.text = "High Score : " + highScore;
        restartButton = uiDocument.rootVisualElement.Q<Button>("RestartButton");
        restartButton.clicked += ReloadScene;
        restartButton.style.display = DisplayStyle.None;
        highScoreText.style.display = DisplayStyle.None;
    }

    void Update()
    {
        UpdateScore();
        MovePlayer();
        HScore();
        FlameFLicker();
    }
    void MovePlayer()
    {
        if (moveForward.IsPressed())
        {
            // Calculate mouse direction
            Vector3 mousePos = Camera.main.ScreenToWorldPoint(lookPosition.ReadValue<Vector2>());
            Vector2 direction = (mousePos - transform.position).normalized;

            // Move player in direction of mouse
            transform.up = direction;
            rb.AddForce(direction * thrustForce);
        }

        if (moveForward.WasPressedThisFrame())
        {
            boosterFlame.SetActive(true);
        }
        else if (moveForward.WasReleasedThisFrame())
        {
            boosterFlame.SetActive(false);
        }

        if (rb.linearVelocity.magnitude > maxSpeed)
        {
            rb.linearVelocity = rb.linearVelocity.normalized * maxSpeed;
        }
    }  

    void UpdateScore()
    {
        elapsedTime += Time.deltaTime;
        score = Mathf.FloorToInt(elapsedTime * scoreMultiplier);
        
        scoreText.text = "Score : " + score;
        Debug.Log("Score : " + score);
    }

    void FlameFLicker()
    {
        timer += Time.deltaTime;
        if (timer >= interval)
        {
            timer = 0f;
            float randomScale = Random.Range(minScale, maxScale);
            boosterFlame.transform.localScale = baseScale * randomScale;
        }
    }
        void HScore()
    {
        if (highScore < score)
        {
            highScore = score;
            PlayerPrefs.SetFloat("HighScore", highScore);
        }
        highScoreText.text = "High Score : " + highScore;
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        //Existing logic: explosion, restart button, destroy self
        Instantiate(explosionEffect, transform.position, transform.rotation);
        restartButton.style.display = DisplayStyle.Flex;
        highScoreText.style.display = DisplayStyle.Flex;
        Destroy(gameObject);
        borderParent.SetActive(false);
    }

    void ReloadScene()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}