using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCollisionManager : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        //Closes the text bubbles when the player exits their office.
        //Prvents the bubbles appearing through walls.
        switch (other.gameObject.tag)
        {
            case "W1Shush":
                GameObject.Find("Worker 1").GetComponent<EmployeeController>().CloseChat();
                break;
            case "W2Shush":
                GameObject.Find("Worker 2").GetComponent<EmployeeController>().CloseChat();
                break;
            case "W3Shush":
                GameObject.Find("Worker 3").GetComponent<EmployeeController>().CloseChat();
                break;
            case "BossShush":
                GameObject.Find("Bosso").GetComponent<EmployeeController>().CloseChat();
                break;
        }
    }
}
