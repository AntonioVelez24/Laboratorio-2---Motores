using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuControl : MonoBehaviour
{
    [SerializeField] GameObject amongus;
    [SerializeField] float rotationSpeed;
    public void LoadGameScene()
    {
        SceneManager.LoadScene("Game");
    }
    public void GameExit()
    {
        Application.Quit();
    }
    void Update()
    {
        amongus.transform.Rotate(0,0, rotationSpeed * Time.deltaTime);
    }
}
