using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System.Collections;
using Mirror;
using UnityEngine.EventSystems;
using StarterAssets;

public class PlayerSpeechBubble : NetworkBehaviour
{
    public GameObject speechBubble;
    public TextMeshProUGUI messageText;
    private float messageDuration = 3f;
    private Coroutine messageCoroutine;
    private TMP_InputField inputField;
    private Button sendButton;

    private ThirdPersonController playerInput;
    [SyncVar(hook = nameof(OnMessageChanged))]
    private string networkMessage = "";

    void Start()
    {
        speechBubble.SetActive(false);

        // Use isLocalPlayer instead of isOwned
        if (!isLocalPlayer) return;

        inputField = GameManager.Instance.chatInput;
        sendButton = GameManager.Instance.sendButton;

        if (sendButton != null)
            sendButton.onClick.AddListener(OnSendMessage);

        playerInput = GetComponent<ThirdPersonController>();

        if (inputField != null)
        {
            inputField.characterLimit = 150;
            inputField.onSelect.AddListener(delegate { playerInput.IsTyping = true; });
            inputField.onDeselect.AddListener(delegate { playerInput.IsTyping = false; });
        }
    }

    void Update()
    {
        if (!isLocalPlayer) return;

        if (Input.GetKeyDown(KeyCode.Return))
        {
            if (!inputField.isFocused)
            {
                inputField.Select();
                inputField.ActivateInputField();
                playerInput.IsTyping = true;
            }
            else if (inputField.text.Trim().Length > 0)
            {
                OnSendMessage();
            }
        }
    }

    public void DisplayMessage(string message)
    {
        messageText.text = message;
        speechBubble.SetActive(true);

        if (messageCoroutine != null)
        {
            StopCoroutine(messageCoroutine);
        }

        messageCoroutine = StartCoroutine(HideMessageAfterDelay());
    }

    private IEnumerator HideMessageAfterDelay()
    {
        yield return new WaitForSeconds(messageDuration);
        speechBubble.SetActive(false);
    }

    private void OnSendMessage()
    {
        string message = inputField.text.Trim();
        if (message.Length > 0)
        {
            CmdSendMessageToServer(message);
            inputField.text = "";
            inputField.DeactivateInputField();
            EventSystem.current.SetSelectedGameObject(null);
            playerInput.IsTyping = false;
        }
    }

    [Command]
    private void CmdSendMessageToServer(string message)
    {
        networkMessage = message;
    }

    private void OnMessageChanged(string oldValue, string newValue)
    {
        DisplayMessage(newValue);
    }
}