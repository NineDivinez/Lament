using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseScript : MonoBehaviour
{
    //variables
    public GameObject paused;
    public bool isPaused = false;
    public bool waiting = false;

    //objects
    public GameObject remove;
    public GameObject pauseMenu;
    public GameObject optionsMenu;
    public GameObject pauseMain;
    public GameObject deathScreen;
    public GameObject uiElements;

    void pauseGame()
    {
        Time.timeScale = 0f;
        remove.SetActive(false);
        uiElements.SetActive(false);
        pauseMenu.SetActive(true);
        paused.SetActive(true);
        pauseMain.SetActive(true);
        isPaused = true;
    }

    void unpauseGame()
    {
        Time.timeScale = 1.0f;
        remove.SetActive(true);
        uiElements.SetActive(true);
        pauseMenu.SetActive(false);
        paused.SetActive(false);
        optionsMenu.SetActive(false);
        isPaused = false;

    }

    void Start()
    {
        if (isPaused == false)
        {
            unpauseGame();
            waiting = false;
        }

        else if (waiting == false)
        {
            pauseGame();
            waiting = true;
        }
    }

    void LateUpdate()
    {
        if (!deathScreen.activeInHierarchy)
        {
            if (Input.GetKeyDown(KeyCode.Escape) && isPaused == false)
            {
                pauseGame();
            }

            else if (Input.GetKeyDown(KeyCode.Escape) && isPaused == true)
            {
                unpauseGame();
                waiting = false;
            }

            if (paused.activeInHierarchy && waiting == false)
            {
                pauseGame();
                waiting = true;
            }
            else if (paused.activeInHierarchy == false)
            {
                unpauseGame();
                waiting = false;
            }
        }
    }

    public void exitMainMenu()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex -1);
    }
}