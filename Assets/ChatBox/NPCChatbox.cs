using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class NPCChatbox : MonoBehaviour
{
    public GameObject chatboxPrefab;
    private GameObject myChatbox;
    private TMP_Text chatText;
    private TMP_InputField inputField;
    private Button sendButton;

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
            AddMessage(inputField.text);  // Send the text as a message
            inputField.text = ""; // Clear the input field after sending
        }
    }

    public void AddMessage(string message)
    {
        chatText.text += name + ": " + message + "\n";  // Add the message to the chat text
    }

    private void OnDestroy()
    {
        // Clean up to avoid memory leaks
        if (myChatbox)
            Destroy(myChatbox);
    }
}
