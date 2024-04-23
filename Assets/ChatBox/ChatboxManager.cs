using UnityEngine;
using TMPro;

public class ChatboxManager : MonoBehaviour
{
    public TMP_Text chatText;
    private string chatLog = "";

    // Method to add a message to the chat box
    public void AddMessage(string senderName, string message)
    {
        chatLog += senderName + ": " + message + "\n";
        chatText.text = chatLog;
    }
}
