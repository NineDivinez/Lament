using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GhostFlameRotation : MonoBehaviour
{
    //Variables Declaired
    public float speed = 25f;
    public float distance = 0f;

    //Game Objects
    public GameObject ghostFlameController;

    void Update()
    {
        distance = Vector3.Distance(ghostFlameController.transform.position, transform.position);
        speed = (100 / distance) + distance;

        if (speed > 30)
        {
            speed = 30;
        }
        transform.RotateAround(ghostFlameController.transform.position, Vector3.up, speed *Time.deltaTime);
    }
}
