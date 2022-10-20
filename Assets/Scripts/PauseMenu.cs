using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public class PauseMenu : MonoBehaviour
{
    public static bool gameIsPaused = false;
    public GameObject pauseMenuUI;

    public void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (gameIsPaused) //Resume game if game is already paused
            {
                Resume();
            }
            else
            {
                Pause();
            }
        }
    }
    public void Resume()
    {
        pauseMenuUI.SetActive(false);
        Time.timeScale = 1f; //Sets normal time
        gameIsPaused = false;
    }

    private void Pause()
    {
        pauseMenuUI.SetActive(true);
        Time.timeScale = 0f; //Pauses the time
        gameIsPaused = true;
    }

    public void QuitToMenu()
    {
        gameIsPaused = false;
        Time.timeScale = 1f; //Sets normal time
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex - 1); //Mainmenu index = 0
    }
}
