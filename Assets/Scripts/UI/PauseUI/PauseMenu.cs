using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    public GameObject pausePanel; //drag panel here
    private bool isPaused = false;

    void Update()
    {
        //Press Escape or P to toggle pause
        if (Input.GetKeyDown(KeyCode.Escape) || Input.GetKeyDown(KeyCode.P))
        {
            if (isPaused)
                ResumeGame();
            else
                PauseGame();
        }
    }

    public void PauseGame()
    {
        pausePanel.SetActive(true);
        Time.timeScale = 0f; //freeze game
        isPaused = true;
    }

    public void ResumeGame()
    {
        pausePanel.SetActive(false);
        Time.timeScale = 1f; //resume
        isPaused = false;
    }

    public void QuitToMainMenu()
    {
        Time.timeScale = 1f; //reset time scale before loading scene
        SceneManager.LoadScene("MainMenu");
    }
}
