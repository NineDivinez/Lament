using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerDeathTimer : MonoBehaviour
{
    //Variables
    public float playerHealth;
    public static float timeAlive;
    public List<float> timer = new List<float>(); //Stores timers
    public bool timerUpdated = false;

    //Menu references
    public GameObject pauseMenu;
    public GameObject deathScreen;
    public Text writeTimer;

    void Start()
    {
        timer.Add(1f);
    }

    void Update()
    {
        playerHealth = PlayerControllerMovements.health;

        if (playerHealth <= 0)
        {
            if (timerUpdated == false)
            {
                timer[0] += Time.time;
                timerUpdated = true;
            }
            timeAlive = Time.time;
            if (wait(0, 3) == true)
            {
                Time.timeScale = 0f;
                pauseMenu.SetActive(false);
                deathScreen.SetActive(true);
                writeTimer.text = Mathf.Round(Time.time).ToString();
                Cursor.visible = true;
                Cursor.lockState = CursorLockMode.None;
            }
        }
    }

    bool wait(int index, float increaseMin, float increaseMax = 1) //True or false wait timer
    {
        if (increaseMax < increaseMin)
        {
            increaseMax = increaseMin;
        }

        checkList(index, increaseMin, increaseMax);
        if (Time.time > timer[index])
        {
            float randomWaitTime = Random.Range(increaseMin, increaseMax);
            timer[index] += randomWaitTime;
            return true;
        }
        else
        {
            return false;
        }
    }
    int checkList(int index, float increaseMin, float increaseMax)
    {
        if (timer.Count < index + 1)
        {
            print("Error.  Index out of range.  Increasing.");
            float randomWaitTime = Random.Range(increaseMin, increaseMax);
            do
                timer.Add(randomWaitTime);
            while (timer.Count < index + 1);
        }
        return index;
    }

    public void endGameButton()
    {
        Application.Quit();
    }
}
