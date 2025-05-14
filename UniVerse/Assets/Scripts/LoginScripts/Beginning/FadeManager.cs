using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections;

public class FadeManager : MonoBehaviour
{
    public RawImage fadeImage;
    public float fadeDuration = 1f;
    public string nextSceneName;

    private bool isFading = false;

    void Start()
    {
        fadeImage.color = new Color(0, 0, 0, 0);  // Start with transparent
    }

    public void StartFadeOut()
    {
        if (!isFading)
        {
            StartCoroutine(FadeOut());
        }
    }

    private IEnumerator FadeOut()
    {
        isFading = true;
        float timeElapsed = 0f;
        Color startColor = fadeImage.color;

        // Fade to black
        while (timeElapsed < fadeDuration)
        {
            timeElapsed += Time.deltaTime;
            fadeImage.color = Color.Lerp(startColor, Color.black, timeElapsed / fadeDuration);
            yield return null;
        }

        // Load next scene after fade
        SceneManager.LoadScene(nextSceneName);
    }
}

