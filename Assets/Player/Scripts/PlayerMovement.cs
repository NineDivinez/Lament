using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerMovement : MonoBehaviour
{
    public float movementRate = 2.0f;
    public double verticalThreshold = 0.8;
    public double horizontalThreshold = 0.8;
    public GameObject player;
    public GameObject isPaused;
    public Rigidbody playerRB;

    public float mouseSensitivity = 100.0f;
    public float clampAngle = 80.0f;

    private float rotY = 0.0f;
    private float rotX = 0.0f;

    public bool debugging = false;

    void Start()
    {
        Vector3 rot = transform.localRotation.eulerAngles;
        rotY = rot.y;
        rotX = rot.x;
    }

    public void OnValueChanged(float newValue)
    {
        mouseSensitivity = newValue;
    }

    void debugingMode()
    {
        if (debugging == false)
        {
            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.None;
            debugging = true;
        }
        else if (debugging == true)
        {
            Cursor.visible = false;
            Cursor.lockState = CursorLockMode.Locked;
            debugging = false;
        }
    }

    void Update()
    {
        float horizontalInput = Input.GetAxis("Horizontal");
        float verticalInput = Input.GetAxis("Vertical");
        
        //cursor handling
        if (Input.GetKeyDown(KeyCode.P) && isPaused.activeInHierarchy == false)
        {
            debugingMode();
        }

        if (isPaused.activeInHierarchy == true)
        {
            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.None;
            debugging = false;
        }
        else if (isPaused.activeInHierarchy == false && debugging == false)
        {
            Cursor.visible = false;
            Cursor.lockState = CursorLockMode.Locked;
        }

        //movement handling
        if (horizontalInput >= horizontalThreshold)
        {
            playerRB.AddForce(transform.right * movementRate);
            print("Turning right.");

            if (verticalInput >= verticalThreshold)
            {
                playerRB.AddForce(transform.forward * movementRate);
                print("Moving forward.");
            }
            else if (verticalInput <= -verticalThreshold)
            {
                playerRB.AddForce(-transform.forward * movementRate);
                print("Moving backward.");
            }
        }
        else if (horizontalInput <= -horizontalThreshold)
        {
            playerRB.AddForce(-transform.right * movementRate);
            print("Turning left.");

            if (verticalInput >= verticalThreshold)
            {
                playerRB.AddForce(transform.forward * movementRate);
                print("Moving forward.");
            }
            else if (verticalInput <= -verticalThreshold)
            {
                playerRB.AddForce(-transform.forward * movementRate);
                print("Moving backward.");
            }
        }
        else if (verticalInput >= verticalThreshold)
        {
            playerRB.AddForce(transform.forward * movementRate);
            print("Moving forward.");

            if (horizontalInput >= horizontalThreshold)
            {
                playerRB.AddForce(transform.right * movementRate);
                print("Turning right.");
            }
            else if (horizontalInput <= -horizontalThreshold)
            {
                playerRB.AddForce(-transform.right * movementRate);
                print("Turning left.");
            }
        }
        else if (verticalInput <= -horizontalThreshold)
        {
            playerRB.AddForce(-transform.forward * movementRate);
            print("Moving backward.");

            if (horizontalInput >= horizontalThreshold)
            {
                playerRB.AddForce(transform.right * movementRate);
                print("Turning right.");
            }
            else if (horizontalInput <= -horizontalThreshold)
            {
                playerRB.AddForce(-transform.right * movementRate);
                print("Turning left.");
            }
        }

        float mouseX = Input.GetAxis("Mouse X");
        float mouseY = -Input.GetAxis("Mouse Y");

        rotY += mouseX * mouseSensitivity * Time.deltaTime;
        rotX += mouseY * mouseSensitivity * Time.deltaTime;

        rotX = Mathf.Clamp(rotX, -clampAngle, clampAngle);

        Quaternion localRotation = Quaternion.Euler(rotX, rotY, 0.0f);
        transform.rotation = localRotation;
    }
}