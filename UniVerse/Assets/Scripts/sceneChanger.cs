using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;


public class SceneChanger : MonoBehaviour{
    public RectTransform animationPanel;
    public float animationTime;

    public IEnumerator ChangeScene(string sceneName, Vector3 targetPosition){
        float startTime = 0f;
        Vector3 startPosition = animationPanel.position; //panelin durduğu yer ilk başta

        while (startTime < animationTime){
            animationPanel.position = Vector3.Lerp(startPosition, targetPosition, startTime / animationTime);
            startTime += Time.deltaTime; 
            yield return null; // yavaş yavas hareket etmesi için
        }

        animationPanel.position = targetPosition;
        SceneManager.LoadScene(sceneName);
    }
}
