using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateFlame : MonoBehaviour
{
    //Declared Variables
    public float speed = 0f;
    public Vector3 startingPos;
    public float distance;

    //Game Objects
    public GameObject flameContainer;

    void Start()
    {
        flameContainer = GameObject.FindWithTag("FlameSpawner");
        startingPos = this.transform.position; //gets the starting position for deletion
        distance = Vector3.Distance(flameContainer.transform.position, transform.position); //gets the distance from the flame to the spawner
        speed = (100 / distance) * 4; //sets the movement speed for the flame.
    }

    void Update()
    {
        //You spin me round baby, round, right round!
        transform.RotateAround(flameContainer.transform.position, Vector3.up, speed *Time.deltaTime);
        
        if (Time.deltaTime >= 1)
        {
            if (this.transform.position.Round(0) == startingPos.Round(0))
            {
                Destroy(this);
            }
        }
    }
}