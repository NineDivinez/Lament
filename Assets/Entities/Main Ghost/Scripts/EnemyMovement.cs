using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyMovement : MonoBehaviour
{
    public Rigidbody rb;
    public UnityEngine.AI.NavMeshAgent navMesh;
    public Transform target;
    public GameObject ghost;
    public Vector3 destination;
    public GameObject[] positions;
    public int posChosen = 0;
    public GameObject behaviors;

    //variables used for tracking movement
    public Vector3[] checks = new Vector3[6]; //Used to store positions to make sure the ghost is not getting stuck
    public int lastCheck = 0; //used with making sure the ghost is not stuck.  Tracks positions, not a timer
    public bool targetSighted; 
    public bool ghostReportedStuck = false;
    public Vector3 compare = new Vector3(); //Used to make sure that none of the positions recorded for the ghost getting stuck is blank.
    public Vector3 size; //Used for stalking the player.
    public Vector3 stayOut; //Also used for stalking the player, determins the area the ghost cannot move inside.
    public float startingSpeed = 2.5f;
    public static float ghostSpeed;

    void Start()
    {
        positions = GameObject.FindGameObjectsWithTag("Position");
        target = PlayerManager.instance.player.transform;
    }

    void Update()
    {
        behaviors = GameObject.FindGameObjectWithTag("Behaviors");
        targetSighted = EnemyController.targetSighted;
        movementHandler(targetSighted);
        ghostSpeed = navMesh.speed;
    }

    //tracking positions code start
    void checkPositions()
    {
        int numberOfChecks = checks.Length -1;
        for (int i = 0; i <= numberOfChecks; i++)
        {
            for (int x = numberOfChecks; x >= 0; x--)//two loops to run through all the coordinates we have recorded.
            {
                if (checks[x].Round(0) == checks[i].Round(0))//If any of the recorded coordinates are the same...
                {
                    if (x != i)//and we are not comparing a coordinate to itself...
                    {
                        if (checks[x] != compare)//and neither is not the same as compare (A blank Vector3)...
                        {
                            if (checks[i] == compare)//same as previous statement
                            {
                                print("There was a snag...");
                                print("This: " + checks[x].Round(0) + "\nWas the same as this: " + checks[i].Round(0)); //reporting what the issue was.
                                ghostReportedStuck = true; //ghost is stuck!
                                lastCheck = 0;
                                numberOfChecks = 0;
                                resetChecks();//This will reset all the recorded coordinates to blank and start again.
                                movementHandler(targetSighted);//clearly, we are not moving, so find a new location and report if the player is sighted.
                                break;
                            }
                        }
                    }
                }
            }
        }
    }

    void resetChecks() //Resets recorded coordinates when we made sure the ghost is not stuck.
    {
        for (int z = 0; z <= checks.Length - 1; z++)
        {
            checks[z] = new Vector3();
        }
    }
    //tracking positions code end

    //Movement methods
    void goToDest(Vector3 dest) //Handles setting the destination and the speed to get there.
    {
        print("Now going to : " + dest);
        destination = dest;
        float destDist = Vector3.Distance(this.transform.position, dest);
        print("Distance to destination: " + destDist);
        speedChange(Behavior.aggressionLevel);
        navMesh.SetDestination(dest);
    }

    void chooseDest()
    {
        print("Choosing new destination!");
        posChosen = Random.Range(1, positions.Length);
        print("We chose " + posChosen);
        if (posChosen != 0)
        {
            goToDest(positions[posChosen].transform.position);
        }
    }

    public void movementHandler(bool playerFound)
    {
        playerFound = EnemyController.targetSighted;
        if (playerFound == false)
        {
            //Makes sure we can run the check if the ghost is stuck
            if (TimerContainer.wait(6, 0.5f))
            {
                checks[lastCheck] = rb.position;
                lastCheck += 1;
                ghostReportedStuck = false;
                checkPositions();
                if (lastCheck >= 6) //Resets the points recorded to check if the ghost is stuck.
                {
                    lastCheck = 0;
                    resetChecks();
                }

                if (!ghostReportedStuck)
                {
                    print("There are no one positions that are the same.  Continue as normal!");
                }
                else
                {
                    print("The ghost has reported being stuck.  Searching for a new position!");
                    chooseDest();
                }
            }
            if (behaviors.name == "Searching")
            {
                if (TimerContainer.wait(7, 2f))
                {
                    print("Searching behavior chosen.");
                    searching();
                }
            }
            else
            {
                print("There was an erorr. Searching behavior is not active, but player is not found!");
            }
        }
        else
        {
            if (behaviors.name == "Aggressive")
            {
                if (TimerContainer.wait(7, 2f))
                {
                    print("Aggressive behavior chosen");
                }
                aggressive();
            }
            else if (behaviors.name == "Stalking")
            {
                if (TimerContainer.wait(7, 2f))
                {
                    print("Stalk behavior chosen");
                    stalk();
                }
            }
            
        }
    }
    //Movement methods end

    

    void stalk()
    {
        print("Stalking player...");
        
        Vector3 stalkPos = target.transform.position + new Vector3(Random.Range(-size.x / 2, size.x / 2), Random.Range(-size.y / 2, size.y / 2), Random.Range(-size.z / 2, size.z / 2));
        if (stalkPos.x <= 0)
        {
            stalkPos.x -= stayOut.x;
        }
        else if (stalkPos.x >= 0)
        {
            stalkPos.x += stayOut.x;
        }
        if (stalkPos.z >= 0)
        {
            stalkPos.z -= stayOut.z;
        }
        else if (stalkPos.z >= 0)
        {
            stalkPos.z += stayOut.z;
        }

        goToDest(stalkPos);
    }
    void aggressive()
    {
        goToDest(target.transform.position);
    }
    void searching()
    {
        if (TimerContainer.wait(8, 3f, 8f))
        {
            print("Player not found, searching for a wander position");
            chooseDest();
        }
        else if (behaviors.name != "Searching")
        {
            navMesh.Stop();
            movementHandler(targetSighted);
        }
    }
    void tracking()
    {

    }

    void speedChange(int aggroLevel)
    {
        float maxSpeed = 8f;
        float currentSpeed;
        float speedIncrease = 0.75f;

         if (aggroLevel > 25)
        {
            currentSpeed = (Mathf.Round(aggroLevel / 25) * speedIncrease) + startingSpeed;
        }
        else
        {
            currentSpeed = startingSpeed;
        }

        if (currentSpeed > maxSpeed)
        {
            currentSpeed = maxSpeed;
        }

        navMesh.speed = currentSpeed;
    }

}


static class ExtensionMethods
{
    /// <summary>
    /// Rounds Vector3.
    /// </summary>
    /// <param name="vector3"></param>
    /// <param name="decimalPlaces"></param>
    /// <returns></returns>
    public static Vector3 Round(this Vector3 vector3, int decimalPlaces = 2)
    {
        float multiplier = 1;
        for (int i = 0; i < decimalPlaces; i++)
        {
            multiplier *= 10f;
        }
        return new Vector3(
            Mathf.Round(vector3.x * multiplier) / multiplier,
            Mathf.Round(vector3.y * multiplier) / multiplier,
            Mathf.Round(vector3.z * multiplier) / multiplier);
    }
}