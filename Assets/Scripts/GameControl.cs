using System;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameControl : MonoBehaviour
{
    [SerializeField] private PlayerControl player;
    [SerializeField] private GameObject pausePanel;
    [SerializeField] private TextMeshProUGUI timeText;
    [SerializeField] private TextMeshProUGUI scoreText;
    [SerializeField] private Image healthBar;
    [SerializeField] private float passedTime;
    [SerializeField] private GameObject pauseButton;
    [SerializeField] private Transform[] points1;
    [SerializeField] private Transform[] points2;
    [SerializeField] private Transform[] points3;
    [SerializeField] private int enemySpeed;
    [SerializeField] private GameObject enemy1;
    [SerializeField] private GameObject enemy2;
    [SerializeField] private GameObject enemy3;
    [SerializeField] private GameObject coinPrefab;
    [SerializeField] private GameObject heartPrefab;
    [SerializeField] private float coinSpawnTime;
    [SerializeField] private float heartSpawnTime;

    private SpriteRenderer mySpriteRenderer;
    private int currentPoint1 = 0;
    private int currentPoint2 = 0;
    private int currentPoint3 = 0;

    private bool isPaused = false;
    public bool canChangeColor = true;

    public event Action OnGameOver;
    public event Action OnGameWon;
    void OnEnable()
    {
        player.OnHealthChanged += UpdateHealthBar;
        player.OnScoreChanged += UpdateScoreText;
        OnGameOver += GameOver;
        OnGameWon += GameWon;
    }
    void OnDisable()
    {
        player.OnHealthChanged -= UpdateHealthBar;
        player.OnScoreChanged -= UpdateScoreText;
        OnGameOver -= GameOver;
        OnGameWon -= GameWon;
    }
    void Awake()
    {
        mySpriteRenderer = player.GetComponent<SpriteRenderer>();
    }
    public void ColorRed()
    {
        ChangeColor(Color.red);
    }
    public void ColorBlue()
    {
        ChangeColor(Color.blue);
    }
    public void ColorGreen()
    {
        ChangeColor(Color.green);
    }
    private void ChangeColor(Color newColor)
    {
        if (canChangeColor)
        {
            mySpriteRenderer.color = newColor;
        }
        
    }
    public void Pause()
    {
        if (isPaused == false)
        {
            isPaused = true;
            Time.timeScale = 0f;
            pausePanel.SetActive(true);
            pauseButton.SetActive(false);
        }        
    }
    public void ResumeGame()
    {
        if (isPaused == true)
        {
            isPaused = false;
            Time.timeScale = 1f;
            pausePanel.SetActive(false);
            pauseButton.SetActive(true);
        }
    }
    public void GameWon()
    {
        PlayerPrefs.SetString("GameResult", "You Win!");
        PlayerPrefs.SetInt("SpriteResult", 0);

        SceneManager.LoadScene("EndScreen");
    }
    public void GameOver()
    {
        PlayerPrefs.SetString("GameResult", "Game Over");
        PlayerPrefs.SetString("TimeResult", "Final Time: " + Mathf.FloorToInt(passedTime));
        PlayerPrefs.SetString("ScoreResult", "Final Score: " + player.Score);
        PlayerPrefs.SetInt("SpriteResult", 1);

        SceneManager.LoadScene("EndScreen");
    }
    public void UpdateHealthBar()
    {
        healthBar.fillAmount = player.Health / 10f;

        if (player.Health <= 4)
        {
            healthBar.color = Color.yellow;
        }       

        if (player.Health <= 0)
        {
            OnGameOver.Invoke();
        }
    }
    public void UpdateScoreText()
    {
        scoreText.text = ("Score: " + player.Score);
    }

    private void SpawnCoin(GameObject itemPrefab, float time)
    {        
        coinSpawnTime -= Time.deltaTime;
        if (coinSpawnTime <= 0)
        {
            float randomX = UnityEngine.Random.Range(-8f, 8f);
            float randomY = UnityEngine.Random.Range(-3f, 2.5f);
            Vector3 randomPosition = new Vector3(randomX, randomY);
            
            Instantiate(itemPrefab, randomPosition, Quaternion.identity);
            coinSpawnTime = time;
        }
    }
    private void SpawnHeart(GameObject itemPrefab, float time)
    {
        heartSpawnTime -= Time.deltaTime;
        if (heartSpawnTime <= 0)
        {
            float randomX = UnityEngine.Random.Range(-8f, 8f);
            float randomY = UnityEngine.Random.Range(-3f, 2.5f);
            Vector3 randomPosition = new Vector3(randomX, randomY);

            Instantiate(itemPrefab, randomPosition, Quaternion.identity);
            heartSpawnTime = time;
        }
    }
    void Update()
    {
        if (!isPaused)
        {
            passedTime += Time.deltaTime;
            int seconds = Mathf.FloorToInt(passedTime);
            timeText.text = "Time: " + seconds;
            if (passedTime >= 60)
            {
                OnGameWon?.Invoke();
                PlayerPrefs.SetString("TimeResult", "Final Time: " + seconds);
            }
        }
        if (Vector3.Distance(enemy1.transform.position, points1[currentPoint1].position) < 0.1f)
        {
            currentPoint1 = (currentPoint1 + 1) % points1.Length;
        }
        enemy1.transform.position = Vector3.MoveTowards(enemy1.transform.position, points1[currentPoint1].position, enemySpeed * Time.deltaTime);

        if (Vector3.Distance(enemy2.transform.position, points2[currentPoint2].position) < 0.1f)
        {
            currentPoint2 = (currentPoint2 + 1) % points2.Length;
        }
        enemy2.transform.position = Vector3.MoveTowards(enemy2.transform.position, points2[currentPoint2].position, enemySpeed * Time.deltaTime);

        if (Vector3.Distance(enemy3.transform.position, points3[currentPoint3].position) < 0.1f)
        {
            currentPoint3 = (currentPoint3 + 1) % points3.Length;
        }
        enemy3.transform.position = Vector3.MoveTowards(enemy3.transform.position, points3[currentPoint3].position, enemySpeed * Time.deltaTime);

        SpawnCoin(coinPrefab, 3f);
        SpawnHeart(heartPrefab, 15f);
    }
}
