using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class UserChatInput : MonoBehaviour
{
    [SerializeField] private TMP_InputField inputField;   // The TextMeshPro InputField
    [SerializeField] private Transform chatParent;         // Where the chat bubbles will be placed (usually a parent GameObject for chat bubbles)
    [SerializeField] private Button sendButton;            // A Button to send the message (optional)

    private void Start()
    {
        // If using a Button, add a listener to it
        if (sendButton != null)
        {
            sendButton.onClick.AddListener(OnSendButtonClicked);
        }

        // Optional: Automatically submit on pressing Enter
        inputField.onEndEdit.AddListener((text) =>
        {
            if (Input.GetKeyDown(KeyCode.Return))
            {
                OnSendButtonClicked();
            }
        });
    }

    // Called when the send button is clicked
    private void OnSendButtonClicked()
    {
        string message = inputField.text.Trim();

        if (!string.IsNullOrEmpty(message))
        {
            // Create the chat bubble with the user's message
            ChatBubble.Create(chatParent, Vector3.up * 2f, message);

            // Optionally clear the input field after sending
            inputField.text = string.Empty;
        }
    }
}
