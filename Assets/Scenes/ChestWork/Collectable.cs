using UnityEngine;

public class Collectable : MonoBehaviour
{
    public Item item; // Reference to the item scriptable object

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // Check if the object we collided with has the tag "NPC"
        if (collision.CompareTag("NPC"))
        {
            // Assuming there's a way to get the player's inventory
            InventoryPlayer playerInventory = FindObjectOfType<InventoryPlayer>();
            if (playerInventory != null)
            {
                bool wasAdded = playerInventory.AddItem(item); // Try to add the item to the player's inventory
                if (wasAdded)
                {
                    Destroy(gameObject); // Destroy the collectable item only if it was successfully added
                }
            }
        }
    }
}