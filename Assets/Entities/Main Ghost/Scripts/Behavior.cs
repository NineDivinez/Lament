using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class Behavior : MonoBehaviour
{
    #region Singleton
    public static Behavior instance;
    void Awake()
    {
        instance = this;
    }
    #endregion   
    //opens the script to have its data pulled by other scripts

    //declared variables
    public static int aggressionLevel = 0; //This determins how aggressive she will be with each behavior.  The higher the number, the more likely she is to attack despite being in passive or aggressive behaviors.
    bool targetSighted;
    public static bool targetSightedRecently;
    float lastSeen;
    public string currentBehavior = "";
    public bool increasedAggressionTesting = false;

    //References to game objects
    public Rigidbody rb; //The ghost's Rigidbody Component.
    public GameObject[] behaviorObjects;

    //Referance to code to access all variables and code to EnemyMovement
    public EnemyMovement ghostMovementSys = new EnemyMovement();

    void Start()
    {
        behaviorObjects = GameObject.FindGameObjectsWithTag("Behaviors"); //Assigns the behaviors so we can announce them to other scripts.
        for (int i = 0; i <= behaviorObjects.Length -1; i++) //Deactivates all other behaviors except the default one.
        {
            if (behaviorObjects[i].name != "Searching")
            {
                behaviorObjects[i].SetActive(false);
            }
        }
    }

    //use this externally only
    public static void recentlySighted()
    {
        targetSightedRecently = true;
    }

    void Update()
    {
        if (increasedAggressionTesting)
        {
            aggressionLevel += TimerContainer.wait(4, 5f, -2, 10, 7); //wait for 5 to 10 seoconds, and increase the 
        }
        else
        {
            aggressionLevel += TimerContainer.wait(4, 30, -2, 45, 7);
        }
        
        for (int i = 0; i <= behaviorObjects.Length -1; i++)
        {
            if (behaviorObjects[i].activeInHierarchy) //loops through and finds an active object for the current behavior
            {
                currentBehavior = behaviorObjects[i].name; //Assings the current behavrior to a string so we can use it later
                break; //We've already found the current behavior, so exit the loop.
            }
        }

        if (Input.GetKeyDown(KeyCode.Q))
        {
            aggressionLevel = 100;
        }

        targetSighted = EnemyController.targetSighted;
        if (targetSighted == false && currentBehavior != "Searching") 
        {
            print("Searching behavior chosen.");
            setBehavior(0);
        }
        else if (targetSighted == true /*&& currentBehavior == "Searching"*/)
        {
            targetFound(EnemyController.targetSighted, targetSightedRecently);
        }
        else
        {
            /*print("There's been an error with deciding behaviors... no paramaters were met.");
            print("Current time: " + Time.time);
            print(EnemyController.targetSighted);
            print(targetSighted);
            print(currentBehavior);*/
        }
        targetSightedRecently = false;
    }

    void targetFound(bool yesNo, bool recently) //Target sighted and recently
    {
        if (yesNo == true)
        {
            if (TimerContainer.wait(5, 10f, 30f))
            {
                print("Time to wait until next check: " + TimerContainer.timer[5]);
                chooseBehavior();
            }
            else if (recently == true)
            {
                print("Update required before timer struck.");
                chooseBehavior();
            }
        }
        else
        {
            print("There was an error... player was not really found?");
            print("Current time: " + Time.time);
            print(EnemyController.targetSighted);
            print(targetSighted);
            print(currentBehavior);
        }
    }

    void chooseBehavior()
    { //The code is failing somewhere around here.  Add prints to debug and find the source... it's detecting the player but not updating the behavriors
        //random number generator to decide the behavior
        float decider = Random.Range(1f, (behaviorObjects.Length -1) * 100f);
        int chosenBehavior;
        if (EnemyController.targetSighted == true)
        {
            //1 for Aggressive, 2 for Stalking.
            chosenBehavior = (int)(Mathf.Round(decider / 100f)); //Chooses a random number, from 1 to the length of the available behaviors.
            print("Chosen behavior before " + currentBehavior);
            //This is to allow for easier addition of new behaviors.
        }
        else if(decider /100 <= 0)
        {
            print("Is this your error?");
            int rnd = Random.Range(1, 2);
            chosenBehavior = rnd;
        }
        else
        {
            chosenBehavior = 0; //Sets to searching, because no player was found.
        }
        
        if (aggressionLevel >= 100) 
        {
            setBehavior(1);
        }
        else if (aggressionLevel < 100 && EnemyController.targetSighted)
        {
            int maxNumb = behaviorObjects.Length - 1 * 1000;
            int rnd = Random.Range(1, maxNumb);
            if (aggressionLevel < 25) //if aggression is less than 25
            {
                //chance is 25%
                if (chosenBehavior < maxNumb * (1 / 4))
                {
                    chosenBehavior = 1;
                }
                else
                {
                    chosenBehavior = 2;
                }
            }
            else if (aggressionLevel < 50 && aggressionLevel > 25) //If aggression is more than 25, but less than 50
            {
                //chance is 50/50
                chosenBehavior = (rnd / 1000);
            }
            else if (aggressionLevel < 75 && aggressionLevel > 50) //If aggression is more than 50, but less than 75
            {
                if (chosenBehavior < maxNumb * (3 / 4)) //chance is 75%
                {
                    chosenBehavior = 1;
                }
                else
                {
                    chosenBehavior = 2;
                }
            }
            else if (aggressionLevel < 100 && aggressionLevel > 75) //If aggression is more than 75, but less than 100
            {
                if (chosenBehavior <= maxNumb) //chance is 85% (Not filled in, but 100% chance for aggressive is already done.)
                {
                    chosenBehavior = 1;
                }
                else
                {
                    chosenBehavior = 2;
                }
            }
            setBehavior(chosenBehavior);
        }
    }

    void setBehavior(int behavior) //Use this to assign the ghost's current behavior after it is chosen.
    {
        for (int i = 0; i <= behaviorObjects.Length - 1; i++) //Deactivates all other behaviors except the chosen one.
        {
            if (behaviorObjects[i].activeInHierarchy)
            {
                if (behaviorObjects[i].name != behaviorObjects[behavior].name)
                {
                    print("Now disabling " + behaviorObjects[i].name);
                    behaviorObjects[i].SetActive(false);
                }
            }
        }
        behaviorObjects[behavior].SetActive(true);
    }

    public static void changeAggression(string action) //This should be called whenever the player performs an action needing to change aggression levels.
    {
        if (action == "journal touch")
        {
            aggressionLevel += 20;
        }
        else if (action == "journal take")
        {
            aggressionLevel += 30;
        }
        else if (action == "journal turn page")
        {
            aggressionLevel += 5;
        }
        else if (action == "enter house")
        {
            aggressionLevel += 10;
        }
        else if (action == "leave house")
        {
            aggressionLevel -= 7;
        }
        else if (action == "drops book")
        {
            aggressionLevel -= 15;
        }
        else if (action == "enter secret area with journal")
        {
            aggressionLevel += 20;
        }
        else if (action == "touched matches")
        {
            aggressionLevel += 45;
        }
    }
}