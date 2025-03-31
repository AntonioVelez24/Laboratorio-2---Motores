using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameControl : MonoBehaviour
{
    [SerializeField] private PlayerControl player;
    [SerializeField] private GameObject pausePanel;
    [SerializeField] private TextMeshProUGUI timeText;
    [SerializeField] private Image healthBar;
    [SerializeField] public float passedTime;
    [SerializeField] private GameObject pauseButton;

    private SpriteRenderer mySpriteRenderer;

    private bool isPaused = false;
    public bool canChangeColor = true;
    
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
    public void UpdateHealthBar()
    {
        healthBar.fillAmount = player.Health / 10f;

        if (player.Health <= 0)
        {
            PlayerPrefs.SetString("GameResult", "Game Over");
            PlayerPrefs.SetString("TimeResult", "Final Time" + Mathf.FloorToInt(passedTime));

            SceneManager.LoadScene("EndScreen");
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
                PlayerPrefs.SetString("GameResult", "You Win!");
                PlayerPrefs.SetString("TimeResult", "Final Time" + seconds);

                SceneManager.LoadScene("EndScreen");
            }
        }
    }
}
