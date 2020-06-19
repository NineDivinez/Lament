using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimerContainer : MonoBehaviour
{
    #region Singleton
    public static TimerContainer instance;
    void Awake()
    {
        instance = this;
    }
    #endregion
    //opens the script to have its data pulled by other scripts

    //Variables
    public static List<float> timer = new List<float>(); //Stores timers
    public static List<bool> countdownStarted = new List<bool>();
    public static List<float> countDownTime = new List<float>();
    public static List<bool> immediateTrue = new List<bool>();

    //
    //Timer 0: movement Checks (EnemyController) 4 to 8 seconds
    //Timer 1: player checks (EnemyController) 4 to 8 seconds
    //Timer 2: detection chance decrease timer (EnemyController) 10 seconds
    //Timer 3: Second detection chance decrease timer (EnemyController) 10 seconds
    //Timer 4: aggression level tiemr (Behavior) 5 to 10 seconds for testing, 30 to 45 seconds normally
    //Timer 5: Behavior update timer (Behavior) 10 to 30 seconds.
    //Timer 6: check if the ghost is stuck (EnemyMovement) 0.5 seconds
    //Timer 7: debug print for behaviors chosen (EnemyMovement) 2 seconds
    //Timer 8: chooses a new point to move to when player is not found (EnemyMovement) 3 to 8 seconds.
    //Timer 9: helps with deciding when to flicker the flashlight. (flicker) 3 to 5 seconds.
    //Timer 10: Forgot to make note... sorry
    //timer 11: 
    //

    public static int wait(int index, float increaseMin, int addMin, float increaseMax = 1, int addMax = 1) //increases timer when time runs out.
    {
        //First step is make sure that the mins and maxs are set up to generate a random number properly.
        if (increaseMax < increaseMin)
        {
            increaseMax = increaseMin;
        }
        if (addMax < addMin)
        {
            addMax = addMin;
        }

        checkList(index, increaseMin, increaseMax); //Esnures that the index exists in the timers.
        if (Time.time > timer[index])
        {
            float randomWaitTime = Random.Range(increaseMin, increaseMax);
            timer[index] += randomWaitTime;

            //generates a random number to increase the return int by.
            int rnd = Random.Range(addMin, addMax);
            return rnd;
        }
        else
        {
            return 0;
        }
    }

    public static bool wait(int index, float increaseMin, float increaseMax = 1) //True or false wait timer
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

    public static void checkList(int index, float minIncrease, float maxIncrease)
    {
        if (timer.Count < index + 1)
        {
            print("Error:  Index out of range.  Increasing.");
            do
                timer.Add(0);
            while (timer.Count < index + 1);
        }
        else if (timer[index] == 0)
        {
            print("Error:  Timer is blank.  Increasing timer.");
            float randomWaitTime = Random.Range(minIncrease, maxIncrease);
            timer[index] = randomWaitTime + Time.time;
        }
    }

    //
    //countdown 0: Defocuse an object the player interacts with. (Interact) 5 seconds
    //countdown 1: changes aggression for ghost should the player enter her house and stay. (RangeToHouse) 20 seconds
    //countdown 2: prevents the game from continuing so the flashlight does not flicker too quickly (flicker) 0.2 - 1 second seconds
    //Countdown 3: debug printing for interaction system. (Interact) 5 seconds

    public static bool countdown(int index, float start, bool startTimer, bool returnImmediateTrue = false)
    {
        float currentTime = Time.time;
        checkListBool(index, currentTime, start, startTimer, returnImmediateTrue);
        bool timerDone = false;

        if (immediateTrue[index] == true)
        {
            immediateTrue[index] = false;
            return true;
        }

        if (!startTimer)
        {
            if (currentTime >= countDownTime[index])
            {
                timerDone = true;
            }
        }
        else if (!countdownStarted[index])
        {
            countdownStarted[index] = true;
            countDownTime[index] = currentTime + start;
            timerDone = false;
        }
        else if (currentTime < countDownTime[index])
        {
            timerDone = false;
        }
        else if (currentTime >= countDownTime[index])
        {
            countdownStarted[index] = false;
            timerDone = true;
        }
        else
        {
            print("There is an unknown error involving countdowns.");
        }
        return timerDone;
    }

    public static void checkListBool(int index, float currentTime, float start, bool startTimer, bool returnImmediateTrue)
    {
        if (countdownStarted.Count < index +1)
        {
            print("Error:  Index for bool list out of range.  Increasing.");
            int loopsBool = 0;
            int loopsCountdown = 0;
            int loops = 0;
            do
            {
                countdownStarted.Add(returnImmediateTrue);
                loopsBool += 1;
            } while (countdownStarted.Count < index + 1);
            print("Looped " + loopsBool + " time(s).");

            if (countDownTime.Count < index + 1)
            {
                print("Error:  Index for countdown out of range.  Increasing.");
                do
                {
                    countDownTime.Add(currentTime + start);
                    loopsCountdown += 1;
                } while (countDownTime.Count < index + 1);
                print("Looped " + loopsCountdown + " time(s)");
            }
            else
            {
                print("THERE WAS A CRITICAL ERROR WITH COUNTDOWNS.");
            }

            if (immediateTrue.Count < index +1)
            {
                print("Error:  Index for immediateTrue list out of range.  Increasing.");
                do
                {
                    immediateTrue.Add(true);
                    loops += 1;
                } while (immediateTrue.Count < index + 1);
                print("Looped " + loops + " time(s)");
            }
        }
    }
}