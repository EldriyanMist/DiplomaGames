using UnityEngine;
using TMPro;
using System.Collections.Generic;

public class ChatboxManager : MonoBehaviour
{
    public static ChatboxManager Instance { get; private set; }

    public TMP_Text chatText;
    private string chatLog = "";
    private List<Message> globalMessages = new List<Message>();

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    // Method to add a message to the chat box
    public void AddMessage(Message message)
    {
        globalMessages.Add(message);
        chatLog += message.ToString() + "\n";
        chatText.text = chatLog;

        // Notify the receiving NPC
        NPCChatbox[] npcs = FindObjectsOfType<NPCChatbox>();
        foreach (NPCChatbox npc in npcs)
        {
            npc.ReceiveMessage(message);
        }
    }
}
