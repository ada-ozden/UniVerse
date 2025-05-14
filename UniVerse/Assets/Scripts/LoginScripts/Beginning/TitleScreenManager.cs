using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TitleScreenManager : MonoBehaviour
{
    public string nextSceneName = "LoginPage";//Takes the player to the login page
    public FadeManager FadeManager;

    void Start()
    {
        FadeManager.nextSceneName = "LoginPage";//set the next scene dynamically
    }

    // Update is called once per frame
    void Update()
    {
        // Check if the player clicks anywhere on the screen
        if (Input.GetMouseButtonDown(0)) // 0 = Left mouse button
        {
            // Load the next scene
           FadeManager.StartFadeOut();
        }
    }
}
