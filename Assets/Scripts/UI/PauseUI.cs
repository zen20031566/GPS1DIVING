using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseUI : MonoBehaviour
{
    public bool isPaused = false;

    private void OnEnable()
    {
        PauseGame();
    }

    private void OnDisable()
    {
        ResumeGame();
    }

    public void PauseGame()
    {
        if (!isPaused)
        {
            //Time.timeScale = 0f; //freeze game
            isPaused = true;
        }
    }

    public void ResumeGame()
    {
        if (isPaused)
        {
            //Time.timeScale = 1f; //resume
            isPaused = false;
        }
    }

    public void QuitToMainMenu()
    {
        ResumeGame();
        SceneManager.LoadScene("MainMenu");
    }
}
