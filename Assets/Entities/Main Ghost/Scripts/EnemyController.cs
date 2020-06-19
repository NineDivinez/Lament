using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class EnemyController : MonoBehaviour
{
    #region Singleton
    public static EnemyController instance;
    void Awake()
    {
        instance = this;
    }
    #endregion 
    //opens the script to have its data pulled by other scripts

    //Declared Variables
    public static float time = 0;
    public int maxViewDist /*Do not change*/ = 20;
    public float movementSpeed = 330f;
    public static bool targetInfront = false;
    public static bool targetSighted = false;
    public static float detectionDistPlayer = 0f;
    public static int detectionChance = 0;
    public static int nextCheck = 0;
    public static int nextMoveCheck = 16;
    public int waitTime = 4;
    public static int minChance = 1;    //these will set the range for rolls for
    public static int maxChance = 120;  //the ghost to detect the player
    public static int detectionRoll = 0;
    public bool debugging = false;
    public bool ghostMovementDetection = true;
    public static float distanceFromPlayer = 0;
    Vector3 right;
    Vector3 left;

    //Referances to game objects
    public static GameObject chosenBehavior;
    public Rigidbody rb;
    Transform target;
    public GameObject scream;

    //Referance to access all variables and code to EnemyMovement
    public EnemyMovement ghostMovementSys = new EnemyMovement();

    void Start()
    {
        target = PlayerManager.instance.player.transform; //tells the ghost she is looking for this object in the world.
    }

    void runDetection(float distance, bool targetAlreadyFound = false)
    {
        //check if we can detect player
        bool tryToFind = true;
        if (distance >= 40 && distance < 150)
        {
            minChance = (-Mathf.RoundToInt(distance) + Mathf.RoundToInt(detectionDistPlayer)); //lowest min chance is -15.
            maxChance = 100 + (Mathf.RoundToInt(distance) - Mathf.RoundToInt(maxViewDist)); //lowest max chance is 120.
        }
        else if (distance >= 150) //makes it a 0% chance for the ghost to find the player after 150 feet distance.
        {
            print("There is no chance to find the player... must wander....");
            tryToFind = false;
        }
        else if (!targetAlreadyFound)
        {
            if (distance < 40 && distance > 20)
            {
                minChance = 100; //makes it a 17.3% chance for the ghost to fail to find the player if they are less then 40 units away.
            }
            else if (distance <= 20)
            {
                minChance = 115; //makes it a 4.2% chance the ghost will fail to find the player if they are less then 20 feet away.
            }
        }
        else if (targetAlreadyFound)
        {
            //this needs to change how calculations are done so that the player has a chance to escape and the ghost goes back to searching like before.
            if (distance <= 20)
            {
                minChance = 119;
            }
            else if (distance >= 70 && distance < 150)
            {
                if (targetInfront)
                {
                    if (TimerContainer.wait(2, 10f)) //Timer to make sure the ghost is given time before decreasing the chance
                    {
                        minChance = 100;
                    }
                    else
                    {
                        minChance = 110;
                    }
                }
                else
                {
                    if (TimerContainer.wait(3, 10f)) //Second timer to make sure the ghost is given time before decreasing the chance
                    {
                        minChance = 90;
                    }
                    else
                    {
                        minChance = 100;
                    }
                }
            }
        }

        if (tryToFind) //this tells us if we CAN check for the player.
        {
            detectionRoll = Random.Range(minChance, maxChance); //the ROLL

            if ((maxChance + minChance) > maxChance) //this is meant to make sure the required min role does not exceed possible roles.
            {
                if (distance < 40 && distance > 20)
                {
                    int aditionalChances = (maxChance - minChance) / 4;
                    detectionChance = minChance + aditionalChances;
                }
                else if (distance <= 20)
                {
                    int aditionalChances = (maxChance - minChance) / 10;
                    detectionChance = minChance + aditionalChances;
                }

            }
            else
            {
                detectionChance = maxChance + minChance; //Min required roll to detect player
            }

            bool agroDetectIncrease = false;

            if (targetAlreadyFound == true)
            {
                print("Target is found.  Decreasing chance of losing target. (-40)");
                detectionChance -= 40;
                if (agroDetectIncrease == false)
                {
                    if (Behavior.aggressionLevel >= 100 || chosenBehavior.name == "Aggressive")
                    {
                        print("Ghost is aggressive.  Decreasing chance further. (-60)");
                        detectionChance -= 60;
                        agroDetectIncrease = true;
                    }
                }
                agroDetectIncrease = false;
            }

            if (targetInfront) //Pretty straight forward... if target is infront... increase the odds of finding the player.
            {
                print("Target is in front of ghost.  Decreasing chance of losing target. (-30)");
                detectionChance -= 30;
                if (agroDetectIncrease == false)
                {
                    if (Behavior.aggressionLevel >= 100 || chosenBehavior.name == "Aggressive")
                    {
                        print("Ghost is aggressive.  Decreasing chance further. (-60)");
                        detectionChance -= 60;
                        agroDetectIncrease = true;
                    }
                }
                agroDetectIncrease = false;
            }
            else
            {
                if (distance > 70)
                {
                    print("Ghost is not close to target.  Increasing chance of losing target. (+15)");
                    detectionChance += 15;
                }
            }

            print("Final result of the ghost detecting the player: " + detectionChance);

            if (detectionRoll >= detectionChance) //Should run when the ghost finds the player.
            {
                targetSighted = true;
                Behavior.recentlySighted();
            }
            else
            {
                if (targetSighted)
                {
                    targetSighted = false;
                    print("Target was lost.");
                    print(detectionChance);
                    print(detectionRoll);
                }
            }
        }

        else //this should run if the ghost fails to find the player
        {
            targetSighted = false;
        }
    }

    void Update()
    {
        float distance = Vector3.Distance(target.position, transform.position);
        distanceFromPlayer = distance;
        time = Time.time;

        chosenBehavior = GameObject.FindGameObjectWithTag("Behaviors");

        if (Behavior.aggressionLevel >= 100)
        {
            scream.SetActive(true);
        }
        else if (Behavior.aggressionLevel <= 75)
        {
            scream.SetActive(false);
        }

        if (targetSighted && distance >= 150)
        {
            targetSighted = false;
        }

        detectionDistPlayer = PlayerControllerMovements.detectionDist; //pulls the info from the player movement script to see what the weight of detection is from them.

        //check if we are facing player
        float dot = Vector3.Dot(transform.forward, (target.position - transform.position).normalized);
        if (dot > 0.7f)
        {
            targetInfront = true;
        }
        else
        {
            targetInfront = false;
        }
        //facing player check end.

        if (TimerContainer.wait(1, 4f, 8f)) //handles the timer to check and see if the target can be found.
        {
            detectionRoll = 0;
            runDetection(distance, targetSighted);
            print("Detection check ran.  Next check at: " + TimerContainer.timer[1]);
        }
        else if (Input.GetKeyDown(KeyCode.I))
        {
            detectionRoll = 0;
            print("Forcing detection check");
            runDetection(distance, targetSighted);
        }

        if (Input.GetKeyDown(KeyCode.P)) //enter debugging mode
        {
            if (debugging)
            {
                debugging = false;
            }
            else
            {
                debugging = true;
            }
        }

        if (debugging)
        {
            Vector3 right = (transform.TransformDirection(Vector3.forward) + transform.TransformDirection(Vector3.right)) * 100;
            Vector3 left = (transform.TransformDirection(Vector3.forward) - transform.TransformDirection(Vector3.right)) * 100;
            Debug.DrawRay(transform.position, right, Color.green);
            Debug.DrawRay(transform.position, left, Color.green);
            Debug.DrawRay(transform.position, transform.forward, Color.yellow);
            Debug.DrawRay(transform.position, (target.position - transform.position).normalized, Color.yellow);
        }
    }

    /*void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, maxViewDist);
    }*/
}