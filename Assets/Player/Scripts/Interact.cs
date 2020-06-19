using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Interact : MonoBehaviour
{
    //Variables
    public Interactable focus;
    bool focused = false;
    bool interactedWithMatches = false;
    public float activateDist = 5f;

    //Game Objects
    public Camera cam;
    public AudioSource logSFX;
    public AudioSource useless;
    public GameObject ghostSFX;
    public GameObject interactIndicator;
    public Text interactText;
    public GameObject isPaused;

    void affect()
    {
        if (focus.gameObject.name == "Log")
        {
            if (!logSFX.isPlaying)
            {
                print("Playing sound");
                logSFX.Play(0);
            }
        }
        else if (focus.gameObject.name == "Matches")
        {
            if (!interactedWithMatches)
            {
                ghostSFX.SetActive(true);
                Behavior.changeAggression("touched matches");
                interactedWithMatches = false;
            }
        }
        else //This should happen when there is no code set up for the object.
        {
            useless = GetComponent<AudioSource>();
            useless.Play(0);
        }
    }

    void Update()
    {
        RaycastHit lookingAt;
        if (Physics.Raycast(cam.transform.position, transform.forward, out lookingAt, 3))
        {
            Interactable interactable = lookingAt.collider.GetComponent<Interactable>();

            if (interactable != null && !isPaused.activeInHierarchy)
            {
                if (TimerContainer.countdown(3, 5f, true, true))
                {
                    print("Interactable object detected");
                }
                interactIndicator.SetActive(true);
                interactText.text = "Touch " + interactable.gameObject.name + "?";
            }
        }
        else
        {
            print("No interactable object detected.");
            interactIndicator.SetActive(false);
        }

        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = cam.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit, activateDist))
            {
                Interactable interactable = hit.collider.GetComponent<Interactable>();

                if (interactable != null)
                {
                    print("We found an interactable object");
                    if (Vector3.Distance(this.transform.position, hit.collider.transform.position) <= activateDist)
                    {
                        print("Object is within range.");
                        setFocus(interactable);
                        affect();
                    }
                }
                else
                {
                    print("Not an interactable object.  Is this an error?");
                }
            }
        }
        focused = focus != null;
        if (focused)
        {
            if (TimerContainer.countdown(0, 0.2f, focused, focused))
            {
                affect();
                removeFocus();
            }
        }
    }

    void setFocus(Interactable newFocus)
    {
        if (newFocus != focus)
        {
            if (focused)
            {
                print("There is already a focus.  Resetting.");
                focus.onDefocused();
            }
            focus = newFocus;
        }
        newFocus.onFocused(this.transform);
    }
    void removeFocus()
    {
        print("Object has been defocused.");
        focus.onDefocused();
        focus = null;
    }
}
