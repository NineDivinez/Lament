using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DebuggingCamera : MonoBehaviour
{
    public GameObject playerCamera;
    public GameObject ghostCamera;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.L))
        {
            if (playerCamera.activeInHierarchy)
            {
                playerCamera.SetActive(false);
                ghostCamera.SetActive(true);
            }
            else if (!playerCamera.activeInHierarchy)
            {
                playerCamera.SetActive(true);
                ghostCamera.SetActive(false);
            }
        }
    }
}
