using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DebuggingDisplayController : MonoBehaviour
{
    public GameObject debugDisplay;
    public Text currentTime;
    public Text timeToNextPlayerCheck;
    public Text timeToNextMovementCheck;
    public Text distanceToPlayer;
    public Text playerWeightForDetection;
    public Text rollToDetectPlayer;
    public Text chanceOfDetection;
    public Text detectionMinChance;
    public Text detectionMaxChance;
    public Text playerInfrontOfGhost;
    public Text playerFound;
    public Text ghostMood;
    public Text recordedPosition1;
    public Text recordedPosition2;
    public Text recordedPosition3;
    public Text ghostStuck;
    public Text playerHealth;
    public Text aggressionLevel;
    public Text ghostSpeed;

    public EnemyMovement ghostMovementSys = new EnemyMovement();

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.H)) //print debugging
        {
            if (!debugDisplay.activeInHierarchy)
            {
                debugDisplay.SetActive(true);
                print("Debugging mode has been activated!");
            }
            else if (debugDisplay.activeInHierarchy)
            {
                debugDisplay.SetActive(false);
                print("Debugging mode has been deactivated.. No fun!");
            }

        }

        if (debugDisplay.activeInHierarchy) //displays all the debugging info to the screen
        {
            currentTime.text = ("Current time: " + EnemyController.time.ToString());
            timeToNextMovementCheck.text = ("Next movement check: " + EnemyController.nextMoveCheck.ToString());
            timeToNextPlayerCheck.text = ("Next player detection check: " + EnemyController.nextCheck.ToString());
            distanceToPlayer.text = ("Distance from ghost to player: " + EnemyController.distanceFromPlayer.ToString());
            playerWeightForDetection.text = ("Player Detection Level: " + EnemyController.detectionDistPlayer.ToString());
            detectionMinChance.text = ("Min chance of detection: " + EnemyController.minChance.ToString());
            detectionMaxChance.text = ("Max chance of detection: " + EnemyController.maxChance.ToString());
            chanceOfDetection.text = ("Chance of ghost detecting player: " + EnemyController.detectionChance.ToString());
            rollToDetectPlayer.text = ("Value of ghost's attempt to find player: " + EnemyController.detectionRoll.ToString());
            playerFound.text = ("Has player been found: " + EnemyController.targetSighted.ToString());
            playerInfrontOfGhost.text = ("Is target in front of the ghost: " + EnemyController.targetInfront.ToString());
            ghostMood.text = ("Current mood: " + EnemyController.chosenBehavior.name.ToString());
            recordedPosition1.text = ("First recorded position: " + ghostMovementSys.checks[0].ToString());
            recordedPosition2.text = ("Second recorded position: " + ghostMovementSys.checks[1].ToString());
            recordedPosition3.text = ("Third recorded position: " + ghostMovementSys.checks[2]).ToString();
            ghostStuck.text = ("Ghost reporting stuck: " + ghostMovementSys.ghostReportedStuck.ToString());
            playerHealth.text = ("Current player health is: " + PlayerControllerMovements.health.ToString());
            aggressionLevel.text = ("Current ghost aggression: " + Behavior.aggressionLevel).ToString();
            ghostSpeed.text = ("Current ghost movement speed: " + EnemyMovement.ghostSpeed.ToString());
        }
    }
}
