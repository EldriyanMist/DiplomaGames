using UnityEngine;

public class NPCinteraction : MonoBehaviour
{
    // This method is called when another collider enters the trigger collider attached to this game object
    public ChatboxManager chatManager;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        // Check if the object colliding is another NPC
        if (collision.gameObject.CompareTag("NPC"))
        {
            Debug.Log("NPC has encountered another NPC!");
            // Here you can add code to open a chat box or initiate dialogue
        }

        if (collision.gameObject.CompareTag("NPC"))
        {
            if (chatManager != null)
            {
                chatManager.AddMessage(gameObject.name, "Hello there!");
            }
        }
    }
}
