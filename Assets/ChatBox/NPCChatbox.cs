using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System.Collections.Generic;

public class NPCChatbox : MonoBehaviour
{
    public GameObject chatboxPrefab;
    private GameObject myChatbox;
    private TMP_Text chatText;
    private TMP_InputField inputField;
    private Button sendButton;
    private static int messageIDCounter = 0;

    private List<Message> messages = new List<Message>();

    void Start()
    {
        // Instantiate the chatbox as a child of the canvas
        myChatbox = Instantiate(chatboxPrefab, GameObject.Find("GameCanvas").transform);
        chatText = myChatbox.GetComponentInChildren<TMP_Text>();
        inputField = myChatbox.GetComponentInChildren<TMP_InputField>();
        sendButton = myChatbox.GetComponentInChildren<Button>();

        // Add listener for the send button
        sendButton.onClick.AddListener(HandleSendClicked);
    }

    void Update()
    {
        // Ensure the chatbox follows the NPC in screen space
        Vector2 screenPosition = Camera.main.WorldToScreenPoint(transform.position + new Vector3(0, 1.5f, 0));
        myChatbox.transform.position = screenPosition;
    }

    private void HandleSendClicked()
    {
        if (!string.IsNullOrEmpty(inputField.text))
        {
            SendMessageTo("NPC-2", inputField.text);  // Example receiver NPC-2
            inputField.text = ""; // Clear the input field after sending
        }
    }

    public void SendMessageTo(string receiver, string content)
    {
        string timestamp = System.DateTime.Now.ToString("HH:mm:ss");  // Using real time for simplicity
        Message message = new Message(name, receiver, content, timestamp, messageIDCounter++);
        messages.Add(message);
        ChatboxManager.Instance.AddMessage(message);
    }

    public void ReceiveMessage(Message message)
    {
        if (message.Receiver == name)
        {
            messages.Add(message);
            DisplayMessages();
        }
    }

    private void DisplayMessages()
    {
        messages.Sort((x, y) => y.ID.CompareTo(x.ID));  // Sort messages by ID, newest first
        chatText.text = "";
        foreach (Message msg in messages)
        {
            chatText.text += msg.Content + "\n";
        }
    }

    private void OnDestroy()
    {
        // Clean up to avoid memory leaks
        if (myChatbox)
            Destroy(myChatbox);
    }
}
