using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuController : MonoBehaviour
{
    private void Start()
    {
        //Disables useless Exit button on WebGL.
        #if !UNITY_EDITOR && UNITY_WEBGL
            GameObject.Find("ExitButton").SetActive(false);
        #endif
    }

    void Update()
    {
        //Closes on ESC
        if (Input.GetButtonDown("Cancel"))
        {
            Application.Quit();
        }
    }

    //Exit Button
    public void ExitGame() { Application.Quit(); }

    //Start Button
    public void StartGame() { SceneManager.LoadScene(1); }
}
