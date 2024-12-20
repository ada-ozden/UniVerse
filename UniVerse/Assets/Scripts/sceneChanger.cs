
using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class SceneChanger : MonoBehaviour
{
    public RectTransform animationPanel;  // Panel that will animate across the screen
    public float animationTime = 0.5f;    // Time taken for the animation
    public string targetSceneName;        // Target scene to load

    // This function will be triggered by the UI Button's onClick event.
    // It doesn't need a parameter now, as we'll get the button's position internally.
    public void StartSceneChange(string sceneName)
    {
        targetSceneName = sceneName;

        // Get the currently selected button via the EventSystem
        GameObject selectedButton = EventSystem.current.currentSelectedGameObject;

        // Ensure the selected button has a RectTransform
        RectTransform targetButton = selectedButton.GetComponent<RectTransform>();
        
        if (targetButton != null)
        {
            // Start the animation and scene change
            StartCoroutine(ChangeScene(targetButton));
        }
        else
        {
            Debug.LogError("Selected button does not have a RectTransform!");
        }
    }

    // Coroutine to animate and load the scene
    private IEnumerator ChangeScene(RectTransform targetButton)
    {
        // Ensure the animation panel has a RectTransform to work with
        if (animationPanel == null)
        {
            Debug.LogError("Animation panel RectTransform is not assigned!");
            yield break; // Exit the coroutine if no animation panel is assigned
        }

        // Slide the panel out of the screen vertically (up or down)
        Vector3 startPosition = animationPanel.position;

        // Get the target position from the button's position
        Vector3 targetPosition = targetButton.position;

        float elapsedTime = 0f;

        while (elapsedTime < animationTime)
        {
            animationPanel.position = Vector3.Lerp(startPosition, targetPosition, elapsedTime / animationTime);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        // Ensure the panel reaches the target position
        animationPanel.position = targetPosition;

        // Load the new scene
        SceneManager.LoadScene(targetSceneName);
    }
}

