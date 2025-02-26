using TMPro;
using UnityEngine;
using UnityEngine.UI;  // For Button and InputField
using System.Collections;

public class PlayerSpeechBubble : MonoBehaviour
{
    public GameObject speechBubble;  // Speech bubble UI
    public TextMeshProUGUI messageText; // Text inside the speech bubble
    private float messageDuration = 3f;  // Time before the message disappears
    private Coroutine messageCoroutine;

    public TMP_InputField inputField;  // Reference to the InputField
    public Button sendButton;          // Reference to the Send button

    void Start()
    {
        speechBubble.SetActive(false); // Hide bubble initially

        sendButton.onClick.AddListener(OnSendMessage);  // Add listener for the button
    }

    public void DisplayMessage(string message)
    {
        messageText.text = message;
        speechBubble.SetActive(true); // Show speech bubble

        if (messageCoroutine != null)
            StopCoroutine(messageCoroutine);

        messageCoroutine = StartCoroutine(HideMessageAfterDelay());
    }

    private IEnumerator HideMessageAfterDelay()
    {
        yield return new WaitForSeconds(messageDuration);
        speechBubble.SetActive(false); // Hide speech bubble
    }

    // Called when the player clicks the Send button
    private void OnSendMessage()
    {
        string message = inputField.text;  // Get the text from the input field

        if (!string.IsNullOrEmpty(message))  // Make sure the message isn't empty
        {
            DisplayMessage(message);  // Display the message in the speech bubble
            inputField.text = "";  // Clear the input field
        }
    }
}
