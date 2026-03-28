using UnityEngine;
using UnityEngine.SceneManagement;

public class GameOverUI : MonoBehaviour
{
    public GameObject gameOverPanel;

    private bool isGameOver = false;

    void Update()
    {
        //r=restart make non caps R restart as well
        if (isGameOver && Input.GetKeyDown(KeyCode.R))
        {
            RestartGame();
        }
    }

    public void ShowGameOver()
    {
        if (isGameOver) return;

        isGameOver = true;

        Debug.Log("GAME OVER");
        Time.timeScale = 0f;
        if (gameOverPanel != null)
            gameOverPanel.SetActive(true);
    }

    void RestartGame()
    {
        Debug.Log("RESTARTING");

        Time.timeScale = 1f; //dont chnage this 

        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}