using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerControllerMovements : MonoBehaviour
{
    #region Singleton
    public static PlayerControllerMovements instance;
    void Awake()
    {
        instance = this;
    }
    #endregion
    //opens the script to have its data pulled by other scripts

    //Variables
    float walkingSpeed = 200f;
    float runningSpeed = 400f;
    float currentMoveRate;
    public float maxViewDist = 10f;
    public static float detectionDist = 10f;
    public double verticalThreshold = 0.1;
    public double horizontalThreshold = 0.1;
    public float sprint = 0f;
    float speed = 50;
    float maxSprint = 100f;
    bool cooldownWait = false;
    public bool debugging = false;
    public static float health = 100f;
    public bool isKillable = true;

    public float mouseSensitivity = 100.0f;
    public float clampAngle = 80.0f;
    private float rotY = 0.0f;
    private float rotX = 0.0f;

    //Game Objects
    public Light flashlight;
    public GameObject player;
    public GameObject isPaused;
    public CharacterController playerController;
    public GameObject light;
    public Image sprintImage;
    public Animator walkingAnimation;
    public GameObject ghost;
    public GameObject debuggingObject;
    public Image healthBar;
    public GameObject deathScreen;
    public AudioSource sfx;

    void Start()
    {
        Vector3 rot = transform.localRotation.eulerAngles;
        rotY = rot.y;
        rotX = rot.x;
        debugingMode();
    }

    public void changeMouseSensitivity(float newValue)
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
            light.SetActive(true);
            debuggingObject.SetActive(false);
        }
        else if (debugging == true)
        {
            Cursor.visible = false;
            Cursor.lockState = CursorLockMode.Locked;
            debugging = false;
            light.SetActive(false);
            debuggingObject.SetActive(true);
        }
    }

    void Update()
    {
        float horizontalInput = Input.GetAxis("Horizontal");
        float verticalInput = Input.GetAxis("Vertical");

        float distanceToGhost = Vector3.Distance(ghost.transform.position, this.transform.position);

        //Health Manager
        if (distanceToGhost <= 15)
        {
            if (health > 0)
            {
                if (debuggingObject.activeInHierarchy) //do not change this, or it will break debugging mode as well as sprint... No idea why
                {
                    int healthDrainRate = 15;
                    if (isKillable)
                    {
                        health -= healthDrainRate * (2 * Time.deltaTime) / (distanceToGhost / 3);
                    }
                }
            }
        }
        else if (distanceToGhost > 15)
        {
            if (health < 100 && health != 0)
            {
                health += 3 *Time.deltaTime;
            }
            if (health > 100)
            {
                health = 100;
            }

        }
        if (Input.GetKeyDown(KeyCode.Q))
        {
            health = 50;
        }

        if ((1 - (health / 100)) < 0)
        {
            healthBar.fillAmount = (1 - (health / 100)) * -1; //shows the health bar so it increases as you burn (Cuz I like that more than decreasing)
        }
        else
        {
            healthBar.fillAmount = (1 - (health / 100)); //shows the health bar so it increases as you burn (Cuz I like that more than decreasing)
        }
        //Health Manager End

        //cursor handling
        if (Input.GetKeyDown(KeyCode.P))
        {
            debugingMode();
        }

        if (health > 0)
        {
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
        }

        //movement handling start
        float mouseX = Input.GetAxis("Mouse X");
        float mouseY = -Input.GetAxis("Mouse Y");
        bool sprinting = false;

        if (health > 0) //only allows the player to move if they are alive
        {
            if (horizontalInput >= horizontalThreshold && verticalInput == 0)
            {
                playerController.SimpleMove(transform.right * currentMoveRate * Time.deltaTime);

            }
            else if (horizontalInput <= -horizontalThreshold && verticalInput == 0)
            {
                playerController.SimpleMove(-transform.right * currentMoveRate * Time.deltaTime);
            }
            else if (verticalInput >= verticalThreshold)
            {
                if (horizontalInput == 0)
                {
                    playerController.SimpleMove(transform.forward * currentMoveRate * Time.deltaTime);
                }
                else if (horizontalInput >= horizontalThreshold)
                {
                    playerController.SimpleMove((transform.forward + transform.right) * currentMoveRate * Time.deltaTime);
                }
                else if (horizontalInput <= -horizontalThreshold)
                {
                    playerController.SimpleMove((transform.forward + -transform.right) * currentMoveRate * Time.deltaTime);
                }
            }
            else if (verticalInput <= -verticalThreshold)
            {
                if (horizontalInput == 0)
                {
                    playerController.SimpleMove(-transform.forward * currentMoveRate * Time.deltaTime);
                }
                else if (-horizontalInput >= horizontalThreshold)
                {
                    playerController.SimpleMove((-transform.forward + -transform.right) * currentMoveRate * Time.deltaTime);
                }
                else if (-horizontalInput <= horizontalThreshold)
                {
                    playerController.SimpleMove((-transform.forward + transform.right) * currentMoveRate * Time.deltaTime);
                }
            }
            else if (verticalInput == 0 && horizontalInput == 0)
            {
                if (flashlight.intensity == 1f)
                {
                    detectionDist = 10f;
                }
                else if (flashlight.intensity < 1f)
                {
                    detectionDist = 5f;
                }
            }
            //sprinting and movement detection
            if (sprint > 0)
            {
                if (sprint > 0 && cooldownWait == true)
                {
                    sprint -= speed * Time.deltaTime;
                    sprintImage.fillAmount = sprint / 100;
                }
                else if (sprint > 0 && !Input.GetKey(KeyCode.LeftShift))
                {
                    sprint -= speed * Time.deltaTime;
                    sprintImage.fillAmount = sprint / 100;
                }
                if (sprint < 0)
                {
                    sprint = 0;
                    cooldownWait = false;
                }
            }

            //toggling fire
            if (Input.GetKeyDown(KeyCode.Space) && flashlight.intensity == 1f)
            {
                flashlight.intensity = 0f;
                sfx.Play();
            }
            else if (Input.GetKeyDown(KeyCode.Space) && flashlight.intensity < 1f)
            {
                flashlight.intensity = 1f;
                sfx.Play();
            }
            
            if (Input.GetKey(KeyCode.LeftShift) && sprint < maxSprint && cooldownWait == false)
            {
                sprinting = true;
                if (cooldownWait == false)
                {
                    currentMoveRate = runningSpeed;
                    if (horizontalInput != 0 || verticalInput != 0)
                    {
                        if (!debugging)
                        {
                            sprint += speed * Time.deltaTime;
                            sprintImage.fillAmount = sprint / 100;
                        }
                        if (flashlight.intensity == 1f)
                        {
                            detectionDist = 25f;
                        }
                        else if (flashlight.intensity < 1f)
                        {
                            detectionDist = 20f;
                        }
                    }

                    if (sprint >= maxSprint)
                    {
                        cooldownWait = true;
                    }
                }
            }
            else if (!Input.GetKey(KeyCode.LeftShift))
            {
                sprinting = false;
                currentMoveRate = walkingSpeed;
                if (horizontalInput != 0 || verticalInput != 0)
                {
                    if (flashlight.intensity < 1f)
                    {
                        detectionDist = 12f;
                    }
                    else if (flashlight.intensity == 1f)
                    {
                        detectionDist = 17f;
                    }
                }
            }

            //mouse movements
            rotY += mouseX * mouseSensitivity * Time.deltaTime;
            rotX += mouseY * mouseSensitivity * Time.deltaTime;

            rotX = Mathf.Clamp(rotX, -clampAngle, clampAngle);

            Quaternion localRotation = Quaternion.Euler(rotX, rotY, 0.0f);
            transform.rotation = localRotation;
        }
        //movement handling end

        //handles the animations
        runAnimation(sprinting, horizontalInput, verticalInput); 
    }

    void runAnimation(bool sprinting, float horizontal, float vertical)
    {
        if (health > 0)
        {
            if (sprinting) //increase playspeed of walking animation
            {
                walkingAnimation.SetFloat("speedPercent", 0.4444445f, .1f, Time.time);
            }
            else if (horizontal > 0 || horizontal < 0) //play walking animation if moving horizontal.
            {
                walkingAnimation.SetFloat("speedPercent", 0.2222222f, .1f, Time.time);
            }
            else if (vertical < 0 || vertical > 0) //play walking animation if moving vertical.
            {
                walkingAnimation.SetFloat("speedPercent", 0.2222222f, .1f, Time.time);
            }
            else //Sets the animation back to idle.
            {
                walkingAnimation.SetFloat("speedPercent", 0f, .1f, Time.time);
            }
        }
        else if (health <= 0) //Prevents the player from moving and performs the death animation.
        {
            walkingAnimation.SetFloat("speedPercent", 0.6666667f, .2f, Time.deltaTime);
        }
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position, maxViewDist);
    }
}