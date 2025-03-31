using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class ScreenControl : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI resultText;
    [SerializeField] private TextMeshProUGUI timeText;
    [SerializeField] private Sprite[] endSprites;
    [SerializeField] private SpriteRenderer endRenderer;

    void Start()
    {
        string timeResult = PlayerPrefs.GetString("TimeResult");
        timeText.text = timeResult;

        string gameResult = PlayerPrefs.GetString("GameResult");
        resultText.text = gameResult;

        int resultSprite = PlayerPrefs.GetInt("SpriteResult");
        endRenderer.sprite = endSprites[resultSprite];

    }
    public void RestartGame()
    {
        SceneManager.LoadScene("Game");
    }
    public void MainMenu()
    {
        SceneManager.LoadScene("Menu");
    }
}
