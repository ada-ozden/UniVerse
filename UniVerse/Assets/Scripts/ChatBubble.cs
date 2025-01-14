using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class ChatBubble : MonoBehaviour
{
    [SerializeField] private Image backgroundImage;
    [SerializeField] private TextMeshProUGUI textMeshPro;

    private Camera mainCamera;

    private void Awake()
    {
        // Get the main camera reference
        mainCamera = Camera.main;

        // Ensure references are set
        if (backgroundImage == null)
            backgroundImage = transform.Find("Background").GetComponent<Image>();
        if (textMeshPro == null)
            textMeshPro = transform.Find("Text").GetComponent<TextMeshProUGUI>();
    }

    private void LateUpdate()
    {
        // Ensure the chat bubble always faces the camera
        FaceCamera();
    }

    public void Setup(string message)
    {
        // Set the text
        textMeshPro.text = message;

        // Adjust the background size based on the text size
        textMeshPro.ForceMeshUpdate();
        Vector2 textSize = textMeshPro.GetRenderedValues(false);
        backgroundImage.rectTransform.sizeDelta = textSize + new Vector2(20f, 10f); // Add padding
    }

    // Function to make the chat bubble always face the camera
    private void FaceCamera()
    {
        if (mainCamera != null)
        {
            transform.forward = mainCamera.transform.forward;
        }
    }

    // Static method to create the chat bubble at a specified position above a character
    public static ChatBubble Create(Transform parent, Vector3 localOffset, string message)
    {
        // Instantiate the chat bubble prefab
        GameObject chatBubblePrefab = GameAssets.Instance.chatBubblePrefab;
        GameObject chatBubbleObject = Instantiate(chatBubblePrefab, parent);

        // Set its position (slightly above the character's head)
        chatBubbleObject.transform.localPosition = localOffset;

        // Get the ChatBubble component and setup its data
        ChatBubble chatBubble = chatBubbleObject.GetComponent<ChatBubble>();
        chatBubble.Setup(message);

        // Destroy the chat bubble after 4 seconds
        Destroy(chatBubbleObject, 4f);

        return chatBubble;
    }
}
 