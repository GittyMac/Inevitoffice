using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BillboardController : MonoBehaviour
{
    //Player to look at.
    private GameObject player;

    private void Start() { player = FindObjectOfType<PlayerController>().gameObject; }
    void Update()
    {
        //Looks at the player horizontally.
        transform.LookAt(transform.position + player.transform.rotation * Vector3.forward, player.transform.rotation * Vector3.up);
        Vector3 eulerAngles = transform.eulerAngles;
        eulerAngles.x = 0;
        transform.eulerAngles = eulerAngles;
    }
}
