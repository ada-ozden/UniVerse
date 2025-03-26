using UnityEngine;
using TMPro;
using UnityEngine.UI;
using Unity.Netcode;

public class GameManager : NetworkBehaviour
{
    public static GameManager Instance; // Singleton instance

    public TMP_InputField chatInput; // Chat input field
    public Button sendButton; // Send button
    public TextMeshProUGUI chatDisplay; // Chat display area

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        sendButton.onClick.AddListener(SendMessage);
    }

    private void SendMessage()
    {
        if (string.IsNullOrWhiteSpace(chatInput.text)) return;

        SendChatMessageServerRpc(chatInput.text);
        chatInput.text = "";
    }

    [ServerRpc(RequireOwnership = false)]
    private void SendChatMessageServerRpc(string message, ServerRpcParams rpcParams = default)
    {
        ulong senderId = rpcParams.Receive.SenderClientId;
        string formattedMessage = $"Player {senderId}: {message}";

        DisplayMessageClientRpc(formattedMessage);
    }

    [ClientRpc]
    private void DisplayMessageClientRpc(string message)
    {
        chatDisplay.text += message + "\n";
    }
}
