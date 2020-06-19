using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenuHandler : MonoBehaviour
{
    //Game References
    public GameObject main;
    public GameObject options;
    public GameObject universalController;
    public GameObject title;
    public GameObject instructions;
    public Text timeToStart;

    //Variables
    public List<float> timer = new List<float>();
    bool buttonPressed = false;

    public void playGame()
    {
        instructions.SetActive(true);
        title.SetActive(false);
        main.SetActive(false);
    }

    void Start()
    {
        timer.Add(20);
    }

    void Update()
    {
        
        if (instructions.activeInHierarchy)
        {
            timeToStart.text = (Mathf.Round(timer[0] - Time.time)).ToString() + " Seconds";
            if (buttonPressed == false)
            {
                timer[0] += Time.time;
                buttonPressed = true;
            }
            if (wait(0, 20))
            {
                instructions.SetActive(false);
                SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
            }
        }
    }

    public void quitGame()
    {
        Application.Quit();
    }

    //Timer code
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
}
