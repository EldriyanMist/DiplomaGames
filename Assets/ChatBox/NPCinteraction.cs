using UnityEngine;

public class NPCinteraction : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("NPC"))
        {
            NPCChatbox myChatbox = GetComponent<NPCChatbox>();
            if (myChatbox != null)
            {
                myChatbox.AddMessage("Hello there!");  // This NPC sends a message
            }

            NPCChatbox otherChatbox = collision.GetComponent<NPCChatbox>();
            if (otherChatbox != null)
            {
                otherChatbox.AddMessage("Hi, nice to meet you!");  // The other NPC responds
            }
        }
    }
}
