using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{
    //Mouselook
    Vector2 rotation = Vector2.zero;
    public float aimSpeed = 3;
    public float aimLimitY = 15f;

    //Movement
    private CharacterController characterController;
    private Vector3 gravityMovement = Vector3.zero;
    public float gravitySpeed = 10f;
    public float moveSpeed = 3;

    //Desk
    public bool deskTime = false;
    public int whoBeggedForDesk = 0;

    //Score
    public int score = 0;
    public Text scoreText;

    //Game Over
    public GameObject failScreen;
    private bool isOver = false;

    void Start()
    {
        //If being restarted, prevents game from being frozen in time.
        Time.timeScale = 1f;

        //Initializing elements.
        scoreText = GameObject.Find("ScoreText").GetComponent<Text>();
        characterController = FindObjectOfType<CharacterController>();

        //Locks the cursor and hides it.
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    void Update()
    {
        //Exit
        if (Input.GetButtonDown("Cancel")) { Application.Quit(); }

        //Updates score text.
        scoreText.text = "Grade - " + score;

        if (!isOver)
        {
            MouseAim();
            PlayerMovement();

            //Interaction
            if (Input.GetButtonDown("Fire1"))
            {
                RaycastHit hit;
                if (Physics.Raycast(transform.position, transform.forward, out hit))
                {
                    if (hit.distance < 2f)
                    {
                        //Talking
                        if (hit.transform.gameObject.GetComponent<EmployeeController>() != null)
                        {
                            hit.transform.gameObject.GetComponent<EmployeeController>().Talk();
                        }
                        //Desk
                        else if (deskTime && hit.transform.gameObject.tag == "Desk")
                        {
                            switch (whoBeggedForDesk)
                            {
                                case 1:
                                    GameObject.Find("Worker 1").GetComponent<EmployeeController>().Win();
                                    break;
                                case 2:
                                    GameObject.Find("Worker 2").GetComponent<EmployeeController>().Win();
                                    break;
                                case 3:
                                    GameObject.Find("Worker 3").GetComponent<EmployeeController>().Win();
                                    break;
                            }
                        }
                    }
                }
            }
        }
    }

    void MouseAim()
    {
        rotation.y += Input.GetAxis("Mouse X");
        rotation.x += -Input.GetAxis("Mouse Y");
        rotation.x = Mathf.Clamp(rotation.x, -aimLimitY, aimLimitY);
        transform.eulerAngles = (Vector2)rotation * aimSpeed;
    }

    void PlayerMovement()
    {
        Vector3 movement = Quaternion.Euler(0, this.transform.eulerAngles.y, 0) * new Vector3(Input.GetAxis("Horizontal"), 0.0f, Input.GetAxis("Vertical"));

        if (!characterController.isGrounded)
        {
            gravityMovement.y -= gravitySpeed * Time.deltaTime;
        }

        characterController.Move(moveSpeed * Time.deltaTime * movement + gravityMovement * Time.deltaTime); 
    }

    //When one of the workers is at 0%
    public void Lose()
    {
        //Prevents player input.
        isOver = true;
        rotation = Vector2.zero;
        
        //Allows the mouse to move.
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;

        //Freezes time.
        Time.timeScale = 0f;

        //Shows fail screen and final score.
        failScreen.SetActive(true);
        GameObject.Find("FailLabel").GetComponent<Text>().text = "You got a grade of " + score + ".";
    }

    //Reloads the level.
    public void Restart() { SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex); }
}
