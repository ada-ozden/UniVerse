using System.Collections.Generic;
using UnityEngine;
using System.IO;
public class ChatManager : MonoBehaviour
{
    private MessageFilter messageFilter;

    void Start()
    {
        messageFilter = GetComponent<MessageFilter>();
    }

    public void OnMessageReceived(string userMessage)
    {
        string filteredMessage = messageFilter.FilterMessage(userMessage);
        DisplayMessage(filteredMessage);
    }

    private void DisplayMessage(string message)
    {
        // Mesajı ekranda göstermenin yolu
        Debug.Log(message);
    }
}