using UnityEngine;
using UnityEngine.SceneManagement; 
public class SceneChanger : MonoBehaviour
{
    public RectTransform animationPanel; 
    public float animationTime; 
        public void ChangeScene(string sceneName, Vector3 targetPosition){
        float startTime = 0f;
        Vector3 startPosition = animationPanel.position; // Panelin başlangıç pozisyonu
        while (startTime < animationTime){
            animationPanel.position = Vector3.Lerp(startPosition, targetPosition, startTime / animationTime);
            startTime += Time.deltaTime; 
            yield return null; // Bir sonraki kareye kadar bekle
        }
        animationPanel.position = targetPosition;
        SceneManager.LoadScene(sceneName); 
    }
}
