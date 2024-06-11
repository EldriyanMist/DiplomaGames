using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InventoryPlayer : MonoBehaviour
{
    public GameObject slot;   // Reference to the single slot
    public GameObject statusWindow; // Reference to the status window
    private Item currentItem; // Reference to the current item
    public List<ItemPrefabMapping> itemPrefabMappings; // List to map items to their prefabs

    void Start()
    {
        currentItem = null; // Initialize currentItem as null
        slot.SetActive(false); // Ensure the slot is hidden at the start
    }

    public bool AddItem(Item newItem)
    {
        if (currentItem == null) // Check if there's no current item
        {
            currentItem = newItem; // Add the new item
            UpdateSlot(); // Update the slot with the new item's icon
            return true; // Indicate that the item was successfully added
        }
        return false; // Indicate that the item was not added because the inventory is full
    }

    void UpdateSlot()
    {
        Image slotImage = slot.GetComponent<Image>();
        slotImage.sprite = currentItem.icon; // Set the slot's icon to the item's icon
        slot.SetActive(true); // Make the slot visible
    }

    public void RemoveItem()
    {
        currentItem = null; // Remove the current item
        slot.SetActive(false); // Hide the slot
    }

    public void DropItem()
    {
        if (currentItem != null)
        {
            // Find the corresponding prefab for the current item
            GameObject prefabToDrop = itemPrefabMappings.Find(mapping => mapping.item == currentItem)?.prefab;
            if (prefabToDrop != null)
            {
                // Instantiate the collectable item prefab at the player's position
                Vector3 dropPosition = transform.position;
                GameObject droppedItem = Instantiate(prefabToDrop, dropPosition, Quaternion.identity);

                // Set the item's properties on the dropped prefab
                Collectable collectable = droppedItem.GetComponent<Collectable>();
                if (collectable != null)
                {
                    collectable.item = currentItem; // Assign the current item to the collectable script
                }

                RemoveItem(); // Remove the item from the inventory
            }
            else
            {
                Debug.LogError("No prefab found for the current item.");
            }
        }
    }

    public bool PutItemInChest(InventoryChest chest)
    {
        if (currentItem != null)
        {
            bool wasAdded = chest.AddItem(currentItem);
            if (wasAdded)
            {
                RemoveItem(); // Remove the item from the NPC's inventory
                return true;
            }
        }
        return false;
    }

    public bool TakeItemFromChest(InventoryChest chest, int slotIndex)
    {
        if (currentItem == null) // Check if the NPC's inventory is empty
        {
            Item itemToTake = chest.RemoveItem(slotIndex);
            if (itemToTake != null)
            {
                currentItem = itemToTake; // Add the item to the NPC's inventory
                UpdateSlot(); // Update the NPC's slot with the new item's icon
                return true;
            }
        }
        return false;
    }

    public Item GetCurrentItem()
    {
        return currentItem; // Return the current item in the NPC's inventory
    }
}


[System.Serializable]
public class ItemPrefabMapping
{
    public Item item;         // The item type
    public GameObject prefab; // The corresponding prefab
}