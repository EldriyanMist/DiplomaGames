using UnityEngine;

public class Collectable : MonoBehaviour
{
    public Item item; // Reference to the item scriptable object

    private void OnMouseDown()
    {
        // Assuming there's a way to get the player's inventory
        InventoryChest playerInventory = FindObjectOfType<InventoryChest>();
        if (playerInventory != null)
        {
            playerInventory.AddItem(item);
            Destroy(gameObject); // Destroy the collectable item after it's collected
        }
    }
}