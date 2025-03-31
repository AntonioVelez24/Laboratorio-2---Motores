using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class ScreenControl : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI resultText;
    [SerializeField] private TextMeshProUGUI timeText;

    public bool gameWin;

    void Start()
    {
        string timeResult = PlayerPrefs.GetString("TimeResult");
        timeText.text = timeResult;

        string gameResult = PlayerPrefs.GetString("GameResult");
        resultText.text = gameResult;
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
