using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Interactable : MonoBehaviour
{
    //Variables
    public float radius = 5f;
    public bool isFocused = false;
    bool interactedWith = false;

    //Game Objects
    Transform player;
    

    void Update()
    {
        if (isFocused)
        {
            float distance = Vector3.Distance(player.position, this.transform.position);

            if (distance <= radius && !interactedWith)
            {
                print("Interacted with " + this.name + "?");
                interactedWith = true;
            }
        }
    }

    public void onFocused(Transform playerTransform)
    {
        isFocused = true;
        player = playerTransform;
    }

    public void onDefocused ()
    {
        isFocused = false;
    }
}
