using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EmployeeController : MonoBehaviour
{
    //Employee IDs
    //1/2/3 | Worker 1/2/3
    //4     | Bosso
    public int EmployeeID = 0;

    //Handles animations
    private Animator animator;

    //UI Elements
    public float percentage = 100;
    public Slider progressBar;
    public Text progressText;
    public GameObject alertUI;

    //Text Bubble
    public Text requestText;
    public GameObject textBubble;

    //Request IDs
    //1 | Talk to Worker
    //2 | Go to Desk
    //3 | Talk to Bosso
    private int request = 0;
    
    //Conditions of Asking Requests
    private bool readyToAsk = true;
    private bool asking = false;

    //Being Talked To
    public bool beingAsked = false;
    public int whoAsked = 0;

    //Phrases to Say    | The phrases for requests.
    //Prases to Respond | The phrases for when a Worker talks to them.
    public Phrases phrasesToSay;
    public Phrases phrasesToRespond;

    //Audio
    private AudioSource audioSource;
    public AudioClip talk;
    public AudioClip angryTalk;
    public AudioClip happyTalk;
    public AudioClip alert;

    private void Start() { animator = GetComponent<Animator>(); audioSource = GetComponent<AudioSource>(); }

    void Update()
    {
        //Prevents overflowing/underflowing progress bars.
        if(percentage > 100) { percentage = 100; }
        else if(percentage < 0) { percentage = 0; }

        //Fail Condition
        if(percentage == 0) { FindObjectOfType<PlayerController>().Lose(); }

        //Progressbar Text and Values
        progressText.text = percentage + "%";
        progressBar.value = percentage / 100;

        //Creates the request for the player.
        if (readyToAsk && EmployeeID != 4 && !beingAsked)
        {
            Random.InitState(System.DateTime.Now.Millisecond);
            float askTime = Random.Range(5, 10);
            Invoke("Ask", askTime);
            readyToAsk = false;
        }
    }

    void Ask()
    {
        //Generates a random request.
        Random.InitState(System.DateTime.Now.Millisecond);
        request = Random.Range(0, phrasesToSay.wordsToSay.Length);

        bool goingToAsk = true;

        switch (request)
        {
            //Talk to another worker.
            case 0:
                switch (EmployeeID)
                {
                    case 1:
                        GameObject.Find("Worker 2").GetComponent<EmployeeController>().beingAsked = true;
                        GameObject.Find("Worker 2").GetComponent<EmployeeController>().whoAsked = 1;
                        break;
                    case 2:
                        GameObject.Find("Worker 3").GetComponent<EmployeeController>().beingAsked = true;
                        GameObject.Find("Worker 3").GetComponent<EmployeeController>().whoAsked = 2;
                        break;
                    case 3:
                        GameObject.Find("Worker 1").GetComponent<EmployeeController>().beingAsked = true;
                        GameObject.Find("Worker 1").GetComponent<EmployeeController>().whoAsked = 3;
                        break;
                }
                break;

            //Go to desk.
            case 1:
                if(!FindObjectOfType<PlayerController>().deskTime)
                {
                    switch (EmployeeID)
                    {
                        case 1:
                            FindObjectOfType<PlayerController>().deskTime = true;
                            FindObjectOfType<PlayerController>().whoBeggedForDesk = 1;
                            break;
                        case 2:
                            FindObjectOfType<PlayerController>().deskTime = true;
                            FindObjectOfType<PlayerController>().whoBeggedForDesk = 2;
                            break;
                        case 3:
                            FindObjectOfType<PlayerController>().deskTime = true;
                            FindObjectOfType<PlayerController>().whoBeggedForDesk = 3;
                            break;
                    }
                }
                else { goingToAsk = false; }
                break;

            //Talk to Bosso.
            case 2:
                if (!GameObject.Find("Bosso").GetComponent<EmployeeController>().beingAsked)
                {
                    switch (EmployeeID)
                    {
                        case 1:
                            GameObject.Find("Bosso").GetComponent<EmployeeController>().beingAsked = true;
                            GameObject.Find("Bosso").GetComponent<EmployeeController>().whoAsked = 1;
                            break;
                        case 2:
                            GameObject.Find("Bosso").GetComponent<EmployeeController>().beingAsked = true;
                            GameObject.Find("Bosso").GetComponent<EmployeeController>().whoAsked = 2;
                            break;
                        case 3:
                            GameObject.Find("Bosso").GetComponent<EmployeeController>().beingAsked = true;
                            GameObject.Find("Bosso").GetComponent<EmployeeController>().whoAsked = 3;
                            break;
                    }
                }
                else { goingToAsk = false;  }
                break;
        }

        //If there aren't any colliding requests, it starts the request.
        if (goingToAsk)
        {
            audioSource.clip = alert;
            audioSource.Play();
            asking = true;
            alertUI.SetActive(true);
            Invoke("Fail", 15);
        }
        else { readyToAsk = true; }
    }

    //When the player times out the request.
    void Fail()
    {
        //Stops the workers from constantly waiting for the messages.
        switch (EmployeeID)
        {
            case 1:
                GameObject.Find("Worker 2").GetComponent<EmployeeController>().beingAsked = false;
                break;
            case 2:
                GameObject.Find("Worker 3").GetComponent<EmployeeController>().beingAsked = false;
                break;
            case 3:
                GameObject.Find("Worker 1").GetComponent<EmployeeController>().beingAsked = false;
                break;
        }

        audioSource.clip = angryTalk;
        audioSource.Play();
        animator.Play("Anger");
        alertUI.SetActive(false);
        percentage -= 15;
        readyToAsk = true;
        asking = false;
        if (request == 1) { FindObjectOfType<PlayerController>().deskTime = false; }
    }

    public void Talk()
    {
        //When prompted to talk by a worker and does not have a request for a player, unless everyone is being asked.
        if (beingAsked && !asking || 
            (GameObject.Find("Worker 1").GetComponent<EmployeeController>().beingAsked &&
             GameObject.Find("Worker 2").GetComponent<EmployeeController>().beingAsked &&
             GameObject.Find("Worker 3").GetComponent<EmployeeController>().beingAsked))
        {
            switch (whoAsked)
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

            //Bosso removes 10% as sort of a timer and represents his losing patience.
            if(EmployeeID == 4) 
            {   percentage -= 10; 
                audioSource.clip = angryTalk;
                audioSource.Play();
            }
            else
            {
                audioSource.clip = talk;
                audioSource.Play();
            }

            requestText.text = phrasesToRespond.wordsToSay[Random.Range(0, phrasesToRespond.wordsToSay.Length)];
            textBubble.SetActive(true);
            Invoke("CloseChat", 5);
            beingAsked = false;
        }
        else
        {
            if (asking == true)
            {
                //If you talk to them with a request.
                audioSource.clip = talk;
                audioSource.Play();
                requestText.text = phrasesToSay.wordsToSay[request];
                textBubble.SetActive(true);
                Invoke("CloseChat", 5);
            }
            else
            {
                //If you talk to them when they don't need to be talked to.
                audioSource.clip = angryTalk;
                audioSource.Play();
                animator.Play("Anger");
                requestText.text = "I'm busy working! Stop interrupting...";
                textBubble.SetActive(true);
                Invoke("CloseChat", 5);
                percentage -= 20;
            }
        }
    }

    //Closes the text bubble.
    public void CloseChat() { textBubble.SetActive(false); }

    //When a task is successful.
    public void Win()
    {
        audioSource.clip = happyTalk;
        audioSource.Play();
        FindObjectOfType<PlayerController>().score += 100;
        CloseChat();
        CancelInvoke();
        alertUI.SetActive(false);
        readyToAsk = true;
        asking = false;
        if(request == 1) { FindObjectOfType<PlayerController>().deskTime = false; }
    }
}
